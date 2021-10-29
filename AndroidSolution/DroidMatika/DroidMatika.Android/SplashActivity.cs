using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using AndroidX.AppCompat.App;

namespace DroidMatika.Android
{
    [Activity(Theme = "@style/Theme.Splash",  MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        private static readonly string m_tag = "X:" + typeof(SplashActivity).Name;
        private bool m_oneTime; // static to musi byt protoze se pri kazdem prekliknuti mezi aplikacemi vytvari nova instance activity 

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);

            Log.Debug(m_tag, "SplashActivity.OnCreate");
        }

        // Launches the startup task
        protected override async void OnResume()
        {
            base.OnResume();

            if (m_oneTime)
                return;

            m_oneTime = true;
            
            // todo nejaka animace aby se clovek nenudil
            // todo boostrapper - pusteni installeru 

            // var bootstrapper = new Bootstrapper();
            //
            // await Task.Run(() => bootstrapper.Start());
            
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}