using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Purchase_Line_Page : ContentPage
    {
        private PurchaseLine_ViewModel viewModel;

        public Purchase_Line_Page(string soChungTu)
        {
            InitializeComponent();
            viewModel = new PurchaseLine_ViewModel(soChungTu);
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }
        protected override  void OnAppearing()
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
            var item = o as PurchaseLine_Model;
            if (item != null)
            {
                if (item.Name.ToLower().Contains(filterText) || item.Color.ToLower().Contains(filterText) || item.Art.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }
    }
}