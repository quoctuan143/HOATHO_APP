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
using APP_HOATHO.Models.HangKhongGanNhan;

namespace APP_HOATHO.ViewModels.HangKhongGanNhan
{
    public class HangKhongGanNhan_Header_ViewModel : BaseViewModel 
    {
        #region "Property"
        public INavigation navigation;       
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        
        private ObservableCollection<Nhap_Hang_Khong_Gan_Nhan_Header_Model> _listItem;
        public ObservableCollection<Nhap_Hang_Khong_Gan_Nhan_Header_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }

        #endregion "Property"

        #region "Command"
      
        public ICommand LoadDataCommand { get; set; }
        public ICommand FindCommand { get; set; }
        #endregion "Command"

        #region "Function"
        public HangKhongGanNhan_Header_ViewModel()
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            
            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ToDate = date.AddMonths(1).AddDays(-1);
            ListItem = new ObservableCollection<Nhap_Hang_Khong_Gan_Nhan_Header_Model>();           
            LoadDataCommand = new Command(loadData);
            FindCommand = new Command(async () =>
            {
                try
                {
                    ShowLoading("Đang tải dữ liệu. vui lòng đợi");
                    await Task.Delay(1000);
                    string url = $"api/HangKhongGanNhan/LayDanhSachPhieuNhapHangKhongGanNhan?fromdate={string.Format("{0:yyyy-MM-dd}", FromDate)}&todate={string.Format("{0:yyyy-MM-dd}", ToDate)}";
                    ListItem.Clear();
                    var a = await RunHttpClientGet<Nhap_Hang_Khong_Gan_Nhan_Header_Model>(url);
                    ListItem = a.Lists;

                    HideLoading();
                }
                catch (Exception ex)
                {
                    HideLoading();
                    await new MessageBox(ex.Message).Show();
                }
            });
        }


        private async void loadData()
        {
            try
            {
                ShowLoading("Đang tải dữ liệu. vui lòng đợi");
                await Task.Delay(1000);
                string url = $"api/HangKhongGanNhan/LayDanhSachPhieuNhapHangKhongGanNhan?fromdate={string.Format("{0:yyyy-MM-dd}", FromDate)}&todate={string.Format("{0:yyyy-MM-dd}", ToDate)}";                
                var a = await RunHttpClientGet<Nhap_Hang_Khong_Gan_Nhan_Header_Model>(url);
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
