using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.Nha_May_Soi.KyLenhXuatHang;
using APP_HOATHO.Views.Nha_May_Soi.KyLenhXuatHang;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.Nha_May_Soi.KyLenhXuatHang
{
    public class KyLenhXuatHangHeader_ViewModel : BaseViewModel
    {
        #region "Field"

        private ObservableCollection<LenhXuatHangHeader_Model> _listItem;
        public INavigation navigation { get; set; }
        public AppoveType approveType { get; set; }   
        #endregion "Field"

        #region "Command"

        public Command LoadCommand { get; set; }

        #endregion "Command"

        #region "Constructor"

        public ObservableCollection<LenhXuatHangHeader_Model> ListItem
        { get => _listItem; set { SetProperty(ref _listItem, value); } }

        private LenhXuatHangHeader_Model _selectItem;

        public LenhXuatHangHeader_Model SelectItem
        {
            get => _selectItem; set
            {
                SetProperty(ref _selectItem, value);
                if (SelectItem != null)
                {
                    navigation.PushAsync(new KyLenhXuatHangLine_Page(approveType, SelectItem.No_));
                }
            }
        }

        public KyLenhXuatHangHeader_ViewModel(AppoveType appoveType)
        {
            Title = "Ký lệnh xuất hàng";
            ListItem = new ObservableCollection<LenhXuatHangHeader_Model>();
            this.approveType = appoveType;
            MessagingCenter.Subscribe<KyLenhXuatHangLine_ViewModel, string>(this, "DuyetChungTu", (sender, args) =>
            {
                try
                {
                    ListItem.Remove(SelectItem);
                    MessagingCenter.Send(this, "langngheduyet");
                }
                catch (Exception ex)
                {

                    new MessageBox(ex.Message).Show();
                }
               
            });
            LoadCommand = new Command(async () =>
            {
                try
                {
                    if (IsBusy == true) return;
                    IsBusy = true;
                    ShowLoading("Đang tải vui lòng đợi");
                    await Task.Delay(1000);
                    ListItem.Clear();
                    var a = await RunHttpClientGet<LenhXuatHangHeader_Model>($"api/Soi/GetLenhXuatHang?userid={Preferences.Get(Config.User, "")}&permision={appoveType}");
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
            });
        }

        #endregion "Constructor"
    }
}