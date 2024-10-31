using APP_HOATHO.Dialog;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using APP_HOATHO.Models.PhuLieu;

namespace APP_HOATHO.ViewModels.PhuLieu
{
    public class Nhap_Phu_Lieu_ViewModel : BaseViewModel
    {
        public INavigation navigation;   
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        
        private ObservableCollection<DanhSachPhieuNhapXuatPhuLieu_Model> _listItem;
        public ObservableCollection<DanhSachPhieuNhapXuatPhuLieu_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public DanhSachPhieuNhapXuatPhuLieu_Model SelectedItem { get; set; }
        public ICommand FindCommand { get; set; }
        public ICommand LoadDataCommand { get; set; }

        public Nhap_Phu_Lieu_ViewModel()
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);           
            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ToDate = date.AddMonths(1).AddDays(-1);
     
            ListItem = new ObservableCollection<DanhSachPhieuNhapXuatPhuLieu_Model>();
            FindCommand = new Command(findData);
            LoadDataCommand = new Command(loadData);            
        }

        private async void loadData()
        {
            try
            {
                ShowLoading("Đang tải dữ liệu. vui lòng đợi");
                await Task.Delay(1000);
                string url = $"api/PhuLieu/getPhieuNhapPhuLieu";
                ListItem.Clear();
                var a = await RunHttpClientGet<DanhSachPhieuNhapXuatPhuLieu_Model>(url, new {FromDate = this.FromDate, ToDate = this.ToDate});
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
                string url = $"api/PhuLieu/getPhieuNhapPhuLieu";
                ListItem.Clear();
                var a = await RunHttpClientGet<DanhSachPhieuNhapXuatPhuLieu_Model>(url, new { FromDate = this.FromDate, ToDate = this.ToDate });
                ListItem = a.Lists;
                HideLoading();
            }
            catch (Exception ex)
            {
                HideLoading();
                await new MessageBox(ex.Message).Show();
            }
        }

    }
}