using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Media;
using Plugin.FirebasePushNotification;
using System.Collections.Generic;
using Android.Content;
using Plugin.LocalNotification;
using FFImageLoading.Forms.Platform;
using Android;
using System.Linq;
using AndroidX.Core.Content;
using AndroidX.Core.App;

namespace APP_HOATHO.Droid
{
    [Activity(Label = "HOATHO", Icon = "@mipmap/logo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private bool IsNotification = false;
        private object NotificationData;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            NotificationCenter.CreateNotificationChannel();
            await CrossMedia.Current.Initialize();
            Rg.Plugins.Popup.Popup.Init(this);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CachedImageRenderer.Init(true);
            NotificationCenter.NotifyNotificationTapped(Intent );
            FirebasePushNotificationManager.ProcessIntent(this, Intent);   
         
#if DEBUG
            FirebasePushNotificationManager.Initialize(this,
                          new NotificationUserCategory[]
                          {
                    new NotificationUserCategory("message",new List<NotificationUserAction> {
                        new NotificationUserAction("Reply","Reply", NotificationActionType.Foreground),
                        new NotificationUserAction("Forward","Forward", NotificationActionType.Foreground)

                    }),
                    new NotificationUserCategory("request",new List<NotificationUserAction> {
                    new NotificationUserAction("Accept","Accept", NotificationActionType.Default, "check"),
                    new NotificationUserAction("Reject","Reject", NotificationActionType.Default, "cancel")
                    })
                          }, true);
#else
  FirebasePushNotificationManager.Initialize(this,
                new NotificationUserCategory[]
                {
                    new NotificationUserCategory("message",new List<NotificationUserAction> {
                        new NotificationUserAction("Reply","Reply", NotificationActionType.Foreground),
                        new NotificationUserAction("Forward","Forward", NotificationActionType.Foreground)

                    }),
                    new NotificationUserCategory("request",new List<NotificationUserAction> {
                    new NotificationUserAction("Accept","Accept", NotificationActionType.Default, "check"),
                    new NotificationUserAction("Reject","Reject", NotificationActionType.Default, "cancel")
                    })
                }, false);
#endif
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("NOTIFICATION RECEIVED", p.Data);
                NotificationData = p.Data;
                IsNotification = false;                
            };
            LoadApplication(new App(IsNotification, NotificationData));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (Xamarin.Essentials.DeviceInfo.Version.Major >= 13 && (permissions.Where(p => p.Equals("android.permission.WRITE_EXTERNAL_STORAGE")).Any() || permissions.Where(p => p.Equals("android.permission.READ_EXTERNAL_STORAGE")).Any()))
            {
                var wIdx = Array.IndexOf(permissions, "android.permission.WRITE_EXTERNAL_STORAGE");
                var rIdx = Array.IndexOf(permissions, "android.permission.READ_EXTERNAL_STORAGE");

                if (wIdx != -1 && wIdx < permissions.Length) grantResults[wIdx] = Permission.Granted;
                if (rIdx != -1 && rIdx < permissions.Length) grantResults[rIdx] = Permission.Granted;
            }
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.PostNotifications) != Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.PostNotifications }, 101);
                }
            }
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnNewIntent(Intent intent)
        {
            NotificationCenter.NotifyNotificationTapped(intent);
            base.OnNewIntent(intent);
        }
        protected override void OnStart()
        {
            base.OnStart();
            const int requestLocationId = 0;

            string[] notiPermission =
            {
                Manifest.Permission.PostNotifications
            };

            if ((int)Build.VERSION.SdkInt < 33) return;

            if (this.CheckSelfPermission(Manifest.Permission.PostNotifications) != Permission.Granted)
            {
                this.RequestPermissions(notiPermission, requestLocationId);
            }
        }

    }
}