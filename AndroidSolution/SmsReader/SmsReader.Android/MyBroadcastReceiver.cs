#region

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Telephony;
using Android.Widget;
using AndroidX.Core.App;
using Java.Lang;

#endregion

namespace SmsReader.Android
{
    [BroadcastReceiver]
    [IntentFilter(new[] {"android.provider.Telephony.SMS_RECEIVED"}, Priority = (int) IntentFilterPriority.HighPriority)]
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
                        wl?.Acquire(3000); //set your time in milliseconds
                    }
                }
                
    
                                        // NOTIFIKACE
                                        
                // Instantiate the builder and set notification elements:
                NotificationCompat.Builder builder = new NotificationCompat.Builder(MainActivity.instance, "CHANNEL_ID")
                    .SetContentTitle ("Sample Notification")
                    .SetContentText ("Hello World! This is my first notification!")
                    .SetDefaults((int) NotificationDefaults.Sound)  // zapnuti zvuku notifikace
                    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Alarm))  // zvuk notifikace
                    .SetSmallIcon (Resource.Drawable.Planet);

              
                
                // Build the notification:
                Notification notification = builder.Build();

                // Get the notification manager:
                NotificationManager notificationManager = MainActivity.instance.GetSystemService (Context.NotificationService) as NotificationManager;

                // Publish the notification:
                const int notificationId = 0;
                notificationManager?.Notify (notificationId, notification);
                
                // var playService = new PlaySoundService();
                // playService.PlaySystemSound();
                
                Toast.MakeText(context, m_message, ToastLength.Long)?.Show();
            }
        }
    }
}