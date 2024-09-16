using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Thiet_Bi_Van_Phong;
using APP_HOATHO.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Thiet_Bi_Van_Phong
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IT_Tiep_Nhan_Yeu_Cau : PopupPage
    {
        TaskCompletionSource<DialogReturn> _tsk = null;
        BaseViewModel BaseView { get; set; }
        DevicesMaintenanceHistory Item;
        public IT_Tiep_Nhan_Yeu_Cau(DevicesMaintenanceHistory item)
        {
            Item = item;
            BaseView = new BaseViewModel();
            InitializeComponent();
        }

        public async Task<DialogReturn> Show()
        {
            _tsk = new TaskCompletionSource<DialogReturn>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }

        private async void btnOK_Clicked(object sender, EventArgs e)
        {
            try
            {
                BaseView.ShowLoading("đang xử lý...");
                var time = this.timePicker.Time;
                if (time == null)
                {
                    DependencyService.Get<IMessage>().LongAlert("Vui lòng nhập thời gian dự kiến hoàn thành");
                    return;
                }
                Item.Du_Kien_Hoan_Thanh = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.Hours, time.Minutes, time.Seconds);
                var ok = await BaseView.RunHttpClientPost($"api/qltb/IT_Tiep_Nhan_Yeu_Cau", Item);
                if (ok.IsSuccessStatusCode)
                {
                    BaseView.HideLoading();
                    DependencyService.Get<IMessage>().LongAlert("Đã gửi thông báo tiếp nhận đến user yêu cầu");
                    await Navigation.PopPopupAsync();
                    await Navigation.PopAsync();
                }

                else
                {
                    BaseView.HideLoading();
                    DependencyService.Get<IMessage>().LongAlert(ok.Content.ReadAsStringAsync().Result);
                }    
                    
            }
            catch
            {
                
            }
            finally
            {
                BaseView.HideLoading();
            }
        }
    }
}