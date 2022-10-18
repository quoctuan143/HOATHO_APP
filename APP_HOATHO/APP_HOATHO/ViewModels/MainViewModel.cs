using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.ViewModels.DuyetChungTu;
using APP_HOATHO.Views;
using APP_HOATHO.Views.DuyetChungTu;
using APP_HOATHO.Views.KiDienTu;
using APP_HOATHO.Views.Kiem_Ke_Thiet_Bi;
using APP_HOATHO.Views.Tabpage;
using APP_HOATHO.Views.ThietBi;
using Newtonsoft.Json;
using Plugin.LatestVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region "Field"
        public INavigation Navigation { get; set; }
        public string FullName { get; set; }
        public bool IsDanhMucThietBi { get; set; }
        public bool IsLichXichBaoTri { get; set; }
        public bool IsDuyetDatPhuTung { get; set; }
        public bool IsDuyetDonDatMua { get; set; }
        public bool IsDuyetLCPFOB { get; set; }
        public bool IsDuyetLCPGC { get; set; }
        public bool IsDNTT { get; set; }
        public bool IsMainThietBi { get; set; }
        public bool IsMainDonHang { get; set; }
        public bool IsKiDienTuPhuTung { get; set; }
        public bool IsKiDienTuThietBi { get; set; }
        public bool IsDuyetYeuCauThueThietBi { get; set; }
        public bool IsDuyetPhieuTraThietBi { get; set; }
        public bool IsKiemKeThietBi { get; set; }
        public int NofiLCP_FOB { get; set; }
        public int NofiLCP_GC { get; set; }
        public int NofiDuyetDatMua { get; set; }
        public int NofiDuyetDatMuaPhuTung { get; set; }
        public int NofiKidienTuPhuTung { get; set; }
        public int NofiKidienTuThietBi { get; set; }
        public int NofiLichXich { get; set; }
        public int NofiDanhMucThietBi { get; set; }
        public int NofiDeNghiTT { get; set; }
        public int NofiYeuCauThueThietBi { get; set; }
        public int NofiTraThietBi { get; set;}
        DuyetChungTuPhuTung_Header duyetChungTuPhuTung_Header;
        DuyetDonDatMua DuyetDonDatMua;
        DuyetLCP_FOB duyetLCP_FOB;
        DuyetLCP_GIACONG DuyetLCP_GIACONG;
        DuyetDeNghiThanhToanTabpage DuyetDeNghiThanhToanTabpage;
        #endregion
        #region "Command"
        public ICommand DanhMucThietBiCommand { get; set; }
        public ICommand LichXichBaoTriCommand { get; set; }
        public ICommand KiemKeThietBiCommand { get; set; }
        public ICommand DuyetYeuCauThueThietBiCommand { get; set; }
        public ICommand DuyetYeuCauTraThietBiCommand { get; set; }
        public ICommand DuyetDatPhuTungCommand { get; set; }
        public ICommand DuyetKyDienTuXuatPhuTung { get; set; }
        public ICommand DuyetKyDienTuXuatThietBi  { get; set; }
        public ICommand DuyetDonDatMuaCommand { get; set; }
        public ICommand DuyetLenhCapPhatFOBCommand { get; set; }
        public ICommand DuyetLenhCapPhatGiaCongCommand { get; set; }
        public ICommand DuyetDeNghiThanhToanCommand { get; set; }
        public ICommand LoadDataCommand  { get; set; }
        #endregion
        public MainViewModel()
        {
            ShowLoading("Đang tải dữ liệu");
            LoadDataCommand = new Command( async() => await Load());
            DuyetDeNghiThanhToanCommand = new Command(async () => {
                if (DuyetDeNghiThanhToanTabpage == null)
                {
                    DuyetDeNghiThanhToanTabpage = new DuyetDeNghiThanhToanTabpage();
                }
                await DuyetDeNghiThanhToanTabpage.LoadData();
                await Navigation.PushAsync(DuyetDeNghiThanhToanTabpage);
            });
            DuyetLenhCapPhatGiaCongCommand = new Command(async () => {
                if (DuyetLCP_GIACONG == null)
                {
                    DuyetLCP_GIACONG = new DuyetLCP_GIACONG();
                }
                await DuyetLCP_GIACONG.LoadData();
                await Navigation.PushAsync(DuyetLCP_GIACONG);
            });
            DuyetLenhCapPhatFOBCommand = new Command(async () => {
                if (duyetLCP_FOB == null)
                {
                    duyetLCP_FOB = new DuyetLCP_FOB();
                }
                await duyetLCP_FOB.LoadData();
                await Navigation.PushAsync(duyetLCP_FOB);
            });
            DuyetDonDatMuaCommand = new Command(async () => {
                if (DuyetDonDatMua == null)
                {
                    DuyetDonDatMua = new DuyetDonDatMua();
                }
                await DuyetDonDatMua.LoadData();
                await Navigation.PushAsync(DuyetDonDatMua);
            });
            DanhMucThietBiCommand = new Command( async() => await Navigation.PushAsync(new Danh_Muc_Thiet_Bi()));
            LichXichBaoTriCommand = new Command(async () => await Navigation.PushAsync(new KeHoachBaoTri()));
            KiemKeThietBiCommand = new Command(async () => await Navigation.PushAsync(new Kiem_Ke_Thiet_Bi_Header_Page()));
            DuyetYeuCauThueThietBiCommand = new Command(async () => await Navigation.PushAsync(new DuyetYeuCauThueThietBiPage_Header(DocumentType.DuyetYeuCauThueThietBi)));
            DuyetYeuCauTraThietBiCommand = new Command(async () => await Navigation.PushAsync(new DuyetTraThietBi_Page(DocumentType.DuyetTraThietBi)));
            DuyetDatPhuTungCommand = new Command(async () => {
                if (duyetChungTuPhuTung_Header == null)
                {
                    duyetChungTuPhuTung_Header = new DuyetChungTuPhuTung_Header(DocumentType.DuyetDatMuaPhuTung);
                }
                await duyetChungTuPhuTung_Header.LoadData();
                await Navigation.PushAsync(duyetChungTuPhuTung_Header);
            });
            DuyetKyDienTuXuatPhuTung = new Command(async () => await Navigation.PushAsync(new DuyetKiDienTuPhuTung_Page(DocumentType.KiDienTuPhuTung)));
            DuyetKyDienTuXuatThietBi = new Command(async () => await Navigation.PushAsync(new DuyetKiDienTuThietBi_Page(DocumentType.KiDienTuThietBi)));
            Task.Factory.StartNew(() => Load().Wait());
            
            FullName = Preferences.Get(Config.FullName, "");
            OnPropertyChanged(nameof(FullName));
            //lắng nghe các trạng thái duyệt lcp fob, giacong dat mua
            MessagingCenter.Subscribe<DuyetLenhCapPhatGiaCong_ViewModel, DocumentType>(this, "langngheduyet", (obj, item) =>
            {
                NofiLCP_GC--;
                OnPropertyChanged(nameof(NofiLCP_GC));
            });
            MessagingCenter.Subscribe<DuyetLenhCapPhatFOBViewModel, DocumentType>(this, "langngheduyet", (obj, item) =>
            {
                NofiLCP_FOB--;
                OnPropertyChanged(nameof(NofiLCP_FOB));
            });
            MessagingCenter.Subscribe<DuyetChungTuDatMua_ViewModel, DocumentType>(this, "langngheduyet", (obj, item) =>
            {
                NofiDuyetDatMua--;
                OnPropertyChanged(nameof(NofiDuyetDatMua));
            });
            MessagingCenter.Subscribe<DuyetDeNghiThanhToan_ViewModel, DocumentType>(this, "langngheduyet", (obj, item) =>
            {
                NofiDeNghiTT--;
                OnPropertyChanged(nameof(NofiDeNghiTT));
            });
            MessagingCenter.Subscribe<DuyetDatMuaPhuTung_ViewModel, DocumentType>(this, "langngheduyet", (obj, item) =>
            {
                NofiDuyetDatMuaPhuTung--;
                OnPropertyChanged(nameof(NofiDuyetDatMuaPhuTung));
            });
            MessagingCenter.Subscribe<DuyetKiDienTuPhuTung_ViewModel, DocumentType>(this, "langngheduyet", (obj, item) =>
            {                
                NofiKidienTuPhuTung--;
                OnPropertyChanged(nameof(NofiKidienTuPhuTung));
            });
            MessagingCenter.Subscribe<DuyetKiDienTuThietBi_ViewModel, DocumentType>(this, "langngheduyet", (obj, item) =>
            {
                NofiKidienTuThietBi--;
                OnPropertyChanged(nameof(NofiKidienTuThietBi));
            });
            MessagingCenter.Subscribe<DuyetTraThietBi_ViewModel, DocumentType>(this, "langngheduyet", (obj, item) =>
            {
                NofiTraThietBi--;
                OnPropertyChanged(nameof(NofiTraThietBi));
            });
            MessagingCenter.Subscribe<DuyetYeuCauThueThietBi_ViewModel, DocumentType>(this, "langngheduyet", (obj, item) =>
            {
                NofiYeuCauThueThietBi--;
                OnPropertyChanged(nameof(NofiYeuCauThueThietBi));
            });            
        }
        
        async Task Load()
        {
            //load danh sách duyêt đặt mua 
            try
            {
                await Device.InvokeOnMainThreadAsync(async () =>
                {
                    try
                    {
                        var res = await RunHttpClientGet<object>("api/qltb/getUser?username=" + Preferences.Get(Config.User, "") + "&password=" + Preferences.Get(Config.Password, ""));
                        var _json = res.Status.Content.ReadAsStringAsync().Result;
                        _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                        if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                        {
                            Int32 from = _json.IndexOf("[");
                            Int32 to = _json.IndexOf("]");
                            _json = _json.Substring(from, to - from + 1);
                            dynamic body = JsonConvert.DeserializeObject(_json);
                            IsMainThietBi = Convert.ToBoolean(ISDBNULL(body[0].THIET_BI_CONG_NGHE.Value, false));
                            OnPropertyChanged(nameof(IsMainThietBi));
                            IsMainDonHang = Convert.ToBoolean(ISDBNULL(body[0].KY_DIEN_TU.Value, false));
                            OnPropertyChanged(nameof(IsMainDonHang));
                            IsDuyetDonDatMua = Convert.ToBoolean(ISDBNULL(body[0].DON_DAT_MUA.Value, false));
                            OnPropertyChanged(nameof(IsDuyetDonDatMua));
                            if (IsDuyetDonDatMua)
                            {
                                System.Diagnostics.Debug.WriteLine("đang chay task 1");
                                string url = $"api/DuyetChungTu/getDonDatMua?username={Preferences.Get(Config.User, "")}";
                                var a = await RunHttpClientGet<object>(url);
                                NofiDuyetDatMua = a.Lists.Count;
                                OnPropertyChanged(nameof(NofiDuyetDatMua));
                                System.Diagnostics.Debug.WriteLine("chay xong task 1");
                            } 
                           
                            IsDanhMucThietBi = Convert.ToBoolean(ISDBNULL(body[0].DANH_MUC_THIET_BI.Value, false));
                            OnPropertyChanged(nameof(IsDanhMucThietBi));
                            if (IsDanhMucThietBi)
                            {
                                System.Diagnostics.Debug.WriteLine("đang chay task 2");
                                string url = "api/qltb/getThietBi?username=" + Preferences.Get(Config.User, "");
                                var g = await RunHttpClientGet<object>(url);
                                NofiDanhMucThietBi = g.Lists.Count();
                                OnPropertyChanged(nameof(NofiDanhMucThietBi));
                                System.Diagnostics.Debug.WriteLine("chay xong task 2");
                            } 
                            
                            IsLichXichBaoTri = Convert.ToBoolean(ISDBNULL(body[0].LICH_XICH_BAO_TRI.Value, false));
                            OnPropertyChanged(nameof(IsLichXichBaoTri));
                            if (IsLichXichBaoTri)
                            {
                                System.Diagnostics.Debug.WriteLine(" đang chay task 3");
                                string  url = "api/qltb/getKeHoachBaoTri?user=" + Preferences.Get(Config.User, "") + "&nam=" + DateTime.Now.Year.ToString();
                                var f = await RunHttpClientGet<object>(url);
                                NofiLichXich = f.Lists.Count();
                                OnPropertyChanged(nameof(NofiLichXich));                                
                                System.Diagnostics.Debug.WriteLine("chay xong task 3");
                            }
                            
                            IsDuyetDatPhuTung = Convert.ToBoolean(ISDBNULL(body[0].DUYET_DAT_HANG_PHU_TUNG.Value, false));
                            OnPropertyChanged(nameof(IsDuyetDatPhuTung));
                            if (IsDuyetDatPhuTung)
                            {
                                System.Diagnostics.Debug.WriteLine(" đang chay task 4");                                
                                string url = $"api/DuyetChungTu/getDatMuaPhuTung?username={Preferences.Get(Config.User, "")}";
                                var d = await RunHttpClientGet<object>(url);
                                NofiDuyetDatMuaPhuTung = d.Lists.Count();
                                OnPropertyChanged(nameof(NofiDuyetDatMuaPhuTung));                               
                                System.Diagnostics.Debug.WriteLine("chay xong task 4");
                            } 
                            
                            IsDuyetLCPFOB = Convert.ToBoolean(ISDBNULL(body[0].LCP_FOB.Value, false));
                            OnPropertyChanged(nameof(IsDuyetLCPFOB));
                            if (IsDuyetLCPFOB)
                            {
                                System.Diagnostics.Debug.WriteLine(" đang chay task 5");                               
                                string url  = $"api/DuyetChungTu/getLenhCapPhat?username={Preferences.Get(Config.User, "")}";
                                var b = await RunHttpClientGet<object>(url);
                                NofiLCP_FOB = b.Lists.Count();
                                OnPropertyChanged(nameof(NofiLCP_FOB));                                
                                System.Diagnostics.Debug.WriteLine("chay xong task 5");
                            }    
                            
                            IsDuyetLCPGC = Convert.ToBoolean(ISDBNULL(body[0].LCP_GIA_CONG.Value, false));
                            OnPropertyChanged(nameof(IsDuyetLCPGC));
                            if (IsDuyetLCPGC)
                            {
                                System.Diagnostics.Debug.WriteLine(" đang chay task 6");                                
                                string url  = $"api/DuyetChungTu/getLenhCapPhat_GC?username={Preferences.Get(Config.User, "")}";
                                var c = await RunHttpClientGet<object>(url);
                                NofiLCP_GC = c.Lists.Count();
                                OnPropertyChanged(nameof(NofiLCP_GC));                                
                                System.Diagnostics.Debug.WriteLine("chay xong task 6");
                            }

                            IsDNTT = Convert.ToBoolean(ISDBNULL(body[0].DNTT.Value, false));
                            OnPropertyChanged(nameof(IsDNTT));
                            if (IsDNTT)
                            {
                                System.Diagnostics.Debug.WriteLine(" đang chay task 7");                                
                                string url = $"api/DuyetChungTu/getDeNghiThanhToan?username={Preferences.Get(Config.User, "")}";
                                var e = await RunHttpClientGet<object>(url);
                                NofiDeNghiTT = e.Lists.Count();
                                OnPropertyChanged(nameof(NofiDeNghiTT));                                
                                System.Diagnostics.Debug.WriteLine("chay xong task 7");
                            }

                            IsKiDienTuPhuTung = Convert.ToBoolean(ISDBNULL(body[0].KY_DIEN_TU_XUAT_PHU_TUNG.Value, false));
                            OnPropertyChanged(nameof(IsKiDienTuPhuTung));
                            if (IsKiDienTuPhuTung )
                            {
                                System.Diagnostics.Debug.WriteLine(" đang chay task 8");                                
                                string url = $"api/DuyetChungTu/getKiDienTuPhuTung?nhamay={Preferences.Get(Config.NhaMay, "")}";
                                var h = await RunHttpClientGet<object>(url);
                                NofiKidienTuPhuTung = h.Lists.Count();
                                OnPropertyChanged(nameof(NofiKidienTuPhuTung));                               
                                System.Diagnostics.Debug.WriteLine("chay xong task 8");
                            }    

                            IsKiDienTuThietBi = Convert.ToBoolean(ISDBNULL(body[0].KY_DIEN_TU_XUAT_THIET_BI.Value, false));
                            OnPropertyChanged(nameof(IsKiDienTuThietBi));
                            if (IsKiDienTuThietBi )
                            {
                                System.Diagnostics.Debug.WriteLine(" đang chay task 9");                                
                                string url = $"api/DuyetChungTu/getKiDienTuThietBi?nhamay={Preferences.Get(Config.NhaMay, "")}";
                               var h = await RunHttpClientGet<object>(url);
                                NofiKidienTuThietBi = h.Lists.Count();
                                OnPropertyChanged(nameof(NofiKidienTuThietBi));                                
                                System.Diagnostics.Debug.WriteLine("chay xong task 9");
                            }    

                            IsDuyetYeuCauThueThietBi = Convert.ToBoolean(ISDBNULL(body[0].DUYET_YEU_CAU_THUE_THIET_BI.Value, false));
                            OnPropertyChanged(nameof(IsDuyetYeuCauThueThietBi));
                            if (IsDuyetYeuCauThueThietBi)
                            {
                                System.Diagnostics.Debug.WriteLine(" đang chay task 10");                               
                                string url = $"api/DuyetChungTu/getYeuCauThueThietBi?username={Preferences.Get(Config.User, "")}";
                                var h = await RunHttpClientGet<object>(url);
                                NofiYeuCauThueThietBi = h.Lists.Count();
                                OnPropertyChanged(nameof(NofiYeuCauThueThietBi));                                
                                System.Diagnostics.Debug.WriteLine("chay xong task 10");
                            }    

                            IsDuyetPhieuTraThietBi = Convert.ToBoolean(ISDBNULL(body[0].DUYET_PHIEU_TRA_THIET_BI.Value, false));
                            OnPropertyChanged(nameof(IsDuyetPhieuTraThietBi));
                            if(IsDuyetPhieuTraThietBi)
                            {
                                System.Diagnostics.Debug.WriteLine(" đang chay task 11");                                
                                string url = $"api/DuyetChungTu/geDuyetTraThietBi?username={Preferences.Get(Config.User, "")}";
                               var h = await RunHttpClientGet<object>(url);
                                NofiTraThietBi = h.Lists.Count();
                                OnPropertyChanged(nameof(NofiTraThietBi));                                
                                System.Diagnostics.Debug.WriteLine("chay xong task 11");
                            }    

                            IsKiemKeThietBi = Convert.ToBoolean(ISDBNULL(body[0].KIEM_KE_THIET_BI.Value, false));
                            OnPropertyChanged(nameof(IsKiemKeThietBi));
                            
                        }                        
                        
                        HideLoading();
                        await NewVersion();
                    }
                    catch (Exception ex)
                    {
                        HideLoading();
                    }

                }).ConfigureAwait(false);
            }
            catch (Exception)
            {
            }
        }

        async Task NewVersion()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                var version = Config.client.GetStringAsync("https://play.google.com/store/apps/details?id=com.companyname.app_hoathocorp").Result;
                if (version.Contains(AppInfo.VersionString) == false)
                {
                    var update = await new MessageYesNo("Có phiên bản mới trên app store. Bạn có muốn cập nhật không").Show();
                    if (update == DialogReturn.OK)
                    {
                        await CrossLatestVersion.Current.OpenAppInStore();
                    }
                }
            }
            else
            {
                bool isLatest = await CrossLatestVersion.Current.IsUsingLatestVersion();
                if (!isLatest)
                {
                    var update = await new MessageYesNo("Có phiên bản mới trên app store. Bạn có muốn cập nhật không").Show();
                    if (update == DialogReturn.OK)
                    {
                        await CrossLatestVersion.Current.OpenAppInStore();
                    }
                }
            }
        }
    }
}
