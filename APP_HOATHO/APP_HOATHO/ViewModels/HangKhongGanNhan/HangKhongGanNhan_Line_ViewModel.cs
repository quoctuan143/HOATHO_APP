using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models;
using APP_HOATHO.Models.HangKhongGanNhan;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Barcode;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;

namespace APP_HOATHO.ViewModels.HangKhongGanNhan
{
    public class HangKhongGanNhan_Line_ViewModel : BaseViewModel
    {
        public INavigation navigation { get; set; }
        private string _titleButton;
        public string TitleButton { get => _titleButton; set => SetProperty(ref _titleButton, value); }
        private string SoChungTu;
        public string ViTriOVai;
        private ObservableCollection<PurchaseLinePackingList_Model> _listItem;
        public ObservableCollection<PurchaseLinePackingList_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public ICommand LoadDataCommand { get; set; }
        public ICommand QuetOChuaCayVaiCommand { get; set; }
        public ICommand XuatKhoTheoPackingListCommand { get; set; }
        public HangKhongGanNhan_Line_ViewModel(string soChungTu)
        {
            TitleButton = "Quét kệ chứa cây vải";
            SoChungTu = soChungTu;
            ListItem = new ObservableCollection<PurchaseLinePackingList_Model>();
            LoadDataCommand = new Command(async () =>
            {
                try
                {
                    ListItem.Clear();
                    var url = $"api/HangKhongGanNhan/LayDanhSachPackingListHangKhongGanNhan?sochungtu={SoChungTu}";
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
                    var items = ListItem.Where(x => x.Chon == true).ToList();
                    if (items.Count == 0)
                    {
                        await new MessageBox("Vui lòng chọn cây vải trước khi xếp lên kệ").Show();
                        return;
                    }
                    var ask = await new MessageYesNo("Bạn có muốn định vị những cây vải này không?").Show();
                    if (ask == Global.DialogReturn.OK )
                    {
                        ScanBarcode scan = new ScanBarcode(false, "Quét kệ chứa cây vải");
                        scan.ScanBarcodeResult += (s, result) =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                CellPositionModel item = new CellPositionModel { Code = result };
                                ShowLoading("Đang kiểm tra kệ vải. vui lòng đợi");
                                var ok = await RunHttpClientPost("api/qltb/PostKiemTraOChuaVai", item);
                                HideLoading();
                                if (ok.IsSuccessStatusCode)
                                {
                                    items.ForEach(x =>
                                    {
                                        x.PositionId = result;
                                    });
                                    ShowLoading("Đang xử lý. vui lòng đợi");
                                    var post = await RunHttpClientPost($"api/HangKhongGanNhan/XepVaiVaoKe", items);
                                    HideLoading();
                                    if (post.IsSuccessStatusCode)
                                    {
                                        await new MessageBox("Cập nhật cây vải lên kệ thành công").Show();
                                        LoadDataCommand.Execute(null);
                                    }
                                    else
                                    {
                                        await new MessageBox(post.Content.ReadAsStringAsync().Result).Show();
                                    }
                                }
                                else
                                {
                                    await new MessageBox($"Barcode kệ vải {result} không tồn tại trong hệ thống").Show();
                                }
                            });
                        };
                        await navigation.PushAsync(scan);
                    }    
                    
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });

            XuatKhoTheoPackingListCommand = new Command(async () =>
            {
                try
                {
                    var items = ListItem.Where(x => x.Chon == true).ToList();
                    if (items.Count == 0)
                    {
                        await new MessageBox("Vui lòng chọn cây vải trước khi xuất").Show();
                        return;
                    }
                    var ask = await new MessageYesNo("Bạn có muốn xuất những cây vải này không?").Show();
                    if (ask == Global.DialogReturn.OK)
                    {
                        ShowLoading("Đang xử lý. vui lòng đợi...");
                        var post = await RunHttpClientPost($"api/HangKhongGanNhan/XuatVaiKhoiKe?userId={Preferences.Get(Config.User, "")}", items);
                        HideLoading();
                        if (post.IsSuccessStatusCode)
                        {
                            await new MessageBox("Xuất vải ra khỏi kệ thành công").Show();
                            foreach (var item in ListItem)
                            {
                                item.Chon = false;
                            }
                            await navigation.PopAsync();
                        }
                        else
                        {
                            await new MessageBox(post.Content.ReadAsStringAsync().Result).Show();
                        }
                    }

                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
        }
    }
}