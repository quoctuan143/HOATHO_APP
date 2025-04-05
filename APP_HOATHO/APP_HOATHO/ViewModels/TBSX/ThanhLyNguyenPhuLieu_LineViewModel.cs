using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.DuyetChungTu;
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
    public class ThanhLyNguyenPhuLieu_LineViewModel : BaseViewModel
    {
        public ObservableCollection<MaterialsLiquidationLine> ListItem { get; set; }
        public INavigation NavigationPage { get; set; }
        public ICommand LoadCommand { get; set; }
        public Command ApproveCommand { get; set; }

        public string DocumentNo { get; set; }
        public ThanhLyNguyenPhuLieu_LineViewModel(string documentno)
        {
            ListItem = new ObservableCollection<MaterialsLiquidationLine>();
            LoadCommand = new Command(async () =>
            {
                try
                {
                    var data = await RunHttpClientGet<MaterialsLiquidationLine>($"api/DuyetChungTu/GetChiTietThanhLyNPL?documentNo={documentno}");
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
                        DocumentNo = documentno
                    ,
                        UserApprove = Preferences.Get(Config.User, ""),
                        Permision = (AppoveType)Preferences.Get(Config.Role, 0)
                    };

                    var asked = await new MessageYesNo("Bạn có muốn duyệt không?").Show();
                    if (asked == DialogReturn.OK)
                    {
                        ShowLoading("Đang xử lý. vui lòng đợi...");
                        var send = await RunHttpClientPost("api/DuyetChungTu/DuyetThanhLyNPL", request);
                        HideLoading();
                        if (send.IsSuccessStatusCode)
                        {
                            await NavigationPage.PopAsync();
                            LongAlert("Đã duyệt thành công!");
                            MessagingCenter.Send(this, "thanhlynpl", documentno);
                        }
                        else
                        {
                            await new MessageBox(await send.Content.ReadAsStringAsync()).Show();
                        }
                    }
                }
                catch (Exception ex)
                {
                    HideLoading();
                    await new MessageBox(ex.Message).Show(); ;
                }

            });
        }
    }
}
