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
    public partial class DuyetDonDatMua : TabbedPage
    {
        public DuyetDonDatMua()
        {
            InitializeComponent();
            ContentPage navigationPage = new DuyetChungTu.DuyetChungTuDatMua_Header(Global.DocumentType.DuyetMuaHang);
            navigationPage.IconImageSource = "baseline_check_circle_black.png";
            navigationPage.Title = "Chờ Duyệt";

            ContentPage content = new DuyetChungTu.MoLaiChungTuDatMua_Header(Global.DocumentType.MoLaiDatMua);
            content.Title = "Mở Lại";
            content.IconImageSource = "baseline_open_in_browser_black.png";
            Children.Add(navigationPage);
            Children.Add(content);

        }

        public async Task LoadData()
        {
            DuyetChungTu.DuyetChungTuDatMua_Header list = Children[0] as DuyetChungTu.DuyetChungTuDatMua_Header;
            list.viewModel.IsBusy = false;
            list.viewModel.LoadCommand.Execute(null);
        }
    }
}