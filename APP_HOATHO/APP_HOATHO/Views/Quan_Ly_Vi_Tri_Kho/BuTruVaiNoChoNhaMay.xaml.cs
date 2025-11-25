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
    public partial class BuTruVaiNoChoNhaMay : ContentPage
    {
        public BuTruVaiNoChoNhaMayViewModel viewModel { get; set; }
        public event EventHandler<double> ClosePage;
        public BuTruVaiNoChoNhaMay(BuTruVaiRequest request)
        {
            InitializeComponent();
            viewModel = new BuTruVaiNoChoNhaMayViewModel(request);
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadDataCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            double sum = viewModel.ListItem.Sum(x => x.Balance);
            if (sum > 0)
            {
                ClosePage?.Invoke(this, sum);
            }
        }
    }
}