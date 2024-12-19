using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.KiDienTuPhuTung;
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

namespace APP_HOATHO.ViewModels.Ki_Dien_Tu_Thiet_Bi 
{
   public  class Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Line_ViewModel : BaseViewModel
    {
        #region "Field"
        ObservableCollection<Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Line> _listItem;       
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }
        public Command ApproveCommand { get; set; }
        #endregion

        #region "Constructor"
        public INavigation navigation { get; set; }
        public ObservableCollection<Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Line> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header KiemChungTuModel { get; set; }


        public Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Line_ViewModel(Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header duyetChungTuModel) 
        {
            this.KiemChungTuModel = duyetChungTuModel;          
            Title = "Chi tiết tổng hợp";
            ListItem = new ObservableCollection<Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Line>();
            LoadCommand = new Command(OnLoadExcute);
            ApproveCommand = new Command(ApproveClick);
        }
        #endregion

        #region "Method"
        private async void ApproveClick(object obj) 
        {
            try
            {
                KiemChungTuModel.UserNameApprove = Preferences.Get(Config.FullName, "");
                var ok = await new MessageYesNo("Bạn có muốn duyệt không?").Show();
                if (ok == DialogReturn.OK )
                {
                    ShowLoading("Đang xử lý....");
                    var result = await RunHttpClientPost("api/DuyetChungTu/DuyetKiDienTuTongHop_NhaMay", KiemChungTuModel);
                    HideLoading();
                    if (result.StatusCode == System.Net.HttpStatusCode.OK )
                    {                      
                       await navigation.PopAsync();
                       DependencyService.Get<IMessage>().LongAlert("Đã duyệt và gửi mail thành công");
                       MessagingCenter.Send(this, "DuyetChungTu", KiemChungTuModel.No_);
                    }
                    
                }    
            }
            catch (Exception ex)
            {
                HideLoading();
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
                string url = $"api/DuyetChungTu/getKiDienTuTongHop_NhaMay_ChiTiet?documentno={KiemChungTuModel.No_}";
                ListItem.Clear();
                var a = await RunHttpClientGet<Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Line>(url);
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
