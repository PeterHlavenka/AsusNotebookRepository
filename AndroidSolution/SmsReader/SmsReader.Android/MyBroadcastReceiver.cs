// #region
//
// using Android.App;
// using Android.Content;
// using Android.Telephony;
// using Android.Widget;
// using Java.Lang;
//
// #endregion
//
// namespace SmsReader.Android
// {
//     [BroadcastReceiver]
//     [IntentFilter(new[] {"android.provider.Telephony.SMS_RECEIVED"}, Priority = (int) IntentFilterPriority.HighPriority)]
//     public class MyBroadcastReceiver : BroadcastReceiver
//     {
//         private string m_message = string.Empty;
//         private string m_address = string.Empty;
//         private const string Name = "pdus";
//
//         public override void OnReceive(Context context, Intent intent)
//         {
//             if (!intent.HasExtra(Name) || intent.Extras == null) return;
//             
//             var smsArray = (Object[]) intent.Extras.Get(Name);
//
//             if (smsArray == null) return;
//             
//             foreach (var item in smsArray)
//             {
//                 var sms = SmsMessage.CreateFromPdu((byte[]) item, null);
//
//                 if (sms == null) continue;
//                 
//                 // m_address = sms.OriginatingAddress;
//                 m_message = sms.MessageBody;
//
//                 // if (m_message != null && m_message.Contains("critical:"))
//                 // {
//                     Toast.MakeText(context, m_message, ToastLength.Long)?.Show();
//
//                     var playService = new PlaySoundService();
//                     playService.PlaySystemSound();
//                 // }
//             }
//         }
//     }
// }

using Android.App;
using Android.Content;
using Android.Telephony;
using Android.Widget;

namespace SmsReader.Android
{
    [BroadcastReceiver]
    [IntentFilter(new[]{"android.provider.Telephony.SMS_RECEIVED"}, Priority = (int)IntentFilterPriority.HighPriority)]
    public class MyBroadcastReceiver : BroadcastReceiver
    {
        public static readonly string INTENT_ACTION = "android.provider.Telephony.SMS_RECEIVED";
        protected string message, address = "";
        
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.HasExtra("pdus"))
            {
                var smsArray = (Java.Lang.Object[])intent.Extras.Get("pdus");

                foreach (var item in smsArray)
                {
                    var sms = SmsMessage.CreateFromPdu((byte[]) item);
                    address = sms.OriginatingAddress;
                    message = sms.MessageBody;
                    
                    Toast.MakeText(context, "Number" + address + "Message:"+ message,ToastLength.Short)?.Show();
                    
                    var playService = new PlaySoundService();
                    playService.PlaySystemSound();
                }
            }
        }
    }
}