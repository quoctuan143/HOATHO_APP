using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.TBSX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.TBSX
{
    public class TBSX_DC_Line_ViewModel : BaseViewModel
    {
        public ObservableCollection<TBSXDC_Line_Model> ListItem { get; set; }
        public INavigation NavigationPage { get; set; }
        public ICommand LoadCommand { get; set; }
        public Command ApproveCommand { get; set; }

        public string DocumentNo { get; set; }
        public TBSX_DC_Line_ViewModel(string documentno)
        {
            ListItem = new ObservableCollection<TBSXDC_Line_Model>();
            LoadCommand = new Command(async () =>
            {
                try
                {
                    var data = await RunHttpClientGet<TBSXDC_Line_Model>($"api/tbsx/getPhuLucTBSX_Line?documentno={documentno}");
                    ListItem = data.Lists;
                    OnPropertyChanged("ListItem");
                }
                catch (Exception ex)
                {

                    await new MessageBox(ex.Message).Show();
                }

            });

            ApproveCommand = new Command(async async =>
            {
                try
                {
                    ApproveRequest request = new ApproveRequest
                    {
                        DocumentNo = documentno,
                        UserApprove = Preferences.Get(Config.User, ""),
                        Permision = (AppoveType)Preferences.Get(Config.Role, 0)
                    };

                    var asked = await new MessageYesNo("Bạn có muốn duyệt không?").Show();
                    if (asked == DialogReturn.OK)
                    {
                        ShowLoading("Đang xử lý. vui lòng đợi...");
                        var send = await RunHttpClientPost("api/tbsx/postPhuLucThongBaoSanXuat_Header", request);
                        HideLoading();
                        if (send.IsSuccessStatusCode)
                        {
                            LongAlert("Đã duyệt và send email đến các bộ phận liên quan!");
                            MessagingCenter.Send(this, "tbsxdc", documentno);
                            await NavigationPage.PopAsync();

                        }
                        else
                        {
                            await new MessageBox(await send.Content.ReadAsStringAsync()).Show();
                        }
                    }
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show(); ;
                }

            });
        }
    }
}
