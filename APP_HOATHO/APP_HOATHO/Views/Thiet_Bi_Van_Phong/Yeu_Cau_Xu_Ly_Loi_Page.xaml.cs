using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Thiet_Bi_Van_Phong;
using APP_HOATHO.ViewModels;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using static Xamarin.Essentials.Permissions;

namespace APP_HOATHO.Views.Thiet_Bi_Van_Phong
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Yeu_Cau_Xu_Ly_Loi_Page : ContentPage,INotifyPropertyChanged
    {
       public DevicesMaintenanceHistory Item { get; set; }
        BaseViewModel BaseView { get; set; }
        MediaFile media;
        public Yeu_Cau_Xu_Ly_Loi_Page(DevicesMaintenanceHistory item)
        {
            InitializeComponent();
            BaseView = new BaseViewModel();
            Item = item;
            this.BindingContext = Item;
        }

        private async void GuiYeuCau_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Item.NoiDungLoi == "")
                {
                    DependencyService.Get<IMessage>().LongAlert("Vui lòng nhập nội dung muốn gửi IT");
                    return;
                }
                if (Item.YeuCauTheoThietBi == true && Item.DocumentNo_ == "")
                {
                    DependencyService.Get<IMessage>().LongAlert("Vui lòng quét thiết bị để gửi yêu cầu. nếu bạn muốn nhập mã thì vui lòng bỏ chọn Yêu cầu theo thiết bị");
                    return;
                }
                if (Item.YeuCauTheoThietBi == false && Item.DocumentNo_ == "")
                {
                    Item.DocumentNo_ = "Máy tính";
                }
                BaseView.ShowLoading("Đang gửi yêu cầu tới IT");
                //post ảnh trước 
                if (media != null)
                {
                    var content = new MultipartFormDataContent();
                    if (media != null)
                    {
                        content.Add(new StreamContent(media.GetStream()), "\"file\"", $"\"{media.Path}\"");
                    }
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(Config.URL);
                    var ok1 = client.PostAsync($"api/qltb/UploadImageChoITXuLy", content).Result;
                    var filename = media.Path.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                    Item.ImageLink = filename;
                }
                var time = Item.TimeRequire ?? DateTime.Now.TimeOfDay;
                var date = DateTime.Now.Date;
                Item.UserTimeRespect = new DateTime(date.Year,date.Month,date.Day, time.Hours,time.Minutes,0);
                var ok = await BaseView.RunHttpClientPost("api/qltb/GuiYeuCauXuLyLoiThietBi", Item);
                BaseView.HideLoading();
                if (ok.IsSuccessStatusCode)
                {
                    await new MessageBox("Đã gửi yêu cầu xử lý lỗi thành công").Show();
                    await Navigation.PopAsync();
                }
                else
                    DependencyService.Get<IMessage>().LongAlert(ok.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                BaseView.HideLoading();
                await new MessageBox(ex.Message).Show();
            }
            finally
            {
                BaseView.HideLoading();
            }
            
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await new MessageBox("Điện thoại không hỗ trợ chức năng chụp hình").Show();
                    return;
                }
                media = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions { SaveToAlbum = true });
                if (media == null) return;
                imagePicture.Source = ImageSource.FromStream(() =>
                {
                    return media.GetStream();
                });
            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }
        }

        private async void btnQuetQr_Clicked(object sender, EventArgs e)
        {
            try
            {
                var scan = new ZXingScannerPage();
                scan.Title = "Tìm kiếm thiết bị";
                Shell.SetTabBarIsVisible(scan, false);
                await Navigation.PushAsync(scan);
                scan.OnScanResult += (result) =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //show form lên
                        try
                        {
                            DependencyService.Get<IBeepService>().Beep();
                            if (IsBusy) return;

                            IsBusy = true;
                            string ma = result.Text.Split('=')[1];
                            var Item = await BaseView.RunHttpClientGet<DanhMuc_ThietBi>("api/qltb/getTimKiemThietBi?mathietbi=" + ma);
                            if (Item.Lists.Count > 0)
                            {
                                await Navigation.PopAsync();
                                this.Item.DocumentNo_ = ma;
                                this.Item.TenThietBi = Item.Lists[0].Description2;                              
                            }
                            else
                                DependencyService.Get<IMessage>().LongAlert("Không tìm thấy thiết bị này trong hệ thống");
                        }
                        catch { }
                        finally { IsBusy = false; }
                    });
                };
            }
            catch (Exception ex)
            {
                await new MessageBox(ex.Message).Show();
            }
        }

        private async void ChonHinh_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await new MessageBox("Điện thoại không hỗ trợ chức năng chụp hình").Show();
                    return;
                }
                media = await CrossMedia.Current.PickPhotoAsync();
                if (media == null) return;
                imagePicture.Source = ImageSource.FromStream(() =>
                {
                    return media.GetStream();
                });

            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }
        }

        private async void ChupHinh_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await new MessageBox("Điện thoại không hỗ trợ chức năng chụp hình").Show();
                    return;
                }
                media = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions { SaveToAlbum = true });
                if (media == null) return;
                imagePicture.Source = ImageSource.FromStream(() =>
                {
                    return media.GetStream();
                });
            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }
        }
    }
}