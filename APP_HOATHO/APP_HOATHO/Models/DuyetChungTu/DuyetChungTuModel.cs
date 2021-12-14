using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.DuyetChungTu
{
    public class DuyetChungTuModel
    {
        string no_;
        public string No_ { get => no_; set { no_ = value; no_ = no_.Replace("|", @"\"); }
        }
        public string ExternalDocNo { get; set; }
        public DateTime PostingDate { get; set; }
        public string WorkCenter { get; set; }
        public string CustomerName { get; set; }
        public string VendorName { get; set; } 
        public string PO { get; set; }
        public string Style { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserRequest { get; set; }
    }

}
