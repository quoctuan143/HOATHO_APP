using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models.HangKhongGanNhan;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.HangKhongGanNhan
{    
    public class Xuat_Hang_Khong_Gan_Nhan_ViewModel : BaseViewModel
    {
        public INavigation navigation { get; set; }
        public string Title { get; set; }
        private string KeVai;    
        private ObservableCollection<Xuat_Hang_Khong_Gan_Nhan_Model> _listItem;
        public ObservableCollection<Xuat_Hang_Khong_Gan_Nhan_Model> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }
        public ICommand LoadDataCommand { get; set; }
        public ICommand QuetOChuaCayVaiCommand { get; set; }

        public Xuat_Hang_Khong_Gan_Nhan_ViewModel(string kevai)
        {            
            KeVai = kevai;
            Title = $"Xuất hàng khỏi kệ {kevai}";
            ListItem = new ObservableCollection<Xuat_Hang_Khong_Gan_Nhan_Model>();
            LoadDataCommand = new Command(async () =>
            {
                try
                {
                    ListItem.Clear();
                    var url = $"api/HangKhongGanNhan/LayTonKhoTrenKe?kevai={kevai}";
                    var a = await RunHttpClientGet<Xuat_Hang_Khong_Gan_Nhan_Model>(url);
                    ListItem = a.Lists;
                }
                catch (System.Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });

            QuetOChuaCayVaiCommand = new Command(async () =>
            {
                try
                {
                    var items = ListItem.Where(x => x.Chon).ToList();
                    if (items.Count == 0)
                    {
                        await new MessageBox("Vui lòng chọn cây vải trước khi xuất ra khỏi kệ").Show();
                        return;
                    }

                    var ask = await new MessageYesNo("Bạn có muốn xuất kho không?").Show();
                    if (ask == Global.DialogReturn.OK)
                    {
                        ShowLoading("Đang xử lý. vui lòng đợi...");
                        var post = await RunHttpClientPost($"api/HangKhongGanNhan/XuatVaiKhoiKe?userId={Preferences.Get (Config.User,"")}", items);
                        HideLoading();
                        if (post.IsSuccessStatusCode)
                        {
                            await new MessageBox("Xuất vải ra khỏi kệ thành công").Show();
                            foreach (var item in ListItem)
                            {
                                item.Chon = false;
                            }
                            await navigation.PopAsync();                            
                        }
                        else
                        {
                            await new MessageBox(post.Content.ReadAsStringAsync().Result).Show();
                        }
                    }    
                    
                }
                catch (Exception ex)
                {
                    HideLoading();
                    await new MessageBox(ex.Message).Show();
                }
            });
        }
    }
}
