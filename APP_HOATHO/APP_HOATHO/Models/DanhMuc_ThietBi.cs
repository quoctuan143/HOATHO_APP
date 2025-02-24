using APP_HOATHO.Models.Nha_May_Soi;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models
{
   public class DanhMuc_ThietBi : Bindable
    {
        public string No_ { get; set; }
        public string No_2 { get; set; }
        public string No_3 { get; set; }
        public string NameVN { get; set; }
        public string ItemCategory { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string LinkVideo { get; set; }
        public string TaiLieuKiThuat { get; set; }
        public string Picture { get; set; } // link ảnh
        public string ManufacturerCode { get; set; } //hãng sản xuất
        public string CountryofOriginCode { get; set; } //xuất xứ
        public string QualityMeasureCode { get; set; } // công suất
        public string KhoNhaMay { get; set; } // 
        public string TinhTrangThietBi { get; set; } // công suất 
        public DateTime? NgaySuDung { get; set; } // ngày đưa vào sử dụng
        public string Position { get; set; } // vi tri lưu
        public string PositionName { get; set; } // vi tri lưu 
        public string TrangThai { get; set; } // Đang Sử Dụng,Thanh Lý/Tồn 0,Chờ Thanh Lý
        public string ThietBiRoi { get; set; }
        public string NhaMay { get; set; }
        public string Xuong { get; set; }
        public string ToSanXuat { get; set; }

        public string TenNhaMay { get; set; }
        public string TenXuong { get; set; }
        public string TenToSanXuat { get; set; } 
    }
}
