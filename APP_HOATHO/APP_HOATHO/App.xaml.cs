using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using APP_HOATHO.Services;
using APP_HOATHO.Views;
using APP_HOATHO.Global;

namespace APP_HOATHO
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA4MTIyQDMxMzgyZTMyMmUzME4rVWJvRGdVY0ZibWlYbUFBN1dyNVFjemJ5djZ5dWQzZzdMaDNEQ1hBN3M9");
            DependencyService.Register<MockDataStore>();
            new Config();
            MainPage = new SplashPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
