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
    public partial class De_Nghi_Thanh_Toan_Cho_Nhan_Vien_Page : ContentPage, INotifyPropertyChanged
    {
        SuggestedPaymentRequest _suggestedPayment;
        public SuggestedPaymentRequest SuggestedPayment { get => _suggestedPayment; set => SetProperty(ref _suggestedPayment,value); }
        public BaseViewModel viewModel { get; set; }
        public ObservableCollection<LookupValueInt> VATs { get; set; }
        public List<LookupValue> HinhThucThanhToans { get; set; }
        public List<LookupValue> LoaiHangHoas { get; set; }
        public List<LookupValue> LoaiDoiTuongs { get; set; }
        public List<LookupValue> LoaiThanhToans { get; set; }
        public ObservableCollection<LookupValue> DoiTuongs { get; set; }
        double _total;
        public double Total { get => _total; set { _total = value; OnPropertyChanging("Total"); } }
        string DocumentNo;
        public  De_Nghi_Thanh_Toan_Cho_Nhan_Vien_Page(string documentno)
        {
            InitializeComponent();
            this.DocumentNo = documentno;
            viewModel = new BaseViewModel();            
               
            OnPropertyChanged("VATs");

            HinhThucThanhToans = new List<LookupValue> { new LookupValue { Code = "1", Name = "Chuyển khoản" },
                                                         new LookupValue { Code = "2", Name = "Tiền mặt" },
                                                         new LookupValue { Code = "6", Name = "Nộp điện tử" },
                                                         new LookupValue { Code = "7", Name = "Qua lương" }};
            OnPropertyChanged("HinhThucThanhToans");

            LoaiHangHoas = new List<LookupValue> { new LookupValue { Code = "2", Name = "Khác" },
                                                   new LookupValue { Code = "4", Name = "Công tác phí" }};
            OnPropertyChanged("LoaiHangHoas");
            LoaiDoiTuongs = new  List<LookupValue>{ new LookupValue { Code = "3", Name = "Nhà cung cấp" },
                                                               new LookupValue { Code = "5", Name = "Nhân viên" }};
            OnPropertyChanged("LoaiDoiTuongs");

            LoaiThanhToans = new List<LookupValue>{ new LookupValue { Code = "3", Name = " NĐ Trả trước" },
                                                               new LookupValue { Code = "4", Name = "NĐ Trả sau" }};
            OnPropertyChanged("LoaiDoiTuongs");


            DoiTuongs = new ObservableCollection<LookupValue>();
            BindingContext = this;
        }

        protected  override async void OnAppearing()
        {
            base.OnAppearing();
            if (SuggestedPayment != null)
                return;

            if (!string.IsNullOrEmpty(DocumentNo))
            {
                SuggestedPayment = await viewModel.RunHttpClientGetObject<SuggestedPaymentRequest>($"api/dntt/GetChiTietDNTT?documentNo={DocumentNo}");
            }
            else
            {
                SuggestedPayment = new SuggestedPaymentRequest
                {
                    SuggestedPaymentHeader = new SuggestedPaymentHeader
                    {
                        PaytoType = 5, // loại đối tượng
                        PaymentMethod = 2, // hình thức thanh toán
                        DocumentType = 4, // nội địa trả sau
                        Type = 4, // loai hàng hóa,
                        LoginID = Preferences.Get(Config.User, "")
                    }
                };
                SuggestedPayment.SuggestedPaymentLines = new ObservableCollection<SuggestedPaymentLine>();
                SuggestedPayment.SuggestedPaymentLines.Add(new SuggestedPaymentLine { VAT = "8", Amount = 0 });
            }
            OnPropertyChanged("SuggestedPayment");
        }
        private async void cbLoaiDoiTuong_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            LookupValue item = cbLoaiDoiTuong.SelectedItem as LookupValue;  
            if (item == null) return;
            var doituong = SuggestedPayment.SuggestedPaymentHeader.PaytoVendorNo_;
            DoiTuongs.Clear();  
            if (item.Code == "5")
            {
                viewModel.ShowLoading("Đang tải dữ liệu nhân viên");
                await Task.Delay(1000);
                var  result = await viewModel.RunHttpClientGet<LookupValue>($"api/dntt/GetEmployeeOfWorkShop?workcenter={Preferences.Get(Config.NhaMay,"")}&workshop={Preferences.Get(Config.MaXuong,"")}");
                DoiTuongs = result.Lists;                         
                viewModel.HideLoading();
            }   
            else
            {
                viewModel.ShowLoading("Đang tải dữ liệu nhà cung cấp");
                await Task.Delay(1000);
                var result = await viewModel.RunHttpClientGet<LookupValue>($"api/dntt/GetVendors");
                DoiTuongs = result.Lists;                
                viewModel.HideLoading();
            }
            
            if (!string.IsNullOrEmpty(doituong))
            {
                var item1 = DoiTuongs.FirstOrDefault(x => x.Code == SuggestedPayment.SuggestedPaymentHeader.PaytoVendorNo_);
                cbdoituongthanhtoan.SelectedItem = item1;
            }
            OnPropertyChanged("DoiTuongs");
        }

        private async void addChiTiet_Tapped(object sender, EventArgs e)
        {
            try
            {
                SuggestedPayment.SuggestedPaymentLines.Insert(0, new SuggestedPaymentLine { VAT = "8", Amount = 0 , IsShow = SuggestedPayment.SuggestedPaymentHeader.Type != 2 ? true : false });
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
                        Total = SuggestedPayment.SuggestedPaymentLines.Sum(x => x.AmountIncludingVAT ?? 0);
                        OnPropertyChanged("Total");
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageBox(ex.Message).Show();
                return;
            }
            
        }

        

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if (entry != null)
            {
                var item = entry.BindingContext as SuggestedPaymentLine;
                if (item != null)
                {
                   switch(  entry.StyleId )
                    {                        
                        case "txtSoNgay":
                        case "txtDonGia":      
                            if (item.DonGia > 0 || item.SoNgay > 0)
                            {
                                item.VAT = "0";
                                item.AmountChuaVat = item.DonGia * item.SoNgay;
                               
                            }
                            break;
                    }

                   Total = SuggestedPayment.SuggestedPaymentLines.Sum(x => x.AmountIncludingVAT ?? 0);
                   OnPropertyChanged("Total");
                }
            }
            
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

        private void cbLoaiHangHoa_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var item = cbLoaiHangHoa.SelectedItem as LookupValue;
            if (item != null)
            {
                if (item.Code == "4")
                {
                    SuggestedPayment.SuggestedPaymentLines.ForEach(x => { x.IsShow = true;});
                    OnPropertyChanged("SuggestedPayment");
                }    
                else
                {
                    SuggestedPayment.SuggestedPaymentLines.ForEach(x => x.IsShow = false);
                    OnPropertyChanged("SuggestedPayment");
                }    
            }    
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
           [CallerMemberName] string propertyName = "",
           Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private async void XemDNTT_Clicked(object sender, EventArgs e)
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
                    if (SuggestedPayment.SuggestedPaymentHeader.DocumentType == 0)
                    {
                        await new MessageBox("Vui lòng chọn loại thanh toán trả trước hoặc trả sau").Show();
                        return;
                    }
                if (SuggestedPayment.SuggestedPaymentHeader.HanThanhToan.HasValue && (SuggestedPayment.SuggestedPaymentHeader.HanThanhToan.Value.DayOfWeek == DayOfWeek.Saturday || SuggestedPayment.SuggestedPaymentHeader.HanThanhToan.Value.DayOfWeek == DayOfWeek.Sunday))
                {
                    await new MessageBox("Hạn thanh toán không được chọn thứ 7 hoặc chủ nhật").Show();
                    return;
                }
                if (SuggestedPayment.SuggestedPaymentLines.Count == 0)
                    {
                        await new MessageBox($"Chưa có chi tiết thanh toán").Show();
                        return;
                    }
                    foreach (var item in SuggestedPayment.SuggestedPaymentLines.Select((value, index) => (value, index)).ToList())
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

                    var doituong = DoiTuongs.FirstOrDefault(x => x.Code == SuggestedPayment.SuggestedPaymentHeader.PaytoVendorNo_);
                    ViewSuggestedPayment viewSuggested = new ViewSuggestedPayment
                    {
                        SuggestedPaymentRequest = SuggestedPayment,
                        ThoiHanThanhToan = SuggestedPayment.SuggestedPaymentHeader.HanThanhToan.Value,
                        NoiDung = SuggestedPayment.SuggestedPaymentHeader.Description,
                        NganHang = doituong?.Description2,
                        SoTaiKhoan = doituong?.Description,
                        TenDoiTuong = doituong.Name,
                        SoTien = this.Total,
                        HinhThucThanhToan = HinhThucThanhToans.FirstOrDefault(x=>x.Code == SuggestedPayment.SuggestedPaymentHeader.PaymentMethod.ToString())?.Name,
                        LoaiThanhToan = LoaiThanhToans.FirstOrDefault(x => x.Code == SuggestedPayment.SuggestedPaymentHeader.DocumentType.ToString())?.Name,
                    };
                    await Navigation.PushAsync(new Xem_Truoc_De_Nghi_Thanh_Toan_Page(viewSuggested));
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }

                
            
            
        }

        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var entry = sender as Picker;
            if (entry != null)
            {
                var item = entry.BindingContext as SuggestedPaymentLine;  
                Total = SuggestedPayment.SuggestedPaymentLines.Sum(x => x.AmountIncludingVAT ?? 0);
                OnPropertyChanged("Total");
             }
            

        }
    }
}