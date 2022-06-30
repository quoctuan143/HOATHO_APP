using APP_HOATHO.Global;
using APP_HOATHO.Models.KiDienTuPhuTung;
using APP_HOATHO.ViewModels.DuyetChungTu;
using APP_HOATHO.Views.DuyetChungTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.ThietBi 
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuyetTraThietBi_Page : ContentPage
    {
       
        public DuyetTraThietBi_ViewModel viewModel;
        public DuyetTraThietBi_Page(DocumentType type)
        {
            InitializeComponent();
            BindingContext = viewModel = new DuyetTraThietBi_ViewModel(type);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListItem.Count == 0)
            viewModel.LoadCommand.Execute(null);
        }
        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            DuyetKiDienTuPhuTungModel _selectItem = listChiTiet.SelectedItem as DuyetKiDienTuPhuTungModel;
            if (_selectItem == null) return;
            Navigation.PushAsync(new DuyetTraThietBiLine_Page(_selectItem, viewModel._documentType)); 
        }
    }

}