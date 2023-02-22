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
    public partial class Danh_Sach_Cho_Xu_Ly_Page : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<DevicesMaintenanceHistory> ListItem { get; set; }
        BaseViewModel viewModel;
        
        public Danh_Sach_Cho_Xu_Ly_Page()
        {
            InitializeComponent();            
            viewModel = new BaseViewModel();
            ListItem = new ObservableCollection<DevicesMaintenanceHistory>();
            Task.Run(async () => await ExcuteLoadLichSuBaoTri());
            BindingContext = this;
            MessagingCenter.Subscribe<Cap_Nhat_Thong_Tin_Loi_Page, string>(this, "capnhatxulyloi", (s, e) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                    ListItem.Remove(ListItem.FirstOrDefault(x => x.DocumentNo_ == e));
                    OnPropertyChanged (nameof(ListItem));                    
                });
            });
        }
        async Task ExcuteLoadLichSuBaoTri()
        {
            IsBusy = true;           
            try
            {
                ListItem.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/DanhSachThietBiChoXuLy").Result;                
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

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {                
                var item = e.SelectedItem as DevicesMaintenanceHistory;
                DevicesMaintenanceHistory yeucau = new DevicesMaintenanceHistory
                {
                    DocumentNo_ = item.DocumentNo_,
                    ITXuLy = Preferences.Get(Config.FullName, ""),
                    TenThietBi = item.Description2,
                    NguoiGuiYeuCau = item.NguoiGuiYeuCau 
                };
                await Navigation.PushAsync (new Cap_Nhat_Thong_Tin_Loi_Page(yeucau));
            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }
        }

       
            
    }

    
}