using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APP_HOATHO.Droid
{
    public class NotificationSender
    {
        public static void ShowNotification(Context context, string title, string message)
        {
            string channelId = "default_channel"; // Trùng với channel đã tạo.            
            var notificationBuilder = new NotificationCompat.Builder(context, channelId)
                .SetSmallIcon(Resource.Drawable.logo) // Icon nhỏ.
                .SetContentTitle(title)
                .SetContentText(message)
                .SetPriority((int)NotificationPriority.Default)
                .SetAutoCancel(true);

            // Intent mở ứng dụng khi nhấn vào thông báo.
            var intent = new Intent(context, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.Immutable);
            notificationBuilder.SetContentIntent(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(1001, notificationBuilder.Build()); // 1001 là ID thông báo.
        }
    }
}