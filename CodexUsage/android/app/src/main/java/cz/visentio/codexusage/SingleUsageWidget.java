package cz.visentio.codexusage;

import android.appwidget.*;
import android.content.*;

public class SingleUsageWidget extends UsageWidget {
    static final String PINNED="cz.visentio.codexusage.SINGLE_PINNED";
    @Override public void onReceive(Context c,Intent intent) {
        if(PINNED.equals(intent.getAction())) {
            int id=intent.getIntExtra(AppWidgetManager.EXTRA_APPWIDGET_ID,AppWidgetManager.INVALID_APPWIDGET_ID);
            String account=intent.getStringExtra("accountId");
            AppWidgetProviderInfo info=AppWidgetManager.getInstance(c).getAppWidgetInfo(id);
            if(account!=null&&info!=null&&info.provider.equals(new ComponentName(c,SingleUsageWidget.class))) {
                c.getSharedPreferences("widgets",0).edit().putString("account_"+id,account).apply();
                render(c);UsageJob.schedule(c);UsageJob.now(c);
            }
        } else super.onReceive(c,intent);
    }
}
