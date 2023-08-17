using APP_HOATHO.Converter;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DanhSachPhieuXuatKhoGomVaiChiTiet_Page : ContentPage
    {
        private DanhSachPhieuXuatKhoGomVaiChiTiet_ViewModel viewModel;

        public DanhSachPhieuXuatKhoGomVaiChiTiet_Page(string sochungtu)
        {
            InitializeComponent();
            viewModel = new DanhSachPhieuXuatKhoGomVaiChiTiet_ViewModel(sochungtu);
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }

        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            //DanhSachPhieuXuatKhoChiTiet_Model item = listChiTiet.SelectedItem as DanhSachPhieuXuatKhoChiTiet_Model;
            //if (item != null)
            //{
            //    Navigation.PushAsync(new Purchase_Line_Page(item.No_));
            //}
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
            var item = o as DanhSachPhieuXuatKhoChiTiet_Model;
            if (item != null)
            {
                if(ChonNhom1 == StatusFormatColor.TatCa )
                {
                    if (item.ItemName.ToLower().Contains(filterText) || item.Color.ToLower().Contains(filterText) || item.DVT.ToLower().Contains(filterText))
                        return true;
                }    
                else
                {
                    if ((item.ItemName.ToLower().Contains(filterText) || item.Color.ToLower().Contains(filterText) || item.DVT.ToLower().Contains(filterText)) && item.XuatOk == ChonNhom1)
                        return true;
                }    
            }
            return false;
        }

        private void listChiTiet_SwipeEnded(object sender, Syncfusion.SfDataGrid.XForms.SwipeEndedEventArgs e)
        {
            viewModel.SelectItem = e.RowData as DanhSachPhieuXuatKhoChiTiet_Model;
        }

        private void btnXemViTri_Tapped(object sender, System.EventArgs e)
        {
            viewModel.XemViTriKhoCommand.Execute(null);
        }

        private void btnXuatKho_Tapped(object sender, System.EventArgs e)
        {
            viewModel.XuatKhoCommand.Execute(null);
        }
        StatusFormatColor ChonNhom1 = StatusFormatColor.TatCa;
        
        private void ChonNhom_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {           
                       
            if (e.Value.ToString() == "Tất Cả")
            {
                ChonNhom1 = StatusFormatColor.TatCa;
            }
            if (e.Value.ToString() == "Chưa Hoàn Thành")
            {
                ChonNhom1 = StatusFormatColor.ChuaHoanThanh;
            }
            if (e.Value.ToString() == "Dở Dang")
            {
                ChonNhom1 = StatusFormatColor.DoDang;
            }
            if (e.Value.ToString() == "Hoàn Thành")
            {
                ChonNhom1 = StatusFormatColor.HoanThanh;
            }
            listChiTiet.View.Filter = FilterRecords;
            listChiTiet.View.RefreshFilter();
        }
    }
}