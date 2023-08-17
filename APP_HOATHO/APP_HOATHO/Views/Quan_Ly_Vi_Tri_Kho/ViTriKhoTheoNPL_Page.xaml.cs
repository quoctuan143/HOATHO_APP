using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViTriKhoTheoNPL_Page : ContentPage,INotifyPropertyChanged
    {
        public ObservableCollection<PurchaseLinePackingList_Model> ListItem { get;set; }

        public ViTriKhoTheoNPL_Page(ObservableCollection<PurchaseLinePackingList_Model> source)
        {
            InitializeComponent();            
            ListItem = source;
            BindingContext = this;
        }
    }
}