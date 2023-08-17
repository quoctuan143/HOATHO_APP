using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Barcode;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho
{
    public class XuatKhoTheoCayVai_ViewModel : BaseViewModel    
    {
        
        public INavigation Navigation { get; set; }
        DanhSachPhieuXuatKhoChiTiet_Model XuatKho { get; set; }
        public XuatKhoTheoCayVai_Model SelectItem { get; set; } 
        private ObservableCollection<XuatKhoTheoCayVai_Model> _listItem;
        public ObservableCollection<XuatKhoTheoCayVai_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }

        public ICommand QuetQrCommand { get; set; }
        public ICommand CapNhatCommand { get; set; }
        public ICommand DeleteIdVaiCommand { get; set; } 
        public XuatKhoTheoCayVai_ViewModel(DanhSachPhieuXuatKhoChiTiet_Model xuatKho)
        {
            XuatKho = xuatKho;
            Title = "Xuất kho " + string.Format("{0:#,##0.##}",xuatKho.Quantity);
            ListItem = new ObservableCollection<XuatKhoTheoCayVai_Model>();
            QuetQrCommand = new Command( async() =>
            {
                try
                {
                    //kiem tra nếu đủ tồn rồi thì k cần quét nữa
                    double sum1 = 0;
                    ListItem.ForEach(x =>
                    {
                        sum1 += x.CanXuat;
                    });
                    if (sum1 >= xuatKho.Quantity)
                    {
                        await new MessageBox("Số lượng cây vải đã xuất đủ cho mã NPL này").Show();
                        return;
                    }
                    ScanBarcode scan = new ScanBarcode(true, "Quét BarcodeId cây vải");
                    scan.ScanBarcodeResult += (s, result) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                if (ListItem.Where(x => x.BarcodeId == result && x.RowID == "0").Any())
                                {                                   
                                    await new MessageBox("Mã này đã được quét rồi. không quét nữa").Show();
                                    return;
                                }

                                var ok = await RunHttpClientGet<XuatKhoTheoCayVai_Model>($"api/qltb/GetTonKhoTheoCayVai?barcodeId={result}");
                                if (ok.Status.IsSuccessStatusCode)
                                {
                                    if (ok.Lists.Count > 0) // chỉ trả về 1 dòng thôi
                                    {
                                        //kiểm tra xem mã NPL này có đúng với mã cần xuất không                                        

                                        XuatKhoTheoCayVai_Model ketqua = ok.Lists[0];
                                        if (ketqua.ItemNo != xuatKho.ItemNo || ketqua.Color != xuatKho.Color)
                                        {
                                            await new MessageBox("Cây vải này không đúng với mã NPL mà bạn cần xuất kho").Show();
                                            return;
                                        }

                                        double sum = 0;
                                        ListItem.ForEach(x =>
                                        {
                                            sum += x.CanXuat;
                                        });
                                        if (sum <= xuatKho.Quantity)
                                        {
                                            if (sum + ketqua.TonKho > xuatKho.Quantity)
                                            {
                                                ketqua.CanXuat = xuatKho.Quantity - sum;
                                            }
                                            else
                                            {
                                                ketqua.CanXuat = ketqua.TonKho;
                                            }
                                            if (ketqua.CanXuat > 0)
                                            {
                                                ketqua.UserId = Preferences.Get(Config.User, "");
                                                ketqua.DocumentNo = XuatKho.DocumentNo;
                                                ListItem.Add(ketqua);
                                                DependencyService.Get<IMessage>().LongAlert("Đã quét thành công");
                                            }
                                            if (sum + ketqua.CanXuat == xuatKho.Quantity)
                                            {
                                                await new MessageBox("Số lượng cây vải đã xuất đủ cho mã NPL này").Show();
                                                await Navigation.PopAsync();
                                            }
                                        }
                                        else
                                        {
                                            await new MessageBox("Số lượng cây vải đã xuất đủ cho mã NPL này").Show();
                                        }
                                    }
                                    else
                                    {
                                        await new MessageBox("Barcode này chưa có trong hệ thống").Show();
                                    }
                                }
                                else
                                {
                                    await new MessageBox(ok.ErrorString).Show();
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
            });
            CapNhatCommand = new Command(async () =>
            {
                try
                {                  
                   if (ListItem.Count==0)
                    {
                        await new MessageBox("Chưa có danh sách xuất kho").Show();
                        return;
                    }    
                   if (await new MessageYesNo("Bạn có muốn xuất những cây vải này không?").Show() == DialogReturn.OK)
                    {
                        var ok  = await RunHttpClientPost("api/qltb/XuatKhoTheoDanhSachIdCayVai",ListItem.Where(x=>x.RowID == "0").ToList());
                        if (ok.IsSuccessStatusCode)
                        {
                            await new MessageBox("Cập nhật thành công").Show();
                            await Navigation.PopAsync();
                        }    
                    }    
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
            DeleteIdVaiCommand = new Command(async () =>
            {
                try
                {             
                    if (SelectItem == null)
                    {
                        await new MessageBox("Vui lòng chọn cây vải cần xóa").Show();
                        return;
                    }    
                    if (SelectItem.RowID != "0")
                    {
                        await new MessageBox("Cây vải này đã xuất kho rồi nên không xóa được").Show();
                        return;
                    }    
                    if (await new MessageYesNo("Bạn có muốn xóa cây vải này không?").Show() == DialogReturn.OK)
                    {
                        var ok = await RunHttpClientPost("api/qltb/XoaXuatKhoTheoIdCayVai", SelectItem);
                        if (ok.IsSuccessStatusCode)
                        {
                            DependencyService.Get<IMessage>().ShortAlert("Đã xóa thành công!");
                            ListItem.Remove(SelectItem);                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
            OnAppearing();
        }

        protected override async void OnAppearing()
        {
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                ShowLoading("Đang tải dữ liệu. vui lòng đợi");
                await Task.Delay(1000);
                string url = $"api/qltb/GetIdCayVaiTheoPhieuXuat?itemno={XuatKho.ItemNo}&color={XuatKho.Color}&documentno={XuatKho.DocumentNo}";
                ListItem.Clear();
                var a = await RunHttpClientGet<XuatKhoTheoCayVai_Model>(url);
                ListItem = a.Lists;
                HideLoading();
            }
            catch (Exception ex)
            {
                IsBusy = false;
                HideLoading();
                await new MessageBox(ex.Message).Show();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}