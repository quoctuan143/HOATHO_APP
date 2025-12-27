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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Xuat_Kho_Theo_Ke_Vai_Page : ContentPage,INotifyPropertyChanged
    {
        public ObservableCollection<ThongTinChiTietKeVai_Model> ListItem {get;set;}
        private  BaseViewModel viewModel;
        public Xuat_Kho_Theo_Ke_Vai_Page()
        {
            InitializeComponent();
            ListItem = new ObservableCollection<ThongTinChiTietKeVai_Model>();
            viewModel = new BaseViewModel();           
            BindingContext = this;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(500);
            this.sochungtu.Focus();
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

        private async void btnXuatKho_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.sochungtu.Text))
                {
                    await new MessageBox("Vui lòng chọn số phiếu xuất").Show();
                    return;
                }
                var listXuat = ListItem.Where(x => x.Chon == true).ToList();
                if (listXuat.Count == 0)
                {
                    await new MessageBox("Chọn cây vải để xuất").Show();
                    return;
                }
                listXuat.ForEach(x =>
                {
                    x.SoChungTuXuat = this.sochungtu.Text.Trim();
                });
                var ask = await new MessageYesNo("Bạn có muốn xuất không?").Show();
                if (ask == Global.DialogReturn.OK)
                {
                    viewModel.ShowLoading("Đang cập nhật. vui lòng đợi...");
                    var ok = await viewModel.RunHttpClientPost($"api/qltb/XuatVaiKhoiKe?userId={Preferences.Get(Global.Config.User, "")}", listXuat);
                    viewModel.HideLoading();
                    if (ok.IsSuccessStatusCode)
                    {
                        listXuat.Clear();
                        ListItem.Clear();
                        OnPropertyChanged("ListItem");
                        await new MessageBox("Xuất kho thành công").Show();                        
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await new MessageBox(ok.Content.ReadAsStringAsync().Result.ToString()).Show();
                    }
                }
            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }
            
        }

        private async void btnChuyenKe_Clicked(object sender, EventArgs e)
        {
            //xử lý chuyển ở đây
            var listXuat = ListItem.Where(x => x.Chon == true).ToList();
            if (listXuat.Count == 0)
            {
                await new MessageBox("Chọn cây vải để chuyển").Show();
                return;
            }
            ScanBarcode scan = new ScanBarcode(false, "Chọn kệ chuyển đến");
            scan.ScanBarcodeResult += (s, result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        BaseViewModel viewModel = new BaseViewModel();
                        CellPositionModel item = new CellPositionModel { Code = result, PositionId = "" };
                        viewModel.ShowLoading("Đang xử lý. vui lòng đợi...");
                        var ok1 = await viewModel.RunHttpClientPost("api/qltb/PostKiemTraOChuaVai", item);
                        viewModel.HideLoading();
                        if (!ok1.IsSuccessStatusCode)
                        {
                            await new MessageBox($"Kệ : {result} này không tồn tại trong hệ thống").Show();
                            return;
                        }                        

                        var ask = await new MessageYesNo($"Bạn có muốn chuyển những cây vải này sang kệ {result} không?").Show();
                        if (ask == Global.DialogReturn.OK)
                        {
                            viewModel.ShowLoading("Đang chuyển kệ. vui lòng đợi...");
                            var ok = await viewModel.RunHttpClientPost($"api/qltb/ChuyenVaiQuaKeKhac?kemoi={result}", listXuat);
                            viewModel.HideLoading();
                            if (ok.IsSuccessStatusCode)
                            {
                                await new MessageBox("Chuyển kệ thành công").Show();
                                foreach (var vai in ListItem)
                                {
                                    vai.Chon = false;
                                }
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                await new MessageBox(await ok.Content.ReadAsStringAsync()).Show();
                            }
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
    }

    public class TraCuuCayVai
    {
        public string KeVai { get; set; }
        public string RollNo { get; set; }
        public string BarcodeId { get; set; }
    }
}