using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderTemplate : ContentView
    {
        public HeaderTemplate()
        {
            InitializeComponent();
        }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            animation.Play();
        }
    }
}