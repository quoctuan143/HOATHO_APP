using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
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
    public class TBSX_DC_Header_ViewModel : BaseViewModel
    {
        public ObservableCollection<TBSXDC_Header_Model> ListItem { get; set; }
        public INavigation NavigationPage { get; set; }
        public ICommand LoadCommand { get; set; }
        public Command<string> ViewDetailCommand { get; set; }
        public TBSX_DC_Header_ViewModel()
        {
            ListItem = new ObservableCollection<TBSXDC_Header_Model>();
            MessagingCenter.Subscribe<TBSX_DC_Line_ViewModel, string>(this, "tbsxdc", (s, e) =>
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
                    var permision = Preferences.Get(Config.Role, 1);
                    var data = await RunHttpClientGet<TBSXDC_Header_Model>($"api/tbsx/getPhuLucTBSX_Header?userid={Preferences.Get(Config.User, "")}&tongiamdoc={permision}");
                    ListItem = data.Lists;
                    OnPropertyChanged("ListItem");
                }
                catch (Exception ex)
                {

                    await new MessageBox(ex.Message).Show();
                }
                
            });

            ViewDetailCommand = new Command<string>( async doc =>
            {
                try
                {
                    await NavigationPage.PushAsync(new TBSX_DC_Line_Page(doc));
                }
                catch (Exception ex)
                {

                    await new MessageBox(ex.Message).Show();
                }
                
            });
        }

        
    }
}
