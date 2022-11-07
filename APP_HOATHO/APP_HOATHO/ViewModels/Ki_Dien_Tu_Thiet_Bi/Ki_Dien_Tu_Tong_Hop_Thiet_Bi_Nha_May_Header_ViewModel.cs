using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.KiDienTuPhuTung;
using APP_HOATHO.Models.Kiem_Ke_Thiet_Bi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.Ki_Dien_Tu_Thiet_Bi
{
   public  class Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header_ViewModel :BaseViewModel 
    {
        #region "Field"

        ObservableCollection<Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header> _listItem;
       
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }


        #endregion

        #region "Constructor"       
        public ObservableCollection<Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header> ListItem { get => _listItem; set { SetProperty(ref _listItem, value); } }

        public Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header_ViewModel()
        {            
            Title = "Kí tổng hợp thiết bị";
            ListItem = new ObservableCollection<Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header>();
            LoadCommand = new Command(OnLoadExcute);
            // form sẽ gửi về là form DuyetChungTu_Line , giá trị trả về là 1 docno khi duyệt 1 chứng từ thì xóa nó đi
            MessagingCenter.Subscribe<Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Line_ViewModel, string>(this, "DuyetChungTu", (sender, docno) =>
            {
                var q = ListItem.Where(p => p.No_ == docno).FirstOrDefault();
                if (q != null)
                {
                    ListItem.Remove(q);
                    //gửi về form main để trừ đi thông báo
                    MessagingCenter.Send(this, "langngheduyet", DocumentType.KyDienTuTongHopThietBiNhaMay);
                    OnPropertyChanged(nameof(ListItem));//cập nhật lại thông tin lên view
                }
            });
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
                var a = await RunHttpClientGet<Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header>($"api/DuyetChungTu/getKiDienTuTongHop_NhaMay?username={Preferences.Get(Config.User, "")}");
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
