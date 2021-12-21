using APP_HOATHO.Global;
using APP_HOATHO.Models;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }
        private async void SfButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(btnusername.Text) || string.IsNullOrEmpty(btnpassword.Text))
                {
                    await DisplayAlert("Thông Báo", "Vui lòng điền đẩy đủ username và password", "Ok");
                    return;
                }
                await DependencyService.Get<IProcessLoader>().Show("Vui lòng đợi");
              

                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getUser?username=" + btnusername.Text + "&password=" + btnpassword.Text).Result;
                await Task.Delay(3000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    await DependencyService.Get<IProcessLoader>().Hide();
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                   List< User> user = JsonConvert.DeserializeObject<List<User>>(result);
                    if (swRememer.IsChecked  == true)
                    {
                        Preferences.Set(Config.User, btnusername.Text);
                        Preferences.Set(Config.Password, btnpassword.Text);                       
                    }
                    Preferences.Set(Config.Role, user[0].Role.ToString());
                    Preferences.Set(Config.PhoneNumber, user[0].PhoneNumber);
                    Preferences.Set(Config.NhaMay, user[0].SourceCode);
                    Preferences.Set(Config.MaXuong, user[0].MaXuong);
                    Preferences.Set(Config.FullName, user[0].FullName);
                    Preferences.Set(Config.Email , user[0].Email);
                    //lưu lại token 
                   // if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                   // {
                        TokenFirebase firebase = new TokenFirebase();
                        firebase.Device = Device.RuntimePlatform;
                        firebase.Token = CrossFirebasePushNotification.Current.Token;
                        firebase.UserID = btnusername.Text;
                        firebase.Topic = user[0].Role.ToString();
                        var respon = Config.client.PostAsJsonAsync("api/DuyetChungTu/UpdateTokenFireBase", firebase).Result;
                   // }                        
                    App.Current.MainPage = new AppShell();
                }
                else
                {
                    
                    await DisplayAlert("Thông Báo", "Thông tin đăng nhập không chính xác", "Ok");
                    await DependencyService.Get<IProcessLoader>().Hide();
                    return;
                }
                

                
            }
            catch (Exception ex)
            {

                await DependencyService.Get<IProcessLoader>().Hide();
                await DisplayAlert("Lỗi", ex.Message, "Ok");
            }
        }
    }
}