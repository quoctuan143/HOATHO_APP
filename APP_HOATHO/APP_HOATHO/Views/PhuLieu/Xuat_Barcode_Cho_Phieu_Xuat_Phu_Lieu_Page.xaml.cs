using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels;
using APP_HOATHO.ViewModels.PhuLieu;
using APP_HOATHO.Views.Barcode;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms; 
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.PhuLieu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Xuat_Barcode_Cho_Phieu_Xuat_Phu_Lieu_Page : ContentPage,INotifyPropertyChanged
    {
        public List<Chi_Tiet_Xuat_Phu_Lieu_Model> ListItem { get; set; }
        BaseViewModel viewModel = new BaseViewModel  ();
        public string BarcodeId { get; set; }
        public string Title { get; set; }
        public Xuat_Barcode_Cho_Phieu_Xuat_Phu_Lieu_Page(List<Chi_Tiet_Xuat_Phu_Lieu_Model> list , string barcodeId)
        {
            InitializeComponent();
            BarcodeId = barcodeId;
            ListItem = list;
            Title = $"Xuất barcode {barcodeId}";
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
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
            var item = o as Chi_Tiet_Xuat_Phu_Lieu_Model;
            if (item != null)
            {
                if (item.Name.ToLower().Contains(filterText) || item.Color.ToLower().Contains(filterText) || item.Art.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }        

        private async void btnXuatKho_Clicked(object sender, EventArgs e)
        {           

          var listNhap =  ListItem.Where(x=> x.SoLuongCanXuat >0).Select(x => new SoKhoNhapXuatPhuLieuTheoBarcode
          {
              BarcodeId = this.BarcodeId,
              Color_No_ = x.Color,
              Document_No_ = x.DocumentNo,
              Item_No_ = x.No_,
              Quantity = Convert.ToDecimal(x.SoLuongCanXuat),
              UserId = Preferences.Get(Config.User,"")
          }).ToList();

            if (listNhap.Count == 0) 
            {
                await new MessageBox("Vui lòng nhập số lượng cần xuất kho").Show();
                return;
            }
            string a = JsonConvert.SerializeObject(listNhap);
            var ok = await new MessageYesNo($"Bạn có muốn xuất kho cho thùng có barcode {this.BarcodeId}").Show();

            if (ok == DialogReturn.OK) 
            {
                viewModel.ShowLoading("Đang xử lý. vui lòng đợi...");
                var runOk = await viewModel.RunHttpClientPost("api/PhuLieu/xuatKhoPhuLieu", listNhap);
                viewModel.HideLoading();

                if (runOk.StatusCode  == System.Net.HttpStatusCode.OK)
                {                   
                    await new MessageBox("Đã xuất kho thành công").Show();
                    await Navigation.PopAsync();
                    MessagingCenter.Send(this, "reloadXuatKhoPhuLieu");
                }  
                else
                {
                    await new MessageBox(runOk.Content.ReadAsStringAsync().ToString()).Show();
                    return;
                }    
            }

        }
    }
}