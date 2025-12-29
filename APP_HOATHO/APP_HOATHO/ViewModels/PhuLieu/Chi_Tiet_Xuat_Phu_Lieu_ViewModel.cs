using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Views.Barcode;
using APP_HOATHO.Views.PhuLieu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.PhuLieu
{
        public class Chi_Tiet_Xuat_Phu_Lieu_ViewModel : BaseViewModel
    {
        public string SoChungTu { get; set; }
        public INavigation navigation { get; set; }
        ObservableCollection<Chi_Tiet_Nhap_Phu_Lieu_Model> _listItem;
        public ObservableCollection<Chi_Tiet_Nhap_Phu_Lieu_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public Chi_Tiet_Nhap_Phu_Lieu_Model SelectItem { get; set; }
        public ICommand LoadDataCommand { get; set; }
        public ICommand AddPackingListCommand { get; set; }

        public Chi_Tiet_Xuat_Phu_Lieu_ViewModel(string soChungTu)
        {
            SoChungTu = soChungTu;
            ListItem = new ObservableCollection<Chi_Tiet_Nhap_Phu_Lieu_Model>();
            LoadDataCommand = new Command(async () =>
            {
                try
                {
                    ListItem.Clear();
                    var url = $"api/PhuLieu/GetPhieuInXuatKho?documentNo={SoChungTu}&userId={Preferences.Get(Config.User,"")}" ;
                    ListItem = await RunHttpClientGetObject<ObservableCollection<Chi_Tiet_Nhap_Phu_Lieu_Model>>(url);
                    OnPropertyChanged(nameof(ListItem));
                }
                catch (System.Exception ex)
                {

                    await new MessageBox(ex.Message).Show();
                }
            });

           
            MessagingCenter.Subscribe<Xuat_Barcode_Cho_Phieu_Xuat_Phu_Lieu_Page>(this, "reloadXuatKhoPhuLieu", (e) => LoadDataCommand.Execute(null));
        }
    }
}
