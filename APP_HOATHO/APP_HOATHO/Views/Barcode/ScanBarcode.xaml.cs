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
        bool IdVai;
        public ScanBarcode(bool loop , string title, bool IdVai = false)
        {
            InitializeComponent();
            Loop = loop;
            this.Title = title;
            this.IdVai = IdVai;
        }

        private async void btnChon_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBarcode.Text))
                {
                    await new MessageBox("Vui lòng nhập mã barcode").Show();
                    return;
                }
                if (Loop)
                {
                    if (IdVai)
                    {
                        var result = txtBarcode.Text.ToUpper();
                        if (result.Length != 9)
                        {
                            try
                            {
                                var number = Convert.ToDouble(result);
                                result = number.ToString("000000000");
                            }
                            catch (Exception ex)
                            {

                                await new MessageBox("Mã barcode là kiểu number").Show();
                                return;
                            }
                        }
                        ScanBarcodeResult?.Invoke(this, result);
                       
                    }
                    else
                    {
                        ScanBarcodeResult?.Invoke(this, txtBarcode.Text.ToUpper());                       
                    }
                    
                }

                else
                {
                    if (IdVai)
                    {
                        var result = txtBarcode.Text.ToUpper();
                        if (result.Length != 9)
                        {
                            try
                            {
                                var number = Convert.ToDouble(result);
                                result = number.ToString("000000000");
                            }
                            catch (Exception ex)
                            {

                                await new MessageBox("Mã barcode là kiểu number").Show();
                                return;
                            }
                        }
                        ScanBarcodeResult?.Invoke(this, result);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        ScanBarcodeResult?.Invoke(this, txtBarcode.Text.ToUpper());
                        await Navigation.PopAsync();
                    }

                }
            }
            catch (Exception ex)
            {
                await new MessageBox(ex.Message).Show();

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