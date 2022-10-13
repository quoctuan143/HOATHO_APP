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
   
    public class MoLaiLenhCapPhatGiaCong_ViewModel : BaseViewModel
    {
        #region "Field"

        ObservableCollection<DuyetChungTuModel> _listItem;
        public DocumentType _documentType { get; set; }
        DateTime _fromDate;
        DateTime _toDate;
        #endregion

        #region "Command"
        public Command LoadCommand { get; set; }
        public Command DateChangeCommand { get; set; }

        #endregion

        #region "Constructor"
        public INavigation navigation { get; set; }
        public ObservableCollection<DuyetChungTuModel> ListItem { get => _listItem; set { SetProperty(ref _listItem, value); } }

        public DateTime FromDate { get => _fromDate; set { SetProperty(ref _fromDate, value); } }
        public DateTime ToDate
        {
            get => _toDate;

            set
            {
                SetProperty(ref _toDate, value);
            }
        }


        public MoLaiLenhCapPhatGiaCong_ViewModel(DocumentType type) 
        {
            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ToDate = FromDate.AddMonths(1).AddDays(-1);
            this._documentType = type;
            Title = "MỞ LẠI LCP GIA CÔNG";
            ListItem = new ObservableCollection<DuyetChungTuModel>();
            LoadCommand = new Command(OnLoadExcute);
            DateChangeCommand = new Command(OnLoadExcute);
            // form sẽ gửi về là form DuyetChungTu_Line , giá trị trả về là 1 docno khi duyệt 1 chứng từ thì xóa nó đi
            MessagingCenter.Subscribe<MoLaiChungTaDaDuyet_Line_ViewModel, string>(this, "MoChungTu", (sender, docno) =>
            {
                var q = ListItem.Where(p => p.No_ == docno).FirstOrDefault();
                if (q != null)
                {
                    ListItem.Remove(q);
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
                var a = await RunHttpClientGet<DuyetChungTuModel>($"api/DuyetChungTu/getMoLaiLCP_GC?username={Preferences.Get(Config.User, "")}&fromdate={string.Format("{0:yyyy-MM-dd}", FromDate)}&todate={string.Format("{0:yyyy-MM-dd}", ToDate)}");
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
