using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Thiet_Bi_Van_Phong;
using APP_HOATHO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Thiet_Bi_Van_Phong
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Yeu_Cau_Xu_Ly_Loi_Page : ContentPage
    {
       public DevicesMaintenanceHistory Item { get; set; }
        BaseViewModel BaseView { get; set; }    
        public Yeu_Cau_Xu_Ly_Loi_Page(DevicesMaintenanceHistory item)
        {
            InitializeComponent();
            BaseView = new BaseViewModel();
            Item = item;
            this.BindingContext = Item;
        }

        private async void GuiYeuCau_Clicked(object sender, EventArgs e)
        {
            if (Item.NoiDungLoi == "")
            {
                DependencyService.Get<IMessage>().LongAlert("Vui lòng nhập nội dung muốn gửi IT");
                return;
            }
            var ok = await BaseView.RunHttpClientPost("api/qltb/GuiYeuCauXuLyLoiThietBi" , Item);
            if (ok.IsSuccessStatusCode)
            {
                DependencyService.Get<IMessage>().LongAlert("Đã gửi yêu cầu xử lý lỗi thành công");
                await Navigation.PopAsync();
            }                    
            else
                DependencyService.Get<IMessage>().LongAlert(ok.Content.ReadAsStringAsync().Result );
        }
    }
}