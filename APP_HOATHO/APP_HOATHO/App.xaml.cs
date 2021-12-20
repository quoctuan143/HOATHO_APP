using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using APP_HOATHO.Services;
using APP_HOATHO.Views;
using APP_HOATHO.Global;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;
using APP_HOATHO.Dialog;
using APP_HOATHO.Interface;

namespace APP_HOATHO
{
    public partial class App : Application
    {
        static bool IsInForeground { get; set; }
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA4MTIyQDMxMzgyZTMyMmUzME4rVWJvRGdVY0ZibWlYbUFBN1dyNVFjemJ5djZ5dWQzZzdMaDNEQ1hBN3M9");
            DependencyService.Register<MockDataStore>();
            new Config();
            Device.SetFlags(new string[] { "Brush_Experimental" });
            MainPage = new SplashPage();
           
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                string token =  CrossFirebasePushNotification.Current.Token;
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            };
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {               
               
                if (IsInForeground == true )
                {
                    new MessageBox(p.Data["body"].ToString()).Show();
                }    
              
              

            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }


            };
            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }

                }

            };         
        }

        protected override void OnStart()
        {
            IsInForeground = true;
        }

        protected override void OnSleep()
        {
            IsInForeground = false ;
        }

        protected override void OnResume()
        {
            IsInForeground = true;
        }
    }
}
