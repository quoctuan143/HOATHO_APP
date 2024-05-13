using APP_HOATHO.Converter;
using APP_HOATHO.Models.Nha_May_Soi;

namespace APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho
{
    public class PurchaseLinePackingList_Model : Bindable
    {
        public string Name { get; set; }    
        public int RowID { get; set; }

        public string Quality { get; set; }

        public string Color { get; set; }

        public string ShadeLot { get; set; }

        public string RollNo { get; set; }

        public string ParcelNo { get; set; }
        double _grossLength =0;
        public double? GrossLength { get; set; }
        public string FormatNumberGrossLength
        {
            get => string.Format("{0:#,##0.##}", GrossLength);
            set
            {
                if (!CheckThapPhan(value))
                {
                    try
                    {
                        FormatNumberString(ref _grossLength, value);
                        GrossLength = _grossLength;
                        OnPropertyChanged("FormatNumberGrossLength");
                        OnPropertyChanged("GrossLength");
                    }
                    catch
                    {

                    }
                   
                }
            }
        }
        double _allowance = 0;
        public double? Allowance { get; set; }
        public string FormatNumberAllowance
        {
            get => string.Format("{0:#,##0.##}", Allowance);
            set
            {
                if (!CheckThapPhan(value))
                {
                    try
                    {
                        FormatNumberString(ref _allowance, value);
                        Allowance = _allowance;
                        OnPropertyChanged("FormatNumberAllowance");
                        OnPropertyChanged("Allowance");
                    }
                    catch 
                    {
                       
                    }
                   
                }
            }
        }
        double _invoicedMeter = 0;
        public double? InvoicedMeter { get; set; }
        public string FormatNumberInvoicedMeter
        {
            get => string.Format("{0:#,##0.##}", InvoicedMeter);
            set
            {
                if (!CheckThapPhan(value))
                {
                    try
                    {
                        FormatNumberString(ref _invoicedMeter, value);
                        InvoicedMeter = _invoicedMeter;
                        OnPropertyChanged("FormatNumberInvoicedMeter");
                        OnPropertyChanged("InvoicedMeter");
                    }
                    catch
                    { 
                       
                    }
                    
                }
            }
        }
        double _grossWeight = 0;    
        public double? GrossWeight { get; set; }
        public string FormatNumberGrossWeight
        {
            get => string.Format("{0:#,##0.##}", GrossWeight);
            set
            {
                if (!CheckThapPhan(value))
                {
                    try
                    {
                        FormatNumberString(ref _grossWeight, value);                        
                        GrossWeight = _grossWeight;
                        OnPropertyChanged("FormatNumberGrossWeight");
                        OnPropertyChanged("GrossWeight");
                    }
                    catch
                    {
                      
                    }
                   
                }
            }
        }
        public decimal? Width { get; set; }

        public string UserId { get; set; }
        public string DocumentNo { get; set; }
        public string ItemNo { get; set; }

        public string Id { get; set; }

        public string _barcodeId;
        public string BarcodeId { get => _barcodeId; set
            {
                SetProperty(ref _barcodeId, value);
                if (_barcodeId != "")
                {
                    DaGanNhan = StatusFormatColor.HoanThanh;
                    OnPropertyChanged("DaGanNhan");
                }  
                else
                {
                    DaGanNhan = StatusFormatColor.ChuaHoanThanh;
                    OnPropertyChanged("DaGanNhan");
                }    
            }
        }

        public string PositionId { get; set; }

        public double? TonKho { get; set; }

        public StatusFormatColor DaGanNhan { get; set; }        
        public string DonViTinh { get; set; } 

        public string ViTriDay { get; set; }

        public string Art { get; set; } 
    }

    public class UpdateCayVaiRequest
    {
        public string Id { get; set; }
        public double? SoLuong { get; set; }
        public string DocumentNo { get; set; }
        public string RollNo { get; set; }
    }
}