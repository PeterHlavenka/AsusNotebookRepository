package cz.visentio.codexusage;

import android.app.Service;
import android.content.*;
import android.net.Uri;
import android.os.IBinder;
import android.util.Base64;
import org.json.*;
import java.net.*;
import java.io.*;
import java.security.*;
import java.nio.charset.StandardCharsets;
import java.util.UUID;

/** Loopback authorization-code flow with PKCE, state validation and a five-minute lifetime. */
public class LoginService extends Service {
    private volatile ServerSocket listener;
    private volatile boolean running;
    @Override public IBinder onBind(Intent intent){return null;}
    @Override public int onStartCommand(Intent intent,int flags,int id) {
        startForeground(21,Notifications.make(this,"Přihlášení do Codexu","Dokonči přihlášení v prohlížeči"));
        if(running)return START_NOT_STICKY;
        if(intent==null){stopSelf();return START_NOT_STICKY;}
        running=true;
        getSharedPreferences("settings",0).edit().remove("loginError").apply();
        String kind=intent.getStringExtra("kind");
        new Thread(()->login(kind),"codex-login").start();
        return START_NOT_STICKY;
    }
    private void login(String kind) {
        try {
            listener=new ServerSocket();listener.setReuseAddress(true);
            listener.bind(new InetSocketAddress(InetAddress.getByName("127.0.0.1"),1455)); listener.setSoTimeout(300000);
            String verifier=AuthFlow.random(),state=AuthFlow.random();
            String challenge=AuthFlow.challenge(verifier);
            String redirect="http://localhost:1455/auth/callback";
            String url="https://auth.openai.com/oauth/authorize?"+Api.form("response_type","code","client_id",Api.CLIENT,
                "redirect_uri",redirect,"scope","openid profile email offline_access","code_challenge",challenge,
                "code_challenge_method","S256","state",state,"id_token_add_organizations","true","codex_cli_simplified_flow","true","prompt","login");
            startActivity(new Intent(Intent.ACTION_VIEW,Uri.parse(url)).addFlags(Intent.FLAG_ACTIVITY_NEW_TASK));
            long deadline=System.currentTimeMillis()+300000;
            boolean complete=false;
            while(System.currentTimeMillis()<deadline&&!complete) {
                listener.setSoTimeout((int)Math.max(1,deadline-System.currentTimeMillis()));
                try(Socket socket=listener.accept()) {
                    socket.setSoTimeout(5000);
                    BufferedReader input=new BufferedReader(new InputStreamReader(socket.getInputStream(),StandardCharsets.US_ASCII));
                    String line=input.readLine();
                    if(line==null||line.length()>16384){reply(socket,400,"Neplatný požadavek");continue;}
                    String[] parts=line.split(" ");
                    if(parts.length!=3||!parts[0].equals("GET")){reply(socket,400,"Neplatný požadavek");continue;}
                    Uri callback=Uri.parse(parts[1]);
                    if(!"/auth/callback".equals(callback.getPath())){reply(socket,404,"Nenalezeno");continue;}
                    String returnedState=callback.getQueryParameter("state");
                    if(!AuthFlow.matches(state,returnedState)) {
                        reply(socket,400,"Neplatný stav přihlášení");continue;
                    }
                    String code=callback.getQueryParameter("code");
                    if(code==null){reply(socket,400,"Přihlášení bylo zrušeno");throw new Exception("Přihlášení bylo zrušeno");}
                    try {
                        JSONObject token=Api.request("https://auth.openai.com/oauth/token",Api.form("grant_type","authorization_code",
                            "client_id",Api.CLIENT,"code",code,"redirect_uri",redirect,"code_verifier",verifier),null,null);
                        JSONObject account=new JSONObject().put("id",UUID.randomUUID().toString()).put("kind",kind==null?"work":kind);
                        Api.updateTokens(account,token);
                        if(account.optString("accountId").isEmpty()||!account.has("refresh"))throw new Exception("Přihlášení neobsahuje účet Codexu");
                        Store.add(this,account);
                        reply(socket,200,"Přihlášení dokončeno. Vrať se do aplikace Codex Usage.");complete=true;
                    } catch(Exception error) {reply(socket,500,"Přihlášení nelze dokončit. Vrať se do aplikace.");throw error;}
                }
            }
            getSharedPreferences("settings",0).edit().remove("loginError").apply();
            Api.WORK.execute(()->Api.refresh(this)); UsageJob.schedule(this);
        } catch(Exception error) {
            String message=error instanceof BindException?"Port přihlášení je obsazený; zavři jiné přihlašování":error instanceof SocketTimeoutException?"Přihlášení vypršelo; zkus znovu":error instanceof Api.HttpError?error.getMessage():error instanceof IOException?"Přihlášení přerušeno nebo není připojení":error instanceof JSONException?"Přihlášení vrátilo nečekanou odpověď":error.getMessage();
            getSharedPreferences("settings",0).edit().putString("loginError",message==null?"Přihlášení se nezdařilo":message).apply();
        } finally {running=false;close();stopForeground(STOP_FOREGROUND_REMOVE);stopSelf();}
    }
    private static void reply(Socket socket,int status,String message) throws IOException {
        byte[] body=("<!doctype html><meta charset=utf-8><meta name=viewport content='width=device-width'><body style='background:#202421;color:#b8ceaa;font:18px sans-serif;padding:32px'><h2>Codex Usage</h2><p>"+message+"</p>").getBytes(StandardCharsets.UTF_8);
        OutputStream out=socket.getOutputStream();
        out.write(("HTTP/1.1 "+status+" OK\r\nContent-Type: text/html; charset=utf-8\r\nContent-Length: "+body.length+"\r\nConnection: close\r\nCache-Control: no-store\r\n\r\n").getBytes(StandardCharsets.US_ASCII));out.write(body);out.flush();
    }
    private void close(){try{if(listener!=null)listener.close();}catch(IOException ignored){}}
    @Override public void onDestroy(){close();super.onDestroy();}
    @Override public void onTimeout(int startId,int foregroundServiceType){close();stopSelf();}
}
