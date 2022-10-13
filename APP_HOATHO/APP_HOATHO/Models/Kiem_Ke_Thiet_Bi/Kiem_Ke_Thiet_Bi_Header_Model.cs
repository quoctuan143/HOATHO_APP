using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.Kiem_Ke_Thiet_Bi
{
   public  class Kiem_Ke_Thiet_Bi_Header_Model
    {
        [JsonProperty]
        public string No_ { get; set; }        
        public string Period_Checking { get; set; }        
        public string Location_Name { get; set; }        
        public string Work_Center_Name { get; set; }        
        public string User_Create { get; set; } 
    }
    public class Kiem_Ke_Thiet_Bi_Line_Model 
    {
        public string Document_No_ { get; set; } 
        public string Ma_Thiet_Bi { get; set; }
        public string Ten_Thiet_Bi { get; set; } 
        public string Model { get; set; }
        public string Serial { get; set; }
        public double  So_Luong { get; set; }
        public string Nguoi_Kiem_Ke { get; set; }       
        public string Don_Vi_Tinh { get; set; }        
    }
}
