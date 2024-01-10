using System;

namespace APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho
{
    public class ThongTinCayVaiBase
    {
        public string RollNo { get; set; }
        public string ParcelNo { get; set; }
        public string BarcodeId { get; set; }
        public string ItemNo { get; set; }
        public string Color { get; set; }
        public string Art { get; set; }
        public string ItemName { get; set; }
        public double? TonKho { get; set; }
        public string PositionId { get; set; }
    }

    public class ThongTinChiTietCayVai_Model : ThongTinCayVaiBase
    {
        public double? GrossLength { get; set; }
        public double? Quantity { get; set; }
        public double? Width { get; set; }   
        public string ViTri { get; set; }
        public string PhieuNhap { get; set; }
        public string LoVai { get; set; }
        public string DVT { get; set; }
        public DateTime? NgayNhap { get; set; } 
    }

    public class ThongTinChiTietKeVai_Model : ThongTinCayVaiBase
    {
        public string Id { get; set; }
        public double? InvoicedMeter { get; set; }        
    }
}