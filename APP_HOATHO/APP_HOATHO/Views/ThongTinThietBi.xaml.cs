using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models;
using APP_HOATHO.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThongTinThietBi : ContentPage
    {
        ThongTinThietBiViewModel viewModel;
        MediaFile media;
        public ThongTinThietBi(DanhMuc_ThietBi item)
        {
            InitializeComponent();
            BindingContext = viewModel = new ThongTinThietBiViewModel(item);

        }

        private void SfTabView_TabItemTapped(object sender, Syncfusion.XForms.TabView.TabItemTappedEventArgs e)
        {
            if (e.TabItem.Title =="Lịch Sử" )
            {
                if (viewModel.lICH_SU_BAO_TRIs.Count == 0)
                    viewModel.LoadLichSuBaoTri.Execute(viewModel.Item.No_);
            }
            if (e.TabItem.Title == "Quy Trình")
            {
                if (viewModel.QUY_TRINH_BAO_TRIs.Count == 0)
                    viewModel.LoadQuyTrinhBaoTri.Execute(viewModel.Item.No_);
            }
            if (e.TabItem.Title == "KH Bảo Trì")
            {
                if (viewModel.KE_HOACH_BAO_TRIs.Count == 0)
                    viewModel.LoadKeHoachBaoTri.Execute(DateTime.Now.Year.ToString());
            }    
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (viewModel.Item.LinkVideo == "" || viewModel.Item.LinkVideo == null)
            {
                await new MessageBox("thông báo", "Không có link video").Show();
                return;
            }    
                
            await Browser.OpenAsync("https://www.youtube.com/embed/" + viewModel.Item.LinkVideo);
        }

        private async void addSuCo_Clicked(object sender, EventArgs e)
        {
             
            APP_HOATHO.Models.KeHoachBaoTri item = listKeHoach.SelectedItem as APP_HOATHO.Models.KeHoachBaoTri;
            if (item != null)
            {
                await Navigation.PushAsync(new TaoLichSuBaoTri(item));
            }
        }

        private async void uploadImage_Clicked(object sender, EventArgs e)       
        {
            try
            {               
                if (media != null )
                {
                    await DependencyService.Get<IProcessLoader>().Show("Đang upload hình vui lòng đợi");
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(media.GetStream()), "\"file\"", $"\"{media.Path}\"");
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(Config.URL );
                    var response = client.PostAsync("api/qltb/PostFileUpload?mathietbi=" + viewModel.Item.No_, content).Result;
                    await DependencyService.Get<IProcessLoader>().Hide();
                    await new MessageBox("Thông báo", response.Content.ReadAsStringAsync().Result).Show();
                }    
                else
                {
                    await new MessageBox("Thông báo", "chưa có hình nào để upload").Show();
                }    
               
                
            }
            catch (Exception ex)
            {


            }
            
            finally
            {
                await DependencyService.Get<IProcessLoader>().Hide();
            }


        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported  )
            {
                await new MessageBox("thông báo", "Điện thoại không hỗ trợ chức năng chụp hình").Show();
                return;
            }
           media  = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions { SaveToAlbum = true }) ;
            if (media == null) return;
            imagePicture.Source = ImageSource.FromStream(() =>
            {
                return media.GetStream();
            });
        }
    }
}