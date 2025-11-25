using APP_HOATHO.Dialog;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Barcode;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DanhSachPhieuXuatKho_Page : ContentPage
    {
        DanhSachPhieuXuatKho_ViewModel viewModel;
        public DanhSachPhieuXuatKho_Page()
        {
            InitializeComponent();
            viewModel = new DanhSachPhieuXuatKho_ViewModel();
            BindingContext = viewModel;
        }

        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            DanhSachPhieuXuatKho_Model item = listChiTiet.SelectedItem as DanhSachPhieuXuatKho_Model;
            if (item != null)
            {
                Navigation.PushAsync(new DanhSachPhieuXuatKhoChiTiet_Page(item.No_));
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

            var item = o as DanhSachPhieuXuatKho_Model;
            if (item != null)
            {

                if (item.ExternalDocumentNo.ToLower().Contains(filterText) || item.PO.ToLower().Contains(filterText) || item.Style.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }

        private async void btnScanPhieuXuat_Tapped(object sender, EventArgs e)
        {
            try
            {
                ScanBarcode scan = new ScanBarcode(false, "Quét Phiếu Xuất");
                scan.ScanBarcodeResult += (s, result) =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            filterText = result;
                            var response = await viewModel.RunHttpClientGet<DocumentNoResponse>($"api/qltb/GetDocumentXuatKhoTheoExtNo?sochungtu={result}");
                            if (response.Lists.Count > 0) 
                            {
                                var document = response.Lists[0].No_;
                                await Navigation.PushAsync(new DanhSachPhieuXuatKhoChiTiet_Page(document));
                            }
                            else
                            {
                                await new MessageBox($"Không tìm thấy số chứng từ {result} trong hệ thống").Show();
                            }    
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

    public class DocumentNoResponse
    {
        public string No_ { get; set; }
    }
}