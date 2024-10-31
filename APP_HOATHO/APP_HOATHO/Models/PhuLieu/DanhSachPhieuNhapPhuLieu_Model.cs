using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.PhuLieu
{
    public class DanhSachPhieuNhapXuatPhuLieu_Model
    {
        public string No_ { get; set; }
        public string ExternalDocumentNo { get; set; }   
        public string LocationName { get; set; }
        public string CustomerName { get; set; }
        public DateTime PostingDate { get; set; }
        public string Description { get; set; }
    }
}
