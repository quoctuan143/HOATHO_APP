using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using APP_HOATHO.Droid.Renderer;
using APP_HOATHO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(BeepService))]
namespace APP_HOATHO.Droid.Renderer
{
    public class BeepService : IBeepService
    {
        public void Beep()
        {
            // Sử dụng AudioManager để phát ra âm thanh "beep"
            var context = Android.App.Application.Context;
            var defaultNotificationUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var mediaPlayer = MediaPlayer.Create(context, defaultNotificationUri);
            mediaPlayer.Start();
        }
    }
}