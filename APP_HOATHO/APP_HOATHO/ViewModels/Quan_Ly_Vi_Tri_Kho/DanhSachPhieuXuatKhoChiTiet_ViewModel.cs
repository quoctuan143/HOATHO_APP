using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho
{
    public class DanhSachPhieuXuatKhoChiTiet_ViewModel : BaseViewModel
    {
        public INavigation navigation { get; set; }
        public DanhSachPhieuXuatKhoChiTiet_Model SelectItem { get; set; }
        private ObservableCollection<DanhSachPhieuXuatKhoChiTiet_Model> _listItem;
        public ObservableCollection<DanhSachPhieuXuatKhoChiTiet_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }

        public ICommand XemViTriKhoCommand { get; set; }
        public ICommand ViTriKhoTheoChungTuCommand { get; set; }
        public ICommand XuatKhoCommand { get; set; }

        public ICommand UpdateSLKhauTruCommand { get; set; }
        private string SoChungTu;

        public DanhSachPhieuXuatKhoChiTiet_ViewModel(string sochungtu)
        {
            SoChungTu = sochungtu;
            ListItem = new ObservableCollection<DanhSachPhieuXuatKhoChiTiet_Model>();
            XemViTriKhoCommand = new Command(async () =>
            {
                try
                {
                    if (SelectItem != null)
                    {
                        var ok = await RunHttpClientGet<PurchaseLinePackingList_Model>($"api/qltb/GetViTriTheoMaNPL?itemno={SelectItem.ItemNo}&color={SelectItem.Color}");
                        if (ok.Status.IsSuccessStatusCode)
                        {
                            ObservableCollection<PurchaseLinePackingList_Model> Source = new ObservableCollection<PurchaseLinePackingList_Model>();
                            Source = ok.Lists;
                            //show form danh mục tồn lên
                            await navigation.PushAsync(new ViTriKhoTheoNPL_Page(Source));
                        }
                        else
                        {
                            await new MessageBox(ok.ErrorString).Show();
                        }
                    }
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
            XuatKhoCommand = new Command(async () =>
            {
                try
                {                 
                    if (SelectItem != null)
                    {
                        var page = new XuatKhoTheoCayVai_Page(SelectItem);
                        page.ClosePage += (s, e) =>
                        {
                            ListItem.ForEach(x =>
                            {
                                if (x.ItemNo == SelectItem.ItemNo && x.Color == SelectItem.Color)
                                {
                                    x.XuatTheoIdVai = e;
                                }
                            });
                        };
                        await navigation.PushAsync(page);
                    }
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
            ViTriKhoTheoChungTuCommand = new Command(async () =>
            {
                try
                {
                    var ok = await RunHttpClientGet<PurchaseLinePackingList_Model>($"api/qltb/GetDanhSachViTriTheoPhieuXuat?sochungtu={SoChungTu}");
                    if (ok.Status.IsSuccessStatusCode)
                    {
                        ObservableCollection<PurchaseLinePackingList_Model> Source = new ObservableCollection<PurchaseLinePackingList_Model>();
                        Source = ok.Lists;
                        //show form danh mục tồn lên
                        await navigation.PushAsync(new ViTriKhoTheoChungTu_Page(Source));
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
            UpdateSLKhauTruCommand = new Command(async (obj) =>
            {
                DanhSachPhieuXuatKhoChiTiet_Model item = obj as DanhSachPhieuXuatKhoChiTiet_Model;
                if (item != null)
                {
                    ShowLoading("Đang cập nhật. vui lòng đợi...");
                    var ok = await Config.client.GetAsync($"api/qltb/CapNhatSoLuongKhauTru?rowid={item.RowID}&soluong={item.SoLuongKhauTru}");
                    HideLoading();
                    if (ok.IsSuccessStatusCode)
                    {
                        await new MessageBox("Cập nhật số lượng khấu trừ thành công").Show();
                    }    
                   else
                    {
                        await new MessageBox(ok.Content.ReadAsStringAsync().Result).Show();
                    }    

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
                string url = $"api/qltb/GetChiTietPhieuXuatKho?sochungtu={SoChungTu}";
                ListItem.Clear();
                var a = await RunHttpClientGet<DanhSachPhieuXuatKhoChiTiet_Model>(url);
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