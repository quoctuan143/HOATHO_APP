using APP_HOATHO.Models.Nha_May_Soi;
using APP_HOATHO.ViewModels.Nha_May_Soi;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Nha_May_Soi.Xuat_Kien_NVL
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Chat_Luong_Kien_Bong_Page : ContentPage
    {
        private Chat_Luong_Kien_Bong_ViewModel viewModel;

        public Chat_Luong_Kien_Bong_Page(string sochungtu)
        {
            InitializeComponent();
            BindingContext = viewModel = new Chat_Luong_Kien_Bong_ViewModel(sochungtu);
            viewModel.navigation = Navigation;
            viewModel.LoadCommand.Execute(sochungtu);
        }
    }
}