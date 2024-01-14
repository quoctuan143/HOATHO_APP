using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho
{
    public class TaoPhieuXuatKhoGomVai_ViewModel : BaseViewModel
    {
        public INavigation navigation { get; set; } 
       public  ObservableCollection<LookupValue> danhSachMaNPL { get; set; }
       public ObservableCollection<LookupValue> danhSachMauNPL { get; set; }

        string maNPL;
        public string MaNPL { get => maNPL; set => SetProperty(ref maNPL, value); }
        string mauNPL;
        public string MauNPL { get => mauNPL; set => SetProperty(ref mauNPL, value); }

        ObservableCollection<Tong_Hop_Gom_Phieu_Xuat_Model> listItem;
        public ObservableCollection<Tong_Hop_Gom_Phieu_Xuat_Model> ListItem { get => listItem; set => SetProperty(ref listItem, value); }   

        public ICommand SearchCommand { get; set; }
        public ICommand TaoPhieuCommand { get; set; }
        public ICommand ChooseMauNPLCommand { get; set; }
        public ICommand ChooseMaNPLCommand { get; set; }
        public TaoPhieuXuatKhoGomVai_ViewModel()
        {
            danhSachMaNPL = new ObservableCollection<LookupValue>();
            danhSachMauNPL = new ObservableCollection<LookupValue>();
            ListItem = new ObservableCollection<Tong_Hop_Gom_Phieu_Xuat_Model>();
            SearchCommand = new Command(async() =>
            {
                var find = await RunHttpClientGet<Tong_Hop_Gom_Phieu_Xuat_Model>($"api/qltb/GetDanhSachTongHopGomVai?manpl={maNPL}&maunpl={mauNPL}");
                ListItem.Clear();
                ListItem = find.Lists;
            });
            TaoPhieuCommand = new Command(async () =>
            {
                try
                {
                    var _list = ListItem.Where(x => x.Chon == true).ToList();
                    if (_list.Count == 0)
                    {
                        await new MessageBox("Bạn chưa chọn phiếu xuất để gom xuất").Show();
                        return;
                    }    
                    if (await new MessageBox ("Bạn có muốn tạo phiếu gộp không?").Show() == Global.DialogReturn.OK)
                    {
                        var find = await RunHttpClientPost($"api/qltb/PostTaoPhieuGomVaiXuatKho?user={Preferences.Get(Config.User,"")}", _list);
                        if (find.IsSuccessStatusCode)
                        {
                            await new MessageBox("Tạo phiếu xuất tổng thành công").Show();
                            _list.ForEach (x=> ListItem.Remove(x));
                        }
                        else
                        {
                            await new MessageBox(find.Content.ReadAsStringAsync().Result).Show();
                        }
                    }    
                    
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();    
                }

              
            });

            ChooseMauNPLCommand = new Command(async () =>
            {
                try
                {                    
                    var lookup = new LookupItem(danhSachMauNPL, "Danh sách màu NPL");
                    lookup.closeForm += (s, e) =>
                    {
                        MauNPL = e.Code;
                        SearchCommand.Execute(null);
                    };

                    await navigation.PushModalAsync(lookup);
                }
                catch
                {                    
                }
                
            });

            ChooseMaNPLCommand = new Command(async () =>
            {
                try
                {
                    var lookup = new LookupItem(danhSachMaNPL, "Danh sách mã NPL");
                    lookup.closeForm += async (s, e) =>
                    {
                        MaNPL = e.Code;
                        SearchCommand.Execute(null);
                        danhSachMauNPL.Clear();
                        var b = await RunHttpClientGet<LookupValue>($"api/qltb/GetDanhSachMauNPLXuatKho?manpl={e.Code}");
                        danhSachMauNPL = b.Lists;
                        OnPropertyChanged("danhSachMauNPL");
                    };

                    await navigation.PushModalAsync(lookup);

                }
                catch 
                {                    
                }
                
            });

            Task.Factory.StartNew(async() =>
            {
                try
                {
                    var a = await RunHttpClientGet<LookupValue>("api/qltb/GetDanhSachNPLXuatKho");   
                    danhSachMaNPL = a.Lists;
                    OnPropertyChanged("danhSachMaNPL");
                }
                catch
                {
                }
                
            });
        }
    }
    
    public class Tong_Hop_Gom_Phieu_Xuat_Model 
    {
        [JsonProperty("Item No_")]
        public string ItemNo { get; set; }
        [JsonProperty("Color No_")]
        public string ColorNo { get; set; }

        [JsonProperty("Document No_")]
        public string DocumentNo { get; set; }

        [JsonProperty("Unit of Measure Code")]
        public string DVT { get; set; }

        public double Quantity { get; set; }

        public bool Chon { get; set; }

        public string RowID { get; set; }
    }
}
