using APP_HOATHO.Dialog;
using APP_HOATHO.Views.Barcode;
using APP_HOATHO.Views.PhuLieu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.PhuLieu
{
        public class Chi_Tiet_Xuat_Phu_Lieu_ViewModel : BaseViewModel
    {
        public string SoChungTu { get; set; }
        public INavigation navigation { get; set; }
        ObservableCollection<Chi_Tiet_Xuat_Phu_Lieu_Model> _listItem;
        public ObservableCollection<Chi_Tiet_Xuat_Phu_Lieu_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public Chi_Tiet_Nhap_Phu_Lieu_Model SelectItem { get; set; }
        public ICommand LoadDataCommand { get; set; }
        public ICommand AddPackingListCommand { get; set; }

        public Chi_Tiet_Xuat_Phu_Lieu_ViewModel(string soChungTu)
        {
            SoChungTu = soChungTu;
            ListItem = new ObservableCollection<Chi_Tiet_Xuat_Phu_Lieu_Model>();
            LoadDataCommand = new Command(async () =>
            {
                try
                {
                    ListItem.Clear();
                    var url = $"api/PhuLieu/getChiTietXuatKhoPhuLieu?documentNo={SoChungTu}";
                    var a = await RunHttpClientGet<Chi_Tiet_Xuat_Phu_Lieu_Model>(url);
                    ListItem = a.Lists;
                }
                catch (System.Exception ex)
                {

                    await new MessageBox(ex.Message).Show();
                }

            });

            AddPackingListCommand = new Command(async () => {
                try
                {
                    ScanBarcode scan = new ScanBarcode(false, "Quét QR thùng PL");
                    scan.ScanBarcodeResult += (s, result) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var tonkho = (await RunHttpClientGet<Chi_Tiet_Xuat_Phu_Lieu_Model>($"api/PhuLieu/getTonKhoTheoBarcode?barcodeid={result}")).Lists;
                            //kiểm tra xem phu liệu theo barcode có trong phiếu xuất kho
                            var check = ListItem.Any(x => tonkho.Any(i => i.No_ == x.No_ && i.Color == x.Color));
                            if (!check)
                            {
                                await new MessageBox($"Barcode {result} không có chứa mã phụ liệu nào cần xuất cho phiếu xuất này cả!. vui lòng thử thùng khác").Show();
                                return;
                            }    

                            var listData = tonkho.Join(ListItem , x=> new { x.No_ , x.Color}, y=> new { y.No_, y.Color}, (x,y)=> new Chi_Tiet_Xuat_Phu_Lieu_Model
                            {
                                No_ = x.No_,
                                Color = x.Color,    
                                Art = x.Art,
                                DocumentNo = y.DocumentNo,
                                Name = x.Name,
                                Quantity = y.Quantity,
                                TonKho = x.TonKho,
                                SoLuongDaXuat = y.SoLuongDaXuat,
                                SoLuongCanXuat = 0                                
                            }).ToList();                           

                            await navigation.PushAsync(new Xuat_Barcode_Cho_Phieu_Xuat_Phu_Lieu_Page(listData, result));
                        });
                    };
                    await navigation.PushAsync(scan);
                    
                }
                catch (System.Exception ex)
                {

                    await new MessageBox(ex.Message).Show();
                }

            });

            MessagingCenter.Subscribe<Xuat_Barcode_Cho_Phieu_Xuat_Phu_Lieu_Page>(this, "reloadXuatKhoPhuLieu", (e) => LoadDataCommand.Execute(null));
        }
    }
}
