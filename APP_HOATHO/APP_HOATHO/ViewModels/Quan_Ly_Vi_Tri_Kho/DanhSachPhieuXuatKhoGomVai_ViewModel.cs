using APP_HOATHO.Dialog;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho
{
    public class DanhSachPhieuXuatKhoGomVai_ViewModel : BaseViewModel
    {
        private DateTime _fromdate;

        public DateTime FromDate
        {
            get => _fromdate;
            set
            {
                SetProperty(ref _fromdate, value);
                OnAppearing();
            }
        }

        private DateTime _todate;

        public DateTime ToDate
        {
            get => _todate;
            set
            {
                SetProperty(ref _todate, value);
                OnAppearing();
            }
        }

        private ObservableCollection<DanhSachPhieuXuatKho_Model> _listItem;
        public ObservableCollection<DanhSachPhieuXuatKho_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }

        public DanhSachPhieuXuatKhoGomVai_ViewModel()
        {
            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ToDate = FromDate.AddMonths(1).AddDays(-1);
            ListItem = new ObservableCollection<DanhSachPhieuXuatKho_Model>();
            OnAppearing();
        }

        protected override async void OnAppearing()
        {
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                ShowLoading("Đang tải dữ liệu. vui lòng đợi");
                await Task.Delay(1000);
                string url = $"api/qltb/GetThongTinPhieuXuatKhoTongHop?fromdate={string.Format("{0:yyyy-MM-dd}", FromDate)}&todate={string.Format("{0:yyyy-MM-dd}", ToDate)}";
                ListItem.Clear();
                var a = await RunHttpClientGet<DanhSachPhieuXuatKho_Model>(url);
                ListItem = a.Lists;

                HideLoading();
            }
            catch (Exception ex)
            {
                IsBusy = false;
                HideLoading();
                await new MessageBox(ex.Message).Show();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}