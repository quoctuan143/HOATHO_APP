using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
using Syncfusion.SfDataGrid.XForms;
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
    public partial class CapNhatViTriChuaOVai_Page : ContentPage
    {
        CapNhatViTriChuaOVai_ViewModel _viewModel;
        public CapNhatViTriChuaOVai_Page()
        {
            InitializeComponent();
            _viewModel = new CapNhatViTriChuaOVai_ViewModel();
            _viewModel.Navigation = Navigation;
            BindingContext = _viewModel;            
        }
    }
}