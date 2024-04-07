using APP_HOATHO.Dialog;
using APP_HOATHO.Models;
using APP_HOATHO.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.LatestVersion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.Global
{
    public class SplashPage : ContentPage
    {
       
        // IMqttClient client;
        Image image;
        public SplashPage()
        {
            

           // if (Device.RuntimePlatform == Device.iOS)
           // {
            NavigationPage.SetHasNavigationBar(this, false);
                var sub = new AbsoluteLayout();
                image = new Image
                {
                    Source = "logo.png",
                    WidthRequest = 150,
                    HeightRequest = 150

                };
                AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(image, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                sub.Children.Add(image);
                this.BackgroundColor = Color.White;
                this.Content = sub;
            //}


        }

        

        protected override async void OnAppearing()
        {
            base.OnAppearing();           
            if (!CrossConnectivity.Current.IsConnected)
            {
                await new MessageBox("Bạn đã mất kết nối internet. vui lòng kiểm tra lại").Show();
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
                await image.ScaleTo(1, 3000);//thời gian khởi tạo
                                             //await image.ScaleTo(0.9, 1500, Easing.Linear);
                                             // await image.ScaleTo(150, 500, Easing.Linear);

            //kiêm tra xem user có thay đổi k
            try
            {
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getUser?username=" + Preferences.Get(Config.User, "1") + "&password=" + Preferences.Get(Config.Password, "1")).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    List<User> user = JsonConvert.DeserializeObject<List<User>>(result);
                   
                    Preferences.Set(Config.Role, user[0].Role ?? -1);                    
                    Preferences.Set(Config.NhaMay, user[0].SourceCode);
                    Preferences.Set(Config.MaXuong, user[0].MaXuong);                    
                    Preferences.Set(Config.FullName, user[0].FullName);
                    Preferences.Set(Config.Email, user[0].Email);
                    Preferences.Set(Config.EmployeeNo, user[0].EmployeeNo);
                    App.Current.MainPage = new AppShell();
                    
                }
                else
                {
                    App.Current.MainPage = new Login();
                }

            }
            catch (Exception ex)
            {

                await ShowMessage("Thông Báo", ex.Message, "OK", "Cancel", () =>
                 { App.Current.MainPage = new Login(); });
            }
          
        }
        
        public async Task ShowMessage(string title, string message, string buttonText,string cancel, Action afterHideCallback)
        {
            await DisplayAlert(
                title,
                message,
                buttonText,cancel );

            afterHideCallback?.Invoke();
        }
    }
}
