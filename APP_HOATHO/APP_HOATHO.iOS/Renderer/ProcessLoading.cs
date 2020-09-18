using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APP_HOATHO.Global;
using APP_HOATHO.iOS.Renderer;
using BigTed;
using Foundation;
using UIKit;
[assembly: Xamarin.Forms.Dependency(typeof(ProcessLoading))]

namespace APP_HOATHO.iOS.Renderer
{
    public class ProcessLoading : IProcessLoader
    {
        public System.Threading.Tasks.Task Hide()
        {
            BTProgressHUD.Dismiss();
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task Show(string title = "Loading")
        {
            BTProgressHUD.Show(title, maskType: ProgressHUD.MaskType.Black);
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}