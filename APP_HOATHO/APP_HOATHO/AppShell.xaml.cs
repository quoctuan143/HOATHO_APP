using APP_HOATHO.Views;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace APP_HOATHO
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Login();
        }
    }
}
