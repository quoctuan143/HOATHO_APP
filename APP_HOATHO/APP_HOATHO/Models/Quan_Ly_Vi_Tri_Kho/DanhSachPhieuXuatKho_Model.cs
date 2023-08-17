using APP_HOATHO.Models.Nha_May_Soi;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho
{
    public class DanhSachPhieuXuatKho_Model : Bindable
    {
        public string No_ { get; set; }
        public string ExternalDocumentNo { get; set; }
        public DateTime PostingDate { get; set; }
        public string Description { get; set; }
        public string PO { get; set; }
        public string Style { get; set; } 

    }
}
