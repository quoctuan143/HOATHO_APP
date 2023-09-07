using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels;
using APP_HOATHO.Views.Barcode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseLinePackingListAdd_Page : ContentPage, INotifyPropertyChanged
    {
        public PurchaseLinePackingList_Model Item { get; set; }

        public PurchaseLinePackingListAdd_Page(PurchaseLinePackingList_Model item)
        {
            InitializeComponent();
            var items = new List<LookupValue> { new LookupValue { Code = "MET", Name = "MÉT" }, new LookupValue { Code = "YARD", Name = "YARD" } };
            cbDonViTinh.DataSource = items.ToList();
            cbDonViTinh.SelectedValue = "MET";
            Item = item;

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

                            var ok = await Config.client.PostAsJsonAsync("api/qltb/PostKiemTraBarcodeIdCayVai", Item);
                            if (ok.StatusCode != System.Net.HttpStatusCode.OK)
                            {
                                Item.BarcodeId = "";
                                await new MessageBox(ok.Content.ReadAsStringAsync().Result.ToString()).Show();
                            }
                            OnPropertyChanged("Item");
                        }
                        catch
                        {
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

                if (Item.InvoicedMeter == 0)
                {
                    await new MessageBox("Số lượng phải khác 0").Show();
                    return;
                }

                if (string.IsNullOrEmpty(Item.DonViTinh))
                {
                    await new MessageBox("Vui lòng chọn đơn vị tính").Show();
                    return;
                }

                if (!string.IsNullOrEmpty( Item.BarcodeId ))
                {
                    if (string.IsNullOrEmpty(Item.PositionId))
                    {
                        await new MessageBox("Vui lòng chọn Ô để lưu").Show();
                        return;
                    }    
                }    
                var ok = await Config.client.PostAsJsonAsync("api/qltb/CapNhatIdCayVaiMoi", Item);
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
                            var ok = await Config.client.PostAsJsonAsync("api/qltb/PostKiemTraOChuaVai", item);
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