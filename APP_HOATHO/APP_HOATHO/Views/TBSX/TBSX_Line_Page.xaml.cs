using APP_HOATHO.ViewModels.TBSX;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.TBSX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TBSX_Line_Page : ContentPage
    {
        private TBSX_Line_ViewModel viewModel;
        string DocumentNo;
        public TBSX_Line_Page(string doc)
        {
            InitializeComponent();
            DocumentNo = doc;
            viewModel = new TBSX_Line_ViewModel (doc);
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