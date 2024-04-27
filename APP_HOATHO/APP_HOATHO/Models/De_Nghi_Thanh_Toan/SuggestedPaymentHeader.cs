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
        public int RowID { get; set; }
        [JsonProperty("Document Type")]
        public int DocumentType { get; set; }
        public string No_ { get; set; }


        [JsonProperty("Buy-from Vendor Name")]
        public string BuyfromVendorName { get; set; }

        [JsonProperty("Pay-to Vendor No_")]
        public string PaytoVendorNo_ { get; set; }

        [JsonProperty("Pay-to Name")]
        public string PaytoName { get; set; }

        [JsonProperty("Pay-to Address")]
        public string PaytoAddress { get; set; }

        [JsonProperty("External Document No_")]
        public string ExternalDocumentNo_ { get; set; }

        [JsonProperty("Posting Date")]
        public DateTime PostingDate { get; set; }

        [JsonProperty("Document Date")]
        public DateTime DocumentDate { get; set; }

        [JsonProperty("Login ID")]
        public string LoginID { get; set; }

        [JsonProperty("Payment Method")]
        public int PaymentMethod { get; set; }


        public string Description { get; set; }
        public int Status { get; set; }

        [JsonProperty("Bank Name Code")]
        public string BankNameCode { get; set; }

        [JsonProperty("Currency Code")]
        public string CurrencyCode { get; set; }

        [JsonProperty("Currency Factor")]
        public decimal CurrencyFactor { get; set; }
        public int Type { get; set; }

        public int KyDienTu { get; set; }

        [JsonProperty("Department Code")]
        public string DepartmentCode { get; set; }

        [JsonProperty("Source Code")]
        public string SourceCode { get; set; }



        [JsonProperty("Pay-to Type")]
        public int PaytoType { get; set; }
        [JsonProperty("Due Date")]
        public DateTime? HanThanhToan { get; set; } = DateTime.Now.Date;
    }

    public class SuggestedPaymentLine : Bindable
    {
        
        public int RowID { get; set; }
        [JsonProperty("Invoice Code")]
        public string InvoiceCode { get; set; }

        double? _amountVAT =0;
        [JsonProperty("Amount VAT")]
        public double? AmountVAT { get => _amountVAT; set {
            SetProperty(ref _amountVAT, value);
                Amount = _amountChuaVat + AmountVAT;
                AmountIncludingVAT = Amount;
                OnPropertyChanged("Amount");  
                OnPropertyChanged("AmountIncludingVAT");                
                OnPropertyChanged("FormatAmount");
                OnPropertyChanged("FormatAmountIncludingVAT");
                OnPropertyChanged("FormatAmountVAT");
            } }        

        [JsonProperty("Amount Including VAT")]
        public double? AmountIncludingVAT { get; set; }

        string _vat;
        public string VAT { get => _vat;set
            {
                SetProperty(ref _vat,value);
                var vat = int.Parse((_vat ?? "0"));                
                AmountVAT = Math.Round(((AmountChuaVat ?? 0) * vat / 100.0),0);
                Amount = AmountChuaVat + AmountVAT;
                AmountIncludingVAT = Amount;
                OnPropertyChanged("Amount");
                OnPropertyChanged("AmountVAT");
                OnPropertyChanged("AmountChuaVat");
                OnPropertyChanged("AmountIncludingVAT");
                OnPropertyChanged("FormatAmountChuaVaT");
                OnPropertyChanged("FormatAmount");
                OnPropertyChanged("FormatAmountIncludingVAT");
                OnPropertyChanged("FormatAmountVAT");
            }
        }
        double? _amountChuaVat;
        public double? AmountChuaVat { get => _amountChuaVat; set {
                SetProperty(ref _amountChuaVat, value);
                var vat = int.Parse((_vat ?? "0"));
                AmountVAT = Math.Round((AmountChuaVat * vat / 100.0).Value, 0);
                Amount = _amountChuaVat + AmountVAT;
                AmountIncludingVAT = Amount;
                OnPropertyChanged("Amount");
                OnPropertyChanged("AmountVAT");
                OnPropertyChanged("AmountChuaVat");
                OnPropertyChanged("AmountIncludingVAT");
                OnPropertyChanged("FormatAmountChuaVaT");
                OnPropertyChanged("FormatAmount");
                OnPropertyChanged("FormatAmountIncludingVAT");
                OnPropertyChanged("FormatAmountVAT");

            } }
        
        public double? Amount
        {
            get;set;
        }
        public string FormatAmountChuaVaT
        {
            get {
                return string.Format("{0:#,##0.##}", AmountChuaVat);
            }
            set
            {
                if (!CheckThapPhan(value))
                {
                    try
                    {
                        double response = 0;
                        FormatNumberString(ref response, value);
                        AmountChuaVat = response;                           
                        OnPropertyChanged("FormatAmountChuaVaT");
                        OnPropertyChanged("AmountChuaVat");
                    }
                    catch
                    {

                    }

                }
            }
        }
        public string FormatAmount
        {
            get
            {
                return string.Format("{0:#,##0.##}", Amount);
            }
            set
            {
                if (!CheckThapPhan(value))
                {
                    try
                    {
                        double response = 0;
                        FormatNumberString(ref response, value);
                        Amount = response;
                        OnPropertyChanged("FormatAmount");
                        OnPropertyChanged("Amount");
                    }
                    catch
                    {

                    }

                }
            }
        }
        public string FormatAmountVAT
        {
            get
            {
                return string.Format("{0:#,##0.##}", AmountVAT);
            }
            set
            {
                if (!CheckThapPhan(value))
                {
                    try
                    {
                        double response = 0;
                        FormatNumberString(ref response, value);
                        AmountVAT = response;
                        OnPropertyChanged("FormatAmountVAT");
                        OnPropertyChanged("AmountVAT");
                    }
                    catch
                    {

                    }

                }
            }
        }
        public string FormatAmountIncludingVAT
        {
            get
            {
                return string.Format("{0:#,##0.##}", AmountIncludingVAT);
            }
            set
            {
                if (!CheckThapPhan(value))
                {
                    try
                    {
                        double response = 0;
                        FormatNumberString(ref response, value);
                        AmountIncludingVAT = response;
                        OnPropertyChanged("FormatAmountIncludingVAT");
                        OnPropertyChanged("AmountIncludingVAT");
                    }
                    catch
                    {

                    }

                }
            }
        }
        double? _soNgay =0;
        [JsonProperty("Date Sum")]
        public double? SoNgay { get => _soNgay; set => SetProperty(ref _soNgay, value); } 
        double? _donGia = 0;
        [JsonProperty("Unit Price")]
        public double? DonGia { get => _donGia; set => SetProperty(ref _donGia, value); } 
        [JsonProperty("Node")]
        public string Notes { get; set; }
        bool _isShow;
        public bool IsShow { get=> _isShow; set => SetProperty(ref _isShow,value); }
        [JsonProperty("Document Date")]
        public DateTime? NgayHoaDon { get; set; } = DateTime.Now.Date;
        public string FormatDonGia
        {
            get
            {
                return string.Format("{0:#,##0.##}", DonGia);
            }
            set
            {
                if (!CheckThapPhan(value))
                {
                    try
                    {
                        double response = 0;
                        FormatNumberString(ref response, value);
                        DonGia = response;
                        OnPropertyChanged("FormatDonGia");
                        OnPropertyChanged("DonGia");
                    }
                    catch
                    {

                    }

                }
            }
        }
    }

    public class SuggestedPaymentRequest:Bindable
    {
        public SuggestedPaymentRequest()
        {
            SuggestedPaymentHeader = new SuggestedPaymentHeader();
            SuggestedPaymentLines = new ObservableCollection<SuggestedPaymentLine>();
        }
        public SuggestedPaymentHeader SuggestedPaymentHeader { get; set; }
        ObservableCollection<SuggestedPaymentLine> _suggestedPaymentLines;
        public ObservableCollection<SuggestedPaymentLine> SuggestedPaymentLines { get => _suggestedPaymentLines; set => SetProperty(ref _suggestedPaymentLines, value); }
    }

    public class ViewSuggestedPayment
    {
        public string TenDoiTuong { get; set; }
        public double SoTien { get; set; }
        public string NoiDung { get; set; }
        public string HinhThucThanhToan { get; set; }
        public string SoTaiKhoan { get; set; }
        public string NganHang { get; set; }
        public DateTime ThoiHanThanhToan { get; set; }
        public SuggestedPaymentRequest SuggestedPaymentRequest { get; set; }
        public string LoaiThanhToan { get; set; }
    }

    public class DanhSachDeNghiThanhToan
    {
        public string No_ { get; set; }
        public string DoiTuong { get; set; }
        public string Description { get; set; }
        public string HinhThucThanhToan { get; set; }
        public double Total { get; set; }
        public DateTime? HanThanhToan { get; set; }
        public string SoTaiKhoan { get; set; }
        public string NganHang { get; set; }
        public int Status { get; set; }
        public string LoaiThanhToan { get; set; }
    }
}
