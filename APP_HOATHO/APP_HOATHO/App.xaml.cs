using APP_HOATHO.Global;
using APP_HOATHO.Services;
using Xamarin.Forms;

namespace APP_HOATHO
{
    public partial class App : Application
    {
        public static bool IsInForeground { get; set; }

        public App(bool isNotification, object dataNotification = null)
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA4MTIyQDMxMzgyZTMyMmUzME4rVWJvRGdVY0ZibWlYbUFBN1dyNVFjemJ5djZ5dWQzZzdMaDNEQ1hBN3M9");
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTE4NTQ2NUAzMjMwMmUzNDJlMzBhaEV4Y3J2TTZyZ2FRdTBkZ1k4VUtPVzUwbHp1Y0pUeWJ0cW56cFJpM1JFPQ==");
            DependencyService.Register<MockDataStore>();
            new Config();
            Device.SetFlags(new string[] { "CollectionView_Experimental", "Brush_Experimental", "SwipeView_Experimental", "CarouseView_Experimental", "IndicatorView_Experimental" });
            if (isNotification == false)
                MainPage = new SplashPage();
        }

        protected override void OnStart()
        {
            IsInForeground = true;
        }

        protected override void OnSleep()
        {
            IsInForeground = false;
        }

        protected override void OnResume()
        {
            IsInForeground = true;
        }
    }
}