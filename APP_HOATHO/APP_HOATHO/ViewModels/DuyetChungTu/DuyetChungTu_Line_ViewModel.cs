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
using APP_HOATHO.Views.DuyetChungTu;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.DuyetChungTu
{
   public class DuyetChungTu_Line_ViewModel : BaseViewModel
    {
        #region "Field"

        ObservableCollection<DuyetChungTuLine_Model>_listItem;     
        DocumentType _documentType;
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }
        public Command DeleteCommand { get; set; } 
        #endregion

        #region "Constructor"
        public INavigation navigation { get; set; }
        public ObservableCollection<DuyetChungTuLine_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public DuyetChungTuModel DuyetChungTuModel { get; set; }
       

        public DuyetChungTu_Line_ViewModel(DuyetChungTuModel duyetChungTuModel, DocumentType type) 
        {
            this.DuyetChungTuModel = duyetChungTuModel;
            this._documentType = type;
            if (type == DocumentType.DuyetLCP)
                Title = "Chi tiết lệnh cấp phát";
            else if (type == DocumentType.DuyetLCP_GC)
                Title = "Chi tiết lệnh cấp phát";
            else if (type == DocumentType.DuyetMuaHang)
                Title = "Chi tiết đặt mua";
            else if (type == DocumentType.DuyetDatMuaPhuTung)
                Title = "Chi tiết đặt phụ tùng";           
            ListItem = new ObservableCollection<DuyetChungTuLine_Model>();
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
                    DuyetChungTuModel.UserName = Preferences.Get(Config.User, "");
                    DuyetChungTuModel.FullName = Preferences.Get(Config.FullName, "");                 
                    if (IsBusy == true) return;
                    IsBusy = true;
                    ShowLoading("Đang xử lý vui lòng đợi");
                    await Task.Delay(1000);                   
                    HttpResponseMessage ok = null  ;
                    if (_documentType == DocumentType.DuyetLCP)
                    {
                        ok = await Config.client.PostAsJsonAsync("api/DuyetChungTu/DuyetLCP", DuyetChungTuModel);
                    }                            
                    else if (_documentType == DocumentType.DuyetLCP_GC)
                    {
                        ok = await Config.client.PostAsJsonAsync("api/DuyetChungTu/DuyetLCP_GC", DuyetChungTuModel) ;                       
                    }                          
                   
                    else if (_documentType == DocumentType.DuyetMuaHang)
                    {
                        ok = await Config.client.PostAsJsonAsync("api/DuyetChungTu/DuyetDonDatMua", DuyetChungTuModel) ;                       
                    }
                    else if (_documentType == DocumentType.DuyetDatMuaPhuTung)
                    {
                        ok = await Config.client.PostAsJsonAsync("api/DuyetChungTu/DuyettDatMuaPhuTung", DuyetChungTuModel);
                    }
                    
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
                string url = "";
                if (_documentType == DocumentType.DuyetLCP)
                    url = $"api/DuyetChungTu/getLenhCapPhat_ChiTiet?documentno={DuyetChungTuModel.No_}";
                else if (_documentType == DocumentType.DuyetLCP_GC)
                    url = $"api/DuyetChungTu/getLenhCapPhat_GC_ChiTiet?documentno={DuyetChungTuModel.No_}";
                else if (_documentType == DocumentType.DuyetMuaHang  )
                    url = $"api/DuyetChungTu/getDonDatMua_ChiTiet?documentno={DuyetChungTuModel.No_}";
                else if (_documentType == DocumentType.DuyetDatMuaPhuTung)
                    url = $"api/DuyetChungTu/getDatMuaPhuTung_ChiTiet?documentno={DuyetChungTuModel.No_}";
                
                ListItem.Clear();
                var a = await RunHttpClientGet<DuyetChungTuLine_Model>(url);
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
