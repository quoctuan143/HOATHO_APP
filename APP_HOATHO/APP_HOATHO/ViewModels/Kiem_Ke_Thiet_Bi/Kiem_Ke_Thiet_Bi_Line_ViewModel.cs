using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Kiem_Ke_Thiet_Bi;
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

namespace APP_HOATHO.ViewModels.Kiem_Ke_Thiet_Bi
{
   public  class Kiem_Ke_Thiet_Bi_Line_ViewModel : BaseViewModel
    {
        #region "Field"
        ObservableCollection<Kiem_Ke_Thiet_Bi_Line_Model> _listItem;       
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }
        public Command QuetQRCodeCommand { get; set; }
        #endregion

        #region "Constructor"
        public INavigation navigation { get; set; }
        public ObservableCollection<Kiem_Ke_Thiet_Bi_Line_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public Kiem_Ke_Thiet_Bi_Header_Model KiemChungTuModel { get; set; }


        public Kiem_Ke_Thiet_Bi_Line_ViewModel(Kiem_Ke_Thiet_Bi_Header_Model duyetChungTuModel) 
        {
            this.KiemChungTuModel = duyetChungTuModel;          
            Title = "Chi tiết kiểm kê";
            ListItem = new ObservableCollection<Kiem_Ke_Thiet_Bi_Line_Model>();
            LoadCommand = new Command(OnLoadExcute);
            QuetQRCodeCommand = new Command(OnScanQrCodeClick);
        }
        #endregion

        #region "Method"
        private async void OnScanQrCodeClick(object obj) 
        {
            try
            {
                var scan = new ZXingScannerPage();
                scan.Title = "Tìm kiếm thiết bị";
                Shell.SetTabBarIsVisible(scan, false);
                await navigation.PushAsync(scan);
                scan.OnScanResult += (result) =>
                {
                    Device.BeginInvokeOnMainThread(async () => {                        
                        //show form lên
                        try
                        {
                            string ma = result.Text.Split('=')[1];
                            var a = await RunHttpClientGet<DanhMuc_ThietBi>("api/qltb/getTimKiemThietBi?mathietbi=" + ma);
                            if (a.Lists.Count == 0)
                            {
                                DependencyService.Get<IMessage>().LongAlert("Không tìm thấy thiết bị này trong hệ thống");
                                return;
                            }    
                            //lưu xuống db
                            var item = new Kiem_Ke_Thiet_Bi_Line_Model
                            {
                                Document_No_ = KiemChungTuModel.No_,
                                Don_Vi_Tinh = "CAI",
                                Ma_Thiet_Bi = ma,
                                Ten_Thiet_Bi = a.Lists [0].NameVN,
                                Model = a.Lists[0].No_2,
                                Serial = a.Lists[0].No_3,
                                Nguoi_Kiem_Ke = Preferences.Get(Config.User, ""),
                                So_Luong = 1                               
                            };    
                            
                            var res = await RunHttpClientPost("api/qltb/CapNhatThietBiKiemKe", item);
                            if (res.StatusCode == System.Net.HttpStatusCode.OK)
                            {                                
                                if (ListItem.Where(x=> x.Ma_Thiet_Bi == item.Ma_Thiet_Bi).Count () == 0)
                                {
                                    DependencyService.Get<IMessage>().LongAlert("Đã cập nhật thành công");
                                    ListItem.Add(item);
                                    OnPropertyChanged(nameof(ListItem));
                                }   
                            }
                            else
                                await new MessageBox(res.Content.ReadAsStringAsync().Result).Show();

                            await navigation.PopAsync();
                        }
                        catch
                        {
                            DependencyService.Get<IMessage>().LongAlert("Barcode không tồn tại trong hệ thống");

                        }
                    });

                };
            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }          
        }
        private async void OnLoadExcute(object obj)
        {
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                ShowLoading("Đang tải vui lòng đợi");
                await Task.Delay(1000);
                string url = $"api/qltb/GetDanhSachKiemKeLine?documentNo={KiemChungTuModel.No_}";
                ListItem.Clear();
                var a = await RunHttpClientGet<Kiem_Ke_Thiet_Bi_Line_Model>(url);
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
        #endregion
    }
}
