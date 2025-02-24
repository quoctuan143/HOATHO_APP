using APP_HOATHO.Global;
using APP_HOATHO.Models;
using APP_HOATHO.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Dialog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CapNhatViTriThietBiPopup : PopupPage, INotifyPropertyChanged
    {
        BaseViewModel viewModel { get; set; }
        public NhaMayOfUser NhaMayTongs { get; set; }
        public ObservableCollection<NhaMayModel> DanhSachNhaMays { get; set; }
        public ObservableCollection<XuongModel> DanhSachXuongs { get; set; }
        public ObservableCollection<ToSanXuatModel> DanhSachTos { get; set; }
        TaskCompletionSource<DialogReturn> _tsk = null;
        public event PropertyChangedEventHandler PropertyChanged;
        public CapNhatViTriThietBi Item { get; set; }
        public string MaThietBi { get; set; }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CapNhatViTriThietBiPopup(string mathietbi)
        {
            InitializeComponent();
            Item = new CapNhatViTriThietBi();
            Item.MaThietBi = mathietbi;            
            viewModel = new BaseViewModel();
            BindingContext = this;
        }

        private async void btnOK_Clicked(object sender, EventArgs e)
        {
            try
            {                
                viewModel.ShowLoading("Đang lưu vị trí thiết bị. vui lòng đợi...");             
                var ok = await viewModel.RunHttpClientPost($"api/qltb/CapNhatViTriThietBi",Item);
                viewModel.HideLoading();
                if (ok.IsSuccessStatusCode)
                {
                    await new MessageBox("Cập nhật vị trí thành công").Show();
                    await Navigation.PopPopupAsync(true);                  
                }    
                else
                {
                    await new MessageBox(await ok.Content.ReadAsStringAsync()).Show();
                }
            }
            catch (Exception ex)
            {
                viewModel.HideLoading();
                await new MessageBox(ex.Message).Show();

            }
            
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync(true);
            _tsk.SetResult(DialogReturn.Cancel);
        }
        public async Task<DialogReturn> Show()
        {
            _tsk = new TaskCompletionSource<DialogReturn>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
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
                OnPropertyChanged("DanhSachNhaMays");   
            }
            catch (Exception ex)
            {
                viewModel.HideLoading();
                await new MessageBox(ex.Message).Show();

            }

        }
        private void cbNhaMay_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {                        
            var item = e.Value as NhaMayModel;
            if (item != null)
            {
                DanhSachXuongs = NhaMayTongs.Xuongs.Where(x => x.WorkCenterCode == item.Code).ToObservableCollection();
                OnPropertyChanged("DanhSachXuongs");
            }
        }

        private void cbxuong_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {                        
            var item = e.Value as XuongModel;
            if (item != null)
            {
                DanhSachTos = NhaMayTongs.Tos.Where(x => x.WorkShopCode == item.Code).ToObservableCollection();
                OnPropertyChanged("DanhSachTos");
            }
        }
    }

    public class CapNhatViTriThietBi
    {
        public string MaNhaMay { get; set; }
        public string MaXuong { get; set; }
        public string MaToSanXuat { get; set; }
        public string MaThietBi { get; set; }
    }
}