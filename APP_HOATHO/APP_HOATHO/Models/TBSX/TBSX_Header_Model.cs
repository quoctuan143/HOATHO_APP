using APP_HOATHO.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.TBSX
{
    public class TBSX_Header_Model : DocumentBase
    {
        
    }

    public class TBSX_Line_Model
    {
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public double? CM { get; set; }
        public double? P { get; set; }
        public double? CMPT { get; set; }
        public double? T { get; set; }
        public double? Amount { get; set; }
        public double? PT { get; set; }
        public string Style { get; set; }
        public string PO { get; set; }

        public DateTime? NgayDongBo { get; set; }
        public DateTime? NgayXuatHang { get; set; }
    }

    public class TBSXDC_Header_Model : DocumentBase
    {        
    }

    public class TBSXDC_Line_Model : TBSX_Line_Model
    {        
    }

    public class ApproveRequest
    {
        public string DocumentNo { get; set; }
        public string UserApprove { get; set; }
        public AppoveType Permision { get; set; }
    }
}
