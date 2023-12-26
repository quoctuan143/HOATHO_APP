using APP_HOATHO.Converter;
using APP_HOATHO.Dialog;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Barcode;
using Syncfusion.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Thong_Tin_Chi_Tiet_Ke_Vai_Page : ContentPage,INotifyPropertyChanged
    {
        public ObservableCollection<ThongTinChiTietKeVai_Model> ListItem {get;set;}
        private  BaseViewModel viewModel;
        public Thong_Tin_Chi_Tiet_Ke_Vai_Page()
        {
            InitializeComponent();
            ListItem = new ObservableCollection<ThongTinChiTietKeVai_Model>();
            viewModel = new BaseViewModel();           
            BindingContext = this;
        }

        private async void listThietBi_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            try
            {
                var item = listThietBi.SelectedItem as ThongTinChiTietKeVai_Model;
                if (item != null)
                {
                    var Item = await viewModel.RunHttpClientGet<ThongTinChiTietCayVai_Model>($"api/qltb/XemThongTinChiTietCayVai?barcodeId={item.BarcodeId}");
                    if (Item.Lists.Count > 0)
                    {
                        await Navigation.PushAsync(new ThongTinChiTietCayVai_Page(Item.Lists[0]));
                    }
                    else
                        await new MessageBox($"Không tìm thấy cây vải có barcodeId {item.BarcodeId} trong hệ thống").Show();
                }
            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }
              
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listThietBi.View.Filter = FilterRecords;
            listThietBi.View.RefreshFilter();
        }
        private string filterText = string.Empty;
        public bool FilterRecords(object o)
        {
            filterText = filterText.ToLower();
            var item = o as ThongTinChiTietKeVai_Model;
            if (item != null)
            {
                if (item.RollNo.ToLower().Contains(filterText) || item.Color.ToLower().Contains(filterText) || item.BarcodeId.ToLower().Contains(filterText))
                    return true;

            }
            return false;
        }

        private async void btnScan_Clicked(object sender, EventArgs e)
        {
            try
            {
                viewModel.ShowLoading("Đang tìm dữ liệu");
                TraCuuCayVai request = new TraCuuCayVai
                {
                    BarcodeId = "%",
                    KeVai = "%",
                    RollNo = String.IsNullOrEmpty(txtRollNo.Text) ? "%" : $"%{txtRollNo.Text}%"
                };

                var Item = await viewModel.RunHttpClientGet<ThongTinChiTietKeVai_Model>($"api/VaiLot/ThongTinKeVai", request);
                ListItem = Item.Lists;
                OnPropertyChanged("ListItem");

                viewModel.HideLoading();
            }
            catch (Exception ex)
            {
                viewModel.HideLoading();
                await new MessageBox(ex.Message).Show();
            }
            
        }

        private async void btnQuetQR_Clicked(object sender, EventArgs e)
        {
            ScanBarcode scan = new ScanBarcode(false, "Quét QR cây vải");
            scan.ScanBarcodeResult += (s, result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var Item = await viewModel.RunHttpClientGet<ThongTinChiTietCayVai_Model>($"api/qltb/XemThongTinChiTietCayVai?barcodeId={result}");
                    if (Item.Lists.Count > 0)
                    {
                        await Navigation.PushAsync(new ThongTinChiTietCayVai_Page(Item.Lists[0]));
                    }
                    else
                        await new MessageBox($"Không tìm thấy cây vải có barcodeId {result} trong hệ thống").Show();
                });
            };
            await Navigation.PushAsync(scan);
        }

        private async void btnQuetQRKeVai_Clicked(object sender, EventArgs e)
        {
            ScanBarcode scan = new ScanBarcode(false, "Quét QR kệ vải");
            scan.ScanBarcodeResult += (s, result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    viewModel.ShowLoading("Đang tìm dữ liệu");
                    TraCuuCayVai request = new TraCuuCayVai
                    {
                        BarcodeId = "%",
                        KeVai = result,
                        RollNo = "%" 
                    };

                    var Item = await viewModel.RunHttpClientGet<ThongTinChiTietKeVai_Model>($"api/VaiLot/ThongTinKeVai", request);
                    ListItem = Item.Lists;
                    OnPropertyChanged("ListItem");

                    viewModel.HideLoading();
                });
            };
            await Navigation.PushAsync(scan);
        }
    }

    public class TraCuuCayVai
    {
        public string KeVai { get; set; }
        public string RollNo { get; set; }
        public string BarcodeId { get; set; }
    }
}