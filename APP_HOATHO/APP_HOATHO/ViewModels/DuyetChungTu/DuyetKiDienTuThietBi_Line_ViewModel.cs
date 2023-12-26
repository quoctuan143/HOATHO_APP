using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models.DuyetChungTu;
using APP_HOATHO.Models.KiDienTuPhuTung;
using APP_HOATHO.Views.DuyetChungTu;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.DuyetChungTu
{
   public class DuyetKiDienTuThietBi_Line_ViewModel : BaseViewModel 
    {
        #region "Field"

        ObservableCollection<DuyetKiDienTuPhuTungModel_Line> _listItem;     
        DocumentType _documentType;
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }
        public Command DeleteCommand { get; set; } 
        #endregion

        #region "Constructor"
        public INavigation navigation { get; set; }
        public ObservableCollection<DuyetKiDienTuPhuTungModel_Line> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public DuyetKiDienTuPhuTungModel DuyetChungTuModel { get; set; }
       

        public DuyetKiDienTuThietBi_Line_ViewModel(DuyetKiDienTuPhuTungModel duyetChungTuModel, DocumentType type) 
        {
            this.DuyetChungTuModel = duyetChungTuModel;
            this._documentType = type;            
            Title = "Chi tiết xuất thiết bị";           
            ListItem = new ObservableCollection<DuyetKiDienTuPhuTungModel_Line>();
            LoadCommand = new Command(OnLoadExcute);
            DeleteCommand = new Command(OnDeleteClick);            
        }
        #endregion

        #region "Method"
        private async void OnDeleteClick(object obj)
        {
            var isDelete = await new MessageYesNo( "Bạn có muốn duyệt không").Show();
            if (isDelete == DialogReturn.OK )
            {
                try
                {                   
                    DuyetChungTuModel.ApproveID = Preferences.Get(Config.User, "");
                    DuyetChungTuModel.ApproveUserName = Preferences.Get(Config.FullName, "");                 
                    if (IsBusy == true) return;
                    IsBusy = true;
                    ShowLoading("Đang xử lý vui lòng đợi");
                    await Task.Delay(1000);                   
                    HttpResponseMessage ok = null  ;
                    ok = await Config.client.PostAsJsonAsync("api/DuyetChungTu/DuyetKiDienTuThietBi", DuyetChungTuModel);

                    if (ok.StatusCode == System.Net.HttpStatusCode.OK )
                    {
                        HideLoading();
                        await navigation.PopAsync();
                        DependencyService.Get<IMessage>().ShortAlert("Đã duyệt và gửi mail cho các bộ phận liên quan");
                        MessagingCenter.Send(this, "DuyetChungTu", DuyetChungTuModel.No_ );
                        
                    }   
                    else if (ok.StatusCode == System.Net.HttpStatusCode.NotFound )
                    {
                        HideLoading();
                        await new MessageBox("Không tìm thấy chứng từ để duyệt ở server").Show();
                        return;
                    }
                   else if (ok.StatusCode ==  System.Net.HttpStatusCode.BadRequest )
                    {
                        HideLoading();
                        await new MessageBox(ok.Content.ReadAsStringAsync().Result ).Show();
                        return;
                    }    
                }
                catch (Exception ex)
                {
                    HideLoading();
                    await new MessageBox(ex.Message).Show();
                }
                finally
                {
                    IsBusy = false;
                }
            }    
        }
        private async void OnLoadExcute(object obj)
        {
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                ShowLoading("Đang tải vui lòng đợi");
                await Task.Delay(1000);
                string url = $"api/DuyetChungTu/getKiDienTuThietBi_ChiTiet?documentno={DuyetChungTuModel.No_}";
                ListItem.Clear();
                var a = await RunHttpClientGet<DuyetKiDienTuPhuTungModel_Line>(url);
                ListItem = a.Lists;                               
                HideLoading();
            }
            catch (Exception ex)
            {
                HideLoading();
                await new MessageBox( ex.Message).Show();
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion
    }
}
