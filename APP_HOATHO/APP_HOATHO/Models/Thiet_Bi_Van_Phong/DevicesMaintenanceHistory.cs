using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.Thiet_Bi_Van_Phong
{
    public  class DevicesMaintenanceHistory
    {
        [JsonProperty("Document No_")]
        public string DocumentNo_ { get; set; }
        public DateTime? ThoiDiemPhatSinh { get; set; }
        public string NoiDungLoi { get; set; }
        public string ITXuLy { get; set; }
        public string FileDinhKem { get; set; }
        public string CachXuLy { get; set; }
        public double? ThoiGianXuLy { get; set; }
        public int? PhanLoaiLoi { get; set; }
        public string NguyenNhan { get; set; }
        public int? Status { get; set; }
        public int? SoLanXuLy { get; set; }
        public DateTime? ThoiDiemHoanThanh { get; set; }

        public string NguoiGuiYeuCau { get; set; }
        public string TenThietBi { get; set; } 
        public string Description2 { get; set; }
    }
}
