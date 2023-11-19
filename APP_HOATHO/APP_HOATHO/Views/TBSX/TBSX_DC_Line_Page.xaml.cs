using APP_HOATHO.ViewModels.TBSX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.TBSX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TBSX_DC_Line_Page : ContentPage
    {       
        private TBSX_DC_Line_ViewModel viewModel;
        string DocumentNo;
        public TBSX_DC_Line_Page(string doc)
        {
            InitializeComponent();
            DocumentNo = doc;
            viewModel = new TBSX_DC_Line_ViewModel(doc);
            viewModel.NavigationPage = Navigation;
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.ListItem.Count == 0)
            {
                viewModel.LoadCommand.Execute(DocumentNo);
            }
        }
    }
}