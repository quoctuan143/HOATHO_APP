using APP_HOATHO.Models.DuyetChungTu;
using APP_HOATHO.Models.TBSX;
using APP_HOATHO.ViewModels.TBSX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.TBSX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThanhLyNguyenPhuLieu : ContentPage
    {
        ThanhLyNguyenPhuLieuViewModel viewModel;
        public ThanhLyNguyenPhuLieu()
        {
            InitializeComponent();
            viewModel = new ThanhLyNguyenPhuLieuViewModel();
            viewModel.NavigationPage = Navigation ;
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
        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            var item = listChiTiet.SelectedItem  as MaterialsLiquidationHeader;
            if (item != null)
            {
                viewModel.ViewDetailCommand.Execute(item.No_);
            }    
        }
    }
}