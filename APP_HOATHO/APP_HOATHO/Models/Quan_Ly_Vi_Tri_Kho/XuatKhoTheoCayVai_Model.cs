using APP_HOATHO.Dialog;
using APP_HOATHO.Models.Nha_May_Soi;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho
{
    public class XuatKhoTheoCayVai_Model : Bindable
    {        
        public string RowID { get; set; }   
        public string Name { get; set; }    
        public string ItemNo { get; set; }
        public string Color { get; set; }
        public double TonKho { get; set; }

        double _canxuat = 0;
        public double CanXuat { get => _canxuat; 
            set 
            { 
                _canxuat = value;
                if  (RowID == "0")
                    {
                    if (_canxuat > TonKho)
                    {
                        new MessageBox("Số lượng xuất vượt quá tồn kho").Show();
                        CanXuat = 0;
                        return;
                    }
                }  
            } 
        } 
        public string BarcodeId { get; set; }   
        public string DocumentNo { get; set; }  
        public string UserId { get; set; }
        public string Id { get; set; }

        public string FormatNumberCanXuat
        {
            get => string.Format("{0:#,##0.##}", CanXuat);
            set
            {
                if (!CheckThapPhan(value))
                {
                    try
                    {
                        FormatNumberString(ref _canxuat, value);
                        CanXuat = _canxuat;
                        OnPropertyChanged("FormatNumberCanXuat");
                        OnPropertyChanged("CanXuat");
                    }
                    catch
                    {

                    }

                }
            }
        }       
        public bool DaXuatKho
        {
            get
            {
                if (RowID == "0")
                    return false;
                else
                    return true;

            }         
        }
    }
}
