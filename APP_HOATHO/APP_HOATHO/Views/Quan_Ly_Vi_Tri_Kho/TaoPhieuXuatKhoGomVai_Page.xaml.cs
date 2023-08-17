using APP_HOATHO.Models;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
using Syncfusion.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaoPhieuXuatKhoGomVai_Page : ContentPage
    {
       public TaoPhieuXuatKhoGomVai_ViewModel viewModel = new TaoPhieuXuatKhoGomVai_ViewModel();
        public TaoPhieuXuatKhoGomVai_Page()
        {
            InitializeComponent();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }      
        
        string filterText = string.Empty;
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listChiTiet.View.Filter = FilterRecords;
            listChiTiet.View.RefreshFilter();
        }
        public bool FilterRecords(object o)
        {

            var item = o as Tong_Hop_Gom_Phieu_Xuat_Model;
            if (item != null)
            {

                if (item.DocumentNo.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }
        
    }
}