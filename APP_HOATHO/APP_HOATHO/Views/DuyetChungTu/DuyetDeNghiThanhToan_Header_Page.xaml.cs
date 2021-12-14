using APP_HOATHO.Models.DuyetChungTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using APP_HOATHO.ViewModels.DuyetChungTu;

namespace APP_HOATHO.Views.DuyetChungTu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuyetDeNghiThanhToan_Header_Page : ContentPage
    {
        public DuyetDeNghiThanhToan_ViewModel viewModel;
        public DuyetDeNghiThanhToan_Header_Page()
        {
            InitializeComponent();
            viewModel = new DuyetDeNghiThanhToan_ViewModel();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }
        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            DeNghiThanhToanHeader_Model _selectItem = listChiTiet.SelectedItem as DeNghiThanhToanHeader_Model;
            if (_selectItem == null) return;
            Navigation.PushAsync(new DuyetDeNghiThanhToan_Line_Page(_selectItem));
        }
    }
}