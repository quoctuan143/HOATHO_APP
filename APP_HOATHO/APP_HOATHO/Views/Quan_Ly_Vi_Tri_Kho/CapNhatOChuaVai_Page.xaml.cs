using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
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
    public partial class CapNhatViTriLuuKho_Page : ContentPage
    {
        CapNhatViTriKho_ViewModel _viewModel;
        public CapNhatViTriLuuKho_Page(string maViTri)
        {
            InitializeComponent();
            _viewModel = new CapNhatViTriKho_ViewModel(maViTri);
            _viewModel.Navigation = Navigation;
            BindingContext = _viewModel;
        }
    }
}