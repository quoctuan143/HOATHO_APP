using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThongTinThietBi_Edit : ContentPage, INotifyPropertyChanged
    {
        public DanhMuc_ThietBi Item { get; set; }
        public ThongTinThietBi_Edit(DanhMuc_ThietBi item)
        {
            InitializeComponent();
            Item = item;
            BindingContext = this;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var ok1 = await new MessageYesNo("Bạn có muốn cập nhật không?").Show();
            if (ok1 == Global.DialogReturn.OK)
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Config.URL);
                var ok = client.PostAsJsonAsync("api/qltb/UpdateThongTinThietBi", Item );
                if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("cập nhật thành công"))
                {

                    DependencyService.Get<IMessage>().ShortAlert("Cập nhật thành công");
                    MessagingCenter.Send(this, "UpdateThietBi", Item );
                    await Navigation.PopAsync();
                }
                else
                {
                    await new MessageBox( ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                }
            }    
        }
    }
}