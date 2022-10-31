using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.LocalNotification;
using Xamarin.Forms;

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

            NotificationCenter.CreateNotificationChannel();  
            
            CreateNotificationChannel();
            instance = this;
            
            Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            NotificationCenter.NotifyNotificationTapped(Intent);
        }

        protected override void OnNewIntent(Intent intent)
        {
            NotificationCenter.NotifyNotificationTapped(intent);
            base.OnNewIntent(intent);
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

        // Function to check and request permission
        // public void checkPermission(String permission, int requestCode)
        // {
        //     // Checking if permission is not granted
        //     if (ContextCompat.checkSelfPermission(MainActivity.this, permission) == PackageManager.PERMISSION_DENIED) {
        //         ActivityCompat.requestPermissions(MainActivity.this, new String[] { permission }, requestCode);
        //     }
        //     else {
        //         Toast.makeText(MainActivity.this, "Permission already granted", Toast.LENGTH_SHORT).show();
        //     }
        // }
    }
}