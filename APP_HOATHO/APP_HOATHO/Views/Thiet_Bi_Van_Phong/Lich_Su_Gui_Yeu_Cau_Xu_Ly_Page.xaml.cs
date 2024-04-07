using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Thiet_Bi_Van_Phong;
using APP_HOATHO.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace APP_HOATHO.Views.Thiet_Bi_Van_Phong
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lich_Su_Gui_Yeu_Cau_Xu_Ly_Page : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<DevicesMaintenanceHistory> ListItem { get; set; }
        BaseViewModel viewModel;
        
        public Lich_Su_Gui_Yeu_Cau_Xu_Ly_Page() 
        {
            InitializeComponent();            
            viewModel = new BaseViewModel();
            ListItem = new ObservableCollection<DevicesMaintenanceHistory>();
            Task.Run(async () => await ExcuteLoadLichSuBaoTri());
            BindingContext = this;            
        }
        async Task ExcuteLoadLichSuBaoTri()
        {
            IsBusy = true;           
            try
            {
                ListItem.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + $"api/qltb/LichSuGuiYeuCauCuaUser?username={Preferences.Get(Config.User,"")}").Result;                
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ListItem = JsonConvert.DeserializeObject<ObservableCollection<DevicesMaintenanceHistory>>(result);
                    OnPropertyChanged("ListItem");
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;               
            }
        }
            
    }

    
}