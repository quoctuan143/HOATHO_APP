using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models;
using APP_HOATHO.Models.DuyetChungTu;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using APP_HOATHO.Views.DuyetChungTu;
using System.Threading.Tasks;
using System.Linq;

namespace APP_HOATHO.ViewModels.DuyetChungTu
{
  
    public class DuyetDeNghiThanhToan_ViewModel : BaseViewModel
    {
        #region "Field"

        ObservableCollection<DeNghiThanhToanHeader_Model> _listItem;       
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }
        public Command DateChangeCommand { get; set; }

        #endregion

        #region "Constructor"
        public INavigation navigation { get; set; }
        public ObservableCollection<DeNghiThanhToanHeader_Model> ListItem { get => _listItem; set { SetProperty(ref _listItem, value); } }

        public DuyetDeNghiThanhToan_ViewModel()
        {
            Title = "Duyệt đề nghị thanh toán";
            ListItem = new ObservableCollection<DeNghiThanhToanHeader_Model>();
            LoadCommand = new Command(OnLoadExcute);
            DateChangeCommand = new Command(OnLoadExcute);

            // form sẽ gửi về là form DuyetChungTu_Line , giá trị trả về là 1 docno khi duyệt 1 chứng từ thì xóa nó đi
            MessagingCenter.Subscribe<DuyetDeNghiThanhToan_Line_ViewModel, string>(this, "DuyetDeNghiThanhToan", (sender, docno) =>
            {
                var q = ListItem.Where(p => p.No_ == docno).FirstOrDefault();
                if (q != null)
                {
                    ListItem.Remove(q);
                    //gửi về form main để trừ đi thông báo
                    MessagingCenter.Send(this, "langngheduyet", DocumentType.DuyetThanhToan );
                    OnPropertyChanged(nameof(ListItem));//cập nhật lại thông tin lên view

                }
            });
        }

        #endregion

        #region "Method"


        private async void OnLoadExcute(object obj)
        {
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                ShowLoading("Đang tải vui lòng đợi");
                await Task.Delay(1000);
                string url = "";                
                url = $"api/DuyetChungTu/getDeNghiThanhToan?username={Preferences.Get(Config.User, "")}";

                HttpResponseMessage respon = await Config.client.GetAsync(url);
                if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string _json = await respon.Content.ReadAsStringAsync();
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("[]") == false)
                    {
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string result = _json.Substring(from, to - from + 1);
                        ListItem = JsonConvert.DeserializeObject<ObservableCollection<DeNghiThanhToanHeader_Model>>(result);
                    }
                    else
                    {
                        ListItem.Clear();
                    }
                }

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
