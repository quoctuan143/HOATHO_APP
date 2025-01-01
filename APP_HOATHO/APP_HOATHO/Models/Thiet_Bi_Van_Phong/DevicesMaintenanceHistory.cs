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
        public string RowID { get; set; }
        string _documentNo_;
        [JsonProperty("Document No_")]
        public string DocumentNo_ { get => _documentNo_; set => SetProperty(ref _documentNo_ , value); }
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
        string _tenThietBi;
        public string TenThietBi { get=> _tenThietBi; set => SetProperty(ref _tenThietBi, value); }
        string _description2;
        public string Description2 { get => _description2; set => SetProperty(ref _description2,value); }
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
        bool _yeuCauTheoThietBi;
        public bool YeuCauTheoThietBi { get => _yeuCauTheoThietBi; set  { 
                SetProperty(ref _yeuCauTheoThietBi, value); 
                NotYeuCauTheoThietBi = !value;
                OnPropertyChanged(nameof(NotYeuCauTheoThietBi));
            } }
        public bool NotYeuCauTheoThietBi { get; set; }
        public DateTime? Du_Kien_Hoan_Thanh { get; set; }
        public string IT_Tiep_Nhan { get; set; }
        public DateTime? UserTimeRespect { get; set; }
        public string UserRequire { get; set; }
        public TimeSpan? TimeRequire { get; set; } 
    }
}
