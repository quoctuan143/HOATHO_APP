using APP_HOATHO.ViewModels.TBSX;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.TBSX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThanhLyNguyenPhuLieu_LineView : ContentPage
    {
        private ThanhLyNguyenPhuLieu_LineViewModel viewModel;
        string DocumentNo;
        public ThanhLyNguyenPhuLieu_LineView(string doc)
        {
            InitializeComponent();
            DocumentNo = doc;
            viewModel = new ThanhLyNguyenPhuLieu_LineViewModel(doc);
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