using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models;
using APP_HOATHO.Models.De_Nghi_Thanh_Toan;
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
    public partial class Danh_Sach_DNTT_Nhan_Vien : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<DanhSachDeNghiThanhToan> ListItem { get; set; }
        BaseViewModel viewModel;
        
        public Danh_Sach_DNTT_Nhan_Vien()
        {
            InitializeComponent();            
            viewModel = new BaseViewModel();
            ListItem = new ObservableCollection<DanhSachDeNghiThanhToan>();
            Task.Run(async () => await ExcuteLoadLichSuBaoTri());
            BindingContext = this;           
        }
        async Task  ExcuteLoadLichSuBaoTri()
        {
            IsBusy = true;           
            try
            {
                ListItem.Clear();
                var result = await viewModel.RunHttpClientGet<DanhSachDeNghiThanhToan>($"api/dntt/GetDNTTNhanVien?username={Preferences.Get(Config.User,"")}");
                ListItem = result.Lists;
                OnPropertyChanged(nameof(ListItem));
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
                var item = e.SelectedItem as DanhSachDeNghiThanhToan;
                if (item != null)
                {
                    if (item.Status == 2)
                    {
                        await new MessageBox("Vui lòng mở lại mới được điều chỉnh").Show();
                        return;
                    }
                    await Navigation.PushAsync(new De_Nghi_Thanh_Toan_Cho_Nhan_Vien_Page(item.No_));
                }    
               
            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }
        }

        private async void reopen_Tapped(object sender, EventArgs e)
        {
            var image = sender as Image;
            if (image != null)
            {
                var item = image.BindingContext as DanhSachDeNghiThanhToan;
                if (item != null)
                {
                    var ask = await new MessageYesNo("Bạn có muốn mở lại không?").Show();
                    if (ask == DialogReturn.OK)
                    {
                        var openOk = await viewModel.RunHttpClientPost($"api/dntt/ReopenDNTT?documentNo={item.No_}", null);
                        if (openOk.IsSuccessStatusCode)
                        {
                            await Navigation.PushAsync(new De_Nghi_Thanh_Toan_Cho_Nhan_Vien_Page(item.No_));
                        }    
                    }    
                }    
            }    
        }
    }

    
}