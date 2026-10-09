package cz.visentio.codexusage;

import android.app.*;
import android.content.*;

final class Notifications {
    static Notification make(Context c,String title,String text) {
        NotificationManager manager=c.getSystemService(NotificationManager.class);
        manager.createNotificationChannel(new NotificationChannel("usage","Codex Usage",NotificationManager.IMPORTANCE_LOW));
        PendingIntent open=PendingIntent.getActivity(c,0,new Intent(c,MainActivity.class),PendingIntent.FLAG_IMMUTABLE|PendingIntent.FLAG_UPDATE_CURRENT);
        return new Notification.Builder(c,"usage").setSmallIcon(R.drawable.visentio).setContentTitle(title)
            .setContentText(text).setContentIntent(open).setOngoing(true).build();
    }
}
