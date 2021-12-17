using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Tabpage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuyetDeNghiThanhToanTabpage : TabbedPage
    {
        public DuyetDeNghiThanhToanTabpage()
        {
            InitializeComponent();           

        }

        public async Task LoadData()
        {
            DuyetChungTu.DuyetDeNghiThanhToan_Header_Page list = Children[0] as DuyetChungTu.DuyetDeNghiThanhToan_Header_Page;
            list.viewModel.IsBusy = false;
            list.viewModel.LoadCommand.Execute(null);
        }
    }
}