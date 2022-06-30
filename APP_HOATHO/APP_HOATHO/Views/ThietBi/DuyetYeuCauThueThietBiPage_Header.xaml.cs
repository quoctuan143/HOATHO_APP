using APP_HOATHO.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_HOATHO.ViewModels.DuyetChungTu;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using APP_HOATHO.Models.DuyetChungTu;

namespace APP_HOATHO.Views.ThietBi 
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuyetYeuCauThueThietBiPage_Header : ContentPage
    { 
      public DuyetYeuCauThueThietBi_ViewModel viewModel;
        public DuyetYeuCauThueThietBiPage_Header(DocumentType type)
        {
            InitializeComponent();
            viewModel = new DuyetYeuCauThueThietBi_ViewModel(type);
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }       
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.IsBusy = false;
            viewModel.LoadCommand.Execute(null);
        }
        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            try
            {
                DuyetChungTuModel _selectItem = listChiTiet.SelectedItem as DuyetChungTuModel;
                if (_selectItem == null) return;
                Navigation.PushAsync(new DuyetYeuCauThueThietBiPage_Line(_selectItem, viewModel._documentType));
            }
            catch (Exception)
            {

               
            }
           
        }

       
    }
}