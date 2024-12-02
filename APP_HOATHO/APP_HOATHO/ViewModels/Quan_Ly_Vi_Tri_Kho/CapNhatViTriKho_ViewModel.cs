using APP_HOATHO.Dialog;
using APP_HOATHO.Interface;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Barcode;
using APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho
{
    public class CapNhatViTriKho_ViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public string MaViTri { get; set; }
        ObservableCollection<PurchaseLinePackingList_Model> _listItem;
        public ObservableCollection<PurchaseLinePackingList_Model> ListItem
        {
            get => _listItem; set => SetProperty(ref _listItem, value);
        }       

        public ICommand QuetQR { get; set; }    
        public CapNhatViTriKho_ViewModel(string _maViTri)
        {
            MaViTri = _maViTri;
            ListItem = new ObservableCollection<PurchaseLinePackingList_Model>();

            QuetQR = new Command( async() =>
            {
                try
                {
                    ScanBarcode scan = new ScanBarcode(true, "Quét BarcodeId cây vải",true);
                    scan.ScanBarcodeResult += (s, result) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {                                
                                if (ListItem.Where(x => x.BarcodeId == result).Any())
                                {
                                    DependencyService.Get<IMessage>().ShortAlert("Đã quét rồi. không quét nữa");
                                    return;
                                }
                                PurchaseLinePackingList_Model item = new PurchaseLinePackingList_Model
                                {
                                    BarcodeId = result,
                                    PositionId = MaViTri
                                };
                                ShowLoading("Đang cập nhật. vui lòng đợi...");
                                var ok = await RunHttpClientPost("api/qltb/CapNhatOChuaCayVai", item);
                                HideLoading();
                                if (ok.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    ListItem.Add(item);
                                    DependencyService.Get<IMessage>().LongAlert("Đã quét thành công");
                                }
                                else
                                {
                                    await new MessageBox(ok.Content.ReadAsStringAsync().Result.ToString()).Show();
                                }
                            }
                            catch (Exception ex)
                            {
                                HideLoading();
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
            });
        }
    }
}
