using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.DuyetChungTu;
using APP_HOATHO.ViewModels.DuyetChungTu;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.MoLaiChungTu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class MoLaiLenhCapPhatGiaCong_Page : ContentPage
    {
        MoLaiLenhCapPhatGiaCong_ViewModel viewModel;
        public MoLaiLenhCapPhatGiaCong_Page(DocumentType type)
        {
            InitializeComponent();
            viewModel = new MoLaiLenhCapPhatGiaCong_ViewModel(type);
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListItem.Count == 0)
            {
                IsBusy = false;
                viewModel.LoadCommand.Execute(null);
            }
        }
        public bool FilterRecords(object o)
        {

            var item = o as DuyetChungTuModel;

            if (item != null)
            {

                if (item.ExternalDocNo.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }
        string filterText;
        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listChiTiet.View.Filter = FilterRecords;
            listChiTiet.View.RefreshFilter();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            viewModel.DateChangeCommand.Execute(null);
        }

        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            try
            {
                DuyetChungTuModel _selectItem = listChiTiet.SelectedItem as DuyetChungTuModel;
                if (_selectItem == null) return;
                Navigation.PushAsync(new MoLaiChungTu_Line(_selectItem, viewModel._documentType));
            }
            catch (Exception ex)
            {
                new MessageBox(ex.Message).Show();

            }

        }
    }
}