using APP_HOATHO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Danh_Muc_Thiet_Bi : ContentPage
    {
        ThietBiViewModel viewModel;
        public Danh_Muc_Thiet_Bi()
        {
            InitializeComponent();
            BindingContext = viewModel = new ThietBiViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (IsBusy == true) return;
            if (viewModel.Items.Count   == 0 )
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void btnGuiYeuCau_Clicked(object sender, EventArgs e)
        {

        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string serial = search.Text;
            if (serial != "")
            {
                listThietBi.ItemsSource = viewModel.Items.Where(p => p.No_2.ToLower().Contains(serial) || p.No_3 .ToLower().Contains(serial) || p.NameVN.ToLower().Contains (serial )).ToList();
            }
            else
            {
                listThietBi.ItemsSource = viewModel.Items;
            }
        }
    }
}