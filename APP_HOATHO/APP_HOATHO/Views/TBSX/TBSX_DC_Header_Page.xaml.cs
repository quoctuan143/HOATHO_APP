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
    public partial class TBSX_DC_Header_Page : ContentPage
    {
        TBSX_DC_Header_ViewModel viewModel; 
        public TBSX_DC_Header_Page()
        {
            InitializeComponent();
            BindingContext = viewModel = new TBSX_DC_Header_ViewModel { NavigationPage = Navigation};
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
            var item = listChiTiet.SelectedItem as TBSXDC_Header_Model;
            if (item != null)
            {
                viewModel.ViewDetailCommand.Execute(item.No_);
            }
        }
    }
}