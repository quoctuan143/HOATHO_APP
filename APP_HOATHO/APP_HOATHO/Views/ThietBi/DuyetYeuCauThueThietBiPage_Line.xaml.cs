using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_HOATHO.Global;
using APP_HOATHO.Models.DuyetChungTu;
using APP_HOATHO.ViewModels.DuyetChungTu;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.ThietBi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuyetYeuCauThueThietBiPage_Line : ContentPage 
    {
        DuyetYeuCauThueThietBi_Line_ViewModel viewModel;
        public DuyetYeuCauThueThietBiPage_Line(DuyetChungTuModel item, DocumentType type)
        {
            InitializeComponent();
            viewModel = new DuyetYeuCauThueThietBi_Line_ViewModel(item, type);
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