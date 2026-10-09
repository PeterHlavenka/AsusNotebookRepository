package cz.visentio.codexusage;

import android.app.*;
import android.appwidget.*;
import android.content.*;
import android.graphics.Color;
import android.view.View;
import android.widget.RemoteViews;
import org.json.*;
import java.time.ZoneId;

public class UsageWidget extends AppWidgetProvider {
    static final String REFRESH="cz.visentio.codexusage.REFRESH";
    @Override public void onUpdate(Context c,AppWidgetManager manager,int[] ids){render(c);UsageJob.schedule(c);enqueue(c);}
    @Override public void onReceive(Context c,Intent intent){
        super.onReceive(c,intent);
        if(REFRESH.equals(intent.getAction()))enqueue(c);
    }
    private void enqueue(Context c){
        // Broadcast work is handed to JobScheduler; network calls never block the launcher.
        UsageJob.now(c);
    }
    @Override public void onEnabled(Context c){UsageJob.schedule(c);}
    @Override public void onDisabled(Context c){UsageJob.cancel(c);Store.live(c,false);c.stopService(new Intent(c,LiveService.class));}
    static void render(Context c) {
        AppWidgetManager manager=AppWidgetManager.getInstance(c);
        int[] ids=manager.getAppWidgetIds(new ComponentName(c,UsageWidget.class)); if(ids.length==0)return;
        RemoteViews views=new RemoteViews(c.getPackageName(),R.layout.usage_widget);
        Intent refresh=new Intent(c,UsageWidget.class).setAction(REFRESH).addFlags(Intent.FLAG_RECEIVER_FOREGROUND);
        PendingIntent refreshIntent=PendingIntent.getBroadcast(c,1,refresh,PendingIntent.FLAG_IMMUTABLE|PendingIntent.FLAG_UPDATE_CURRENT);
        PendingIntent open=PendingIntent.getActivity(c,2,new Intent(c,MainActivity.class),PendingIntent.FLAG_IMMUTABLE|PendingIntent.FLAG_UPDATE_CURRENT);
        views.setOnClickPendingIntent(R.id.root,refreshIntent);
        try {
            JSONArray accounts=Store.read(c);
            // Match the desktop strip: work on the left, personal on the right.
            if(accounts.length()==2&&accounts.getJSONObject(0).optString("kind").equals("personal")&&
                !accounts.getJSONObject(1).optString("kind").equals("personal")) {
                JSONObject first=accounts.getJSONObject(0);
                accounts.put(0,accounts.getJSONObject(1));accounts.put(1,first);
            }
            views.setViewVisibility(R.id.account2,accounts.length()>1?View.VISIBLE:View.GONE);
            views.setViewVisibility(R.id.divider,accounts.length()>1?View.VISIBLE:View.GONE);
            int[] icons={R.id.icon1,R.id.icon2},avatars={R.id.avatar1,R.id.avatar2},percent={R.id.percent1,R.id.percent2},reset={R.id.reset1,R.id.reset2};
            for(int i=0;i<Math.max(1,accounts.length());i++) {
                views.setOnClickPendingIntent(icons[i],open);views.setOnClickPendingIntent(avatars[i],open);
                if(accounts.length()==0){views.setOnClickPendingIntent(R.id.root,open);views.setTextViewText(percent[i],"—\n—");continue;}
                JSONObject a=accounts.getJSONObject(i),snapshot=a.optJSONObject("snapshot");
                boolean work=!a.optString("kind").equals("personal");
                views.setViewVisibility(icons[i],work?View.VISIBLE:View.GONE);views.setViewVisibility(avatars[i],work?View.GONE:View.VISIBLE);
                String email=a.optString("email","Codex"); views.setTextViewText(avatars[i],"P");
                views.setTextViewText(percent[i],UsageData.percent(snapshot,"short")+"\n"+UsageData.percent(snapshot,"week"));
                views.setTextViewText(reset[i],snapshot==null?"Přihlásit":UsageData.reset(snapshot,"short",ZoneId.systemDefault())+"\n"+UsageData.reset(snapshot,"week",ZoneId.systemDefault()));
                boolean stale=UsageData.stale(snapshot,System.currentTimeMillis()/1000)||a.has("error");
                views.setTextColor(percent[i],Color.parseColor(stale?"#938F78":"#B8CEAA"));
                views.setContentDescription(percent[i],email+": "+(stale?"starší nebo nedostupné údaje, ":"")+"zbývá v pětihodinovém a týdenním limitu "+UsageData.percent(snapshot,"short")+", "+UsageData.percent(snapshot,"week"));
            }
        } catch(Exception error){views.setTextViewText(R.id.percent1,"—\n—");views.setTextViewText(R.id.reset1,"Chyba");views.setOnClickPendingIntent(R.id.root,open);}
        manager.updateAppWidget(ids,views);
    }
}
