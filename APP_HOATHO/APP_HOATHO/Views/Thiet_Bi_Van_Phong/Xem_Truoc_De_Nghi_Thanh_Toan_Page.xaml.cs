using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models;
using APP_HOATHO.Models.De_Nghi_Thanh_Toan;
using APP_HOATHO.Models.Thiet_Bi_Van_Phong;
using APP_HOATHO.ViewModels;
using Newtonsoft.Json;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Thiet_Bi_Van_Phong
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Xem_Truoc_De_Nghi_Thanh_Toan_Page : ContentPage, INotifyPropertyChanged
    {
        public ViewSuggestedPayment ViewSuggestedPayment { get; set; }  
        public BaseViewModel viewModel { get; set; }
        public Xem_Truoc_De_Nghi_Thanh_Toan_Page(ViewSuggestedPayment viewSuggested)
        {
            InitializeComponent();
            this.ViewSuggestedPayment = viewSuggested;
            viewModel = new BaseViewModel();  
            BindingContext = this;
        }

        private async void GuiYeuCau_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ViewSuggestedPayment.SuggestedPaymentRequest.SuggestedPaymentHeader.PaytoType == 0)
                {
                    await new MessageBox("Vui lòng chọn loại đối tượng").Show();
                    return;
                }
                if (ViewSuggestedPayment.SuggestedPaymentRequest.SuggestedPaymentHeader.Type == 0)
                {
                    await new MessageBox("Vui lòng chọn loại hàng hóa").Show();
                    return;
                }
                if (ViewSuggestedPayment.SuggestedPaymentRequest.SuggestedPaymentHeader.PaymentMethod == 0)
                {
                    await new MessageBox("Vui lòng chọn hình thức thanh toán").Show();
                    return;
                }
                if (string.IsNullOrEmpty(ViewSuggestedPayment.SuggestedPaymentRequest.SuggestedPaymentHeader.PaytoVendorNo_))
                {
                    await new MessageBox("Vui lòng chọn đối tượng thanh toán").Show();
                    return;
                }
                if (string.IsNullOrEmpty(ViewSuggestedPayment.SuggestedPaymentRequest.SuggestedPaymentHeader.Description))
                {
                    await new MessageBox("Vui lòng nhập nội dung thanh toán").Show();
                    return;
                }
                if (ViewSuggestedPayment.SuggestedPaymentRequest.SuggestedPaymentLines.Count == 0)
                {
                    await new MessageBox($"Chưa có chi tiết thanh toán").Show();
                    return;
                }
                foreach (var item in ViewSuggestedPayment.SuggestedPaymentRequest.SuggestedPaymentLines.Select((value, index) => (value, index)).ToList())
                {
                    if (!string.IsNullOrEmpty(item.value.InvoiceCode) && !item.value.NgayHoaDon.HasValue)
                    {
                        await new MessageBox($"Vui lòng nhập ngày hóa đơn tại dòng thứ {item.index + 1}").Show();
                        return;
                    }
                    if (item.value.Amount == 0)
                    {
                        await new MessageBox($"Vui lòng nhập số tiền cần thanh toán tại dòng thứ {item.index + 1}").Show();
                        return;
                    }
                }

                var ask = await new MessageYesNo("Bạn có muốn gửi yêu cầu thanh toán này không?").Show();
                if (ask == DialogReturn.OK)
                {
                    var a= JsonConvert.SerializeObject(ViewSuggestedPayment.SuggestedPaymentRequest);
                     viewModel.ShowLoading("Đang xử lý. vui lòng đợi...");
                    var response = await viewModel.RunHttpClientPost("api/dntt/GuiDeNghiThanhToan", ViewSuggestedPayment.SuggestedPaymentRequest);
                    viewModel.HideLoading();
                    if (response.IsSuccessStatusCode)
                    {
                        await new MessageBox("Gửi yêu cầu thành công").Show();
                        await Navigation.PopToRootAsync();
                    }
                    else
                    {
                        await new MessageBox(response.Content.ReadAsStringAsync().Result).Show();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageBox(ex.Message).Show();
            }
        }
    }
}