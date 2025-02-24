using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.ViewModels;
using Newtonsoft.Json;
using Syncfusion.Data.Extensions;
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
        public ObservableCollection<NhaMayModel> DanhSachNhaMays { get; set; }
        public ObservableCollection<XuongModel> DanhSachXuongs { get; set; }
        public ObservableCollection<ToSanXuatModel> DanhSachTos { get; set; }
        public NhaMayOfUser NhaMayTongs { get; set; }
        public ThongTinThietBi_Edit(DanhMuc_ThietBi item)
        {
            InitializeComponent();
            viewModel = new BaseViewModel();
            Item = item;
            DanhSachTos = new ObservableCollection<ToSanXuatModel>();
            DanhSachNhaMays = new ObservableCollection<NhaMayModel>();
            DanhSachXuongs = new ObservableCollection<XuongModel>();
            BindingContext = this;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await LoadDanhSachTo();
        }
        private async Task LoadDanhSachTo()
        {
            try
            {
                viewModel.ShowLoading("Đang tải thông tin nhà máy");
                await Task.Delay(1000);
                NhaMayTongs = await viewModel.RunHttpClientGetObject<NhaMayOfUser>($"api/qltb/getNhaMay?username={Preferences.Get(Config.User, "")}");
                viewModel.HideLoading();
                DanhSachNhaMays = NhaMayTongs.NhaMays;
                DanhSachXuongs = NhaMayTongs.Xuongs.Where(x => x.WorkCenterCode == Item.NhaMay).ToObservableCollection();
                DanhSachTos = NhaMayTongs.Tos.Where(x => x.WorkShopCode == Item.Xuong).ToObservableCollection();
                //cbNhaMay.SelectedIndex = DanhSachNhaMays.ToList().FindIndex(x => x.Code == Item.NhaMay);
                //cbxuong.SelectedIndex = DanhSachXuongs.ToList().FindIndex(x => x.Code == Item.Xuong);
                //cbNhaMay.SelectedIndex = DanhSachTos.ToList().FindIndex(x => x.Code == Item.ToSanXuat);
                OnPropertyChanged("DanhSachNhaMays");
                OnPropertyChanged("DanhSachXuongs");
                OnPropertyChanged("DanhSachTos");

            }
            catch (Exception ex)
            {
                viewModel.HideLoading();
                await new MessageBox(ex.Message).Show();
               
            }
            
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var ok1 = await new MessageYesNo("Bạn có muốn cập nhật không?").Show();
            if (ok1 == Global.DialogReturn.OK)
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Config.URL);
                var str = JsonConvert.SerializeObject(Item);
                var ok = await client.PostAsJsonAsync("api/qltb/UpdateThongTinThietBi", Item );
                
                if (ok.IsSuccessStatusCode)
                {
                    Item.TenToSanXuat = DanhSachTos.FirstOrDefault(x => x.Code == Item.ToSanXuat)?.Name;
                    Item.TenNhaMay = DanhSachNhaMays.FirstOrDefault(x => x.Code == Item.NhaMay)?.Name;
                    Item.TenXuong = DanhSachXuongs.FirstOrDefault(x => x.Code == Item.Xuong)?.Name;
                    DependencyService.Get<IMessage>().ShortAlert("Cập nhật thành công");
                    MessagingCenter.Send(this, "UpdateThietBi", Item );
                    await Navigation.PopAsync();
                }
                else
                {
                    await new MessageBox(await ok.Content.ReadAsStringAsync()).Show();
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

        private void cbNhaMay_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            Item.Xuong = null;
            Item.ToSanXuat = null;
            OnPropertyChanged("Item");            
            var item = e.Value  as NhaMayModel;
            if (item != null)
            {
                DanhSachXuongs = NhaMayTongs.Xuongs.Where(x=> x.WorkCenterCode == item.Code).ToObservableCollection();
                OnPropertyChanged("DanhSachXuongs");
            }
        }

        private void cbxuong_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            Item.ToSanXuat = null;
            OnPropertyChanged("Item");
            var item = e.Value as XuongModel;
            if (item != null)
            {
                DanhSachTos = NhaMayTongs.Tos.Where(x => x.WorkShopCode == item.Code).ToObservableCollection();
                OnPropertyChanged("DanhSachTos");
            }
        }
    }
}