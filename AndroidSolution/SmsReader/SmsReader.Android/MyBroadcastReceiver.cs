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
                    
                    Toast.MakeText(context, "Number" + address + "Message:"+ message,ToastLength.Short).Show();
                }
            }
           // Toast.MakeText(context, "Received intent!", ToastLength.Short)?.Show();
        }
    }
}