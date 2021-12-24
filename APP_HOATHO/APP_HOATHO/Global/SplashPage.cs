using APP_HOATHO.Models;
using APP_HOATHO.Views;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
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
                await ShowMessage("Thông Báo", "Vui Lòng kiểm tra lại kết nối mạng", "OK", () =>
                { App.Current.MainPage = new Login(); });
            }
            if (Device.RuntimePlatform == Device.iOS)
            {


                await image.ScaleTo(1, 5000);//thời gian khởi tạo
                await image.ScaleTo(0.9, 1500, Easing.Linear);
                await image.ScaleTo(150, 500, Easing.Linear);

            }
            //kiêm tra xem user có thay đổi k
            try
            {


                var _json = Config.client.GetStringAsync(Config.URL  + "api/qltb/getUser?username=" + Preferences.Get(Config.User, "1") + "&password=" + Preferences.Get(Config.Password, "1")).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    App.Current.MainPage = new AppShell();
                }
                else
                {
                    App.Current.MainPage = new Login();
                }
               
                   

            }
            catch (Exception ex)
            {

                await ShowMessage("Thông Báo", ex.Message, "OK", () =>
                { App.Current.MainPage = new Login(); });
            }


        }

        

        public async Task ShowMessage(string title, string message, string buttonText, Action afterHideCallback)
        {
            await DisplayAlert(
                title,
                message,
                buttonText);

            afterHideCallback?.Invoke();
        }
    }
}
