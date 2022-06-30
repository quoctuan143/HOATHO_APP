using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            DependencyService.Get<ISetStatusBarColor>().SetColoredStatusBar("#06264c");
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            BackButtonPressed();
            return true;
        }
        public async Task BackButtonPressed()
        {
            var ok = await new MessageYesNo("Bạn có muốn thoát chương trình không?").Show();
            if (ok == DialogReturn.OK)
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }
    }
}