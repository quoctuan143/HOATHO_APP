using APP_HOATHO.Global;
using APP_HOATHO.Models.Nha_May_Soi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.Thiet_Bi_Van_Phong
{
    public  class DevicesMaintenanceHistory : Bindable
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
        string _picture;
        public string Picture { get => _picture; set => SetProperty(ref _picture,value); }
        string _imageLink;
        public string ImageLink { get => _imageLink;
             set
            {
                _imageLink =value;
                if (!string.IsNullOrEmpty(value))
                {                    
                    IsShowImage = true;
                    Picture = Config.URL + "image/" + value;
                    OnPropertyChanged("Picture");
                    OnPropertyChanged("IsShowImage");
                }    
            }
        }
       
        public string ThongTinLienLac { get; set; }

        public bool IsShowImage { get; set; }
    }
}
