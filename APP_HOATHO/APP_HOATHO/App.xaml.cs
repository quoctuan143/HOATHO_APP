using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using APP_HOATHO.Services;
using APP_HOATHO.Views;
using APP_HOATHO.Global;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;
using APP_HOATHO.Dialog;
using APP_HOATHO.Views.DuyetChungTu;
using APP_HOATHO.Models.DuyetChungTu;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace APP_HOATHO
{
    public partial class App : Application
    {
       public  static bool IsInForeground { get; set; }
        public App(bool isNotification, object dataNotification = null ) 
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA4MTIyQDMxMzgyZTMyMmUzME4rVWJvRGdVY0ZibWlYbUFBN1dyNVFjemJ5djZ5dWQzZzdMaDNEQ1hBN3M9");
            DependencyService.Register<MockDataStore>();
            new Config();
            Device.SetFlags(new string[] { "CollectionView_Experimental", "Brush_Experimental", "SwipeView_Experimental", "CarouseView_Experimental", "IndicatorView_Experimental" });
            if (isNotification == false  )
            MainPage = new SplashPage();            
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
