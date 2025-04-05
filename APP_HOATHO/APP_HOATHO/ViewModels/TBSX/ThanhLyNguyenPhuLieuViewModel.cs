using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.DuyetChungTu;
using APP_HOATHO.Models.TBSX;
using APP_HOATHO.Views.TBSX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.TBSX
{    
    public class ThanhLyNguyenPhuLieuViewModel : BaseViewModel
    {
        public ObservableCollection<MaterialsLiquidationHeader> ListItem { get; set; }
        public INavigation NavigationPage { get; set; }
        public ICommand LoadCommand { get; set; }
        public Command<string> ViewDetailCommand { get; set; }
        public ThanhLyNguyenPhuLieuViewModel()
        {
            ListItem = new ObservableCollection<MaterialsLiquidationHeader>();
            MessagingCenter.Subscribe<ThanhLyNguyenPhuLieu_LineViewModel, string>(this, "thanhlynpl", (s, e) =>
            {
                var find = ListItem.FirstOrDefault(x => x.No_ == e);
                if (find != null)
                {
                    ListItem.Remove(find);
                    OnPropertyChanged("ListItem");
                    MessagingCenter.Send(this, "langngheduyet");
                }
            });
            LoadCommand = new Command(async () =>
            {
                try
                {                    
                    var data = await RunHttpClientGet<MaterialsLiquidationHeader>($"api/DuyetChungTu/GetThanhLyNPL?username={Preferences.Get(Config.User, "")}");
                    ListItem = data.Lists;
                    OnPropertyChanged("ListItem");
                }
                catch (Exception ex)
                {

                    await new MessageBox(ex.Message).Show();
                }

            });

            ViewDetailCommand = new Command<string>(async doc =>
            {
                try
                {
                    await NavigationPage.PushAsync(new ThanhLyNguyenPhuLieu_LineView(doc));
                }
                catch (Exception ex)
                {

                    await new MessageBox(ex.Message).Show();
                }

            });
        }
    }
}
