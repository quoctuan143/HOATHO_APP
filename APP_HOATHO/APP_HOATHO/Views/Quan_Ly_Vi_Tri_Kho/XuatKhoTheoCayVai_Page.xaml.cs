using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
using Syncfusion.Data.Extensions;
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
    public partial class XuatKhoTheoCayVai_Page : ContentPage
    {
        XuatKhoTheoCayVai_ViewModel viewModel;
        public event EventHandler<double> ClosePage;
        public XuatKhoTheoCayVai_Page(DanhSachPhieuXuatKhoChiTiet_Model Item)
        {
            InitializeComponent();
            viewModel = new XuatKhoTheoCayVai_ViewModel(Item);
            viewModel.Navigation = Navigation;
            BindingContext = viewModel;
        }

        private void btnXoaCayVai_Tapped(object sender, EventArgs e)
        {            
            viewModel.DeleteIdVaiCommand.Execute(null);
        }

        private void listChiTiet_SwipeEnded(object sender, Syncfusion.SfDataGrid.XForms.SwipeEndedEventArgs e)
        {
            viewModel.SelectItem = e.RowData as XuatKhoTheoCayVai_Model;
        }
        
        private void btnCapNhat_Clicked(object sender, EventArgs e)
        {
            viewModel.CapNhatCommand.Execute(null);
            double sum = 0;
            viewModel.ListItem.ForEach(x =>
            {
                sum = sum + x.CanXuat;
            });
            if (sum > 0)
            {
                ClosePage?.Invoke(this, sum);
            }
        }
    }
}