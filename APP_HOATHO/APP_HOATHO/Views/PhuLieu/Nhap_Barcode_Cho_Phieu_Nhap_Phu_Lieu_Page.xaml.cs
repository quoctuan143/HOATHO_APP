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

namespace APP_HOATHO.Views.PhuLieu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Nhap_Barcode_Cho_Phieu_Nhap_Phu_Lieu_Page : ContentPage,INotifyPropertyChanged
    {
        public List<Chi_Tiet_Nhap_Phu_Lieu_Model> ListItem { get; set; }
        BaseViewModel viewModel = new BaseViewModel  ();
        public string BarcodeId { get; set; }
        public string ViTriLuu { get; set; }
        ObservableCollection<LookupValue> _listViTri;
        public ObservableCollection<LookupValue> ListViTri
        {
            get => _listViTri; 
            set
            {
                _listViTri = value;
                OnPropertyChanged("ListViTri");
            }
        }
        public Nhap_Barcode_Cho_Phieu_Nhap_Phu_Lieu_Page(List<Chi_Tiet_Nhap_Phu_Lieu_Model> list)
        {
            InitializeComponent();
            BarcodeId = "Quét QR";
            ViTriLuu = "Vị trí lưu";
            ListItem = list;
            ListItem.ForEach(x => x.SoLuongCanNhap = x.Quantity);
            BindingContext = this;
        }
        protected override  void OnAppearing()
        {
            base.OnAppearing();
            Task.Factory.StartNew(async () =>
            {
                var a = await viewModel.RunHttpClientGet<LookupValue>("api/qltb/GetDanhSachViTriKho");
                ListViTri = a.Lists;
            });

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
            var item = o as Chi_Tiet_Nhap_Phu_Lieu_Model;
            if (item != null)
            {
                if (item.Name.ToLower().Contains(filterText) || item.Color.ToLower().Contains(filterText) || item.Art.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }

        private async void SfButton_Clicked(object sender, EventArgs e)
        {
            ScanBarcode scan = new ScanBarcode(false, "Quét QR thùng PL");
            scan.ScanBarcodeResult += (s, result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    this.BarcodeId = result;
                    OnPropertyChanged(nameof(BarcodeId));
                });
            };
            await Navigation.PushAsync(scan);
        }    

        private async void btnXuatKho_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (this.BarcodeId == "Quét QR")
                {
                    await new MessageBox("Vui lòng quét barcode của thùng trước khi nhập kho").Show();
                    return;
                }

                var listNhap = ListItem.Where(x => x.SoLuongCanNhap > 0 && x.Chon == true).Select(x => new PhieuNhapPhuLieuPackingList
                {
                    BarcodeId = this.BarcodeId,
                    Color_No_ = x.Color,
                    Document_No_ = x.DocumentNo,
                    Item_No_ = x.No_,
                    Quantity = Convert.ToDecimal(x.SoLuongCanNhap),
                    UserId = Preferences.Get(Config.User, ""),
                    ViTriLuu = this.ViTriLuu.ToLower() == "vị trí lưu" ? "" : this.ViTriLuu
                }).ToList();

                if (listNhap.Count == 0)
                {
                    await new MessageBox("Vui lòng nhập số lượng cần nhập kho").Show();
                    return;
                }
                string a = JsonConvert.SerializeObject(listNhap);

                var ok = await new MessageYesNo($"Bạn có muốn nhập kho cho thùng có barcode {this.BarcodeId}").Show();

                if (ok == DialogReturn.OK)
                {
                    viewModel.ShowLoading("Đang lưu, vui lòng đợi");
                    var runOk = await viewModel.RunHttpClientPost("api/PhuLieu/nhapKhoPhuLieu", listNhap);
                    viewModel.HideLoading();
                    if (runOk.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await new MessageBox("Đã nhập kho thành công").Show();
                        await Navigation.PopAsync();
                        MessagingCenter.Send(this, "reloadNhapKhoPhuLieu");
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

        private async void SfButton_Clicked_1(object sender, EventArgs e)
        {
            var lookup = new LookupItem(ListViTri, "Danh sách vị trí kho");
            lookup.closeForm += (s, d) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.ViTriLuu = d.Code;
                    OnPropertyChanged(nameof(ViTriLuu));
                });
            };

            await Navigation.PushModalAsync(lookup);
        }
    }
}