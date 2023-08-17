using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DanhSachPhieuXuatKhoGomVai_Page : ContentPage
    {
        private DanhSachPhieuXuatKhoGomVai_ViewModel viewModel;

        public DanhSachPhieuXuatKhoGomVai_Page()
        {
            InitializeComponent();
            viewModel = new DanhSachPhieuXuatKhoGomVai_ViewModel();
            BindingContext = viewModel;
        }

        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            DanhSachPhieuXuatKho_Model item = listChiTiet.SelectedItem as DanhSachPhieuXuatKho_Model;
            if (item != null)
            {
                Navigation.PushAsync(new DanhSachPhieuXuatKhoGomVaiChiTiet_Page(item.No_));
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
            var item = o as DanhSachPhieuXuatKho_Model;
            if (item != null)
            {
                if (item.ExternalDocumentNo.ToLower().Contains(filterText) || item.PO.ToLower().Contains(filterText) || item.Style.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }
    }
}