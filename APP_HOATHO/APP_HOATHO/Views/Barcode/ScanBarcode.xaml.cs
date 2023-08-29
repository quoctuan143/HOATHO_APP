using APP_HOATHO.Dialog;
using APP_HOATHO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace APP_HOATHO.Views.Barcode
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanBarcode : ContentPage
    {
        public EventHandler <string> ScanBarcodeResult { get; set; }
        bool Loop; // cho phép lặp nhiều lần hoặc quét thành công thì close
        public ScanBarcode(bool loop , string title)
        {
            InitializeComponent();
            Loop = loop;
            this.Title = title;
        }

        private async void btnChon_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBarcode.Text))
            {
                await new MessageBox("Vui lòng nhập mã barcode").Show();
                return;
            }
            if (Loop)
            {
                ScanBarcodeResult?.Invoke(this, txtBarcode.Text.ToUpper());
            }    
            
            else
            {
                ScanBarcodeResult?.Invoke(this, txtBarcode.Text.ToUpper());
                await Navigation.PopAsync();
            }    


        }

        private  void scannerView_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (result.Text != "")
                {
                    DependencyService.Get<IBeepService>().Beep();
                    if (Loop)
                    {
                        ScanBarcodeResult?.Invoke(this, result.Text.ToUpper());
                    }

                    else
                    {
                        ScanBarcodeResult?.Invoke(this, result.Text.ToUpper());
                        await Navigation.PopAsync();
                    }
                }
            });              
            
        }
    }
}