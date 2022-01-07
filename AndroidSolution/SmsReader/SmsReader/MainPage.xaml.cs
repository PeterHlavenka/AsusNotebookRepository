using System;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using Xamarin.Forms;

namespace SmsReader
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
            //NotificationCenter.Current.NotificationTapped += NotificationTapped;

            NotificationCenter.Current.NotificationReceived += NotificationTapped;
        }

        private void NotificationTapped(NotificationEventArgs e)
        {
            // PROBUZENI

            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert(e.Request.Title, e.Request.Description, "Ok");
            });
        }

        private void AlarmButton_OnClicked(object sender, EventArgs e)
        {
            var notification = new NotificationRequest()
            {
                BadgeNumber = 1,
                Description = "TestDescription",
                Title = "Notification",
                ReturningData = "Dummy data",
                NotificationId = 1234 , // you can do some actions via Id for example cancell notification
                Schedule = new NotificationRequestSchedule(){NotifyTime = DateTime.Now.AddSeconds(3)},
                Android = new AndroidOptions()
                {
                    Priority = NotificationPriority.Max,
                    ChannelId = "CHANNEL_ID",
                    
                }
            };

            NotificationCenter.Current.Show(notification);

        }
    }
}