using APP_HOATHO.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.Thong_Ba_San_Xuat
{
    public class ThongBaoSanXuat_HeaderModel
    {
        public DateTime PostingDate { get; set; }
        public string ChungTuNgoai { get; set; }
        public string No_ { get; set; }
        public string Description { get; set; }
        public string SoHopDong { get; set; }
        public string CustomerName { get; set; }
        public string TenNhaMay { get; set; }
    }

    public class ThongBaoSanXuat_LineModel 
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
        public string PO { get; set; }
        public string Style { get; set; }
        public string TenNhaMay { get; set; }
    }

    public class PhuLucThongBaoSanXuat_HeaderModel
    {
        public DateTime PostingDate { get; set; }
        public string ChungTuNgoai { get; set; }
        public string No_ { get; set; }
        public string Description { get; set; }
        public string SoHopDong { get; set; }
        public string CustomerName { get; set; }
        public string TenNhaMay { get; set; }
    }

    public class PhuLucThongBaoSanXuat_LineModel
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
        public string PO { get; set; }
        public string Style { get; set; }
        public string TenNhaMay { get; set; }
    }

    public class ApproveRequest
    {
        public string DocumentNo { get; set; }
        public string UserApprove { get; set; }
        public AppoveType Permision { get; set; }
    }
    
}
