using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.Nha_May_Soi;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace APP_HOATHO.ViewModels.Nha_May_Soi
{
    public class Chat_Luong_Kien_Bong_ViewModel : BaseViewModel
    {
        #region "Field"

        private ObservableCollection<Chat_Luong_Kien_Bong_Model> _listItem;
        public INavigation navigation { get; set; }

        public string SoChungTu { get; set; }
        #endregion "Field"

        #region "Command"

        public Command<string> LoadCommand { get; set; }

        public Command YeuCauDuyetCommand { get; set; }

        #endregion "Command"

        #region "Constructor"

        public ObservableCollection<Chat_Luong_Kien_Bong_Model> ListItem
        { get => _listItem; set { SetProperty(ref _listItem, value); } }

        public Chat_Luong_Kien_Bong_ViewModel(string sochungtu)
        {
            SoChungTu = sochungtu;
            Title = "Chất lượng kiện bông";
            ListItem = new ObservableCollection<Chat_Luong_Kien_Bong_Model>();
            LoadCommand = new Command<string>(OnLoadExcute);            
        }      

        #endregion "Constructor"

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
                var a = await RunHttpClientGet<Chat_Luong_Kien_Bong_Model>($"api/Soi/PostKiemTraChatLuongBong?sochungtu={obj}");
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

        #endregion "Method"
    }
}