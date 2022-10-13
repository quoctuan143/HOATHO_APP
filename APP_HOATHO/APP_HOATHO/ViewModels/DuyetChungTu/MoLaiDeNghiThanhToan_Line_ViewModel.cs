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
  
    public class MoLaiDeNghiThanhToan_Line_ViewModel : BaseViewModel
    {
        #region "Field"

        ObservableCollection<DeNghiThanhToanLine_Model > _listItem;
        
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }
        public Command DeleteCommand { get; set; }
        #endregion

        #region "Constructor"
        public INavigation navigation { get; set; }
        public ObservableCollection<DeNghiThanhToanLine_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public DeNghiThanhToanHeader_Model DuyetChungTuModel { get; set; }


        public MoLaiDeNghiThanhToan_Line_ViewModel(DeNghiThanhToanHeader_Model  duyetChungTuModel)
        {
            this.DuyetChungTuModel = duyetChungTuModel;            
            ListItem = new ObservableCollection<DeNghiThanhToanLine_Model>();
            LoadCommand = new Command(OnLoadExcute);
            DeleteCommand = new Command(OnDeleteClick);
        }
        #endregion

        #region "Method"

        private async void OnDeleteClick(object obj)
        {
            var isDelete = await new MessageYesNo("Bạn có muốn mở lại không").Show();
            if (isDelete == DialogReturn.OK)
            {
                try
                {
                    DuyetChungTuModel.TPKinhDoanhKy = Preferences.Get(Config.User, "");
                    if (IsBusy == true) return;
                    IsBusy = true;
                    ShowLoading("Đang xử lý vui lòng đợi");
                    await Task.Delay(1000);
                    HttpResponseMessage ok = null;
                    ok = await Config.client.PostAsJsonAsync("api/DuyetChungTu/MoLaiDeNghiThanhToan", DuyetChungTuModel);

                    if (ok.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HideLoading();
                        MessagingCenter.Send(this, "MoDeNghiThanhToan", DuyetChungTuModel.No_);
                        DependencyService.Get<IMessage>().ShortAlert("Đã duyệt thành công");                       
                        await navigation.PopAsync();
                    }
                    else if (ok.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        HideLoading();
                        await new MessageBox("Không tìm thấy chứng từ để duyệt ở server").Show();
                        return;
                    }
                    else if (ok.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        HideLoading();
                        await new MessageBox(ok.Content.ReadAsStringAsync().Result).Show();
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
                string url =  $"api/DuyetChungTu/getDeNghiThanhToan_ChiTiet?documentno={DuyetChungTuModel.No_}";
                ListItem.Clear();
                var a = await RunHttpClientGet<DeNghiThanhToanLine_Model>(url);
                ListItem = a.Lists;     
                HideLoading();
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
        #endregion
    }
}
