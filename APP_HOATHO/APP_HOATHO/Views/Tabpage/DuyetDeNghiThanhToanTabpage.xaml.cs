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
            ContentPage navigationPage = new DuyetChungTu.DuyetDeNghiThanhToan_Header_Page();
            //navigationPage.IconImageSource = "duyetdinhmuc.png";
            navigationPage.Title = "Chờ Duyệt";

            ContentPage content = new MoLaiChungTu.MoLaiDeNghiThanhToan_Header_Page();
            content.Title = "Mở Lại";
           // content.IconImageSource = "reopen.png";
            Children.Add(navigationPage);
            Children.Add(content);

        }

        public async Task LoadData()
        {
            DuyetChungTu.DuyetDeNghiThanhToan_Header_Page list = Children[0] as DuyetChungTu.DuyetDeNghiThanhToan_Header_Page;
            list.viewModel.IsBusy = false;
            list.viewModel.LoadCommand.Execute(null);
        }
    }
}