using APP_HOATHO.Global;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using Rg.Plugins.Popup.Extensions;
using Syncfusion.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.OneD;

namespace APP_HOATHO.Dialog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LookupItem : ContentPage, INotifyPropertyChanged
    {
        public   ObservableCollection<LookupValue> ListItem { get; set; }
        public string TieuDe { get; set; }
        public event EventHandler<LookupValue> closeForm;
        public  LookupItem(ObservableCollection<LookupValue> Items , string tieuDe)
        {
            try
            {
                ListItem = Items;
                OnPropertyChanged("ListItem");
                TieuDe = tieuDe;
                OnPropertyChanged("TieuDe");
                BindingContext = this;
                InitializeComponent();
            }
            catch (Exception ex)
            {

                new MessageBox(ex.Message).Show();
            }
                   
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

            var item = o as LookupValue;
            if (item != null)
            {
                filterText = filterText.Normalize(NormalizationForm.FormC).ToLower();
                var name = item.Name.Normalize(NormalizationForm.FormC).ToLower();
                if (item.Code.Normalize(NormalizationForm.FormC).ToLower().Contains(filterText) || name.Contains(filterText) || ( !string.IsNullOrEmpty(item.Description) && item.Description.Normalize(NormalizationForm.FormC).Contains(filterText) ))
                    return true;
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            try
            {
                LookupValue item = listChiTiet.SelectedItem as LookupValue;
                if (item != null)
                {
                    await Navigation.PopModalAsync(true);
                    closeForm?.Invoke(this, item);
                }
            }
            catch (Exception ex)
            {
                await new MessageBox(ex.Message).Show();
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
            closeForm?.Invoke(this, new LookupValue ());
        }
    }
}