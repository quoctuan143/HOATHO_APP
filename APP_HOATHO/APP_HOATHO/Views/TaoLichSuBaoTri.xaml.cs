using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using APP_HOATHO.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;

namespace APP_HOATHO.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class TaoLichSuBaoTri : ContentPage 
    {
        bool _isrunning;
        public Boolean IsRunning { get => _isrunning; set { _isrunning = value; OnPropertyChanged("IsRunning"); } }
       public string MaThietBi { get; set; }
        LICH_SU_BAO_TRI lsu;
        MediaFile media;
        public TaoLichSuBaoTri(string mathietbi) 
        {
            InitializeComponent();
            MaThietBi = mathietbi;          
          
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (entryNoiDung.Text == null || entryNoiDung.Text =="")
            {
                await new MessageBox("Thông báo", "Bạn nhập nội dung sửa chữa").Show();
                return;
            }
            if (entryTinhTrang.Text == null || entryTinhTrang.Text == "")
            {
                await new MessageBox("Thông báo", "Bạn nhập tình trạng sau khi sửa chữa").Show();
                return;
            }
            if (media== null )
            {
                await new MessageBox("Thông báo", "Vui lòng chụp hình kết quả đã sửa chữa").Show();
                return;
            }
            var result = await new MessageYesNo("Thông báo", "Bạn có muốn cập nhật lần bảo trì này không?").Show();
            if (result == Global.DialogReturn.OK )
            {
                var filename = media.Path.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                lsu = new LICH_SU_BAO_TRI();
                lsu.MA_THIET_BI = MaThietBi;
                lsu.MA_PHU_TUNG = entryPhuTung.Text ==  string.Empty ? "" : entryPhuTung.Text;
                lsu.BAO_DUONG_DINH_KY = Convert.ToInt32(chkBaoDuong.IsChecked);
                lsu.IMAGE_LINK = "Upload/LICHSUBAOTRI/" + filename;
                lsu.MA_NHA_MAY = Preferences.Get(Config.NhaMay, "");
                lsu.MA_XUONG = Preferences.Get(Config.MaXuong, "");
                lsu.NGAY_GIO = DateTime.Now;
                lsu.NGUOI_THUC_HIEN = Preferences.Get(Config.User, "");
                lsu.NOI_DUNG = entryNoiDung.Text;
                lsu.TINH_TRANG_HIEN_TAI = entryTinhTrang.Text;
                lsu.STATUS = 1;
                lsu.NGUOI_XAC_NHAN = "";               
                IsRunning = true;
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(media.GetStream()), "\"file\"", $"\"{media.Path}\"");
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Config.URL);
                var response = client.PostAsync("api/qltb/PostLichSuBaoTri_Picture" , content).Result;
               

                //nếu up hình thành công thì up lich sử lên luôn
                if (response.Content.ReadAsStringAsync ().Result.ToLower().Contains( "upload thành công"))
                {
                    var ok = client.PostAsJsonAsync("api/qltb/postLichSuBaoTri", lsu);
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains( "cập nhật thành công"))
                    {
                        await new MessageBox("Thông báo", "Cập nhật thành công").Show();
                        MessagingCenter.Send(this, "AddLichSuBaoTri", lsu);
                        await Navigation.PopAsync();
                    }    
                    else
                    {
                        IsRunning = false;
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                    }    
                }    
                else
                {
                    IsRunning = false;
                    await new MessageBox("Thông báo", response.Content.ReadAsStringAsync().Result).Show();
                    return;
                }
                IsRunning = false;
            }    
           
           
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async  void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await new MessageBox("thông báo", "Điện thoại không hỗ trợ chức năng chụp hình").Show();
                return;
            }
            media = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions { SaveToAlbum = true });
            if (media == null) return;
            imagePicture.Source = ImageSource.FromStream(() =>
            {
                return media.GetStream();
            });
        }
    }
}