using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models.KiDienTuPhuTung;
using APP_HOATHO.Models.Kiem_Ke_Thiet_Bi;
using APP_HOATHO.Models.Nha_May_Soi;
using APP_HOATHO.Views.Nha_May_Soi.Xuat_Kien_NVL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace APP_HOATHO.ViewModels.Nha_May_Soi
{
   public  class Xuat_Kien_Header_ViewModel : BaseViewModel 
    {
        #region "Field"

        ObservableCollection<Xuat_Kien_Header_Model> _listItem;
        public INavigation navigation { get; set; }
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }

        public Command QuetQRComamand { get; set; } 
        #endregion

        #region "Constructor"       
        public ObservableCollection <Xuat_Kien_Header_Model> ListItem { get => _listItem; set { SetProperty(ref _listItem, value); } }

        public Xuat_Kien_Header_ViewModel()
        {            
            Title = "Đề nghị cấp NVL";
            ListItem = new ObservableCollection<Xuat_Kien_Header_Model>();
            LoadCommand = new Command(OnLoadExcute);
            QuetQRComamand = new Command(OnQuetQrClick);
        }

        private async void OnQuetQrClick(object obj)
        {
            try
            {
                var scan = new ZXingScannerPage();
                scan.Title = "Tìm kiếm chứng từ";
                Shell.SetTabBarIsVisible(scan, false);
                await navigation.PushAsync(scan);
                scan.OnScanResult += (result) =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //show form lên
                        try
                        {                          
                            if (IsBusy) return;

                            IsBusy = true;

                            string ma = result.Text;
                            Xuat_Kien_Header_Model _selectItem = ListItem.Where(x => x.External_Document_No_ == ma).FirstOrDefault();
                            if (_selectItem == null )
                            {
                                DependencyService.Get<IMessage>().ShortAlert($"Mã phiếu {ma} không có trong danh sách");
                                return;
                            }
                            await navigation.PushAsync(new Xuat_Kien_Line_Page(_selectItem));
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

        #region "Method"
        private async void OnLoadExcute(object obj)
        {
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                ShowLoading("Đang tải vui lòng đợi");
                await Task.Delay(1000);
                ListItem.Clear();
                var a = await RunHttpClientGet<Xuat_Kien_Header_Model>($"api/Soi/getXuatKienHeader?username={Preferences.Get(Config.User, "")}");
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
