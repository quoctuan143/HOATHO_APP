using APP_HOATHO.Converter;
using APP_HOATHO.Dialog;
using APP_HOATHO.Models.Nha_May_Soi;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho
{
    public class DanhSachPhieuXuatKhoChiTiet_Model : Bindable
    {
        public int RowID { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        
        public double SoLuongPhieu { get; set; }
        public double Quantity { get; set; }
        public string TrimArt { get; set; }
        public string DVT { get; set; }
        double _xuatTheoIdVai;
        public double XuatTheoIdVai { get => _xuatTheoIdVai; set { 
                SetProperty(ref _xuatTheoIdVai, value);
                if (Quantity == XuatTheoIdVai)
                {
                    XuatOk = StatusFormatColor.HoanThanh;
                    OnPropertyChanged("XuatOk");
                }
                else if (XuatTheoIdVai == 0)
                {
                    XuatOk = StatusFormatColor.ChuaHoanThanh;
                    OnPropertyChanged("XuatOk");
                }
                else
                {
                    XuatOk = StatusFormatColor.DoDang;
                    OnPropertyChanged("XuatOk");
                }    
            } 
        }

        double _slkhautru = 0;
        public double SoLuongKhauTru
        {             
        get
            {
                return _slkhautru;
            }
            set
            {
                try
                {
                    if (value > SoLuongPhieu)
                    {
                        new MessageBox("Số lượng khấu trừ vượt quá số lượng phiếu").Show();
                        Quantity = SoLuongPhieu - SoLuongKhauTruBanDau;
                        _slkhautru = SoLuongKhauTruBanDau;
                        value = SoLuongKhauTruBanDau;                       
                        OnPropertyChanged("SoLuongKhauTru");
                        OnPropertyChanged("Quantity");
                        OnPropertyChanged("FormatNumberSLKhauTru");
                    }
                    else
                    {
                        Quantity = SoLuongPhieu - value;
                       
                        OnPropertyChanged("Quantity");
                    }
                    SetProperty(ref _slkhautru, value);
                }
                catch{
                    
                }
                
               
            }
        }
        public double SoLuongKhauTruBanDau { get; set; }
        public string FormatNumberSLKhauTru
        {
            get => string.Format("{0:#,##0.##}", SoLuongKhauTru);
            set
            {
                try
                {
                    if (!CheckThapPhan(value))
                    {
                        FormatNumberString(ref _slkhautru, value);
                        OnPropertyChanged("FormatNumberSLKhauTru");                       
                        SoLuongKhauTru = _slkhautru;
                    }
                }
                catch
                {                    
                }
                
            }
        }
        public StatusFormatColor XuatOk { get; set; }        
    }
}
