using APP_HOATHO.Dialog;
using APP_HOATHO.Models.Nha_May_Soi;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho
{
    public class BuTruVaiNoChoNhaMayViewModel : BaseViewModel
    {
        public string SoChungTu { get; set; }
        public INavigation navigation { get; set; }
        ObservableCollection<NhaMayNoVaiKhoModel> _listItem;
        public ObservableCollection<NhaMayNoVaiKhoModel> ListItem { get => _listItem; set => SetProperty(ref _listItem, value); }      
        public ICommand LoadDataCommand { get; set; }
        public ICommand KhauTruCommand { get; set; }        
        public BuTruVaiNoChoNhaMayViewModel(BuTruVaiRequest request)
        {
            ListItem = new ObservableCollection<NhaMayNoVaiKhoModel>();
            LoadDataCommand = new Command(async () =>
            {
                var a = await RunHttpClientGet<NhaMayNoVaiKhoModel>($"api/NguyenLieu/GetNhaMayNoVai",request);
                ListItem = a.Lists;                
            });
            KhauTruCommand = new Command(async () => {
                var list = ListItem.Where(x=> x.Chon == true).ToList();
                var useid = GetUserId();
                if (list.Count == 0)
                {
                    LongAlert("Vui lòng chọn mã và nhập số lượng để khấu trừ");
                    return;
                }
                list.ForEach(x =>
                {
                    List<BuVaiMeta> meta = new List<BuVaiMeta> ();
                    if (!string.IsNullOrEmpty(x.Meta))
                    {
                        meta = JsonConvert.DeserializeObject<List<BuVaiMeta>>(x.Meta);
                        meta.Add(new BuVaiMeta {DocumentNo = request.DocumentNo, Quantity = x.Balance });
                        x.Meta = JsonConvert.SerializeObject(meta);   
                    }
                    else
                    {
                        meta.Add(new BuVaiMeta { DocumentNo = request.DocumentNo, Quantity = x.Balance });
                        x.Meta = JsonConvert.SerializeObject(meta);
                    } 
                        
                    x.Action = BehaviorEnum.Edit;
                    x.UserId = useid;
                    x.DocumentNo = request.DocumentNo;
                });
                
                var ask = await new MessageYesNo("Bạn có muốn khấu trừ vải cho phiếu xuất này không?").Show();
                if (ask == Global.DialogReturn.OK) 
                {
                    try
                    {
                        ShowLoading("Đang khấu trừ vui lòng đợi");
                        var ok = await RunHttpClientPost($"api/NguyenLieu/GhiNhanNoChoNhaMay", list);
                        HideLoading();
                        ok.ShowThongBaoRunApi("Bù trừ vải thành công cho phiếu xuất này!");     
                        await navigation.PopAsync();
                    }
                    catch (Exception ex)
                    {
                        HideLoading();
                        await new MessageBox(ex.Message).Show();
                    }
                    
                }
            });
        }
    }

    public class NhaMayNoVaiKhoModel :Bindable
    {
        public bool? Chon { get; set; }
        public int RowID { get; set; }
        public string DocumentNo { get; set; }
        public string ItemNo { get; set; }
        public string ColorNo { get; set; }
        public double Total { get; set; }
        double _balance;
        public double Balance { get => _balance; set { _balance = value;
                if (_balance > Total)
                {
                    ShortAlert("Số lượng không thể vượt quá nhà máy nợ");
                    SetProperty(ref _balance, Total);
                }
            } }
        public string Meta { get; set; }
        public string WorkCenterNo { get; set; }
        public string IdCayVai { get; set; }
        public BehaviorEnum Action { get; set; }
        public string UserId { get; set; }
    }

    public class BuTruVaiRequest
    {
        public string DocumentNo { get; set; }
        public string ItemNo { get; set; }
        public string ColorNo { get; set; }       
    }

    public class BuVaiMeta
    {
        public string DocumentNo { get; set; }
        public double Quantity { get; set; }
    }

    public enum BehaviorEnum
    {
        Add = 0,
        Edit = 1,
        Delete = 2

    }
}
