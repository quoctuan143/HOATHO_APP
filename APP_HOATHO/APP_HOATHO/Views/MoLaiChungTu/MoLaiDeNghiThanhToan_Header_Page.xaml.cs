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
    public partial class MoLaiDeNghiThanhToan_Header_Page : ContentPage
    {
       
        public MoLaiDeNghiThanhToan_Header_ViewModel viewModel;
        public MoLaiDeNghiThanhToan_Header_Page()
        {
            InitializeComponent();
            viewModel = new MoLaiDeNghiThanhToan_Header_ViewModel();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }
        public bool FilterRecords(object o)
        {

            var item = o as DeNghiThanhToanHeader_Model;

            if (item != null)
            {

                if (item.ExternalDocumentNo.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }
        string filterText;
        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listChiTiet.View.Filter = FilterRecords;
            listChiTiet.View.RefreshFilter();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            viewModel.DateChangeCommand.Execute(null);
        }
        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            DeNghiThanhToanHeader_Model _selectItem = listChiTiet.SelectedItem as DeNghiThanhToanHeader_Model;
            if (_selectItem == null) return;
            Navigation.PushAsync(new MoLaiDeNghiThanhToan_Line_Page(_selectItem));
        }
    }
}