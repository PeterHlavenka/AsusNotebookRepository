package cz.visentio.codexusage;

import android.app.job.*;
import android.content.*;

public class UsageJob extends JobService {
    static void schedule(Context c){c.getSystemService(JobScheduler.class).schedule(new JobInfo.Builder(41,new ComponentName(c,UsageJob.class)).setRequiredNetworkType(JobInfo.NETWORK_TYPE_ANY).setPeriodic(15*60*1000L).setPersisted(true).build());}
    static void now(Context c){c.getSystemService(JobScheduler.class).schedule(new JobInfo.Builder(42,new ComponentName(c,UsageJob.class)).setRequiredNetworkType(JobInfo.NETWORK_TYPE_ANY).setOverrideDeadline(0).build());}
    static void cancel(Context c){c.getSystemService(JobScheduler.class).cancel(41);c.getSystemService(JobScheduler.class).cancel(42);}
    @Override public boolean onStartJob(JobParameters params){Api.WORK.execute(()->{Api.refresh(this);jobFinished(params,false);});return true;}
    @Override public boolean onStopJob(JobParameters params){return true;}
}
