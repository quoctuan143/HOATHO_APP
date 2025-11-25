using APP_HOATHO.Dialog;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Barcode;
using Syncfusion.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaoPhieuXuatKhoGomVai_Page : ContentPage
    {
       public TaoPhieuXuatKhoGomVai_ViewModel viewModel = new TaoPhieuXuatKhoGomVai_ViewModel();
        public TaoPhieuXuatKhoGomVai_Page()
        {
            InitializeComponent();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }      
        
        string filterText = string.Empty;
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listChiTiet.View.Filter = FilterRecords;
            listChiTiet.View.RefreshFilter();
        }
        public bool FilterRecords(object o)
        {

            var item = o as Tong_Hop_Gom_Phieu_Xuat_Model;
            if (item != null)
            {

                if (item.DocumentNo.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(500);           
            this.qrcode.Focus();
        }
        private async void Entry_Completed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.qrcode.Text))
            {
                viewModel.AddPhieuXuat(this.qrcode.Text);     
                await Task.Delay(1000);
                this.qrcode.Focus();
                this.qrcode.Text = string.Empty;
               
            };
        }

        private async void qrcode_Unfocused(object sender, FocusEventArgs e)
        {
            await Task.Delay(500);
            this.qrcode.Text = string.Empty;
            this.qrcode.Focus();
        }

        private async void btnScanPhieuXuat_Tapped(object sender, EventArgs e)
        {
            try
            {
                ScanBarcode scan = new ScanBarcode(true, "Quét Phiếu Xuất");
                scan.ScanBarcodeResult += (s, result) =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            filterText = result;
                            viewModel.AddPhieuXuat(result);
                        }
                        catch (Exception ex)
                        {
                            await new MessageBox(ex.Message).Show();
                        }

                    });

                };
                await Navigation.PushAsync(scan);
            }
            catch (Exception ex)
            {
                await new MessageBox(ex.Message).Show();
            }
        }
    }
}