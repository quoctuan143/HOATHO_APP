using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels.PhuLieu;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.PhuLieu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Chi_Tiet_Phieu_Xuat_Kho_Phu_Lieu_Page : ContentPage
    {
        private Chi_Tiet_Xuat_Phu_Lieu_ViewModel viewModel;

        public Chi_Tiet_Phieu_Xuat_Kho_Phu_Lieu_Page(string soChungTu)
        {
            InitializeComponent();
            viewModel = new Chi_Tiet_Xuat_Phu_Lieu_ViewModel(soChungTu);
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
        private string filterText = string.Empty;

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listChiTiet.View.Filter = FilterRecords;
            listChiTiet.View.RefreshFilter();
        }

        public bool FilterRecords(object o)
        {
            var item = o as Chi_Tiet_Nhap_Phu_Lieu_Model;
            if (item != null)
            {
                if (item.Name.ToLower().Contains(filterText) || item.Color.ToLower().Contains(filterText) || item.Art.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }
    }
}