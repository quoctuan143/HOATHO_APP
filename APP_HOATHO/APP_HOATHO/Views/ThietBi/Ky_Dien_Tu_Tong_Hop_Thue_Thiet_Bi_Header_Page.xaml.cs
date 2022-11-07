using APP_HOATHO.Global;
using APP_HOATHO.Models.KiDienTuPhuTung;
using APP_HOATHO.ViewModels.DuyetChungTu;
using APP_HOATHO.ViewModels.Ki_Dien_Tu_Thiet_Bi;
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
    public partial class Ky_Dien_Tu_Tong_Hop_Thue_Thiet_Bi_Header_Page : ContentPage 
    {
       
        public Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header_ViewModel viewModel; 
        public Ky_Dien_Tu_Tong_Hop_Thue_Thiet_Bi_Header_Page()
        {
            InitializeComponent();
            BindingContext = viewModel = new Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header_ViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListItem.Count == 0)
            viewModel.LoadCommand.Execute(null);
        }
        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header _selectItem = listChiTiet.SelectedItem as Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header;
            if (_selectItem == null) return;
            Navigation.PushAsync(new Ky_Dien_Tu_Tong_Hop_Thue_Thiet_Bi_Line_Page(_selectItem)); 
        }
    }

}