using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.KiDienTuPhuTung
{
  public  class Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header
    {
        public string No_ { get; set; }
        public string ExternalDocumentNo_ { get; set; }
        public DateTime PostingDate { get; set; }
        public string Description { get; set; }
        public string WorkCenterName { get; set; }
        public string UserRequest { get; set; }
        public string UserApprove { get; set; }
        public string UserNameApprove { get; set; }
    }

    public class Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Line 
    {        
        public string No_ { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }
        public double Quantity { get; set; }
        public string Don_Vi_Tinh { get; set; }
        public double UnitPrice { get; set; } 
        public double Amount { get; set; }
    }
}
