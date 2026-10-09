package cz.visentio.codexusage;

import android.content.Context;
import android.util.Base64;
import org.json.*;
import java.net.*;
import java.io.*;
import java.nio.charset.StandardCharsets;
import java.util.concurrent.Executors;
import java.util.concurrent.ExecutorService;

/** Experimental adapter to the same read-only endpoints used by openai/codex. */
final class Api {
    static final String CLIENT="app_EMoamEEZ73f0CkXaXp7hrann";
    static final ExecutorService WORK=Executors.newSingleThreadExecutor();
    static final class HttpError extends Exception {
        final int status;
        HttpError(int status) { super(status==401?"Přihlásit znovu":status==403?"Přístup zamítnut (403)":status==429?"Příliš mnoho obnov; zkus později":"Server: HTTP "+status); this.status=status; }
    }
    static String form(String... pairs) throws Exception {
        StringBuilder b=new StringBuilder();
        for(int i=0;i<pairs.length;i+=2) { if(i>0)b.append('&'); b.append(URLEncoder.encode(pairs[i],"UTF-8")).append('=').append(URLEncoder.encode(pairs[i+1],"UTF-8")); }
        return b.toString();
    }
    static JSONObject request(String url,String body,String token,String account) throws Exception {
        HttpURLConnection connection=(HttpURLConnection)new URL(url).openConnection();
        connection.setInstanceFollowRedirects(false); connection.setConnectTimeout(12000); connection.setReadTimeout(12000);
        connection.setRequestProperty("Accept","application/json");
        connection.setRequestProperty("User-Agent","CodexUsageAndroid/0.1.2");
        if(token!=null)connection.setRequestProperty("Authorization","Bearer "+token);
        if(account!=null&&!account.isEmpty())connection.setRequestProperty("ChatGPT-Account-Id",account);
        try {
            if(body!=null) {
                connection.setRequestMethod("POST"); connection.setDoOutput(true);
                connection.setRequestProperty("Content-Type","application/x-www-form-urlencoded");
                try(OutputStream out=connection.getOutputStream()){out.write(body.getBytes(StandardCharsets.UTF_8));}
            }
            int status=connection.getResponseCode(); if(status!=200)throw new HttpError(status);
            try(InputStream in=connection.getInputStream(); ByteArrayOutputStream out=new ByteArrayOutputStream()) {
                byte[] buffer=new byte[4096]; int n;
                while((n=in.read(buffer))!=-1){out.write(buffer,0,n);if(out.size()>1024*1024)throw new IOException("Odpověď je příliš velká");}
                return new JSONObject(out.toString("UTF-8"));
            }
        } finally { connection.disconnect(); }
    }
    static JSONObject claims(String token) throws Exception {
        return new JSONObject(new String(Base64.decode(token.split("\\.")[1],Base64.URL_SAFE|Base64.NO_WRAP),StandardCharsets.UTF_8));
    }
    static void updateTokens(JSONObject account,JSONObject response) throws Exception {
        account.put("access",response.getString("access_token"));
        if(response.has("refresh_token"))account.put("refresh",response.getString("refresh_token"));
        if(response.has("id_token")) {
            JSONObject claims=claims(response.getString("id_token"));
            JSONObject profile=claims.optJSONObject("https://api.openai.com/profile");
            account.put("email",claims.optString("email",profile==null?"Codex":profile.optString("email","Codex")));
            JSONObject auth=claims.optJSONObject("https://api.openai.com/auth");
            if(auth!=null)account.put("accountId",auth.optString("chatgpt_account_id"));
        }
        account.put("expires",System.currentTimeMillis()/1000+response.optLong("expires_in",3600));
    }
    private static void renew(Context c,JSONObject a) throws Exception {
        updateTokens(a,request("https://auth.openai.com/oauth/token",form("grant_type","refresh_token","refresh_token",a.getString("refresh"),"client_id",CLIENT),null,null));
        Store.replace(c,a); // Save rotated refresh token before a subsequent request can fail.
    }
    static synchronized void refresh(Context c) {
        try {
            JSONArray accounts=Store.read(c);
            for(int i=0;i<accounts.length();i++) {
                JSONObject a=accounts.getJSONObject(i);
                try {
                    if(a.optLong("expires")<System.currentTimeMillis()/1000+60)renew(c,a);
                    JSONObject payload;
                    try { payload=usage(a); }
                    catch(HttpError error){if(error.status!=401)throw error;renew(c,a);payload=usage(a);}
                    a.put("snapshot",UsageData.parse(payload,System.currentTimeMillis()/1000)); a.remove("error");
                } catch(Exception error) { a.put("error",error instanceof HttpError?error.getMessage():error instanceof java.net.SocketTimeoutException?"Spojení vypršelo":error instanceof IOException?"Bez připojení":error instanceof JSONException?"Server změnil formát údajů":error.getMessage()==null?"Obnova se nezdařila":error.getMessage()); }
                Store.replace(c,a);
            }
        } catch(Exception ignored) { /* UI exposes an unreadable credential store rather than overwriting it. */ }
        UsageWidget.render(c);
    }
    private static JSONObject usage(JSONObject a) throws Exception {
        return request("https://chatgpt.com/backend-api/wham/usage",null,a.getString("access"),a.optString("accountId"));
    }
}
