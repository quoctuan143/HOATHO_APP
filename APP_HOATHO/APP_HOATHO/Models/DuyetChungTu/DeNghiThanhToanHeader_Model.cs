using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.DuyetChungTu
{
    public class DeNghiThanhToanHeader_Model
    {

        public string No_ { get; set; }

        [JsonProperty("Buy-from Vendor Name")]
        public string BuyFromVendorName { get; set; }

        [JsonProperty("Pay-to Vendor No_")]
        public string PayToVendorNo { get; set; }

        [JsonProperty("Pay-to Name")]
        public string PayToName { get; set; }
      public string LoaiChungTu { get; set; }

        [JsonProperty("External Document No_")]
        public string ExternalDocumentNo { get; set; }

        [JsonProperty("Posting Date")]
        public DateTime PostingDate { get; set; }

        [JsonProperty("Login ID")]
        public string LoginID { get; set; }

        public string Description { get; set; }


        [JsonProperty("Bank Name Code")]
        public string BankNameCode { get; set; }

        public string TPKinhDoanhKy { get; set; }
    }

    public class DeNghiThanhToanLine_Model
    {

        [JsonProperty("Document No_")]
        public string DocumentNo { get; set; }

        [JsonProperty("Contact Code")]
        public string ContactCode { get; set; }

        [JsonProperty("Form Code")]
        public string FormCode { get; set; }

        [JsonProperty("Invoice Code")]
        public string InvoiceCode { get; set; }
        public double Amount { get; set; }
        [JsonProperty("Amount VAT")]
        public double  AmountVAT { get; set; }

        [JsonProperty("Amount Including VAT")]
        public double AmountIncludingVAT { get; set; }
        [JsonProperty("Purchase Order No_")]
        public string PurchaseOrderNo { get; set; }


    }
}
