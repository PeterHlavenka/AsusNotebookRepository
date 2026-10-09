package cz.visentio.codexusage;

import android.Manifest;
import android.app.*;
import android.appwidget.AppWidgetManager;
import android.content.*;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.os.*;
import android.view.*;
import android.widget.*;
import org.json.*;

public class MainActivity extends Activity {
    private LinearLayout body;
    private final Handler handler=new Handler(Looper.getMainLooper());
    private final Runnable poll=new Runnable(){public void run(){render();handler.postDelayed(this,3000);}};
    @Override public void onCreate(Bundle saved){super.onCreate(saved);getWindow().setStatusBarColor(Color.rgb(24,28,25));getWindow().setNavigationBarColor(Color.rgb(24,28,25));}
    @Override protected void onResume(){super.onResume();render();handler.postDelayed(poll,3000);}
    @Override protected void onPause(){handler.removeCallbacks(poll);super.onPause();}
    private int dp(int n){return (int)(n*getResources().getDisplayMetrics().density);}
    private TextView text(String value,int size){TextView v=new TextView(this);v.setText(value);v.setTextSize(size);v.setTextColor(Color.parseColor("#B8CEAA"));v.setPadding(0,dp(8),0,dp(8));body.addView(v);return v;}
    private void button(String label,Runnable action){Button b=new Button(this);b.setText(label);body.addView(b);b.setOnClickListener(v->action.run());}
    private void render(){
        ScrollView scroll=new ScrollView(this);scroll.setFillViewport(true);scroll.setBackgroundColor(Color.rgb(24,28,25));
        body=new LinearLayout(this);body.setOrientation(LinearLayout.VERTICAL);body.setPadding(dp(20),dp(20),dp(20),dp(20));scroll.addView(body);setContentView(scroll);
        if(Build.VERSION.SDK_INT>=35) {
            scroll.setOnApplyWindowInsetsListener((view,insets)->{
                android.graphics.Insets bars=insets.getInsets(WindowInsets.Type.systemBars());
                view.setPadding(bars.left,bars.top,bars.right,bars.bottom);return insets;
            });
            scroll.requestApplyInsets();
        }
        text("Codex Usage",28);
        text("Minimalistický widget · limity Codexu",14);
        String error=getSharedPreferences("settings",0).getString("loginError",null);if(error!=null)text(error,14).setTextColor(Color.rgb(232,179,123));
        String notice=getSharedPreferences("settings",0).getString("refreshNotice",null);if(notice!=null)text(notice,13);
        try {
            JSONArray accounts=Store.read(this);
            for(int i=0;i<accounts.length();i++) {
                JSONObject a=accounts.getJSONObject(i),snapshot=a.optJSONObject("snapshot");String id=a.getString("id");
                text((a.optString("kind").equals("personal")?"Osobní · ":"Visentio · ")+a.optString("email"),17);
                text(UsageData.percent(snapshot,"short")+"   "+UsageData.reset(snapshot,"short",java.time.ZoneId.systemDefault())+"\n"+
                    UsageData.percent(snapshot,"week")+"   "+UsageData.reset(snapshot,"week",java.time.ZoneId.systemDefault()),22);
                if(a.has("error"))text(a.optString("error"),13).setTextColor(Color.rgb(232,179,123));
                button("Odebrat účet",()->new AlertDialog.Builder(this).setMessage("Odebrat přihlášení z tohoto telefonu?")
                    .setNegativeButton("Zrušit",null).setPositiveButton("Odebrat",(d,w)->{try{Store.remove(this,id);UsageWidget.render(this);render();}catch(Exception e){message("Odebrání se nezdařilo");}}).show());
            }
            if(accounts.length()<2){button("Přihlásit firemní účet",()->login("work"));button("Přihlásit osobní účet",()->login("personal"));}
        } catch(Exception e) {
            text("Uložené přihlášení nelze dešifrovat. Obnov ho novým přihlášením.",14);
            button("Vymazat poškozené přihlášení",()->{Store.reset(this);UsageWidget.render(this);render();});
        }
        button("Obnovit usage",()->{message("Obnovuji…");UsageJob.now(this);});
        button("Přidat widget na plochu",()->{
            AppWidgetManager manager=AppWidgetManager.getInstance(this);
            if(manager.isRequestPinAppWidgetSupported())manager.requestPinAppWidget(new ComponentName(this,UsageWidget.class),null,null);
            else message("Podrž plochu → Widgety → Codex Usage");
        });
        Switch live=new Switch(this);live.setText("Průběžná obnova každou minutu");live.setTextColor(Color.parseColor("#B8CEAA"));live.setChecked(LiveService.running);body.addView(live);
        live.setOnCheckedChangeListener((v,enabled)->{
            Store.live(this,enabled);
            if(enabled){getSharedPreferences("settings",0).edit().remove("refreshNotice").apply();notifications();startForegroundService(new Intent(this,LiveService.class));}
            else stopService(new Intent(this,LiveService.class));
        });
        text("Běžná obnova: přibližně každých 15 minut. Klepnutí na widget obnoví data hned, jakmile je dostupné připojení. Ikona otevře nastavení. Účty jsou vedle sebe; widget můžeš roztáhnout na šířku. Načtené starší údaje jsou označené tlumenou barvou.",13);
        text("Průběžná obnova používá trvalé oznámení. Android nebo úspora baterie Vivo ji mohou pozastavit. V nastavení telefonu můžeš pro Codex Usage povolit běh na pozadí.",13);
        text("Experimentální aplikace: přihlášení a čtení limitů používají endpointy Codexu, které se mohou změnit. Žádné API klíče ani volání modelů. Přihlášení zůstává šifrované v tomto telefonu.",12);
    }
    private void notifications(){if(Build.VERSION.SDK_INT>=33&&checkSelfPermission(Manifest.permission.POST_NOTIFICATIONS)!=PackageManager.PERMISSION_GRANTED)requestPermissions(new String[]{Manifest.permission.POST_NOTIFICATIONS},1);}
    private void login(String kind){notifications();startForegroundService(new Intent(this,LoginService.class).putExtra("kind",kind));message("Otevírám přihlášení v prohlížeči…");}
    private void message(String message){Toast.makeText(this,message,Toast.LENGTH_LONG).show();}
}
