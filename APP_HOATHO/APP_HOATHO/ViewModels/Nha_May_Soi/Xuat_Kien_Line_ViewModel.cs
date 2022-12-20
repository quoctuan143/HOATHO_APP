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
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace APP_HOATHO.ViewModels.Nha_May_Soi
{
   public  class Xuat_Kien_Line_ViewModel : BaseViewModel
    {
        #region "Field"
        ObservableCollection<Xuat_Kien_Line_Model> _listItem;       
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }
        public Command ApproveCommand { get; set; }
        #endregion

        #region "Constructor"
        public INavigation navigation { get; set; }
        public ObservableCollection<Xuat_Kien_Line_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public Xuat_Kien_Header_Model KiemChungTuModel { get; set; }


        public Xuat_Kien_Line_ViewModel(Xuat_Kien_Header_Model duyetChungTuModel) 
        {
            this.KiemChungTuModel = duyetChungTuModel;          
            Title = "Chi tiết NVL";
            ListItem = new ObservableCollection<Xuat_Kien_Line_Model>();
            LoadCommand = new Command(OnLoadExcute);
            ApproveCommand = new Command(OnApproveClick);
        }

        private async void OnApproveClick(object obj)
        {
            try
            {
                var asked = await new MessageYesNo("Bạn có muốn xác nhận và gửi mail không?").Show();
                if (asked == DialogReturn.OK)
                {
                    ShowLoading("Đang xử lý vui lòng đợi...");
                    var result = await RunHttpClientPost("api/Soi/XacNhanVaGuiMail", KiemChungTuModel);
                    HideLoading();
                    if (result.IsSuccessStatusCode)
                    {
                        DependencyService.Get<IMessage>().LongAlert("Xác nhận và gửi mail thành công");
                        await navigation.PopAsync();
                    }
                    else
                        await new MessageBox(result.Content.ReadAsStringAsync().Result).Show();
                }
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
                string url = $"api/Soi/getXuatKienLine?sochungtu={KiemChungTuModel.No_}";
                ListItem.Clear();
                var a = await RunHttpClientGet<Xuat_Kien_Line_Model>(url);
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
