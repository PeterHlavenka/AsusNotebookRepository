using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SmsReader.Android
{
    [Activity(Label = "SmsReader", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity instance { set; get; }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            CreateNotificationChannel();
            instance = this;
            
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        
        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = "channelName";
            var channelDescription = "description";
            var channel = new NotificationChannel("CHANNEL_ID", channelName, NotificationImportance.Max)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager) GetSystemService(NotificationService);
            notificationManager?.CreateNotificationChannel(channel);
        }
    }
}