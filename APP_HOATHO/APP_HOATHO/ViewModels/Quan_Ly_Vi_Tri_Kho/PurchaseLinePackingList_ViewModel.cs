using APP_HOATHO.Dialog;
using APP_HOATHO.Interface;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Barcode;
using APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using ZXing.Net.Mobile.Forms;

namespace APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho
{
    public class PurchaseLinePackingList_ViewModel : BaseViewModel
    {
        public INavigation navigation { get; set; }
        string _titleButton;
        public string TitleButton { get => _titleButton; set => SetProperty(ref _titleButton ,value); }
        string SoChungTu;
        public string ViTriOVai;
        ObservableCollection<PurchaseLinePackingList_Model> _listItem;
        public ObservableCollection<PurchaseLinePackingList_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public ICommand LoadDataCommand { get; set; }

        public ICommand DeleteBarcodeCommand { get; set; }

        public ICommand QuetOChuaCayVaiCommand { get; set; } 
        public PurchaseLinePackingList_ViewModel(string soChungTu)
        {
            TitleButton = "Quét kệ chứa cây vải";
            SoChungTu = soChungTu;
            ListItem = new ObservableCollection<PurchaseLinePackingList_Model>();
            LoadDataCommand = new Command(async () =>
            {
                try
                {
                    ListItem.Clear();
                    var url = $"api/qltb/GetThongTinPackingList?sochungtu={SoChungTu}";
                    var a = await RunHttpClientGet<PurchaseLinePackingList_Model>(url);
                    ListItem = a.Lists;
                }
                catch (System.Exception ex)
                {

                    await new MessageBox(ex.Message).Show();
                }
            });

            QuetOChuaCayVaiCommand = new Command(async () =>
            {
                try
                {
                    ScanBarcode scan = new ScanBarcode(false, "Quét kệ chứa cây vải");
                    scan.ScanBarcodeResult +=  (s, result) => 
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            CellPositionModel item = new CellPositionModel { Code = result };
                            var ok = await RunHttpClientPost("api/qltb/PostKiemTraOChuaVai", item);
                            if (ok.IsSuccessStatusCode)
                            {
                                ViTriOVai = result;
                                TitleButton = "Id : " + result;
                            }
                            else
                            {
                                await new MessageBox("Barcode này không tồn tại trong hệ thống").Show();
                            }
                        });

                    };
                    await navigation.PushAsync(scan);
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
        }
    }
}
