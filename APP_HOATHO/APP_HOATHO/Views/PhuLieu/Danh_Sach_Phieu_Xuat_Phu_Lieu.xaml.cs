using APP_HOATHO.Dialog;
using APP_HOATHO.Models.PhuLieu;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels.PhuLieu;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Barcode;
using APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.PhuLieu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Danh_Sach_Phieu_Xuat_Phu_Lieu : ContentPage
    {
        public Xuat_Phu_Lieu_ViewModel viewModel = new  Xuat_Phu_Lieu_ViewModel();
        public Danh_Sach_Phieu_Xuat_Phu_Lieu()
        {
            InitializeComponent();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListItem.Count == 0)
            {
                viewModel.LoadDataCommand.Execute(null);
            }
        }

        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            DanhSachPhieuNhapXuatPhuLieu_Model item = listChiTiet.SelectedItem as DanhSachPhieuNhapXuatPhuLieu_Model;
            if (item != null)
            {
                Navigation.PushAsync(new Chi_Tiet_Phieu_Xuat_Kho_Phu_Lieu_Page(item.No_));
            }
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

            var item = o as DanhSachPhieuNhapXuatPhuLieu_Model;
            if (item != null)
            {

                if (item.ExternalDocumentNo.ToLower().Contains(filterText) || item.CustomerName.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }

        private async void btnScanPhieuNhap_Tapped(object sender, EventArgs e)
        {
            try
            {
                ScanBarcode scan = new ScanBarcode(false, "Quét Phiếu Nhập");
                scan.ScanBarcodeResult += (s, result) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        filterText = result;
                        txtSearch.Text = filterText;
                        listChiTiet.View.Filter = FilterRecords;
                        listChiTiet.View.RefreshFilter();
                    });

                };
                await Navigation.PushAsync(scan);
            }
            catch (Exception ex)
            {
                await new MessageBox(ex.Message).Show();
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            viewModel.FindCommand.Execute(null);
        }
    }
}