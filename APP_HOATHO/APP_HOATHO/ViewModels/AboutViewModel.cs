using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        #region "Field"

        INavigation Navigation;
        #endregion

        #region "Contructor"

        public AboutViewModel()
        {
            Title = "Thông tin ứng dụng";

            LogoutCommand = new Command(OnLogoutClicked);
            ChangePasswordCommand = new Command(OnChangePassClicked);
            InformationCommand = new Command(OnShowInformationClicked);
        }





        #endregion

        #region "Method"
        private void OnLogoutClicked(object obj)
        {
            Preferences.Set(Config.User, "1111");
            Preferences.Set(Config.Password, "1111");
            App.Current.MainPage = new Login();
        }
        private async void OnChangePassClicked(object obj)
        {
            try
            {
                var ok = await new MessageChangePassword().Show();
                if (ok == DialogReturn.OK)
                {
                    App.Current.MainPage = new Login();
                }
            }
            catch (Exception)
            {


            }
        }

        private async void OnShowInformationClicked(object obj)
        {
            await new AppInformation().Show();
        }

        #endregion

        #region "Command"
        public Command InformationCommand { get; }
        public Command LogoutCommand { get; }
        public Command ChangePasswordCommand { get; }
        #endregion

    }
}