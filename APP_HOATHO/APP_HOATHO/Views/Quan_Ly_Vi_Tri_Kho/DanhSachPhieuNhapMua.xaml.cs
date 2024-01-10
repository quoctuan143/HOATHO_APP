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
    public partial class DanhSachPhieuNhapMua : ContentPage
    {
       public DanhSachPhieuNhapMuaHang_ViewModel viewModel = new DanhSachPhieuNhapMuaHang_ViewModel();
        public DanhSachPhieuNhapMua()
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
            DanhSachPhieuNhapMuaHang_Model item = listChiTiet.SelectedItem as DanhSachPhieuNhapMuaHang_Model;
            if (item != null)
            {
                Navigation.PushAsync(new Purchase_Line_Page(item.No_));
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

            var item = o as DanhSachPhieuNhapMuaHang_Model;
            if (item != null)
            {

                if (item.ExternalDocumentNo.ToLower().Contains(filterText) || item.CustomerName.ToLower().Contains(filterText) || item.VendorName.ToLower().Contains(filterText) || item.SoKien.ToString().ToLower().Contains(filterText))
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
    }
}