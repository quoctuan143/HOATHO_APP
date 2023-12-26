using APP_HOATHO.Global;
using APP_HOATHO.ViewModels.Nha_May_Soi.KyLenhXuatHang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Nha_May_Soi.KyLenhXuatHang
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KyLehnhXuatHangHeader_Page : ContentPage
    {
        KyLenhXuatHangHeader_ViewModel viewModel;
        public KyLehnhXuatHangHeader_Page(AppoveType appove)
        {
            InitializeComponent();
            viewModel = new KyLenhXuatHangHeader_ViewModel(appove);
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListItem.Count == 0)
            {
                viewModel.LoadCommand.Execute(null );
            }    
        }
    }
}