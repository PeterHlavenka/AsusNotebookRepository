package cz.visentio.codexusage;

import android.app.Activity;
import android.appwidget.*;
import android.content.*;
import android.graphics.Color;
import android.os.*;
import android.widget.*;
import org.json.*;

public class WidgetConfigActivity extends Activity {
    private int widgetId=AppWidgetManager.INVALID_APPWIDGET_ID;
    @Override public void onCreate(Bundle state) {
        super.onCreate(state);setResult(RESULT_CANCELED);
        widgetId=getIntent().getIntExtra(AppWidgetManager.EXTRA_APPWIDGET_ID,AppWidgetManager.INVALID_APPWIDGET_ID);
        AppWidgetProviderInfo info=AppWidgetManager.getInstance(this).getAppWidgetInfo(widgetId);
        if(info==null||!info.provider.equals(new ComponentName(this,SingleUsageWidget.class)))finish();
    }
    @Override public void onResume() {
        super.onResume();if(isFinishing())return;
        LinearLayout content=new LinearLayout(this);content.setOrientation(LinearLayout.VERTICAL);
        int padding=(int)(24*getResources().getDisplayMetrics().density);content.setPadding(padding,padding,padding,padding);
        content.setBackgroundColor(Color.rgb(24,28,25));
        ScrollView scroll=new ScrollView(this);scroll.setFillViewport(true);scroll.addView(content);setContentView(scroll);
        if(Build.VERSION.SDK_INT>=35)scroll.setOnApplyWindowInsetsListener((v,insets)->{
            android.graphics.Insets bars=insets.getInsets(android.view.WindowInsets.Type.systemBars());
            v.setPadding(bars.left,bars.top,bars.right,bars.bottom);return insets;
        });
        TextView title=new TextView(this);title.setText("Vyber účet pro widget 2×1");title.setTextColor(Color.parseColor("#B8CEAA"));title.setTextSize(22);content.addView(title);
        try {
            JSONArray accounts=Store.read(this);
            for(int i=0;i<accounts.length();i++) {
                JSONObject account=accounts.getJSONObject(i);String id=account.getString("id");
                Button button=new Button(this);button.setText((account.optString("kind").equals("personal")?"Osobní":"Visentio")+" · "+account.optString("email"));
                button.setOnClickListener(v->{
                    getSharedPreferences("widgets",0).edit().putString("account_"+widgetId,id).apply();
                    UsageWidget.render(this);UsageJob.schedule(this);UsageJob.now(this);
                    setResult(RESULT_OK,new Intent().putExtra(AppWidgetManager.EXTRA_APPWIDGET_ID,widgetId));finish();
                });content.addView(button);
            }
            if(accounts.length()==0){TextView help=new TextView(this);help.setText("Nejdřív přihlas účet v aplikaci a pak se sem vrať.");content.addView(help);}
        } catch(Exception e){TextView help=new TextView(this);help.setText("Uložené přihlášení není dostupné. Otevři aplikaci.");content.addView(help);}
        Button open=new Button(this);open.setText("Spravovat účty");open.setOnClickListener(v->startActivity(new Intent(this,MainActivity.class)));content.addView(open);
    }
}
