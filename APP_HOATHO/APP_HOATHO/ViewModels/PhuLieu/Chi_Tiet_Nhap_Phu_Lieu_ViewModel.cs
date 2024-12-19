using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.PhuLieu;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.PhuLieu
{    
    public class Chi_Tiet_Nhap_Phu_Lieu_ViewModel : BaseViewModel
    {
        public string SoChungTu { get; set; }
        public INavigation navigation { get; set; }
        ObservableCollection<Chi_Tiet_Nhap_Phu_Lieu_Model> _listItem;
        public ObservableCollection<Chi_Tiet_Nhap_Phu_Lieu_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public Chi_Tiet_Nhap_Phu_Lieu_Model SelectItem { get; set; }
        public ICommand LoadDataCommand { get; set; }
        public ICommand AddPackingListCommand { get; set; }        

        public Chi_Tiet_Nhap_Phu_Lieu_ViewModel(string soChungTu)
        {
            SoChungTu = soChungTu;
            ListItem = new ObservableCollection<Chi_Tiet_Nhap_Phu_Lieu_Model>();
            LoadDataCommand = new Command(async () =>
            {
                try
                {
                    ListItem.Clear();
                    var url = $"api/PhuLieu/getChiTietNhapKhoPhuLieu?documentNo={SoChungTu}";
                    var a = await RunHttpClientGet<Chi_Tiet_Nhap_Phu_Lieu_Model>(url);
                    ListItem = a.Lists;
                }
                catch (System.Exception ex)
                {

                    await new MessageBox(ex.Message).Show();
                }

            });

            AddPackingListCommand = new Command(async () => {
                try
                {                   
                    await navigation.PushAsync(new Nhap_Barcode_Cho_Phieu_Nhap_Phu_Lieu_Page(ListItem.ToList()));
                }
                catch (System.Exception ex)
                {

                    await new MessageBox(ex.Message).Show();
                }

            });
            MessagingCenter.Subscribe<Nhap_Barcode_Cho_Phieu_Nhap_Phu_Lieu_Page>(this, "reloadNhapKhoPhuLieu", (e) => LoadDataCommand.Execute(null));
        }
    }

    public class Chi_Tiet_Nhap_Phu_Lieu_Model : PurchaseLine_Model
    {
        public string DocumentNo { get; set; }  
        public double? SoLuongDaNhap { get; set; }  
        double _soLuongCanNhap = 0;
        public double SoLuongCanNhap { get; set; }
        public string FormatSoLuongCanNhap
        {

            get {
                try
                {
                    return string.Format("{0:#,##0.##}", SoLuongCanNhap);
                }
                catch (Exception)
                {

                    return "0";
                }
                
            }

            set
            {
                if (!CheckThapPhan(value))
                {
                    try
                    {
                        FormatNumberString(ref _soLuongCanNhap, value);
                        SoLuongCanNhap = _soLuongCanNhap;
                        OnPropertyChanged("FormatSoLuongCanNhap");
                        OnPropertyChanged("SoLuongCanNhap");
                    }
                    catch
                    {

                    }

                }
            }
        }
    }

    public class Chi_Tiet_Xuat_Phu_Lieu_Model : PurchaseLine_Model
    {
        public string DocumentNo { get; set; }
        public decimal? SoLuongDaXuat { get; set; }
        public double TonKho { get; set; } = 0;
        double _soLuongCanXuat = 0;
        
        public double SoLuongCanXuat 
        { 
            get => _soLuongCanXuat; 
            set 
            {
                SetProperty(ref _soLuongCanXuat, value);
                if (value > TonKho)
                {
                    new MessageBox("Số lượng cần xuất vượt quá tồn kho").Show();
                    SoLuongCanXuat = 0;
                    OnPropertyChanged("SoLuongCanXuat");
                }    
            } 
        } 
        
        public string FormatSoLuongCanXuat
        {

            get
            {
                try
                {
                    return string.Format("{0:#,##0.##}", SoLuongCanXuat);
                }
                catch (Exception)
                {

                    return "0";
                }

            }

            set
            {
                if (!CheckThapPhan(value))
                {
                    try
                    {
                        FormatNumberString(ref _soLuongCanXuat, value);
                        SoLuongCanXuat = _soLuongCanXuat;
                        OnPropertyChanged("FormatSoLuongCanXuat");
                        OnPropertyChanged("SoLuongCanXuat");
                    }
                    catch
                    {

                    }

                }
            }
        }
    }

    public class PhieuNhapPhuLieuPackingList
    {
        public int RowId { get; set; }
        [JsonProperty("Item No_")]
        public string Item_No_ { get; set; }

        [JsonProperty("Color No_")]
        public string Color_No_ { get; set; }

        public decimal? Quantity { get; set; }

        public string BarcodeId { get; set; }

        public string Position { get; set; }

        [JsonProperty("Document No_")]
        public string Document_No_ { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UserId { get; set; }      
        public string ViTriLuu { get; set; }

    }

    public class SoKhoNhapXuatPhuLieuTheoBarcode
    {
        public int RowId { get; set; }
        [JsonProperty("Item No_")]
        public string Item_No_ { get; set; }
        [JsonProperty("Color No_")]
        public string Color_No_ { get; set; }

        public decimal? Quantity { get; set; }

        public string BarcodeId { get; set; }
        [JsonProperty("Document No_")]
        public string Document_No_ { get; set; }
        [JsonProperty("Document Type")]
        public int? Document_Type { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UserId { get; set; }

    }
}
