﻿using APP_HOATHO.Global;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Dialog
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppInformation : PopupPage
    {
        TaskCompletionSource<DialogReturn> _tsk = null;
        public AppInformation()
        {
            InitializeComponent();
            lblHeDieuHanh.Text = Device.RuntimePlatform;
            lblPhienBan.Text = AppInfo.VersionString;
        }
        public async Task<DialogReturn> Show()
        {
            _tsk = new TaskCompletionSource<DialogReturn>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }

        private async void btnOK_Clicked(object sender, EventArgs e)
        {
            string url = string.Empty;
            var location = RegionInfo.CurrentRegion.Name.ToLower();
            if (Device.RuntimePlatform == Device.Android)
                url = "https://play.google.com/store/apps/details?id=com.companyname.app_hoatho";
            else if (Device.RuntimePlatform == Device.iOS)
                url = "https://itunes.apple.com/" + location + "/app/HoaTho/id1560268571?mt=8";
            await Browser.OpenAsync(url, BrowserLaunchMode.External);
        }
    }
}