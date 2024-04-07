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
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Thiet_Bi_Van_Phong
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class De_Nghi_Thanh_Toan_Cho_Nhan_Vien_Page : ContentPage, INotifyPropertyChanged
    {
        public SuggestedPaymentRequest SuggestedPayment { get; set; }
        private BaseViewModel viewModel;
        public ObservableCollection<LookupValueInt> VATs { get; set; }
        public List<LookupValue> HinhThucThanhToans { get; set; }
        public List<LookupValue> LoaiHangHoas { get; set; }
        public List<LookupValue> LoaiDoiTuongs { get; set; }
        public ObservableCollection<LookupValue> DoiTuongs { get; set; }
        double _total;
        public double Total { get => _total; set { _total = value; OnPropertyChanging("Total"); } }
        public De_Nghi_Thanh_Toan_Cho_Nhan_Vien_Page()
        {
            InitializeComponent();
            SuggestedPayment = new SuggestedPaymentRequest { SuggestedPaymentHeader = new SuggestedPaymentHeader
            {
                PaytoType = 5, // loại đối tượng
                PaymentMethod = 2, // hình thức thanh toán
                DocumentType = 3, // nội địa trả trước
                Type = 2, // loai hàng hóa,
                LoginID = Preferences.Get(Config.User,"")
            } };
            //VATs = new ObservableCollection<LookupValueInt> { new LookupValueInt { Code = 0, Name = "0 %" },
            //                               new LookupValueInt { Code = 8, Name = "8 %" },
            //                               new LookupValueInt { Code = 10, Name = "10 %" }};
            OnPropertyChanged("VATs");

            HinhThucThanhToans = new List<LookupValue> { new LookupValue { Code = "1", Name = "Chuyển khoản" },
                                                         new LookupValue { Code = "2", Name = "Tiền mặt" }};
            OnPropertyChanged("HinhThucThanhToans");

            LoaiHangHoas = new List<LookupValue> { new LookupValue { Code = "2", Name = "Khác" },
                                                   new LookupValue { Code = "4", Name = "Công tác phí" }};
            OnPropertyChanged("LoaiHangHoas");
            LoaiDoiTuongs = new  List<LookupValue>{ new LookupValue { Code = "3", Name = "Nhà cung cấp" },
                                                               new LookupValue { Code = "5", Name = "Nhân viên" }};
            OnPropertyChanged("LoaiDoiTuongs");
            
            DoiTuongs = new ObservableCollection<LookupValue>();
            viewModel = new BaseViewModel();
            SuggestedPayment.SuggestedPaymentLines = new ObservableCollection<SuggestedPaymentLine>();
            SuggestedPayment.SuggestedPaymentLines.Add(new SuggestedPaymentLine { VAT = "8", Amount = 0 });
            
            OnPropertyChanged("SuggestedPayment");
            BindingContext = this;
        }

        private async void cbLoaiDoiTuong_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            LookupValue item = cbLoaiDoiTuong.SelectedItem as LookupValue;  
            if (item == null) return;
            DoiTuongs.Clear();  
            if (item.Code == "5")
            {
                viewModel.ShowLoading("Đang tải dữ liệu nhân viên");
                await Task.Delay(1000);
                var  result = await viewModel.RunHttpClientGet<LookupValue>($"api/dntt/GetEmployeeOfWorkShop?workcenter={Preferences.Get(Config.NhaMay,"")}&workshop={Preferences.Get(Config.MaXuong,"")}");
                DoiTuongs = result.Lists;
                OnPropertyChanged("DoiTuongs");
                viewModel.HideLoading();
            }   
            else
            {
                viewModel.ShowLoading("Đang tải dữ liệu nhà cung cấp");
                await Task.Delay(1000);
                var result = await viewModel.RunHttpClientGet<LookupValue>($"api/dntt/GetVendors");
                DoiTuongs = result.Lists;
                OnPropertyChanged("DoiTuongs");
                viewModel.HideLoading();
            }    
        }

        private async void addChiTiet_Tapped(object sender, EventArgs e)
        {
            try
            {
                SuggestedPayment.SuggestedPaymentLines.Insert(0, new SuggestedPaymentLine { VAT = "8", Amount = 0 });
                OnPropertyChanged("SuggestedPayment.SuggestedPaymentLines");
                Device.BeginInvokeOnMainThread(() =>
                {
                    listChiTiet.ScrollTo(SuggestedPayment.SuggestedPaymentLines.FirstOrDefault(), Xamarin.Forms.ScrollToPosition.Start, true);
                });
            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
                return; 
            }
            
        }

        private async void btnXoaChiTiet_Clicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as SfButton;
                if (button != null)
                {
                    var item = button.BindingContext as SuggestedPaymentLine;
                    if (item != null)
                    {
                        SuggestedPayment.SuggestedPaymentLines.Remove(item);
                        OnPropertyChanged("SuggestedPayment.SuggestedPaymentLines");
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageBox(ex.Message).Show();
                return;
            }
            
        }

        private async void GuiYeuCau_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (SuggestedPayment.SuggestedPaymentHeader.PaytoType == 0)
                {
                    await new MessageBox("Vui lòng chọn loại đối tượng").Show();
                    return;
                }
                if (SuggestedPayment.SuggestedPaymentHeader.Type == 0)
                {
                    await new MessageBox("Vui lòng chọn loại hàng hóa").Show();
                    return;
                }
                if (SuggestedPayment.SuggestedPaymentHeader.PaymentMethod == 0)
                {
                    await new MessageBox("Vui lòng chọn hình thức thanh toán").Show();
                    return;
                }
                if (string.IsNullOrEmpty(SuggestedPayment.SuggestedPaymentHeader.PaytoVendorNo_))
                {
                    await new MessageBox("Vui lòng chọn đối tượng thanh toán").Show();
                    return;
                }
                if (string.IsNullOrEmpty(SuggestedPayment.SuggestedPaymentHeader.Description))
                {
                    await new MessageBox("Vui lòng nhập nội dung thanh toán").Show();
                    return;
                }
                if (SuggestedPayment.SuggestedPaymentLines.Count ==0)
                {
                    await new MessageBox($"Chưa có chi tiết thanh toán").Show();
                    return;
                }
                foreach (var item in SuggestedPayment.SuggestedPaymentLines.Select((value,index) => (value, index)).ToList())
                {

                    if (item.value.Amount == 0)
                    {
                        await new MessageBox($"Vui lòng nhập số tiền cần thanh toán tại dòng thứ {item.index + 1}").Show();
                        return;
                    }    
                }

                var ask = await new MessageYesNo("Bạn có muốn gửi yêu cầu thanh toán này không?").Show();
                if (ask == DialogReturn.OK)
                {
                    var a= JsonConvert.SerializeObject(SuggestedPayment);
                    var response = await viewModel.RunHttpClientPost("api/dntt/GuiDeNghiThanhToan", SuggestedPayment);
                    if (response.IsSuccessStatusCode)
                    {
                        await new MessageBox("Gửi yêu cầu thành công").Show();
                        await Navigation.PopAsync();
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

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Total = SuggestedPayment.SuggestedPaymentLines.Sum(x => x.AmountIncludingVAT);
            OnPropertyChanged(nameof(Total));
        }
        

        private async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            ScrollView scrollView = sender as ScrollView;
            if (scrollView != null)
            {
                // Kiểm tra xem đã cuộn hết lên trên hay chưa
                if (e.ScrollY <= 0)
                {
                    // Cuộn xuống một lượng nhỏ để tránh việc ScrollView bị kẹt
                    await scrollView.ScrollToAsync(0, 1, true);
                }
            }
        }
    }
}