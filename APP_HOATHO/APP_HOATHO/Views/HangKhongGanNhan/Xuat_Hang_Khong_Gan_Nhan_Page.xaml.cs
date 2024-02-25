using APP_HOATHO.Converter;
using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.HangKhongGanNhan;
using APP_HOATHO.ViewModels.HangKhongGanNhan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.HangKhongGanNhan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]   

    public partial class Xuat_Hang_Khong_Gan_Nhan_Page : ContentPage
    {
        private Xuat_Hang_Khong_Gan_Nhan_ViewModel viewModel;

        public Xuat_Hang_Khong_Gan_Nhan_Page(string kevai)
        {
            InitializeComponent();
            viewModel = new Xuat_Hang_Khong_Gan_Nhan_ViewModel(kevai);
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListItem.Count == 0)
            {
                viewModel.LoadDataCommand.Execute(null);
            }
        }

        private string filterText = string.Empty;

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listChiTiet.View.Filter = FilterRecords;
            listChiTiet.View.RefreshFilter();
        }

        public bool FilterRecords(object o)
        {
            filterText = filterText.ToLower();
            var item = o as Xuat_Hang_Khong_Gan_Nhan_Model;
            if (item != null)
            {
                if (ChonNhom1 == StatusFormatColor.TatCa)
                {
                    if (item.RollNo.ToLower().Contains(filterText) || item.Color.ToLower().Contains(filterText) || item.ItemName.ToLower().Contains(filterText) || item.Art.ToLower().Contains(filterText))
                        return true;
                }
                else
                {
                    if ((item.RollNo.ToLower().Contains(filterText) || item.Color.ToLower().Contains(filterText) || item.ItemName.ToLower().Contains(filterText) || item.Art.ToLower().Contains(filterText)) && item.Chon == isChon)
                        return true;
                }
            }
            return false;
        }

        private StatusFormatColor ChonNhom1 = StatusFormatColor.TatCa;
        public bool isChon;

        private void ChonNhom_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (e.Value.ToString() == "Tất Cả")
            {
                ChonNhom1 = StatusFormatColor.TatCa;
            }
            if (e.Value.ToString() == "Chưa chọn")
            {
                ChonNhom1 = StatusFormatColor.ChuaHoanThanh;
                isChon = false;
            }
            
            if (e.Value.ToString() == "Đã chọn")
            {
                ChonNhom1 = StatusFormatColor.DoDang;
                isChon = true;
            }
            listChiTiet.View.Filter = FilterRecords;
            listChiTiet.View.RefreshFilter();
        }
    }
}