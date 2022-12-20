using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Nha_May_Soi;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;

namespace APP_HOATHO.Views.Nha_May_Soi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Thong_Tin_Kien_Page : ContentPage, INotifyPropertyChanged
    {
        public UploadImageKien Item { get; set; }
        MediaFile media;
        
        public Thong_Tin_Kien_Page(UploadImageKien item)
        {
            InitializeComponent();
            Item = item;
            var items = new List<LocationPosition> { new LocationPosition  { Code = "01", Name = "Kho NL chính TCT" }, new LocationPosition { Code = "02", Name = "Kho NL chính Hải Vân" } };
            cbViTriLuu.DataSource = items.ToList();
            foreach (var i in items)
            {               
                if (i.Code == Item.PositionCode )
                {
                    cbViTriLuu.SelectedValue = i.Code;
                }    
            }    
           
            BindingContext = this;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var ok1 = await new MessageYesNo("Bạn có muốn cập nhật không?").Show();
                if (ok1 == Global.DialogReturn.OK)
                {
                    Item.NguoiUploadAnh = Preferences.Get(Config.User, "");
                    var content = new MultipartFormDataContent();
                    if (media != null)
                    {
                        content.Add(new StreamContent(media.GetStream()), "\"file\"", $"\"{media.Path}\"");                        
                    }                    
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(Config.URL);
                    var ok = client.PostAsync($"api/soi/UploadImageKien?PositionCode={Item.PositionCode}&NguoiUploadAnh={Item.NguoiUploadAnh}&PackingDesc={Item.PackingDesc}&RowID={Item.RowID}", content).Result;
                    if (ok.IsSuccessStatusCode)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Cập nhật thành công");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await new MessageBox(ok.Content.ReadAsStringAsync().Result.ToLower()).Show();
                    }
                }
            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }
           
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                DependencyService.Get<IMessage>().ShortAlert("Điện thoại không hỗ trợ chức năng chụp hình");

                return;
            }
            media = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions { SaveToAlbum = true });
            if (media == null) return;
            imagePicture.Source = ImageSource.FromStream(() =>
            {
                return media.GetStream();
            });
        }
        
        class LocationPosition
        {
            public string  Code { get; set; }
            public string Name { get; set; }
        }
    }
}