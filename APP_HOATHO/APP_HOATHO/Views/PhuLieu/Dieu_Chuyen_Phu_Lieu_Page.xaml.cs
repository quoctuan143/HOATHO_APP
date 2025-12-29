using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels;
using APP_HOATHO.ViewModels.PhuLieu;
using APP_HOATHO.Views.Barcode;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace APP_HOATHO.Views.PhuLieu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dieu_Chuyen_Phu_Lieu_Page : ContentPage,INotifyPropertyChanged
    {
        public ObservableCollection<Chi_Tiet_Nhap_Phu_Lieu_Model> ListItem { get; set; }
        BaseViewModel viewModel = new BaseViewModel  ();
        public string BarcodeId { get; set; }
        public string ToBarcodeId { get; set; }
        public string ViTriLuu { get; set; }
        ObservableCollection<LookupValue> _listViTri;
        Chi_Tiet_Nhap_Phu_Lieu_Model SelectItem { get; set; }
        public ObservableCollection<LookupValue> ListViTri
        {
            get => _listViTri; 
            set
            {
                _listViTri = value;
                OnPropertyChanged("ListViTri");
            }
        }
        public Dieu_Chuyen_Phu_Lieu_Page()
        {
            InitializeComponent();
            ListItem = new  ObservableCollection<Chi_Tiet_Nhap_Phu_Lieu_Model>();
            BarcodeId = "QR Rổ Chứa";
            ToBarcodeId = "QR Rổ Đến";
            ViTriLuu = "Vị trí lưu";                    
            BindingContext = this;
        }
        protected override  void OnAppearing()
        {
            base.OnAppearing();
            Task.Factory.StartNew(async () =>
            {
                var a = await viewModel.RunHttpClientGet<LookupValue>("GetDanhSachViTriKhoPhuLieu");
                ListViTri = a.Lists;
            });

        }    
        
        private async void btnXuatKho_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (this.ToBarcodeId == "QR Rổ Đến")
                {
                    await new MessageBox("Vui lòng quét barcode của thùng trước khi nhập kho").Show();
                    return;
                }

                var listNhap = ListItem.Where(x => x.SoLuongDaNhap > 0 && x.Chon == true).Select(x => new PhieuNhapPhuLieuPackingList
                {
                    BarcodeId = x.PackageId,
                    Color_No_ = x.Color,
                    Document_No_ = x.DocumentNo,
                    Item_No_ = x.No_,
                    Quantity = Convert.ToDecimal(x.SoLuongDaNhap),
                    UserId = Preferences.Get(Config.User, ""), 
                    RowId = x.RowId,
                    LotNo = x.LotNo,
                    ExternalDocumentNo = x.PhieuNhap,
                    
                }).ToList();

                if (listNhap.Count == 0)
                {
                    await new MessageBox("Vui lòng chọn số lượng cần nhập kho").Show();
                    return;
                }
                string a = JsonConvert.SerializeObject(listNhap);

                var ok = await new MessageYesNo($"Bạn có muốn điều chuyển cho rổ có barcode {this.ToBarcodeId}").Show();

                if (ok == DialogReturn.OK)
                {
                    viewModel.ShowLoading("Đang lưu, vui lòng đợi");
                    var runOk = await viewModel.RunHttpClientPost($"api/PhuLieu/DieuChuyenPhuLieu?packageId={this.ToBarcodeId}", listNhap);
                    viewModel.HideLoading();
                    if (runOk.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await new MessageBox("Đã điều chuyển thành công").Show();
                        ListItem.Clear();
                        ListItem = await viewModel.RunHttpClientGetObject<ObservableCollection<Chi_Tiet_Nhap_Phu_Lieu_Model>>($"api/PhuLieu/getTonKhoTheoBarcode?barcodeid={this.BarcodeId}");
                        OnPropertyChanged(nameof(ListItem));
                        this.ToBarcodeId = "QR Rổ Đến";
                        OnPropertyChanged(nameof(ToBarcodeId));
                    }
                    else
                    {
                        await new MessageBox(runOk.Content.ReadAsStringAsync().ToString()).Show();
                        return;
                    }
                }
            }
            catch
            {

               
            }
            finally
            {
                viewModel.HideLoading();
            }    
        }

        private async void SfButton_Clicked(object sender, EventArgs e)
        {
            ScanBarcode scan = new ScanBarcode(false, "Quét QR Rổ PL");
            scan.ScanBarcodeResult += (s, result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    this.BarcodeId = result;
                    OnPropertyChanged(nameof(BarcodeId));
                    viewModel.ShowLoading("Đang load dữ liệu.....");
                    ListItem = await viewModel.RunHttpClientGetObject<ObservableCollection< Chi_Tiet_Nhap_Phu_Lieu_Model>>($"api/PhuLieu/getTonKhoTheoBarcode?barcodeid={result}");
                    viewModel.HideLoading();
                    OnPropertyChanged(nameof(ListItem));
                });
            };
            await Navigation.PushAsync(scan);
        }

        private async void SfButton_Clicked_1(object sender, EventArgs e)
        {
            ScanBarcode scan = new ScanBarcode(false, "Quét QR Rổ PL");
            scan.ScanBarcodeResult += (s, result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    this.ToBarcodeId = result;
                    OnPropertyChanged(nameof(ToBarcodeId));
                });
            };
            await Navigation.PushAsync(scan);
        }
    }
}