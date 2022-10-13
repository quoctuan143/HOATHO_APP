using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.KiDienTuPhuTung;
using APP_HOATHO.Models.Kiem_Ke_Thiet_Bi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.Kiem_Ke_Thiet_Bi
{
   public  class Kiem_Ke_Thiet_Bi_Header_ViewModel :BaseViewModel 
    {
        #region "Field"

        ObservableCollection<Kiem_Ke_Thiet_Bi_Header_Model> _listItem;
       
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }


        #endregion

        #region "Constructor"       
        public ObservableCollection<Kiem_Ke_Thiet_Bi_Header_Model> ListItem { get => _listItem; set { SetProperty(ref _listItem, value); } }

        public Kiem_Ke_Thiet_Bi_Header_ViewModel()
        {            
            Title = "Kiểm Kê Thiết Bị";
            ListItem = new ObservableCollection<Kiem_Ke_Thiet_Bi_Header_Model>();
            LoadCommand = new Command(OnLoadExcute);            
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
                var a = await RunHttpClientGet<Kiem_Ke_Thiet_Bi_Header_Model>($"api/qltb/GetDanhSachKiemKeHeader?username={Preferences.Get(Config.User, "")}");
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
