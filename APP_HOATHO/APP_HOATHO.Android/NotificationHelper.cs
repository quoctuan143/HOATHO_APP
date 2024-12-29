using Android.App;
using Android.Content;
using Android.OS;

namespace APP_HOATHO.Droid
{
    public static class NotificationHelper
    {
        public static void CreateNotificationChannel(Context context)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                string channelId = "default_channel";
                string channelName = "Default Channel";
                string channelDescription = "General notifications";
                var importance = NotificationImportance.Default;

                var channel = new NotificationChannel(channelId, channelName, importance)
                {
                    Description = channelDescription
                };

                var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
    }
}