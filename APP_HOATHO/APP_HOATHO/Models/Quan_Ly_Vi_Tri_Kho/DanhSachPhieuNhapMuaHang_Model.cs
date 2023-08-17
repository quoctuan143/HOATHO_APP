using APP_HOATHO.Models.Nha_May_Soi;
using System;

namespace APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho
{
    public class DanhSachPhieuNhapMuaHang_Model : Bindable
    {
        public string No_ { get; set; }
        public string ExternalDocumentNo { get; set; }
        public string VendorName { get; set; }
        public string LocationName { get; set; }
        public string CustomerName { get; set; }
        public DateTime PostingDate { get; set; }
        public double SoKien { get; set; }
        public bool DaCapNhatQR { get; set; }   
    }
}