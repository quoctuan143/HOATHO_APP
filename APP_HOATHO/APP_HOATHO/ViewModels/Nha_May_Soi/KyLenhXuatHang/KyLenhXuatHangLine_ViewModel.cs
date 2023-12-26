using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models.DuyetChungTu;
using APP_HOATHO.Models.Nha_May_Soi;
using APP_HOATHO.Models.Nha_May_Soi.KyLenhXuatHang;
using APP_HOATHO.Models.Thong_Ba_San_Xuat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.Nha_May_Soi.KyLenhXuatHang
{   
    public class KyLenhXuatHangLine_ViewModel : BaseViewModel
    {
        #region "Field"

        private ObservableCollection<LenhXuatHangLine_Model> _listItem;
        public INavigation navigation { get; set; }


        #endregion "Field"

        #region "Command"

        public Command LoadCommand { get; set; }

        public Command DuyetLenhXuatHangCommand { get; set; }

        #endregion "Command"

        #region "Constructor"

        public ObservableCollection<LenhXuatHangLine_Model> ListItem
        { get => _listItem; set { SetProperty(ref _listItem, value); } }

        public KyLenhXuatHangLine_ViewModel(AppoveType appoveType , string documentNo)
        {
            Title = $"Ký LXH {documentNo}";
            ListItem = new ObservableCollection<LenhXuatHangLine_Model>();
            LoadCommand = new Command(async () =>
            {
                try
                {
                    if (IsBusy == true) return;
                    IsBusy = true;
                    ShowLoading("Đang tải vui lòng đợi");
                    await Task.Delay(1000);
                    ListItem.Clear();
                    var a = await RunHttpClientGet<LenhXuatHangLine_Model>($"api/Soi/GetLenhXuatHang_ChiTiet?documentno={documentNo}");
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
            DuyetLenhXuatHangCommand = new Command(async() =>
            {
                ApproveRequest request = new ApproveRequest
                {
                    DocumentNo = documentNo,
                    Permision = appoveType,
                    UserApprove = Preferences.Get(Config.User, "")
                };
                var ask = await new MessageYesNo("Bạn có muốn duyệt không").Show();
                if (ask == DialogReturn.OK )
                {
                    var ok = await RunHttpClientPost("api/Soi/DuyetLenhXuatHang",request );
                    if (ok.IsSuccessStatusCode)
                    {
                        await navigation.PopAsync();
                        DependencyService.Get<IMessage>().ShortAlert("Đã duyệt và gửi mail cho các bộ phận liên quan");
                        MessagingCenter.Send(this, "DuyetChungTu", documentNo );
                    }    
                }    
            });
        }

        #endregion "Constructor"

        #region "Method"

        

        #endregion "Method"
    }
}
