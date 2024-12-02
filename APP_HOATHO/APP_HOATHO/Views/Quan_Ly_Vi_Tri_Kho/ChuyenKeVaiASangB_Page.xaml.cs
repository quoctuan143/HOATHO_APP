using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels;
using APP_HOATHO.Views.Barcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChuyenKeVaiASangB_Page : ContentPage
    {
        public ChuyenKeVaiASangB_Page()
        {
            InitializeComponent();
        }

        private async void ChonKeA_Tapped(object sender, EventArgs e)
        {
            ScanBarcode scan = new ScanBarcode(false, "Chọn kệ cần chuyển");
            scan.ScanBarcodeResult += (s, result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    BaseViewModel viewModel = new BaseViewModel();
                    try
                    {
                        
                        CellPositionModel item = new CellPositionModel { Code = result, PositionId = "" };
                        viewModel.ShowLoading("đang kiểm tra màu");
                        var ok1 = await viewModel.RunHttpClientPost("api/qltb/PostKiemTraOChuaVai", item);
                        viewModel.HideLoading();
                        if (!ok1.IsSuccessStatusCode)
                        {
                            await new MessageBox($"Kệ : {result} này không tồn tại trong hệ thống").Show();
                            return;
                        }
                        txtKeA.Text = result;
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

        private async void btnUpdate_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKeA.Text) || string.IsNullOrEmpty(txtKeB.Text))
            {
                await new MessageBox("Vui lòng chọn thông tin kệ đi và kệ đến").Show();
                return;
            }    

            if (await new MessageYesNo ($"Bạn có muốn chuyển tất cả những cây vải trong kệ {txtKeA.Text} sang kệ {txtKeB.Text}").Show() == Global.DialogReturn.OK)
            {
                BaseViewModel viewModel = new BaseViewModel();
                var ok = await viewModel.RunHttpClientPost($"api/qltb/ChuyenVaiTuKeASangKeB?keA={txtKeA.Text}&keB={txtKeB.Text}&user={Preferences.Get(Config.User,"")}", null);
                if (ok.IsSuccessStatusCode)
                {
                    await new MessageBox("Cập nhật thành công").Show();
                    txtKeA.Text = "";
                    txtKeB.Text = "";
                }
                else
                {
                    await new MessageBox(ok.Content.ReadAsStringAsync().Result).Show();
                }
            }            
        }

        private async void ChonKeB_Tapped(object sender, EventArgs e)
        {
            ScanBarcode scan = new ScanBarcode(false, "Chọn kệ chuyển đến");
            scan.ScanBarcodeResult += (s, result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        BaseViewModel viewModel = new BaseViewModel();
                        CellPositionModel item = new CellPositionModel { Code = result, PositionId = "" };
                        var ok1 = await viewModel.RunHttpClientPost("api/qltb/PostKiemTraOChuaVai", item);
                        if (!ok1.IsSuccessStatusCode)
                        {
                            await new MessageBox($"Kệ : {result} này không tồn tại trong hệ thống").Show();
                            return;
                        }
                        txtKeB.Text = result;
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
}