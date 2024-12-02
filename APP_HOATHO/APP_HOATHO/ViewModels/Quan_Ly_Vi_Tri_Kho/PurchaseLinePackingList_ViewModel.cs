using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Barcode;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho
{
    public class PurchaseLinePackingList_ViewModel : BaseViewModel
    {
        public INavigation navigation { get; set; }
        private string _titleButton;
        public string TitleButton { get => _titleButton; set => SetProperty(ref _titleButton, value); }
        private string SoChungTu;
        public string ViTriOVai;
        private ObservableCollection<PurchaseLinePackingList_Model> _listItem;
        public ObservableCollection<PurchaseLinePackingList_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public ICommand LoadDataCommand { get; set; }

        public ICommand DeleteBarcodeCommand { get; set; }

        public ICommand QuetOChuaCayVaiCommand { get; set; }

        public ICommand CapNhatSoLuongCayVai { get; set; }

        public ICommand CapNhatRollNo { get; set; }

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
                    scan.ScanBarcodeResult += (s, result) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            CellPositionModel item = new CellPositionModel { Code = result };
                            ShowLoading("Đang kiểm tra. vui lòng đợi");
                            var ok = await RunHttpClientPost("api/qltb/PostKiemTraOChuaVai", item);
                            HideLoading();
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
                    HideLoading();
                    await new MessageBox(ex.Message).Show();
                }
            });

            CapNhatSoLuongCayVai = new Command(async (obj) =>
            {
                try
                {
                    PurchaseLinePackingList_Model item = obj as PurchaseLinePackingList_Model;
                    if (item != null)
                    {
                        UpdateCayVaiRequest request = new UpdateCayVaiRequest { DocumentNo = item.DocumentNo, Id = item.Id, SoLuong = item.InvoicedMeter };
                        ShowLoading("Đang cập nhật. vui lòng đợi...");
                        var ok = await RunHttpClientPost($"api/qltb/CapNhatSoLuongPackingList", request);
                        HideLoading();
                        if (ok.IsSuccessStatusCode)
                        {
                            await new MessageBox("Cập nhật số lượng cây vải thành công").Show();
                        }
                        else
                        {
                            await new MessageBox(ok.Content.ReadAsStringAsync().Result).Show();
                        }
                    }
                }
                catch (Exception ex)
                {
                    HideLoading();
                    await new MessageBox(ex.Message).Show();
                }

            });

            CapNhatRollNo = new Command(async (obj) =>
            {
                try
                {
                    PurchaseLinePackingList_Model item = obj as PurchaseLinePackingList_Model;
                    if (item != null)
                    {
                        UpdateCayVaiRequest request = new UpdateCayVaiRequest { DocumentNo = item.DocumentNo, Id = item.Id, RollNo = item.RollNo };
                        ShowLoading("Đang cập nhật. vui lòng đợi...");
                        var ok = await RunHttpClientPost($"api/qltb/CapNhatRollNoPackingList", request);
                        HideLoading();
                        if (ok.IsSuccessStatusCode)
                        {
                            await new MessageBox("Cập nhật roll no thành công").Show();
                        }
                        else
                        {
                            await new MessageBox(ok.Content.ReadAsStringAsync().Result).Show();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowLoading("Đang cập nhật. vui lòng đợi...");
                    await new MessageBox(ex.Message).Show();
                }
                
            });
        }
    }
}