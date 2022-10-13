using APP_HOATHO.Views;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace APP_HOATHO
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }
        private async void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Thông Báo", "Bạn đã mất kết nối internet. vui lòng kiểm tra lại", "OK");
            }
        }
        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Login();
        }
    }
}
