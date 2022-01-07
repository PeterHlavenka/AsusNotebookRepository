#region

using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Telephony;
using AndroidX.Core.App;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using NotificationPriority = Android.App.NotificationPriority;
using Object = Java.Lang.Object;

#endregion

namespace SmsReader.Android
{
    [BroadcastReceiver]
  //  [IntentFilter(new[] {"android.provider.Telephony.SMS_RECEIVED"}, Priority = (int) IntentFilterPriority.HighPriority)]
    public class MyBroadcastReceiver : BroadcastReceiver
    {
        private string m_message = string.Empty;
        private string m_address = string.Empty;
        private const string Name = "pdus";

        public override void OnReceive(Context context, Intent intent)
        {
            if (!intent.HasExtra(Name) || intent.Extras == null) return;
            
            var smsArray = (Object[]) intent.Extras.Get(Name);

            if (smsArray == null) return;
            
            foreach (var item in smsArray)
            {
                var sms = SmsMessage.CreateFromPdu((byte[]) item);

                if (sms == null) continue;
                
                // m_address = sms.OriginatingAddress;
                m_message = sms.MessageBody;

                if (m_message == null || !m_message.Contains("critical:")) continue;
                
                

                                        // PROBUZENI
            
                PowerManager pm = (PowerManager)MainActivity.instance.GetSystemService(Context.PowerService);
                bool isScreenOn =  pm is {IsInteractive: true} ; // check if screen is on
                
                if (!isScreenOn)
                {
                    if (pm != null)
                    {
                        
                        PowerManager.WakeLock wl = pm.NewWakeLock(WakeLockFlags.ScreenDim  | WakeLockFlags.AcquireCausesWakeup, "myApp:notificationLock");
                        wl?.Acquire(0); //set your time in milliseconds
                    }
                }
                
                                        // NOTIFIKACE PRES PLUGIN
                var notification = new NotificationRequest()
                {
                    BadgeNumber = 1,
                    Description = "TestDescription",
                    Title = "Notification",
                    ReturningData = "Dummy data",
                    NotificationId = 1234 , // you can do some actions via Id for example cancell notification
                    Schedule = new NotificationRequestSchedule(){NotifyTime = DateTime.Now.AddSeconds(3)},
                    Sound = RingtoneManager.GetDefaultUri(RingtoneType.Alarm)?.ToString(),
                    Android = new AndroidOptions()
                    {
                        Priority = Plugin.LocalNotification.NotificationPriority.Max,
                        ChannelId = "CHANNEL_ID",
                    
                    }
                };

                NotificationCenter.Current.Show(notification);
                
    
                                        // NOTIFIKACE
                                        
                // Instantiate the builder and set notification elements:
                // NotificationCompat.Builder builder = new NotificationCompat.Builder(MainActivity.instance, "CHANNEL_ID")
                //     .SetContentTitle ("Sample Notification")
                //     .SetContentText (m_message)
                //     .SetDefaults((int) NotificationDefaults.Sound)  // zapnuti zvuku notifikace
                //     .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Alarm))  // zvuk notifikace
                //     .SetSmallIcon (Resource.Drawable.Planet)
                //     .SetPriority((int) NotificationPriority.Max)  // HeadsUp format a ma se zobrazit in a lock screenu
                //     .SetVisibility((int) NotificationVisibility.Public);  // cela zprava je viditelna na lock screenu

              
                     //  Notifikace maka.. 
                // Build the notification:
                // Notification notification = builder.Build();

                

                // // Get the notification manager:
                // NotificationManager notificationManager = MainActivity.instance.GetSystemService (Context.NotificationService) as NotificationManager;
                //
                // // Publish the notification:
                // const int notificationId = 0; 
                // notificationManager?.Notify (notificationId, notification);


                // PlayServica maka jen kdyz je zapnuty device
                // var playService = new PlaySoundService();
                // playService.PlaySystemSound();

                //Toast.MakeText(context, m_message, ToastLength.Long)?.Show();

                // // 3
                // AlertDialog.Builder alertDialog = new AlertDialog.Builder(MainActivity.instance);
                // alertDialog.SetTitle("MyTitle");
                //
                // alertDialog.SetMessage(m_message);
                // alertDialog.SetNeutralButton("Ok", delegate
                // {
                //     alertDialog.Dispose();
                // });
                //
                // alertDialog.Show();

                // 4
                // AlarmManager alarmManager = (AlarmManager) MainActivity.instance.GetSystemService(Context.AlarmService);
                //
                // var alarmIntent = new Intent(MainActivity.instance, typeof(MyBroadcastReceiver));
                // alarmIntent.PutExtra("title", "Hello");
                // alarmIntent.PutExtra("message", "World!");
                //
                // var pending = PendingIntent.GetBroadcast(MainActivity.instance, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
                //
                //
                // alarmManager?.Set(AlarmType.RtcWakeup, SystemClock.ElapsedRealtime() + 2000, pending);

                // var intent = new Intent(this, typeof(MyBroadcastReceiver));

            }
        }
    }
}