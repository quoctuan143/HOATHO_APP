using APP_HOATHO.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace APP_HOATHO.Models.Nha_May_Soi
{
    public class Xuat_Kien_Header_Model
    {
        public string No_ { get; set; }
        public string External_Document_No_ { get; set; }
        public string Posting_Description { get; set; }
        public string Work_Center_Name { get; set; }
        public string User_Create { get; set; }
    }
    public class Xuat_Kien_Line_Model
    {
        public string Document_No_ { get; set; }
        public string Item_No_ { get; set; }
        public string Ten_NVL { get; set; }
        public int Line_No_ { get; set; }
        public string Unit_of_Measure { get; set; }
        public double Quantity { get; set; }
        public double Done_Quantity { get; set; }

    }
    public class Xuat_Kien_Line_Detail_Model : Bindable
    {
        public int RowID { get; set; }
        public string Document_No_ { get; set; }
        public string Item_No_ { get; set; }
        public string Ten_NVL { get; set; }
        public int Line_No_ { get; set; }
        public string Unit_of_Measure { get; set; }
        public double Old_Quantity { get; set; }
        double _quantity;
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                if (_quantity != Old_Quantity && RowID > 0)
                {
                    StatusUpdate = Models.StatusUpdate.Status.Modified;
                }
                else if (_quantity != Old_Quantity && RowID == 0)
                    StatusUpdate = Models.StatusUpdate.Status.Add;
                else if (_quantity == Old_Quantity && StatusUpdate == Models.StatusUpdate.Status.Modified)
                    StatusUpdate = Models.StatusUpdate.Status.None;

                SetProperty(ref _quantity, value);
            }
        }
        public string FormatNumberQuantity { get => string.Format("{0:#,##0.##}", Quantity); 
        set  {
                if (!CheckThapPhan(value))
                {
                    FormatNumberString(ref _quantity, value);
                    OnPropertyChanged("FormatNumberQuantity");
                    Quantity = _quantity;
                }   
            }
        }  
        public string Package_ID { get; set; }
        public StatusUpdate.Status StatusUpdate { get; set; }
    }
    public class Package_Model
    {
        [JsonProperty("Item No_")]
        public string Item_No_ { get; set; }
        [JsonProperty("Base UOM")]
        public string Unit_of_Measure { get; set; }       
        public double Quantity { get; set; }
        [JsonProperty("Package ID")]
        public string Package_ID { get; set; }
    }
    public class Bindable : INotifyPropertyChanged
    {
        public void ShortAlert(string title)
        {
            DependencyService.Get<IMessage>().ShortAlert(title);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T backingStore, T value,
           [CallerMemberName] string propertyName = "",
           Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged     
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
                
        public bool CheckThapPhan(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                char lastChar = value[value.Length - 1];
                if (CultureInfo.InstalledUICulture.Name == "en-US")
                {
                    if (lastChar == '.')
                    {
                        return true;
                    }
                }
                else
                {
                    if (lastChar == ',')
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void FormatNumberString(ref double number, string value)  
        {
            double.TryParse(!string.IsNullOrEmpty(value) ? value : "0", out double quantity);
            number = quantity;
        }
    }

    public class UploadImageKien
    {
        public int RowID { get; set; }
        [JsonProperty("Document No_")]
        public string DocumentNo_ { get; set; }

        [JsonProperty("Item No_")]
        public string ItemNo_ { get; set; }

        [JsonProperty("Package ID")]
        public string PackageID { get; set; }

        [JsonProperty("Qty_ to Receive")]
        public double Qty_toReceive { get; set; }
        public double Quantity { get; set; }
        public string Description { get; set; }

        [JsonProperty("Container ID")]
        public string ContainerID { get; set; }
        public string ImageNCC { get; set; }
        public string NguoiUploadAnh { get; set; }
        public string Barcode { get; set; }

        [JsonProperty("Container No_")]
        public string ContainerNo_ { get; set; }
        [JsonProperty("Position Code")]
        public string PositionCode { get; set; }
        [JsonProperty("Packing Desc")]
        public string PackingDesc { get; set; }
        public string ImageString { get; set; }
        public string MaKien { get; set; } = string.Empty;

    }

    public class Chat_Luong_Kien_Bong_Model : Bindable
    {
        public string Name { get; set; }
        public string PackageId { get; set; }
        public double? MIC { get; set; }
        public double? SF { get; set; }
        public double? UHML { get; set; }
        public double? RD { get; set; }
        public double? B { get; set; } 
        public int Status { get; set; }
        
    }
}
