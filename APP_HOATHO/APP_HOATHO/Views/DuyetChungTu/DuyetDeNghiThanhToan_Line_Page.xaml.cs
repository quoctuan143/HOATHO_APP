using APP_HOATHO.Models.DuyetChungTu;
using APP_HOATHO.ViewModels.DuyetChungTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.DuyetChungTu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuyetDeNghiThanhToan_Line_Page : ContentPage
    {
        DuyetDeNghiThanhToan_Line_ViewModel viewModel;
        public DuyetDeNghiThanhToan_Line_Page(DeNghiThanhToanHeader_Model _item)
        {
            InitializeComponent();
            viewModel = new DuyetDeNghiThanhToan_Line_ViewModel(_item);
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