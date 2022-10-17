using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using APP_HOATHO.ViewModels.DuyetChungTu;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using APP_HOATHO.Models.DuyetChungTu;
using APP_HOATHO.Dialog;
using APP_HOATHO.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plugin.FirebasePushNotification;
using APP_HOATHO.Views.DuyetChungTu;
using APP_HOATHO.Models.KiDienTuPhuTung;
using APP_HOATHO.Views.KiDienTu;
using Plugin.LatestVersion;
using APP_HOATHO.Views.ThietBi;
using APP_HOATHO.Views.Kiem_Ke_Thiet_Bi;
using System.IO;
using APP_HOATHO.ViewModels;

namespace APP_HOATHO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main :  ContentPage, INotifyPropertyChanged 
    {

        MainViewModel viewModel;

        public Main()
        {            
            InitializeComponent();
            DependencyService.Get<ISetStatusBarColor>().SetColoredStatusBar("#06264c");
            viewModel = new MainViewModel();
            viewModel.Navigation = Navigation;
            BindingContext = viewModel  ;                        
            CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;
            CrossFirebasePushNotification.Current.OnNotificationOpened += Current_OnNotificationOpened;                       
        }        
        
        private async void Current_OnNotificationOpened(object source, FirebasePushNotificationResponseEventArgs p)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                DocumentType loaiphieu = DocumentType.DuyetDatMuaPhuTung;
                try
                {
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
                        if (p.Data["loaiphieu"].ToString() == "kidientuphutung")
                        {
                            loaiphieu = DocumentType.KiDienTuPhuTung;
                        }
                        if (p.Data["loaiphieu"].ToString() == "kidientuthietbi")
                        {
                            loaiphieu = DocumentType.KiDienTuThietBi;
                        }
                        if (p.Data["loaiphieu"].ToString() == "duyettrathietbi")
                        {
                            loaiphieu = DocumentType.DuyetTraThietBi ;
                        }
                        if (p.Data["loaiphieu"].ToString() == "duyetyeucauthuethietbi")
                        {
                            loaiphieu = DocumentType.DuyetYeuCauThueThietBi ;
                        }
                        if (loaiphieu == DocumentType.DuyetDatMuaPhuTung)
                            await Navigation.PushAsync(new DuyetChungTuPhuTung_Line(item, loaiphieu));
                        else if (loaiphieu == DocumentType.KiDienTuPhuTung)
                            await Navigation.PushAsync(new DuyetKiDienTuPhuTungLine(new Models.KiDienTuPhuTung.DuyetKiDienTuPhuTungModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else if (loaiphieu == DocumentType.DuyetYeuCauThueThietBi)
                            await Navigation.PushAsync(new DuyetYeuCauThueThietBiPage_Line(new  DuyetChungTuModel  { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else if (loaiphieu == DocumentType.KiDienTuThietBi)
                            await Navigation.PushAsync(new DuyetKiDienTuThietBiLine(new Models.KiDienTuPhuTung.DuyetKiDienTuPhuTungModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else if (loaiphieu == DocumentType.DuyetTraThietBi)
                            await Navigation.PushAsync(new DuyetTraThietBiLine_Page(new Models.KiDienTuPhuTung.DuyetKiDienTuPhuTungModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else
                            await Navigation.PushAsync(new DuyetChungTu_Line(item, loaiphieu));
                    }

                }
                catch
                {
                }
            });
        }

        private async void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs p)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                DocumentType loaiphieu = DocumentType.DuyetDatMuaPhuTung;
                try
                {
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
                        if (p.Data["loaiphieu"].ToString() == "kidientuphutung")
                        {
                            loaiphieu = DocumentType.KiDienTuPhuTung ;
                        }
                        if (p.Data["loaiphieu"].ToString() == "kidientuthietbi")
                        {
                            loaiphieu = DocumentType.KiDienTuThietBi;
                        }
                        if (p.Data["loaiphieu"].ToString() == "duyettrathietbi")
                        {
                            loaiphieu = DocumentType.DuyetTraThietBi;
                        }
                        if (p.Data["loaiphieu"].ToString() == "duyetyeucauthuethietbi")
                        {
                            loaiphieu = DocumentType.DuyetYeuCauThueThietBi;
                        }
                        if (loaiphieu == DocumentType.DuyetDatMuaPhuTung)
                            await Navigation.PushAsync(new DuyetChungTuPhuTung_Line(item, loaiphieu));
                        else if (loaiphieu == DocumentType.KiDienTuPhuTung)
                            await Navigation.PushAsync(new DuyetKiDienTuPhuTungLine(new Models.KiDienTuPhuTung.DuyetKiDienTuPhuTungModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else if (loaiphieu == DocumentType.DuyetYeuCauThueThietBi)
                            await Navigation.PushAsync(new DuyetYeuCauThueThietBiPage_Line(new DuyetChungTuModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else if (loaiphieu == DocumentType.KiDienTuThietBi)
                            await Navigation.PushAsync(new DuyetKiDienTuThietBiLine(new Models.KiDienTuPhuTung.DuyetKiDienTuPhuTungModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else if (loaiphieu == DocumentType.DuyetTraThietBi)
                            await Navigation.PushAsync(new DuyetTraThietBiLine_Page(new Models.KiDienTuPhuTung.DuyetKiDienTuPhuTungModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else
                            await Navigation.PushAsync(new DuyetChungTu_Line(item, loaiphieu));
                    }

                }
                catch
                {
                }
            });
              
        }

        

        #region "Button click"        

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Login();
        }
        
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            BackButtonPressed();
            return true;
        }
        public async Task BackButtonPressed()
        {
            var ok = await new MessageYesNo("Bạn có muốn thoát chương trình không?").Show();
            if (ok == DialogReturn.OK)
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }
        #endregion
       
        private void btnRefresh_Tapped(object sender, EventArgs e)
        {
            viewModel.LoadDataCommand.Execute(null);
        }

       

        

        

        

        
    }
}