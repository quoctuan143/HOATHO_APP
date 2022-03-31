using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace APP_HOATHO.Models.KiDienTuPhuTung
{
  public   class DuyetKiDienTuPhuTungModel_Line
    {
        [JsonProperty("Item No_")]
        public string ItemNo { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }
        public double ? Quantity { get; set; }
        [JsonProperty("Unit Cost")]
        public double? UnitCost { get; set; }
        public double? Amount { get; set; } 
    }
}
