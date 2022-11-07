using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_HOATHO.Global;
using APP_HOATHO.Models.DuyetChungTu;
using APP_HOATHO.Models.KiDienTuPhuTung;
using APP_HOATHO.ViewModels.DuyetChungTu;
using APP_HOATHO.ViewModels.Ki_Dien_Tu_Thiet_Bi;
using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.ThietBi 
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ky_Dien_Tu_Tong_Hop_Thue_Thiet_Bi_Line_Page : ContentPage 
    {
        Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Line_ViewModel viewModel;
       
        public Ky_Dien_Tu_Tong_Hop_Thue_Thiet_Bi_Line_Page(Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header item) 
        {
            InitializeComponent();
             
            viewModel = new Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Line_ViewModel(item);
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
            listChiTiet.QueryRowHeight += DataGrid_QueryRowHeight;
        }
        private void DataGrid_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            if (e.RowIndex != 0)
            {
                //Calculates and sets the height of the row based on its content.
                try
                {
                    e.Height = listChiTiet.GetRowHeight(e.RowIndex);
                    e.Handled = true;
                }
                catch (Exception ex)
                {

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
               
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = false;
            viewModel.LoadCommand.Execute(null);
        }
    }
}