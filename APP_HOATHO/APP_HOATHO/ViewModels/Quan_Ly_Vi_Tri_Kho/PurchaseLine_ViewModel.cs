using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho
{
    public class PurchaseLine_ViewModel : BaseViewModel
    {
        public string SoChungTu { get; set; }
        public INavigation navigation { get; set; }
        ObservableCollection<PurchaseLine_Model> _listItem;
        public ObservableCollection<PurchaseLine_Model> ListItem { get => _listItem ; set => SetProperty (ref _listItem ,value ); }
        public PurchaseLine_Model SelectItem { get; set; }
        public ICommand LoadDataCommand { get; set; }
        public ICommand AddPackingListCommand { get; set; }
        public ICommand ShowPackingListCommand { get; set; }

        public PurchaseLine_ViewModel(string soChungTu)
        {
            SoChungTu = soChungTu;
            ListItem = new ObservableCollection<PurchaseLine_Model>();
            LoadDataCommand = new Command(async () =>
            {
                try
                {
                    ListItem.Clear();
                    var url = $"api/qltb/GetChiTietPhieuNhapMuaHang?sochungtu={SoChungTu}";
                    var a = await RunHttpClientGet<PurchaseLine_Model>(url);
                    ListItem = a.Lists;
                }
                catch (System.Exception ex)
                {

                    await new MessageBox(ex.Message).Show();
                }
               
            });

            AddPackingListCommand = new Command(async () => {
                try
                {
                    if (SelectItem == null)
                    {
                        await new MessageBox("Vui lòng chọn một mã NPL trước khi tạo mới một Id cho cây vải").Show();
                        return;
                    }
                    PurchaseLinePackingList_Model item = new PurchaseLinePackingList_Model
                    {
                        ItemNo = SelectItem.No_,
                        Name = SelectItem.Name,
                        Color = SelectItem.Color,
                        DocumentNo = SoChungTu,
                        UserId = Preferences.Get (Config.User ,"")
                    };
                    await navigation.PushAsync(new PurchaseLinePackingListAdd_Page(item));
                }
                catch (System.Exception ex)
                {

                    await new MessageBox(ex.Message).Show();
                }
               
            });

            ShowPackingListCommand = new Command(async () => {
                await navigation.PushAsync(new PurchaseLinePackingList_Page(SoChungTu));
            });
        }
    }
}