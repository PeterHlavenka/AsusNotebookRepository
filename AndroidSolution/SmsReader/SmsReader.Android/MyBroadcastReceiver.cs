#region

using Android.App;
using Android.Content;
using Android.Telephony;
using Android.Widget;
using Java.Lang;

#endregion

namespace SmsReader.Android
{
    [BroadcastReceiver]
    [IntentFilter(new[] {"android.provider.Telephony.SMS_RECEIVED"}, Priority = (int) IntentFilterPriority.HighPriority)]
    public class MyBroadcastReceiver : BroadcastReceiver
    {
        private string m_message;
        private string m_address = "";
        private const string Name = "pdus";

        public override void OnReceive(Context context, Intent intent)
        {
            if (!intent.HasExtra(Name) || intent.Extras == null) return;
            
            var smsArray = (Object[]) intent.Extras.Get(Name);

            if (smsArray == null) return;
            
            foreach (var item in smsArray)
            {
                var sms = SmsMessage.CreateFromPdu((byte[]) item, null);

                if (sms != null)
                {
                    // m_address = sms.OriginatingAddress;
                    m_message = sms.MessageBody;
                }

                Toast.MakeText(context, m_message, ToastLength.Long)?.Show();
            }
        }
    }
}