using APP_HOATHO.Models.HangKhongGanNhan;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels.HangKhongGanNhan;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.HangKhongGanNhan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HangKhongGanNhan_Header_Page : ContentPage
    {       

        public HangKhongGanNhan_Header_ViewModel viewModel = new HangKhongGanNhan_Header_ViewModel();
        public HangKhongGanNhan_Header_Page()
        {
            InitializeComponent();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListItem.Count == 0)
            {
                viewModel.LoadDataCommand.Execute(null);
            }
        }

        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            Nhap_Hang_Khong_Gan_Nhan_Header_Model item = listChiTiet.SelectedItem as Nhap_Hang_Khong_Gan_Nhan_Header_Model;
            if (item != null)
            {
                Navigation.PushAsync(new HangKhongGanNhan_Line_Page(item.No_));
            }
        }
        string filterText = string.Empty;
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue.ToString().ToLower();
            listChiTiet.View.Filter = FilterRecords;
            listChiTiet.View.RefreshFilter();
        }
        public bool FilterRecords(object o)
        {

            var item = o as Nhap_Hang_Khong_Gan_Nhan_Header_Model;
            if (item != null)
            {

                if (item.No_.ToLower().Contains(filterText)  || item.SoKien.ToString().ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            viewModel.FindCommand.Execute(null);
        }
    }
}