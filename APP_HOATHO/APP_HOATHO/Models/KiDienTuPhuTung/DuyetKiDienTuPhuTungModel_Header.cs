using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.KiDienTuPhuTung
{
  public  class DuyetKiDienTuPhuTungModel 
    {     
        
        public string No_ { get; set; }

        [JsonProperty("External Document No_")]
        public string ExternalDocumentNo_ { get; set; }

        [JsonProperty("Posting Date")]
        public DateTime PostingDate { get; set; }

        [JsonProperty("Work Center No_")]
        public string WorkCenterNo_ { get; set; }
        [JsonProperty("User ID")]
        public string UserID { get; set; }
        [JsonProperty("Approve Status")]
        public int ApproveStatus { get; set; }
        [JsonProperty("Approve ID")]
        public string ApproveID { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        public string ApproveUserName { get; set; }
        public string KhoNhap { get; set; }
    }
}
