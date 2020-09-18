using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Syncfusion.SfPullToRefresh.XForms.iOS;
using Syncfusion.XForms.iOS.TextInputLayout;
using UIKit;

namespace APP_HOATHO.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            Syncfusion.SfDataGrid.XForms.iOS.SfDataGridRenderer.Init();
            new Syncfusion.XForms.iOS.ComboBox.SfComboBoxRenderer();
            Syncfusion.XForms.iOS.Border.SfBorderRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfButtonRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfSwitchRenderer.Init();
            Lottie.Forms.iOS.Renderers.AnimationViewRenderer.Init();
            SfTextInputLayoutRenderer.Init();
            SfPullToRefreshRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();
            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
