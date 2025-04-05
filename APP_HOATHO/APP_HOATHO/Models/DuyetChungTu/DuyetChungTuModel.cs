using APP_HOATHO.Models.Nha_May_Soi;
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
        public DateTime? PostingDate { get; set; }
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
    public class DuyetThanhLyNPLModel
    {
        public string DocumentNo { get; set; }
        public string UserName { get; set; }
        public int OrderStatus { get; set; }
    }

    public class MaterialsLiquidationHeader
    {
        string no_;
        public string No_
        {
            get => no_; set { no_ = value; no_ = no_.Replace("|", @"\"); }
        }        
        public DateTime? PostingDate { get; set; }
        public string WorkCenterCode { get; set; }
        public string CustomerName { get; set; }       
        public string PO { get; set; }
        public string Style { get; set; }
        public string OrderStatus { get; set; }
        public string SoHopDong { get; set; }        
    }

    public class MaterialsLiquidationLine : Bindable
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string Dvt { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Quota { get; set; } // định mức
        public decimal? Scrap { get; set; } // hao hut
        public decimal? Demand { get; set; }    // nhu cầu
        public decimal? BrandQuantity { get; set; } // chenh lech
        public decimal? PSExportQuantityAmount { get; set; }    // thuc nhan
        public decimal? PSLiquidationQuantity { get; set; } // thanh ly theo po style
    }
}
