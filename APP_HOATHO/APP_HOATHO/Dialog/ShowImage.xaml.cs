using APP_HOATHO.Models;
using Rg.Plugins.Popup.Extensions;
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
    public partial class ShowImage : PopupPage  
    {
      
        public   string  IMAGE_LINK { get; set; }

        public ShowImage(string Image)
        {           
            InitializeComponent();
            IMAGE_LINK = Image;
            BindingContext = this;
        }
        public async Task Show()
        {
            try
            {
                await Navigation.PushPopupAsync(this);
            }
            catch 
            {                
            }
                 
        }
    }
}