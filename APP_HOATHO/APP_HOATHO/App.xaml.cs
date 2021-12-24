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

namespace APP_HOATHO
{
    public partial class App : Application
    {
        static bool IsInForeground { get; set; }
        public App(bool isNotification, object dataNotification = null ) 
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA4MTIyQDMxMzgyZTMyMmUzME4rVWJvRGdVY0ZibWlYbUFBN1dyNVFjemJ5djZ5dWQzZzdMaDNEQ1hBN3M9");
            DependencyService.Register<MockDataStore>();
            new Config();
            Device.SetFlags(new string[] { "CollectionView_Experimental", "Brush_Experimental", "SwipeView_Experimental", "CarouseView_Experimental", "IndicatorView_Experimental" });
            if (isNotification == false  )
            MainPage = new SplashPage();
            else
            {
                FirebasePushNotificationDataEventArgs data = dataNotification as FirebasePushNotificationDataEventArgs;
                if (data != null )
                {
                    if (data.Data ["sochungtu"].ToString() != "")
                    {
                        DuyetChungTuModel item = new DuyetChungTuModel { No_ = data.Data["sochungtu"].ToString() };
                        if (data.Data ["loaiphieu"].ToString() == "lcpfob")
                        {
                            MainPage.Navigation.PushAsync(new DuyetChungTu_Line(item, DocumentType.DuyetLCP));
                        }
                        if (data.Data["loaiphieu"].ToString() == "lcpgc")
                        {
                            MainPage.Navigation.PushAsync(new DuyetChungTu_Line(item, DocumentType.DuyetLCP_GC));
                        }
                        if (data.Data["loaiphieu"].ToString() == "duyetdatphutung")
                        {
                            MainPage.Navigation.PushAsync(new DuyetChungTuPhuTung_Line(item, DocumentType.DuyetDatMuaPhuTung));
                        }
                        if (data.Data["loaiphieu"].ToString() == "dondatmua")
                        {
                            MainPage.Navigation.PushAsync(new DuyetChungTu_Line(item, DocumentType.DuyetMuaHang));
                        }
                        if (data.Data["loaiphieu"].ToString() == "denghithanhtoan")
                        {
                            MainPage.Navigation.PushAsync(new DuyetChungTu_Line(item, DocumentType.DuyetThanhToan));
                        }
                    }    
                }    
            }    
           
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                string token =  CrossFirebasePushNotification.Current.Token;
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            };
            CrossFirebasePushNotification.Current.OnNotificationReceived += async (s, p) =>
            {               
               
                if (IsInForeground == true )
                {
                    DocumentType loaiphieu = DocumentType.DuyetDatMuaPhuTung;
                    if (p.Data["sochungtu"].ToString() != "")
                    {
                        DuyetChungTuModel item = new DuyetChungTuModel { No_ = p.Data["sochungtu"].ToString() };
                        if (p.Data["loaiphieu"].ToString() == "lcpfob")
                        {
                            loaiphieu = DocumentType.DuyetLCP;
                        }
                        if (p.Data["loaiphieu"].ToString() == "lcpgc")
                        {
                            loaiphieu= DocumentType.DuyetLCP_GC;
                        }
                        if (p.Data["loaiphieu"].ToString() == "duyetdatphutung")
                        {
                           loaiphieu= DocumentType.DuyetDatMuaPhuTung;
                        }
                        if (p.Data["loaiphieu"].ToString() == "dondatmua")
                        {
                            loaiphieu = DocumentType.DuyetMuaHang;
                        }
                        if (p.Data["loaiphieu"].ToString() == "denghithanhtoan")
                        {
                            loaiphieu = DocumentType.DuyetThanhToan;
                        }
                      await  new MessageOpenDocument(p.Data["body"].ToString(),item.No_, loaiphieu).Show();
                    }    
                    else
                    await  new MessageBox(p.Data["body"].ToString()).Show();
                    
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
            CrossFirebasePushNotification.Current.OnNotificationAction += async (s, p) =>
            {
                DocumentType loaiphieu = DocumentType.DuyetDatMuaPhuTung;
                if (p.Data["sochungtu"].ToString() != "")
                {
                    DuyetChungTuModel item = new DuyetChungTuModel { No_ = p.Data["sochungtu"].ToString() };
                    if (p.Data["loaiphieu"].ToString() == "lcpfob")
                    {
                        loaiphieu = DocumentType.DuyetLCP;
                    }
                    if (p.Data["loaiphieu"].ToString() == "lcpgc")
                    {
                        loaiphieu = DocumentType.DuyetLCP_GC;
                    }
                    if (p.Data["loaiphieu"].ToString() == "duyetdatphutung")
                    {
                        loaiphieu = DocumentType.DuyetDatMuaPhuTung;
                    }
                    if (p.Data["loaiphieu"].ToString() == "dondatmua")
                    {
                        loaiphieu = DocumentType.DuyetMuaHang;
                    }
                    if (p.Data["loaiphieu"].ToString() == "denghithanhtoan")
                    {
                        loaiphieu = DocumentType.DuyetThanhToan;
                    }
                    await new MessageOpenDocument(p.Data["body"].ToString(), item.No_, loaiphieu).Show();
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
