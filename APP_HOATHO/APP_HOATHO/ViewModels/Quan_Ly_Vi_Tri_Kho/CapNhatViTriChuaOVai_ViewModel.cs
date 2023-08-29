using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Nha_May_Soi;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Barcode;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using ZXing.Net.Mobile.Forms;

namespace APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho
{   
    public class CapNhatViTriChuaOVai_ViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        
        ObservableCollection<CellPositionModel> _listItem;
        public ObservableCollection<CellPositionModel> ListItem
        {
            get => _listItem; set => SetProperty(ref _listItem, value);
        }


        ObservableCollection<LookupValue> _listViTri;
        public ObservableCollection<LookupValue> ListViTri
        {
            get => _listViTri; set => SetProperty(ref _listViTri, value);
        }
        public ICommand QuetQR { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand CapNhatCommand { get; set; }
        public CapNhatViTriChuaOVai_ViewModel()        
        {
           
            ListItem = new ObservableCollection<CellPositionModel>();
            ListViTri = new ObservableCollection<LookupValue>();
            Task.Factory.StartNew(async () =>
            {
                var a = await RunHttpClientGet<LookupValue>("api/qltb/GetDanhSachViTriKho");
                ListViTri = a.Lists;
            });
          

            QuetQR = new Command(async () =>
            {
                try
                {
                    ScanBarcode scan = new ScanBarcode(false, "Quét kệ chứa vải");
                    scan.ScanBarcodeResult += (s, result) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {                              

                                if (ListItem.Where(x => x.Code == result).Any())
                                {
                                    DependencyService.Get<IMessage>().ShortAlert("Đã quét rồi. không quét nữa");
                                    return;
                                }

                                CellPositionModel item = new CellPositionModel { Code = result, PositionId = "00.00.00" };
                                var ok1 = await RunHttpClientPost("api/qltb/PostKiemTraOChuaVai", item);
                                if (!ok1.IsSuccessStatusCode)
                                {
                                    await new MessageBox($"Kệ: {result} này không tồn tại trong hệ thống").Show();
                                    return;
                                }

                                ListItem.Add(item);
                                DependencyService.Get<IMessage>().LongAlert("Đã quét thành công");


                                


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

            SearchCommand = new Command(async (obj) =>
            {
                CellPositionModel item = obj as CellPositionModel;
                if (item != null)
                {
                    var lookup = new LookupItem(ListViTri, "Danh sách vị trí kho");
                    lookup.closeForm += (s, e) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            item.PositionId = e.Code;
                            CellPositionModel cell = ListItem.FirstOrDefault(x => x.Code == item.Code);
                            if (cell != null)
                            {
                                cell.PositionId = e.Code;
                            }
                        });               
                    };

                    await Navigation.PushModalAsync(lookup);
                }

            });

            CapNhatCommand = new Command( async () =>
            {
                try
                {
                    var ask = await new MessageYesNo("Bạn có muốn cập nhật không").Show();
                    if (ask == DialogReturn.OK)
                    {
                        var ok = await RunHttpClientPost($"api/qltb/ThayDoiViTriChuaOVai", ListItem);
                        if (ok.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            await new MessageBox("Đã cập nhật thành công").Show();
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await new MessageBox(ok.Content.ReadAsStringAsync().Result.ToString()).Show();
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
