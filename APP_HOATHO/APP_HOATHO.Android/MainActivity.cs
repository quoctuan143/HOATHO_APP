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