using APP_HOATHO.Models.Nha_May_Soi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace APP_HOATHO.Models.De_Nghi_Thanh_Toan
{
    public class SuggestedPaymentHeader : Bindable
    {
        [JsonProperty("Pay-to Type")]
        public int PaytoType { get; set; }

        [JsonProperty("Document Type")]
        public int DocumentType { get; set; }

        [JsonProperty("Payment Method")]
        public int PaymentMethod { get; set; }
        public int Type { get; set; }

        [JsonProperty("Pay-to Vendor No_")]
        public string PaytoVendorNo_ { get; set; }
        public string Description { get; set; }
        [JsonProperty("Login ID")]
        public string LoginID  { get; set; }
    }

    public class SuggestedPaymentLine : Bindable
    {
        public SuggestedPaymentLine()
        {
            VATs = new ObservableCollection<LookupValueInt> { new LookupValueInt { Code = 0, Name = "0 %" },
                                           new LookupValueInt { Code = 8, Name = "8 %" },
                                           new LookupValueInt { Code = 10, Name = "10 %" }};
        }
        [JsonProperty("Invoice Code")]
        public string InvoiceCode { get; set; }
        

        [JsonProperty("Amount VAT")]
        public double AmountVAT { get; set; }
        public double AmountChuaVat { get; set; }

        [JsonProperty("Amount Including VAT")]
        public double AmountIncludingVAT { get; set; }

        string _vat;
        public string VAT { get => _vat; 
            set { 
                SetProperty(ref _vat, value);
                var vat = int.Parse(_vat);
                AmountVAT = Amount * vat / 100.0;
                AmountChuaVat = Amount - AmountVAT;
                AmountIncludingVAT = Amount;
                OnPropertyChanged("AmountVAT");
                OnPropertyChanged("AmountChuaVat");
                OnPropertyChanged("AmountIncludingVAT");
            } 
        }
        double _amount;
        public double Amount
        {
            get => _amount;
            set
            {
                SetProperty(ref _amount, value);
                var vat = int.Parse(VAT);
                AmountVAT = Amount * vat / 100.0;
                AmountChuaVat = Amount - AmountVAT;
                AmountIncludingVAT = Amount;
                OnPropertyChanged("Amount");
                OnPropertyChanged("AmountVAT");
                OnPropertyChanged("AmountChuaVat");
                OnPropertyChanged("AmountIncludingVAT");
            }
        }
        public string FormatNumberAmount
        {
            get { return string.Format("{0:#,##0.##}", Amount);  }
            set
            {
                if (!CheckThapPhan(value))
                {
                    try
                    {
                        FormatNumberString(ref _amount, value);
                        Amount = _amount;
                        OnPropertyChanged("FormatNumberAmount");
                        OnPropertyChanged("Amount");
                    }
                    catch
                    {

                    }

                }
            }
        }
        ObservableCollection<LookupValueInt> _vats;
        public ObservableCollection<LookupValueInt> VATs {get => _vats; set => SetProperty(ref _vats,value); }
    }

    public class SuggestedPaymentRequest:Bindable
    {
        public SuggestedPaymentHeader SuggestedPaymentHeader { get; set; }        
        public ObservableCollection<SuggestedPaymentLine> SuggestedPaymentLines { get; set; }
    }
}
