using APP_HOATHO.Models;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Dialog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CapNhatBaoTri : PopupPage
    {
        DanhMuc_ThietBi Item { get; set; }
        public CapNhatBaoTri(DanhMuc_ThietBi item)
        {
            Item = item;
            InitializeComponent();
        }
    }
}