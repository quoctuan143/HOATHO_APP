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
    public partial class KyLenhXuatHangLine_Page : ContentPage
    {
        KyLenhXuatHangLine_ViewModel viewModel;
        public KyLenhXuatHangLine_Page(AppoveType approveType, string document)
        {
            InitializeComponent();
            viewModel = new KyLenhXuatHangLine_ViewModel(approveType, document);
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListItem.Count == 0)
            {
                viewModel.LoadCommand.Execute(null);
            }    
        }
    }
}