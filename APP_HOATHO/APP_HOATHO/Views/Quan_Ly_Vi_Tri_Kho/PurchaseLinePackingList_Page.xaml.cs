using APP_HOATHO.Converter;
using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Barcode;
using Syncfusion.Data;
using System;
using System.Linq;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseLinePackingList_Page : ContentPage
    {
        private PurchaseLinePackingList_ViewModel viewModel;

        public PurchaseLinePackingList_Page(string soChungTu)
        {
            InitializeComponent();
            viewModel = new PurchaseLinePackingList_ViewModel(soChungTu);
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

        private string filterText = string.Empty;

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listChiTiet.View.Filter = FilterRecords;
            listChiTiet.View.RefreshFilter();
        }

        public bool FilterRecords(object o)
        {
            filterText = filterText.ToLower();
            var item = o as PurchaseLinePackingList_Model;
            if (item != null)
            {
                if (ChonNhom1 == StatusFormatColor.TatCa )
                {
                    if (item.RollNo.ToLower().Contains(filterText) || item.Color.ToLower().Contains(filterText) || item.PositionId.ToLower().Contains(filterText) || item.Art.ToLower().Contains(filterText))
                        return true;
                }    
                else
                {
                    if ((item.RollNo.ToLower().Contains(filterText) || item.Color.ToLower().Contains(filterText) || item.PositionId.ToLower().Contains(filterText) || item.Art.ToLower().Contains(filterText)) && item.DaGanNhan == ChonNhom1)
                        return true;
                }    
               
            }
            return false;
        }

        private async void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.ViTriOVai))
            {
                await new MessageBox("Vui lòng quét kệ chứa cây vải trước khi gán nhãn barcode").Show();
                return;
            }
            PurchaseLinePackingList_Model item = listChiTiet.SelectedItem as PurchaseLinePackingList_Model;
            if (item != null)
            {
                try
                {
                    ScanBarcode scan = new ScanBarcode(false, "Cài đặt BarcodeId cây vải", true);
                    await Navigation.PushAsync(scan);
                    scan.ScanBarcodeResult += (s, result) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                //kiểm tra xem barcode có trong danh sách không?
                                if (viewModel.ListItem.FirstOrDefault(x => x.BarcodeId == result.ToUpper()) != null)
                                {
                                    await new MessageBox("Barcode này đã gán cho cây vải khác nên không thể cập nhật cho cây vải này được").Show();                                     
                                    return;
                                }

                                PurchaseLinePackingList_Model find = viewModel.ListItem.FirstOrDefault(x => x.Id == item.Id);
                                if (find != null)
                                {
                                    find.BarcodeId = result.ToUpper();
                                    find.PositionId = viewModel.ViTriOVai;

                                    var ok = await Config.client.PostAsJsonAsync("api/qltb/PostKiemTraBarcodeIdCayVai", find);
                                    if (ok.StatusCode == System.Net.HttpStatusCode.OK)
                                    {                                       
                                        listChiTiet.Refresh();                                        
                                    }
                                    else
                                    {
                                        find.BarcodeId = "";
                                        await new MessageBox(ok.Content.ReadAsStringAsync().Result.ToString()).Show();
                                    }
                                }
                            }
                            catch
                            {
                                await new MessageBox("Barcode không tồn tại trong hệ thống").Show();
                            }
                        });

                    };                   
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            }
        }

        private PurchaseLinePackingList_Model _selectItem;

        private void listChiTiet_SwipeEnded(object sender, Syncfusion.SfDataGrid.XForms.SwipeEndedEventArgs e)
        {
            _selectItem = e.RowData as PurchaseLinePackingList_Model;
        }

        private async void btnXoaBarcodeId_Tapped(object sender, EventArgs e)
        {
            try
            {
                var find = viewModel.ListItem.FirstOrDefault(x => x.Id == _selectItem.Id);
                if (find != null)
                {
                    viewModel.ShowLoading("Đang xóa....vui lòng đợi");
                    var ok = await Config.client.PostAsJsonAsync("api/qltb/XoaBarcodeIdCayVai", find);
                    viewModel.HideLoading();
                    if (ok.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        find.BarcodeId = "";
                        listChiTiet.Refresh();
                    }
                    else
                    {
                        await new MessageBox(ok.Content.ReadAsStringAsync().Result.ToString()).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                viewModel.HideLoading();
                await new MessageBox(ex.Message).Show();
            }
        }

        StatusFormatColor ChonNhom1 = StatusFormatColor.TatCa;

        private void ChonNhom_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

            if (e.Value.ToString() == "Tất Cả")
            {
                ChonNhom1 = StatusFormatColor.TatCa;
            }
            if (e.Value.ToString() == "Chưa Hoàn Thành")
            {
                ChonNhom1 = StatusFormatColor.ChuaHoanThanh;
            }
            if (e.Value.ToString() == "Dở Dang")
            {
                ChonNhom1 = StatusFormatColor.DoDang;
            }
            if (e.Value.ToString() == "Hoàn Thành")
            {
                ChonNhom1 = StatusFormatColor.HoanThanh;
            }
            listChiTiet.View.Filter = FilterRecords;
            listChiTiet.View.RefreshFilter();
        }
    }
}