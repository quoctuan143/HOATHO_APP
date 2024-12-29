using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Platform;
using Foundation;
using Plugin.FirebasePushNotification;
using Plugin.Media;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfNumericTextBox.XForms.iOS;
using Syncfusion.SfPicker.XForms.iOS;
using Syncfusion.SfPullToRefresh.XForms.iOS;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.EffectsView;
using Syncfusion.XForms.iOS.TextInputLayout;
using UIKit;
using UserNotifications;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace APP_HOATHO.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        private bool IsNotification = false;
        private object NotificationData;
        public override  bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags(new string[] { "CollectionView_Experimental", "Brush_Experimental", "SwipeView_Experimental", "CarouseView_Experimental", "IndicatorView_Experimental" });
            global::Xamarin.Forms.Forms.Init();
            SfPickerRenderer.Init();
            Syncfusion.SfDataGrid.XForms.iOS.SfDataGridRenderer.Init();
            new Syncfusion.XForms.iOS.ComboBox.SfComboBoxRenderer();            
            Syncfusion.XForms.iOS.Buttons.SfSwitchRenderer.Init();
            Syncfusion.XForms.iOS.BadgeView.SfBadgeViewRenderer.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            SfTextInputLayoutRenderer.Init();
            SfPullToRefreshRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();
            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            Syncfusion.XForms.iOS.PopupLayout.SfPopupLayoutRenderer.Init();
            new SfNumericTextBoxRenderer();
            SfCheckBoxRenderer.Init();
            SfListViewRenderer.Init();
            SfEffectsViewRenderer.Init();
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("NOTIFICATION RECEIVED", p.Data);
                NotificationData = p.Data;
                IsNotification = false;
            };
            LoadApplication(new App(IsNotification, NotificationData));
            Syncfusion.XForms.iOS.Border.SfBorderRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfButtonRenderer.Init();
            CachedImageRenderer.Init();
            FirebasePushNotificationManager.Initialize(options, true);
            FirebasePushNotificationManager.Initialize(options, new NotificationUserCategory[]
                        {
                           new NotificationUserCategory("message",new List<NotificationUserAction> {
                               new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground)
                           }),
                           new NotificationUserCategory("request",new List<NotificationUserAction> {
                               new NotificationUserAction("Accept","Accept"),
                               new NotificationUserAction("Reject","Reject",NotificationActionType.Destructive)
                           })  });
            FirebasePushNotificationManager.CurrentNotificationPresentationOption = UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge;
            UIColor color = Color.FromHex("06264c").ToUIColor();
            UINavigationBar.Appearance.BackgroundColor = color;
            UINavigationBar.Appearance.BarTintColor = color;
            UITabBar.Appearance.BackgroundColor  = color;
            UITabBar.Appearance.BarTintColor = color;
            // Gán CustomNotificationDelegate
            UNUserNotificationCenter.Current.Delegate = new CustomNotificationDelegate();

            // Yêu cầu quyền thông báo
            UNUserNotificationCenter.Current.RequestAuthorization(
                UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge,
                (granted, error) =>
                {
                    if (granted)
                    {
                        Console.WriteLine("Notification permission granted.");
                    }
                    else
                    {
                        Console.WriteLine("Notification permission denied.");
                    }
                });
            return base.FinishedLaunching(app, options);
           
        }
       
        public override void WillEnterForeground(UIApplication uiApplication)
        {
           
        }
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);

        }
        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.

            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);
            if (userInfo != null)
            {
                string title = string.Empty;
                string message = string.Empty;

                // Trích xuất title và message từ payload
                if (userInfo.ContainsKey(new NSString("title")))
                {
                    title = userInfo["title"].ToString();
                }
                if (userInfo.ContainsKey(new NSString("body")) || userInfo.ContainsKey(new NSString("message")))
                {
                    message = userInfo.ContainsKey(new NSString("body")) ? userInfo["body"].ToString() : userInfo["message"].ToString();
                }

                // Hiển thị thông báo
                ShowNotification(title, message);
            }

            // Hoàn thành xử lý
            completionHandler(UIBackgroundFetchResult.NewData);           
        }

        public void ShowNotification(string title, string body)
        {
            // Tạo nội dung thông báo
            var content = new UNMutableNotificationContent
            {
                Title = title,
                Body = body,
                Sound = UNNotificationSound.Default
            };

            // Thiết lập thời gian kích hoạt thông báo (sau 5 giây)
            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(5, false);

            // Tạo Request với một ID duy nhất
            var request = UNNotificationRequest.FromIdentifier(Guid.NewGuid().ToString(), content, trigger);

            // Thêm thông báo vào hệ thống
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (error) =>
            {
                if (error != null)
                {
                    Console.WriteLine($"Error: {error.LocalizedDescription}");
                }
            });
        }
    }

    public class CustomNotificationDelegate : UNUserNotificationCenterDelegate
    {
        // Xử lý thông báo khi ứng dụng đang ở chế độ foreground
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Hiển thị thông báo ngay cả khi ứng dụng đang mở
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
        }        
    }
}
