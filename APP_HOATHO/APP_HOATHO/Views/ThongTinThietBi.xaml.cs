using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Syncfusion.SfDataGrid.XForms;
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
            listThietBi.QueryRowHeight += ListThietBi_QueryRowHeight;
            BindingContext = viewModel = new ThongTinThietBiViewModel(item);

        }

        private void ListThietBi_QueryRowHeight(object sender, Syncfusion.SfDataGrid.XForms.QueryRowHeightEventArgs e)
        {
            if (e.RowIndex != 0)
            {
                //Calculates and sets height of the row based on its content 
                e.Height = listThietBi.GetRowHeight(e.RowIndex);
                e.Handled = true;
            }
        }

        private void SfTabView_TabItemTapped(object sender, Syncfusion.XForms.TabView.TabItemTappedEventArgs e)
        {
            try
            {
                if (e.TabItem.Title == "Lịch Sử")
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
            catch (Exception)
            {

              
            }
            
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (viewModel.Item.LinkVideo == "" || viewModel.Item.LinkVideo == null)
            {
                DependencyService.Get<IMessage>().ShortAlert("Không có link video");
                
                return;
            }    
                
            await Browser.OpenAsync("https://www.youtube.com/embed/" + viewModel.Item.LinkVideo);
        }

        private async void addSuCo_Clicked(object sender, EventArgs e)
        {
            try
            {
                APP_HOATHO.Models.KeHoachBaoTri item = listKeHoach.SelectedItem as APP_HOATHO.Models.KeHoachBaoTri;
                if (item != null)
                {
                    await Navigation.PushAsync(new TaoLichSuBaoTri(item));
                }
                else
                {
                    APP_HOATHO.Models.KeHoachBaoTri item1 = new Models.KeHoachBaoTri
                    {
                        Code = "XX",
                        LoaiBaoTri = "XX",
                        Nam = DateTime.Now.Year,
                        Thang = DateTime.Now.Month,
                        NameVN = viewModel.Item.NameVN,
                        No_ = viewModel.Item.No_,
                        No_2 = viewModel.Item.No_2,
                        No_3 = viewModel.Item.No_3,
                        Da_Bao_Tri = false,
                        ItemCategoryCode = viewModel.Item.ItemCategory 

                    };
                    await Navigation.PushAsync(new TaoLichSuBaoTri(item1));
                }
            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }
            
        }

        private async void uploadImage_Clicked(object sender, EventArgs e)        
        {
            try
            {               
                if (media != null )  
                {
                    DependencyService.Get<IMessage>().ShortAlert("Đang upload hình vui lòng đợi");                    
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(media.GetStream()), "\"file\"", $"\"{media.Path}\"");
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(Config.URL );
                    var response = await client.PostAsync("api/qltb/PostFileUpload?mathietbi=" + viewModel.Item.No_, content);
                    
                    await new MessageBox(response.Content.ReadAsStringAsync().Result).Show();
                }    
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("chưa có hình nào để upload");  
                     
                }    
               
                
            }
            catch (Exception ex)
            {


            }
            

        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported  )
            {
                DependencyService.Get<IMessage>().ShortAlert("Điện thoại không hỗ trợ chức năng chụp hình");
                
                return;
            }
           media  = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions { SaveToAlbum = true }) ;
            if (media == null) return;
            imagePicture.Source = ImageSource.FromStream(() =>
            {
                return media.GetStream();
            });
        }

        private void Edit_Clicked(object sender, EventArgs e)
        {

        }

       

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            if (viewModel.Item != null)
            {
                Navigation.PushAsync(new ThongTinThietBi_Edit(viewModel.Item));
            }
        }

        private async void tlKiThuat_Tapped(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Item.TaiLieuKiThuat ))
            {
                DependencyService.Get<IMessage>().ShortAlert("Không có tài liệu kĩ thuật");

                return;
            }

            await Browser.OpenAsync(viewModel.Item.TaiLieuKiThuat );
        }

        private async void listThietBi_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            LICH_SU_BAO_TRI item = listThietBi.SelectedItem as LICH_SU_BAO_TRI;
            if (item != null)
            {
                if (!string.IsNullOrEmpty(item.IMAGE_LINK))
                    await new ShowImage(item.IMAGE_LINK).Show();
            }
        }
    }
}