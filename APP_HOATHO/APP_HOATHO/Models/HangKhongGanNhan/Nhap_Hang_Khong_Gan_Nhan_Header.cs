using APP_HOATHO.Converter;
using APP_HOATHO.Models.Nha_May_Soi;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.HangKhongGanNhan
{
    public class Nhap_Hang_Khong_Gan_Nhan_Header_Model
    {
        public string No_ { get; set; }
        public string ExternalDocumentNo { get; set; }
        public string VendorName { get; set; }
        public string LocationName { get; set; }
        public string CustomerName { get; set; }
        public DateTime PostingDate { get; set; }
        public double SoKien { get; set; }
        public bool DaCapNhatQR { get; set; }      
        public string Description { get; set; }
        public bool DaDinhViKe { get; set; }
    }

    public class Nhap_Hang_Khong_Gan_Nhan_Line_Model :Bindable
    {
        public bool Chon { get; set; }
        public string RollNo { get; set; }
        public double InvoicedMeter { get; set; }
        public string DocumentNo_ { get; set; }
        public string ItemName { get; set; }
        public string Color { get; set; }
        public string Art { get; set; }
        public string Id { get; set; }
        public string PositionId { get; set; }
        public StatusFormatColor DaXepLenKe { set; get; }
        public string LoginId { get; set; }
        bool _isReadonly;
        public bool IsReadonly { get => DaXepLenKe == StatusFormatColor.ChuaHoanThanh; set => SetProperty (ref _isReadonly ,value); }
    }
    public class Xuat_Hang_Khong_Gan_Nhan_Model : Bindable
    {
        public bool Chon { get; set; }
        public string RollNo { get; set; }
        public double InvoicedMeter { get; set; }        
        public string ItemName { get; set; }
        public string Color { get; set; }
        public string Art { get; set; }
        public string Id { get; set; }    
        public string ItemNo { get; set; }
    }
}
