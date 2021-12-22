using Android.Media;
using SmsReader.Android;

[assembly:Xamarin.Forms.Dependency(typeof(PlaySoundService))]
namespace SmsReader.Android
{
    public interface IPlaySoundService 
    {
        void PlaySystemSound();
    }
    
    public class PlaySoundService : IPlaySoundService
    {
        public void PlaySystemSound()
        {
            var uri = RingtoneManager.GetDefaultUri(RingtoneType.Ringtone);
            
            Ringtone rt = RingtoneManager.GetRingtone(MainActivity.instance.ApplicationContext, uri);
            
            rt?.Play();
        }
    }
}