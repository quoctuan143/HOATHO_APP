using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.DuyetChungTu;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using APP_HOATHO.Models.KiDienTuPhuTung;

namespace APP_HOATHO.ViewModels.DuyetChungTu
{
   public class DuyetKiDienTuThietBi_ViewModel : BaseViewModel
    {
        #region "Field"

        ObservableCollection<DuyetKiDienTuPhuTungModel> _listItem;
        public DocumentType _documentType { get; set; }
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }
        public Command DateChangeCommand { get; set; }

        #endregion

        #region "Constructor"       
        public ObservableCollection<DuyetKiDienTuPhuTungModel> ListItem { get => _listItem; set { SetProperty(ref _listItem, value); } }

        public DuyetKiDienTuThietBi_ViewModel(DocumentType type)
        {
            this._documentType = type;
            Title = "KÍ ĐIỆN CHO THUÊ THIẾT BỊ";
            ListItem = new ObservableCollection<DuyetKiDienTuPhuTungModel>();
            LoadCommand = new Command(OnLoadExcute);
            DateChangeCommand = new Command(OnLoadExcute);           
            // form sẽ gửi về là form DuyetChungTu_Line , giá trị trả về là 1 docno khi duyệt 1 chứng từ thì xóa nó đi
            MessagingCenter.Subscribe<DuyetKiDienTuThietBi_Line_ViewModel, string>(this, "DuyetChungTu", (sender, docno) =>
            {
                var q = ListItem.Where(p => p.No_ == docno).FirstOrDefault();
                if (q != null)
                {
                    ListItem.Remove(q);
                    //gửi về form main để trừ đi thông báo
                    MessagingCenter.Send(this, "langngheduyet", _documentType);
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
                ListItem.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + $"api/DuyetChungTu/getKiDienTuThietBi?nhamay={Preferences.Get(Config.NhaMay, "")}").Result;

                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);                  
                  ListItem = JsonConvert.DeserializeObject<ObservableCollection<DuyetKiDienTuPhuTungModel>>(result);
                   
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
