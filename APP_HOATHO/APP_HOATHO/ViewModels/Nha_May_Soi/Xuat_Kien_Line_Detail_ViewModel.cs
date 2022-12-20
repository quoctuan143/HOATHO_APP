using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Kiem_Ke_Thiet_Bi;
using APP_HOATHO.Models.Nha_May_Soi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace APP_HOATHO.ViewModels.Nha_May_Soi
{
    public class Xuat_Kien_Line_Detail_ViewModel : BaseViewModel
    {
        #region "Field"
        ObservableCollection<Xuat_Kien_Line_Detail_Model> _listItem;
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }
        public Command QuetQRCodeCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public Command UpdateAllCommand { get; set; }
        #endregion

        #region "Constructor"
        public INavigation navigation { get; set; }
        public ObservableCollection<Xuat_Kien_Line_Detail_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public Xuat_Kien_Line_Model KiemChungTuModel { get; set; }


        public Xuat_Kien_Line_Detail_ViewModel(Xuat_Kien_Line_Model duyetChungTuModel)
        {
            this.KiemChungTuModel = duyetChungTuModel;
            Title = duyetChungTuModel.Ten_NVL;
            ListItem = new ObservableCollection<Xuat_Kien_Line_Detail_Model>();
            LoadCommand = new Command(OnLoadExcute);
            QuetQRCodeCommand = new Command(OnScanQrCodeClick);
            DeleteCommand = new Command(OnDeleteClick);
            UpdateAllCommand = new Command(OnUpdateAllClick);
        }

        private async void OnUpdateAllClick(object obj)
        {
            try
            {
                var asked = await new MessageYesNo("Bạn có muốn cập nhật không?").Show();
                if (asked == DialogReturn.OK)
                {
                    ShowLoading("Đang cập nhật vui lòng đợi....");
                    var listupdate = ListItem.Where(x => x.StatusUpdate == StatusUpdate.Status.Add || x.StatusUpdate == StatusUpdate.Status.Modified).ToList();
                    var result = await RunHttpClientPost("api/Soi/postXuatKienLineDetail", listupdate);
                    HideLoading();
                    if (result.IsSuccessStatusCode)
                    {
                        DependencyService.Get<IMessage>().LongAlert("Đã cập nhật thành công");
                        await navigation.PopAsync();
                    }
                    else
                    {
                        await new MessageBox(result.Content.ReadAsStringAsync().Result.ToString()).Show();
                    }

                }
            }
            catch (Exception)
            {

                HideLoading();
            }

        }

        private async void OnDeleteClick(object obj)
        {
            Xuat_Kien_Line_Detail_Model item = obj as Xuat_Kien_Line_Detail_Model;
            if (item != null)
            {
                var asked = await new MessageYesNo("Bạn có muốn xóa không?").Show();
                if (asked == DialogReturn.OK)
                {
                    if (item.StatusUpdate == StatusUpdate.Status.Add)
                    {
                        ListItem.Remove(item);
                    }
                    else
                    {
                        //xóa trên database
                        var delete = await RunHttpClientPost("api/Soi/deleteXuatKienLineDetail", item);
                        if (delete.IsSuccessStatusCode)
                        {
                            ListItem.Remove(item);
                        }
                        else
                        {
                            await new MessageBox(delete.Content.ReadAsStringAsync().Result.ToString()).Show();
                        }
                    }
                    DependencyService.Get<IMessage>().LongAlert("Đã xóa thành công");
                }
            }

        }
        #endregion

        #region "Method"        
        private async void OnLoadExcute(object obj)
        {
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                ShowLoading("Đang tải vui lòng đợi");
                await Task.Delay(1000);
                string url = $"api/Soi/getXuatKienLineDetail?sochungtu={KiemChungTuModel.Document_No_}&lineno={KiemChungTuModel.Line_No_}";
                ListItem.Clear();
                var a = await RunHttpClientGet<Xuat_Kien_Line_Detail_Model>(url);
                ListItem = a.Lists;
                HideLoading();
            }
            catch (Exception ex)
            {
                HideLoading();
                await new MessageBox(ex.Message).Show();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnScanQrCodeClick(object obj)
        {
            try
            {
                var scan = new ZXingScannerPage();
                scan.Title = "Tìm kiếm kiện";
                Shell.SetTabBarIsVisible(scan, false);
                await navigation.PushAsync(scan);
                scan.OnScanResult += (result) =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //show form lên
                        try
                        {
                            var kiemchungtu = KiemChungTuModel;
                            if (IsBusy) return;

                            IsBusy = true;

                            string ma = result.Text.Split('_')[0];
                            if (ListItem.Where(x => x.Package_ID == ma).Any())
                            {
                                DependencyService.Get<IMessage>().ShortAlert($"Mã kiện {ma} đã có trong phiếu này rồi. Vui lòng quét kiện khác.");
                                return;
                            }
                            var a = await RunHttpClientGet<Package_Model>("api/Soi/getPackageId?packageid=" + ma + "&item=" + kiemchungtu.Item_No_);
                            if (a.Lists.Count == 0)
                            {
                                DependencyService.Get<IMessage>().ShortAlert("Không tìm thấy kiện này trong hệ thống");
                                return;
                            }
                            //lưu xuống database
                            List<Xuat_Kien_Line_Detail_Model> item = new List<Xuat_Kien_Line_Detail_Model>
                           {
                                new  Xuat_Kien_Line_Detail_Model
                               {
                                RowID = 0,
                                Document_No_ = kiemchungtu.Document_No_,
                                Unit_of_Measure = a.Lists[0].Unit_of_Measure,
                                Item_No_ = kiemchungtu.Item_No_,
                                Line_No_ = kiemchungtu.Line_No_,
                                Package_ID = ma,
                                Quantity = a.Lists[0].Quantity,
                                StatusUpdate = StatusUpdate.Status.Add
                            }
                           };

                            var result1 = await RunHttpClientPost("api/Soi/postXuatKienLineDetail", item);
                            if (result1.IsSuccessStatusCode)
                            {
                                string url = $"api/Soi/getXuatKienLineDetail?sochungtu={KiemChungTuModel.Document_No_}&lineno={KiemChungTuModel.Line_No_}";
                                ListItem.Clear();
                                var a1 = await RunHttpClientGet<Xuat_Kien_Line_Detail_Model>(url);
                                ListItem = a1.Lists;
                                await navigation.PopAsync();
                            }
                            else
                            {
                                await new MessageBox(result1.Content.ReadAsStringAsync().Result.ToString()).Show();
                            }

                        }
                        catch
                        {
                            DependencyService.Get<IMessage>().LongAlert("Barcode không tồn tại trong hệ thống");

                        }
                        finally
                        {
                            IsBusy = false;
                        }
                    });

                };
            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }
        }
        #endregion
    }
}
