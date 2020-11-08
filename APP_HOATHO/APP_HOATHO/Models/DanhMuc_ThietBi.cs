using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models
{
   public class DanhMuc_ThietBi
    {
        public string No_ { get; set; }
        public string No_2 { get; set; }
        public string No_3 { get; set; }
        public string NameVN { get; set; }
        public string Description { get; set; } 
        public string LinkVideo { get; set; }
        public string Picture { get; set; } // link ảnh
        public string ManufacturerCode { get; set; } //hãng sản xuất
        public string CountryofOriginCode { get; set; } //xuất xứ
        public string QualityMeasureCode { get; set; } // công suất
        public string KhoNhaMay { get; set; } // công suất 
        public string TinhTrangThietBi { get; set; } // công suất 
    }
}
