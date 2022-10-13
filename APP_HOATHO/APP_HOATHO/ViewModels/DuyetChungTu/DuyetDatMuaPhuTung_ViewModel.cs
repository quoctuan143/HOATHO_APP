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

namespace APP_HOATHO.ViewModels.DuyetChungTu
{
   public class DuyetDatMuaPhuTung_ViewModel : BaseViewModel
    {
        #region "Field"

        ObservableCollection<DuyetChungTuModel> _listItem;
        public DocumentType _documentType { get; set; }
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }
        public Command DateChangeCommand { get; set; }

        #endregion

        #region "Constructor"
        public INavigation navigation { get; set; }
        public ObservableCollection<DuyetChungTuModel> ListItem { get => _listItem; set { SetProperty(ref _listItem, value); } }

        public DuyetDatMuaPhuTung_ViewModel(DocumentType type)
        {
            this._documentType = type;
            Title = "DUYỆT ĐẶT PHỤ TÙNG";
            ListItem = new ObservableCollection<DuyetChungTuModel>();
            LoadCommand = new Command(OnLoadExcute);
            DateChangeCommand = new Command(OnLoadExcute);

            // form sẽ gửi về là form DuyetChungTu_Line , giá trị trả về là 1 docno khi duyệt 1 chứng từ thì xóa nó đi
            MessagingCenter.Subscribe<DuyetChungTu_Line_ViewModel, string>(this, "DuyetChungTu", (sender, docno) =>
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
                var a = await RunHttpClientGet<DuyetChungTuModel>($"api/DuyetChungTu/getDatMuaPhuTung?username={Preferences.Get(Config.User, "")}");
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
