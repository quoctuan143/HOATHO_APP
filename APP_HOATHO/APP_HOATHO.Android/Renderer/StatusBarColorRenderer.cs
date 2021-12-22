using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using APP_HOATHO.Interface;
using APP_HOATHO.Droid.Renderer;

[assembly: Dependency(typeof(StatusBarColorRenderer))]

namespace APP_HOATHO.Droid.Renderer
{
    
    public class StatusBarColorRenderer : ISetStatusBarColor
    {
        public void SetColoredStatusBar(string hexColor)
        {
            //if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        var currentWindow = GetCurrentWindow();
            //        currentWindow.DecorView.SystemUiVisibility = 0;
            //        currentWindow.SetStatusBarColor(Android.Graphics.Color.ParseColor(hexColor);
            //    });
            //}
        }

        //public void SetWhiteStatusBar()
        //{
        //    if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        //    {
        //        Device.BeginInvokeOnMainThread(() =>
        //        {
        //            var currentWindow = GetCurrentWindow();
        //            currentWindow.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
        //            currentWindow.SetStatusBarColor(Android.Graphics.Color.White);
        //        });
        //    }
        //}

        //Window GetCurrentWindow()
        //{
        //    var window = CrossCurrentActivity.Current.Activity.Window;

        //    // clear FLAG_TRANSLUCENT_STATUS flag:
        //    window.ClearFlags(WindowManagerFlags.TranslucentStatus);

        //    // add FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS flag to the window
        //    window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

        //    return window;
        //}
    }
}