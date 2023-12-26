using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.Nha_May_Soi.KyLenhXuatHang
{
    public class LenhXuatHangHeader_Model : DocumentBase
    {
        
    }

    public class LenhXuatHangLine_Model
    {
        public string No_ { get; set; }
        public string Description { get; set; }
        public string DVT { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public double AmountIncludingVAT { get; set; }
        public double GrossWeight { get; set; }
        public double NetWeight { get; set; }
    }
}
