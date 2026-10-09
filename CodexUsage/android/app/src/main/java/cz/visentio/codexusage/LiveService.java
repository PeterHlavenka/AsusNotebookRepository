package cz.visentio.codexusage;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;
import java.util.concurrent.*;

public class LiveService extends Service {
    static volatile boolean running;
    private ScheduledExecutorService timer;
    @Override public IBinder onBind(Intent intent){return null;}
    @Override public int onStartCommand(Intent intent,int flags,int id){
        startForeground(22,Notifications.make(this,"Codex Usage","Obnova každou minutu · vypnout v aplikaci"));
        running=true;
        if(timer==null){timer=Executors.newSingleThreadScheduledExecutor();timer.scheduleWithFixedDelay(()->Api.refresh(this),0,60,TimeUnit.SECONDS);}
        return START_NOT_STICKY;
    }
    @Override public void onDestroy(){running=false;if(timer!=null)timer.shutdownNow();Store.live(this,false);super.onDestroy();}
    @Override public void onTimeout(int startId,int foregroundServiceType){
        running=false;if(timer!=null)timer.shutdownNow();Store.live(this,false);
        getSharedPreferences("settings",0).edit().putString("refreshNotice","Android ukončil minutovou obnovu kvůli časovému limitu. Běžná obnova každých 15 minut pokračuje.").apply();
        UsageJob.schedule(this);stopSelf();
    }
}
