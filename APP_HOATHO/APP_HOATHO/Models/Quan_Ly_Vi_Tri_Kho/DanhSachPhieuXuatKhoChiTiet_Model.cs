using APP_HOATHO.Converter;
using APP_HOATHO.Models.Nha_May_Soi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho
{
    public class DanhSachPhieuXuatKhoChiTiet_Model : Bindable
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public double Quantity { get; set; }
        public string TrimArt { get; set; }
        public string DVT { get; set; }
        double _xuatTheoIdVai;
        public double XuatTheoIdVai { get => _xuatTheoIdVai; set { 
                SetProperty(ref _xuatTheoIdVai, value);
                if (Quantity == XuatTheoIdVai)
                {
                    XuatOk = StatusFormatColor.HoanThanh;
                    OnPropertyChanged("XuatOk");
                }
                else if (XuatTheoIdVai == 0)
                {
                    XuatOk = StatusFormatColor.ChuaHoanThanh;
                    OnPropertyChanged("XuatOk");
                }
                else
                {
                    XuatOk = StatusFormatColor.DoDang;
                    OnPropertyChanged("XuatOk");
                }    
            } 
        }

        public StatusFormatColor XuatOk { get; set; }
    }
}
