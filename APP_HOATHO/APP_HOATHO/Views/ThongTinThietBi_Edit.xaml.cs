using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThongTinThietBi_Edit : ContentPage, INotifyPropertyChanged
    {
        private BaseViewModel viewModel { get; set; }
        public DanhMuc_ThietBi Item { get; set; }
        public ObservableCollection<LookupValue> DanhSachTos { get; set; }
        public ThongTinThietBi_Edit(DanhMuc_ThietBi item)
        {
            InitializeComponent();
            viewModel = new BaseViewModel();
            Item = item;
            DanhSachTos = new ObservableCollection<LookupValue>();
            BindingContext = this;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await LoadDanhSachTo();
        }
        private async Task LoadDanhSachTo()
        {
            var list = await viewModel.RunHttpClientGet<LookupValue>($"api/qltb/getToSanXuat?username={Preferences.Get(Config.User, "")}");
            DanhSachTos = list.Lists;
            OnPropertyChanged("DanhSachTos");
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var ok1 = await new MessageYesNo("Bạn có muốn cập nhật không?").Show();
            if (ok1 == Global.DialogReturn.OK)
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Config.URL);
                var str = JsonConvert.SerializeObject(Item);
                var ok = client.PostAsJsonAsync("api/qltb/UpdateThongTinThietBi", Item );
                
                if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("cập nhật thành công"))
                {
                    Item.PositionName = DanhSachTos.FirstOrDefault(x => x.Code == Item.Position)?.Name;
                    DependencyService.Get<IMessage>().ShortAlert("Cập nhật thành công");
                    MessagingCenter.Send(this, "UpdateThietBi", Item );
                    await Navigation.PopAsync();
                }
                else
                {
                    await new MessageBox( ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                }
            }    
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion

    }
}