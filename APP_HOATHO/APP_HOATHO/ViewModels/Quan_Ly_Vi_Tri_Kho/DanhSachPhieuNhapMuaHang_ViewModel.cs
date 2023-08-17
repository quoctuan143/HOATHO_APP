using APP_HOATHO.Dialog;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho
{
    public class DanhSachPhieuNhapMuaHang_ViewModel : BaseViewModel
    {
        #region "Property"
        public  INavigation navigation;
        private LookupValue _customer;
        public LookupValue Customer { get => _customer; set => SetProperty(ref _customer, value); }
        private LookupValue _vendor;
        public LookupValue Vendor { get => _vendor; set => SetProperty(ref _vendor, value); }
        private LookupValue _location;
        public LookupValue Location { get => _location; set => SetProperty(ref _location, value); }
        
        public LookupValue UpdateQR { get; set; }
        public ObservableCollection< LookupValue> ListUpdateQR { get; set; } 
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        ObservableCollection<LookupValue> ListCustomer { get; set; }
        ObservableCollection<LookupValue> ListVendor { get; set; }
        ObservableCollection<LookupValue> ListLocation { get; set; }
        private ObservableCollection<DanhSachPhieuNhapMuaHang_Model> _listItem;
        public ObservableCollection<DanhSachPhieuNhapMuaHang_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }

        #endregion "Property"

        #region "Command"

        public ICommand FindCommand { get; set; }
        public ICommand LoadDataCommand { get; set; }
        public ICommand ChooseCustomerCommand { get; set; }
        public ICommand ChooseVendorCommand { get; set; }
        public ICommand ChooseLocationCommand { get; set; }
       
        #endregion "Command"

        #region "Function"

        public DanhSachPhieuNhapMuaHang_ViewModel()
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            UpdateQR = new LookupValue { Code = "0", Name = "" };
            Vendor = new LookupValue { Code = "%", Name = "Tất cả" };
            Customer = new LookupValue { Code = "%", Name = "Tất cả" };
            Location = new LookupValue { Code = "%", Name = "Tất cả" };
            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ToDate = date.AddMonths(1).AddDays(-1);
            ListUpdateQR = new ObservableCollection<LookupValue>(new LookupValue[] { new LookupValue { Code = "0", Name = "Chưa update QR code" },
            new LookupValue { Code = "1", Name = "Đã hoàn thành update QR code" } });

            Task.Factory.StartNew( async() =>
            {
                try
                {
                    ListCustomer = new ObservableCollection<LookupValue>();
                    ListVendor = new ObservableCollection<LookupValue>();
                    ListLocation = new ObservableCollection<LookupValue>();
                    string url = $"api/qltb/GetKhachHang";
                    ListCustomer.Clear();
                    var a = await RunHttpClientGet<LookupValue>(url);
                    ListCustomer = a.Lists;

                    string url1 = $"api/qltb/GetNhaCungCap";
                    ListVendor.Clear();
                    var b = await RunHttpClientGet<LookupValue>(url1);
                    ListVendor = b.Lists;

                    string url2 = $"api/qltb/GetMaKho";
                    ListLocation.Clear();
                    var c = await RunHttpClientGet<LookupValue>(url2);
                    ListLocation = c.Lists;
                }
                catch (Exception)
                {

                    
                }
               
            });
            ListItem = new ObservableCollection<DanhSachPhieuNhapMuaHang_Model>();
            FindCommand = new Command(findData);
            LoadDataCommand = new Command(loadData);
            ChooseCustomerCommand = new Command(async () =>
            {
                var lookup = new LookupItem(ListCustomer,"Danh sách khách hàng");
                lookup.closeForm +=  (s,e) => 
                { 
                    Customer = e;                    
                };
                
               await navigation.PushModalAsync(lookup);
            });
            ChooseLocationCommand = new Command(async () =>
            {
                var lookup = new LookupItem(ListUpdateQR, "Trạng thái cập nhật QR code");
                lookup.closeForm += (s, e) =>
                {
                    UpdateQR = e;
                    OnPropertyChanged(nameof(UpdateQR));
                };

                await navigation.PushModalAsync(lookup);
            });

            ChooseVendorCommand = new Command(async () =>
            {
                var lookup = new LookupItem(ListVendor, "Danh sách nhà cung cấp");
                lookup.closeForm += (s, e) =>
                {
                    Vendor = e;
                };

                await navigation.PushModalAsync(lookup);
            });
        }

        

        private async void loadData()
        {
            try
            {
                ShowLoading("Đang tải dữ liệu. vui lòng đợi");
                await Task.Delay(1000);
                string url = $"api/qltb/GetThongTinPhieuNhapMuaHang?vendor={_vendor.Code}&customer={_customer.Code}&location={_location.Code}&fromdate={string.Format("{0:yyyy-MM-dd}", FromDate)}&todate={string.Format("{0:yyyy-MM-dd}", ToDate)}&updateQR={UpdateQR.Code}";
                ListItem.Clear();
                var a = await RunHttpClientGet<DanhSachPhieuNhapMuaHang_Model>(url);
                ListItem = a.Lists;
                
                HideLoading();
            }
            catch (Exception ex)
            {
                HideLoading();
                await new MessageBox(ex.Message).Show();
            }
        }

        private async void findData()
        {
            try
            {
                ShowLoading("Đang tải dữ liệu. vui lòng đợi");
                await Task.Delay(1000);
                string url = $"api/qltb/GetThongTinPhieuNhapMuaHang?vendor={Vendor.Code}&customer={_customer.Code}&location={_location.Code}&fromdate={string.Format ("{0:yyyy-MM-dd}",FromDate)}&todate={string.Format("{0:yyyy-MM-dd}", ToDate)}&updateQR={UpdateQR.Code}";
                ListItem.Clear();
                var a = await RunHttpClientGet<DanhSachPhieuNhapMuaHang_Model>(url);
                ListItem = a.Lists;
                HideLoading();
            }
            catch (Exception ex)
            {
                HideLoading();
                await new MessageBox(ex.Message).Show();
            }
        }

        #endregion "Function"
    }
}