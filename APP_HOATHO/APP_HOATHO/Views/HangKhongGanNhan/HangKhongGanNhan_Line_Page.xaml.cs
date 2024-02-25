using APP_HOATHO.Converter;
using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.HangKhongGanNhan;
using APP_HOATHO.ViewModels.HangKhongGanNhan;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
using System;
using System.Linq;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.HangKhongGanNhan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HangKhongGanNhan_Line_Page : ContentPage
    {
        private HangKhongGanNhan_Line_ViewModel viewModel;

        public HangKhongGanNhan_Line_Page(string soChungTu)
        {
            InitializeComponent();
            viewModel = new HangKhongGanNhan_Line_ViewModel(soChungTu);
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
            var item = o as Nhap_Hang_Khong_Gan_Nhan_Line_Model;
            if (item != null)
            {
                if (ChonNhom1 == StatusFormatColor.TatCa)
                {
                    if (item.RollNo.ToLower().Contains(filterText) || item.Color.ToLower().Contains(filterText) || item.PositionId.ToLower().Contains(filterText) || item.Art.ToLower().Contains(filterText))
                        return true;
                }
                else
                {
                    if ((item.RollNo.ToLower().Contains(filterText) || item.Color.ToLower().Contains(filterText) || item.PositionId.ToLower().Contains(filterText) || item.Art.ToLower().Contains(filterText)) && item.DaXepLenKe == ChonNhom1)
                        return true;
                }
            }
            return false;
        }

        private Nhap_Hang_Khong_Gan_Nhan_Line_Model _selectItem;

        private void listChiTiet_SwipeEnded(object sender, Syncfusion.SfDataGrid.XForms.SwipeEndedEventArgs e)
        {
            _selectItem = e.RowData as Nhap_Hang_Khong_Gan_Nhan_Line_Model;
        }

        private async void btnXoaBarcodeId_Tapped(object sender, EventArgs e)
        {
            try
            {
                var find = viewModel.ListItem.FirstOrDefault(x => x.Id == _selectItem.Id);
                if (find != null)
                {
                    var ok = await Config.client.PostAsJsonAsync("api/HangKhongGanNhan/XoaCayVaiKhoiKe", find);
                    if (ok.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        find.PositionId = "";
                        find.DaXepLenKe = StatusFormatColor.ChuaHoanThanh;
                        listChiTiet.Refresh();
                    }
                    else
                    {
                        await new MessageBox(ok.Content.ReadAsStringAsync().Result.ToString()).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageBox(ex.Message).Show();
            }
        }

        private StatusFormatColor ChonNhom1 = StatusFormatColor.TatCa;

        private void ChonNhom_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (e.Value.ToString() == "Tất Cả")
            {
                ChonNhom1 = StatusFormatColor.TatCa;
            }
            if (e.Value.ToString() == "Chưa Hoàn Thành")
            {
                ChonNhom1 = StatusFormatColor.ChuaHoanThanh;
            }
            if (e.Value.ToString() == "Dở Dang")
            {
                ChonNhom1 = StatusFormatColor.DoDang;
            }
            if (e.Value.ToString() == "Hoàn Thành")
            {
                ChonNhom1 = StatusFormatColor.HoanThanh;
            }
            listChiTiet.View.Filter = FilterRecords;
            listChiTiet.View.RefreshFilter();
        }
    }
}