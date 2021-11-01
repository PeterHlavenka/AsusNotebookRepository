using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace DroidMatika.Android
{
    [Activity(Label = "DroidMatika", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCenter.Start("c4fb34f4-4d8f-4720-93d3-4c05956464dd",
                typeof(Analytics), typeof(Crashes));
            
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            
            HideNavigation();
            
            LoadApplication(new App());
        }
        
        private void HideNavigation()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                Window?.SetDecorFitsSystemWindows(false);
            }
            else
            {
                var uiOptions = (int) Window?.DecorView.SystemUiVisibility;

                uiOptions |= (int) SystemUiFlags.LowProfile;
                uiOptions |= (int) SystemUiFlags.Fullscreen;
                uiOptions |= (int) SystemUiFlags.HideNavigation;
                uiOptions |= (int) SystemUiFlags.ImmersiveSticky;

                Window.DecorView.SystemUiVisibility = (StatusBarVisibility) uiOptions;
            }
        }
    }
}