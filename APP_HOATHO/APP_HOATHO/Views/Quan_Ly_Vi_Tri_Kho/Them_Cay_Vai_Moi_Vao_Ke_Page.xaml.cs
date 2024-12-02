using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels;
using APP_HOATHO.Views.Barcode;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Them_Cay_Vai_Moi_Vao_Ke_Page : ContentPage, INotifyPropertyChanged
    {
        public BaseViewModel viewModel { get; set; }
        public PurchaseLinePackingList_Model Item { get; set; }
        public List<LookupValue> ListNguyenPhuLieu { get; set; }
        public List<LookupValue> danhSachMauNPL { get; set; }
        public Them_Cay_Vai_Moi_Vao_Ke_Page()
        {
            InitializeComponent();
            viewModel = new BaseViewModel();
            ListNguyenPhuLieu = new List<LookupValue>();
            danhSachMauNPL = new List<LookupValue>();
            var items = new List<LookupValue> { new LookupValue { Code = "MET", Name = "MÉT" }, new LookupValue { Code = "YARD", Name = "YARD" } };
            cbDonViTinh.DataSource = items.ToList();
            cbDonViTinh.SelectedValue = "MET";
            Item = new PurchaseLinePackingList_Model { DocumentNo = "TON_KHO"};

            BindingContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                ScanBarcode scan = new ScanBarcode(false, "Cài đặt BarcodeId cây vải", true);
                scan.ScanBarcodeResult += (s, result) =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            //kiểm tra xem barcode có trong danh sách không?
                            Item.BarcodeId = result;
                            viewModel.ShowLoading("Đang kiểm tra. vui lòng đợi...");
                            var ok = await Config.client.PostAsJsonAsync("api/qltb/PostKiemTraBarcodeIdCayVai", Item);
                            viewModel.HideLoading();
                            if (ok.StatusCode != System.Net.HttpStatusCode.OK)
                            {
                                Item.BarcodeId = "";
                                await new MessageBox(ok.Content.ReadAsStringAsync().Result.ToString()).Show();
                            }
                            OnPropertyChanged("Item");
                        }
                        catch
                        {
                            viewModel.HideLoading();
                            await new MessageBox("Barcode không tồn tại trong hệ thống").Show();
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

        private async void btnCapNhat_Clicked(object sender, EventArgs e)
        {
            try
            {
                //kiểm tra xem barcode có trong danh sách không?
                if (string.IsNullOrEmpty(Item.ItemNo))
                {
                    await new MessageBox("Vui lòng chọn mã nguyên phụ liệu").Show();
                    return;
                }
                
                if (Item.InvoicedMeter == 0)
                {
                    await new MessageBox("chiều dài cây vải phải khác 0").Show();
                    return;
                }

                if (string.IsNullOrEmpty(Item.DonViTinh))
                {
                    await new MessageBox("Vui lòng chọn đơn vị tính").Show();
                    return;
                }
                
                if (string.IsNullOrEmpty(Item.PositionId))
                {
                    await new MessageBox("Vui lòng chọn Ô để lưu").Show();
                    return;
                }
                if (Item.SoCayVai  <= 0)
                {
                    await new MessageBox("Vui lòng nhập số cây vải để lưu").Show();
                    return;
                }
                viewModel.ShowLoading("Đang cập nhật. vui lòng đợi...");
                var ok = await Config.client.PostAsJsonAsync("api/qltb/CapNhatIdCayVaiMoi", Item);
                viewModel.HideLoading();
                if (ok.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await new MessageBox(ok.Content.ReadAsStringAsync().ToString()).Show();
                    return;
                }
                await new MessageBox("Đã cập nhật thành công!").Show();
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                viewModel.HideLoading();
                await new MessageBox(ex.Message).Show();
            }
        }

        private async void btnGanOVai_Clicked(object sender, EventArgs e)
        {
            try
            {
                ScanBarcode scan = new ScanBarcode(false, "Quét kệ chứa cây vải");
                scan.ScanBarcodeResult += (s, result) =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            CellPositionModel item = new CellPositionModel { Code = result };
                            viewModel.ShowLoading("Đang cập nhật. vui lòng đợi...");
                            var ok = await Config.client.PostAsJsonAsync("api/qltb/PostKiemTraOChuaVai", item);
                            viewModel.HideLoading();    
                            if (ok.IsSuccessStatusCode)
                            {
                                Item.PositionId = result;
                                btnGanOVai.Text = $"Kệ : {result}";
                                OnPropertyChanged("Item");
                            }
                            else
                            {
                                await new MessageBox("Barcode này không tồn tại trong hệ thống").Show();
                            }

                        }
                        catch (Exception ex)
                        {
                            viewModel.HideLoading();
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

        private async void btnCheckArt_Clicked(object sender, EventArgs e)
        {
                      
            try
            {            
                if (string.IsNullOrEmpty(txtArt.Text))
                {
                    await new MessageBox("Nhập art để tìm kiếm").Show();
                    return;
                }
                viewModel.ShowLoading("đang kiểm tra mã NPL");
                var ok = await viewModel.RunHttpClientGet<LookupValue>($"api/qltb/LayMaNguyenPhuLieuTheoArt?art={txtArt.Text}");
                viewModel.HideLoading();
                ListNguyenPhuLieu = ok.Lists.ToList();
                cbLoaiDoiTuong.DataSource = ListNguyenPhuLieu;
                OnPropertyChanged("ListNguyenPhuLieu");

            }
            catch (Exception ex)
            {
                viewModel.HideLoading();
                await new MessageBox(ex.Message).Show();
            }
        }

        private async void cbLoaiDoiTuong_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            try
            {                
                LookupValue item = (LookupValue )cbLoaiDoiTuong.SelectedItem;
                if (item != null)
                {
                    viewModel.ShowLoading("đang kiểm tra màu");
                    var b = await viewModel.RunHttpClientGet<LookupValue>($"api/qltb/GetDanhSachMauNPLXuatKho?manpl={item.Code}");
                    viewModel.HideLoading();
                    danhSachMauNPL = b.Lists.ToList();
                    cbmau.DataSource = danhSachMauNPL;
                    OnPropertyChanged("danhSachMauNPL");
                }    
                
            }
            catch (Exception)
            {
                viewModel.HideLoading();
            }
            
        }
    }
}