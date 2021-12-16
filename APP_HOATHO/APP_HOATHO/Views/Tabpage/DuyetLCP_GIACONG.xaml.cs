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
    public partial class DuyetLCP_GIACONG : TabbedPage
    {
        public DuyetLCP_GIACONG()
        {
            InitializeComponent();
            ContentPage navigationPage = new DuyetChungTu.DuyetChungTu_Header(Global.DocumentType.DuyetLCP_GC);
            if (Device.RuntimePlatform == Device.iOS)
            {
                navigationPage.IconImageSource = "duyet.svg";
            }
            else
            {
                navigationPage.IconImageSource = "online.png";
            }
            navigationPage.Title = "Chờ Duyệt";

            ContentPage content = new MoLaiChungTu.MoLaiChungTu_Header(Global.DocumentType.MoLaiLCP_GC);
            content.Title = "Mở Lại";
            if (Device.RuntimePlatform == Device.iOS)
            {
                content.IconImageSource = "molai.svg";
            }
            else
            {
                content.IconImageSource = "molai.png";
            }
            Children.Add(navigationPage);
            Children.Add(content);
        }
        public async Task LoadData()
        {
            DuyetChungTu.DuyetChungTu_Header list = Children[0] as DuyetChungTu.DuyetChungTu_Header;
            list.viewModel.IsBusy = false;
            list.viewModel.LoadCommand.Execute(null);
        }
    }
}