using APP_HOATHO.Models.DuyetChungTu;
using APP_HOATHO.ViewModels.DuyetChungTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.MoLaiChungTu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoLaiDeNghiThanhToan_Line_Page : ContentPage
    {

        MoLaiDeNghiThanhToan_Line_ViewModel viewModel;
        public MoLaiDeNghiThanhToan_Line_Page(DeNghiThanhToanHeader_Model _item)
        {
            InitializeComponent();
            viewModel = new MoLaiDeNghiThanhToan_Line_ViewModel(_item);
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListItem.Count == 0)
            {
                IsBusy = false;
                viewModel.LoadCommand.Execute(null);
            }
        }
    }
}