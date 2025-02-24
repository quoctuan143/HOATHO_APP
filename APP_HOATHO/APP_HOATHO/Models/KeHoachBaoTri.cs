using APP_HOATHO.Models.Nha_May_Soi;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models
{
  public  class KeHoachBaoTri :Bindable
    {
        public string No_ { get; set; } 
        public string No_2 { get; set; }
        public string No_3 { get; set; }
        public string NameVN { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; } 
        public string Code { get; set; }
        public string LoaiBaoTri { get; set; } 
        public Boolean Da_Bao_Tri { get; set; }  
        public string ItemCategoryCode { get; set; }
        public string ViTriLuu { get; set; }
        public string NhaMay { get; set; }
        public string Xuong  { get;set; }
        public string ToSanXuat { get; set; }
    }
}
