﻿using System;
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
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using APP_HOATHO.Interface;
using Syncfusion.SfDataGrid.XForms;
using APP_HOATHO.ViewModels;
using ZXing;

namespace APP_HOATHO.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class TaoLichSuBaoTri : ContentPage 
    {
        bool _isrunning;
        public Boolean IsRunning { get => _isrunning; set { _isrunning = value; OnPropertyChanged("IsRunning"); } }
        ObservableCollection<QUY_TRINH_BAO_TRI> _quytrinh;
      public ObservableCollection <QUY_TRINH_BAO_TRI> QUY_TRINH_BAO_TRIs { get => _quytrinh ; set { _quytrinh = value; OnPropertyChanged("QUY_TRINH_BAO_TRIs"); }  } 
        public  APP_HOATHO.Models.KeHoachBaoTri Item { get; set; }
        LICH_SU_BAO_TRI lsu;
        MediaFile media;
        BaseViewModel BaseViewModel { get; set; }
        public TaoLichSuBaoTri(APP_HOATHO.Models.KeHoachBaoTri item) 
        {
            InitializeComponent();
            BaseViewModel = new BaseViewModel ();
            Item = item;
            QUY_TRINH_BAO_TRIs = new ObservableCollection<QUY_TRINH_BAO_TRI>();
            BindingContext = this;
            //listquytrinh.QueryRowHeight += Listquytrinh_QueryRowHeight;
        }

        private void Listquytrinh_QueryRowHeight(object sender, Syncfusion.SfDataGrid.XForms.QueryRowHeightEventArgs e)
        {
            try
            {
                if (e.RowIndex != 0)
                {
                    //Calculates and sets height of the row based on its content 
                    e.Height = listquytrinh.GetRowHeight(e.RowIndex);
                    e.Handled = true;
                }
            }
            catch (Exception)
            {

                
            }
        }

        async void Save_Clicked(object sender, EventArgs e)

        {
            try
            {
               
                if (string.IsNullOrEmpty(entryTinhTrang.Text))
                   
                {
                    await new MessageBox("Bạn nhập tình trạng sau khi sửa chữa").Show();
                    return;
                }
                
                string noidung = "";
                foreach (QUY_TRINH_BAO_TRI btri in QUY_TRINH_BAO_TRIs)
                {
                    if (btri.IsCheck == true )
                    {
                        if (noidung == "")
                        {
                            noidung = btri.Description;
                        }
                        else
                        {
                            noidung += "||" + btri.Description;
                        }
                    }    
                    

                }
                if (!string.IsNullOrEmpty(entryNoiDungKhac.Text )  )
                {
                    noidung += "||" + entryNoiDungKhac.Text;
                }    
                if (media ==  null)
                {
                    await new MessageBox("Vui lòng chụp hình sau khi bảo trì").Show();
                    return;
                }
                var result = await new MessageYesNo("Bạn có muốn cập nhật lần bảo trì này không?").Show();
                if (result == Global.DialogReturn.OK)
                {

                    
                    lsu = new LICH_SU_BAO_TRI();
                    lsu.NHOM_BAO_TRI = Item.Code;
                    lsu.MA_THIET_BI = Item.No_;
                    lsu.MA_PHU_TUNG = entryPhuTung.Text == string.Empty ? "" : entryPhuTung.Text;
                    lsu.BAO_DUONG_DINH_KY = Convert.ToInt32(chkBaoDuong.IsChecked);
                    if (media != null )
                    {
                        var filename = media.Path.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                        lsu.IMAGE_LINK = filename;
                    }   
                    else
                    {
                        lsu.IMAGE_LINK = "";
                    }    
                    
                    lsu.MA_NHA_MAY = Preferences.Get(Config.NhaMay, "");
                    lsu.MA_XUONG = Preferences.Get(Config.MaXuong, "");
                    lsu.NGAY_GIO = DateTime.Now;
                    lsu.NGUOI_THUC_HIEN = Preferences.Get(Config.User, "");
                    lsu.NOI_DUNG = noidung;
                    lsu.TINH_TRANG_HIEN_TAI = entryTinhTrang.Text;
                    lsu.STATUS = 1;
                    lsu.NGUOI_XAC_NHAN = Preferences.Get(Config.User, ""); ;
                    lsu.THANG = Item.Thang;
                    lsu.NAM = Item.Nam;



                    //nếu up hình thành công thì up lich sử lên luôn
                    
                    if (media != null )
                    {
                        var content = new MultipartFormDataContent();
                        content.Add(new StreamContent(media.GetStream()), "\"file\"", $"\"{media.Path}\"");
                        
                        BaseViewModel.ShowLoading("Đang xử lý. vui lòng đợi....!");
                        var response = await BaseViewModel.RunHttpClientPost("api/qltb/PostLichSuBaoTri_Picture", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var ok = await BaseViewModel.RunHttpClientPost("api/qltb/postLichSuBaoTri_V1", lsu);
                            BaseViewModel.HideLoading();
                            if (ok.IsSuccessStatusCode)   
                            {                                
                                await Navigation.PopAsync();
                                await new MessageBox("Cập nhật thành công").Show();
                                MessagingCenter.Send(this, "AddLichSuBaoTri", lsu);                                
                            }
                            else
                            {
                                var message = await ok.Content.ReadAsStringAsync();
                                await new MessageBox(message).Show();
                            }
                        }
                        else
                        {
                            BaseViewModel.HideLoading();
                            var message = await response.Content.ReadAsStringAsync();
                            await new MessageBox(message).Show();
                            return;
                        }
                    }
                    else
                    {
                        
                        BaseViewModel.ShowLoading("Đang xử lý. vui lòng đợi....!");
                        var ok = await BaseViewModel.RunHttpClientPost("api/qltb/postLichSuBaoTri_V1", lsu);
                        BaseViewModel.HideLoading();
                        if (ok.IsSuccessStatusCode)
                        {                            
                            await new MessageBox("Cập nhật thành công").Show();
                            MessagingCenter.Send(this, "AddLichSuBaoTri", lsu);
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            var message = await ok.Content.ReadAsStringAsync();
                            await new MessageBox(message).Show();
                        }
                    }    
                    

                }
            }
            catch (Exception ex )
            {
                BaseViewModel.HideLoading();
                await new MessageBox( ex.Message ).Show();
            }
           finally
            {
                BaseViewModel.HideLoading();
            }
           
           
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async  void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {

                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Chọn một bức hình"
                });

                var stream = await result.OpenReadAsync();

                imagePicture.Source = ImageSource.FromStream(() => stream);
            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }

        }
        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                var result =await BaseViewModel.RunHttpClientGet<QUY_TRINH_BAO_TRI>("api/qltb/getQuyTrinhBaoTri_ChiTiet?mathietbi=" + Item.No_ + "&nhom=" + Item.ItemCategoryCode + "&mabaotri=" + Item.Code);
                QUY_TRINH_BAO_TRIs = result.Lists;
            }
            catch (Exception ex)
            {

                 new MessageBox(ex.Message).Show();
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