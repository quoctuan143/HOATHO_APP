using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using APP_HOATHO.ViewModels.DuyetChungTu;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using APP_HOATHO.Models.DuyetChungTu;
using APP_HOATHO.Dialog;
using APP_HOATHO.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plugin.FirebasePushNotification;
using APP_HOATHO.Views.DuyetChungTu;
using APP_HOATHO.Models.KiDienTuPhuTung;
using APP_HOATHO.Views.KiDienTu;
using Plugin.LatestVersion;
using APP_HOATHO.Views.ThietBi;
using APP_HOATHO.Views.Kiem_Ke_Thiet_Bi;
using System.IO;

namespace APP_HOATHO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main :  ContentPage, INotifyPropertyChanged 
    {
        public string FullName { get; set; }
        public bool IsDanhMucThietBi { get; set; }
        public bool IsLichXichBaoTri { get; set; }
        public bool IsDuyetDatPhuTung { get; set; }
        public bool isDuyetDonDatMua { get; set; }
        public bool IsDuyetLCPFOB { get; set; }
        public bool IsDuyetLCPGC { get; set; }
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
        public int NofiTraThietBi { get; set; }
        ViewModels.BaseViewModel BaseViewModel { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Main()
        {
            DependencyService.Get<IProcessLoader>().Show("Đang tải dữ liệu....");
            InitializeComponent();
            BaseViewModel = new ViewModels.BaseViewModel();           
            Task.Factory.StartNew(() => Load().Wait());
            DependencyService.Get<ISetStatusBarColor>().SetColoredStatusBar("#06264c");            
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
                NofiKidienTuPhuTung --;
                OnPropertyChanged(nameof(NofiKidienTuPhuTung));
            });
            MessagingCenter.Subscribe<DuyetKiDienTuThietBi_ViewModel, DocumentType>(this, "langngheduyet", (obj, item) =>
            {
                NofiKidienTuThietBi--;
                OnPropertyChanged(nameof(NofiKidienTuThietBi));
            });
            MessagingCenter.Subscribe<DuyetTraThietBi_ViewModel , DocumentType>(this, "langngheduyet", (obj, item) =>
            {
                NofiTraThietBi --;
                OnPropertyChanged(nameof(NofiTraThietBi));
            });
            MessagingCenter.Subscribe<DuyetYeuCauThueThietBi_ViewModel, DocumentType>(this, "langngheduyet", (obj, item) =>
            {
                NofiYeuCauThueThietBi--;
                OnPropertyChanged(nameof(NofiYeuCauThueThietBi));
            });
            //load dữ liệu lên
            System.Diagnostics.Debug.WriteLine("start load dữ liệu");

            
            BindingContext = this;
            System.Diagnostics.Debug.WriteLine("finish load dữ liệu");
            CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;
            CrossFirebasePushNotification.Current.OnNotificationOpened += Current_OnNotificationOpened;
            Task.Run(() => NewVersion()) ;
           
        }
        
        private async void Current_OnNotificationOpened(object source, FirebasePushNotificationResponseEventArgs p)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                DocumentType loaiphieu = DocumentType.DuyetDatMuaPhuTung;
                try
                {
                    if (p.Data["sochungtu"].ToString() != "")
                    {
                        DuyetChungTuModel item = new DuyetChungTuModel { No_ = p.Data["sochungtu"].ToString() };
                        if (p.Data["loaiphieu"].ToString() == "lcpfob")
                        {
                            loaiphieu = DocumentType.DuyetLCP;
                        }
                        if (p.Data["loaiphieu"].ToString() == "lcpgc")
                        {
                            loaiphieu = DocumentType.DuyetLCP_GC;
                        }
                        if (p.Data["loaiphieu"].ToString() == "duyetdatphutung")
                        {
                            loaiphieu = DocumentType.DuyetDatMuaPhuTung;
                        }
                        if (p.Data["loaiphieu"].ToString() == "dondatmua")
                        {
                            loaiphieu = DocumentType.DuyetMuaHang;
                        }
                        if (p.Data["loaiphieu"].ToString() == "denghithanhtoan")
                        {
                            loaiphieu = DocumentType.DuyetThanhToan;
                        }
                        if (p.Data["loaiphieu"].ToString() == "kidientuphutung")
                        {
                            loaiphieu = DocumentType.KiDienTuPhuTung;
                        }
                        if (p.Data["loaiphieu"].ToString() == "kidientuthietbi")
                        {
                            loaiphieu = DocumentType.KiDienTuThietBi;
                        }
                        if (p.Data["loaiphieu"].ToString() == "duyettrathietbi")
                        {
                            loaiphieu = DocumentType.DuyetTraThietBi ;
                        }
                        if (p.Data["loaiphieu"].ToString() == "duyetyeucauthuethietbi")
                        {
                            loaiphieu = DocumentType.DuyetYeuCauThueThietBi ;
                        }
                        if (loaiphieu == DocumentType.DuyetDatMuaPhuTung)
                            await Navigation.PushAsync(new DuyetChungTuPhuTung_Line(item, loaiphieu));
                        else if (loaiphieu == DocumentType.KiDienTuPhuTung)
                            await Navigation.PushAsync(new DuyetKiDienTuPhuTungLine(new Models.KiDienTuPhuTung.DuyetKiDienTuPhuTungModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else if (loaiphieu == DocumentType.DuyetYeuCauThueThietBi)
                            await Navigation.PushAsync(new DuyetYeuCauThueThietBiPage_Line(new  DuyetChungTuModel  { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else if (loaiphieu == DocumentType.KiDienTuThietBi)
                            await Navigation.PushAsync(new DuyetKiDienTuThietBiLine(new Models.KiDienTuPhuTung.DuyetKiDienTuPhuTungModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else if (loaiphieu == DocumentType.DuyetTraThietBi)
                            await Navigation.PushAsync(new DuyetTraThietBiLine_Page(new Models.KiDienTuPhuTung.DuyetKiDienTuPhuTungModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else
                            await Navigation.PushAsync(new DuyetChungTu_Line(item, loaiphieu));
                    }

                }
                catch
                {
                }
            });
        }

        private async void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs p)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                DocumentType loaiphieu = DocumentType.DuyetDatMuaPhuTung;
                try
                {
                    if (p.Data["sochungtu"].ToString() != "")
                    {
                        DuyetChungTuModel item = new DuyetChungTuModel { No_ = p.Data["sochungtu"].ToString() };
                        if (p.Data["loaiphieu"].ToString() == "lcpfob")
                        {
                            loaiphieu = DocumentType.DuyetLCP;
                        }
                        if (p.Data["loaiphieu"].ToString() == "lcpgc")
                        {
                            loaiphieu = DocumentType.DuyetLCP_GC;
                        }
                        if (p.Data["loaiphieu"].ToString() == "duyetdatphutung")
                        {
                            loaiphieu = DocumentType.DuyetDatMuaPhuTung;
                        }
                        if (p.Data["loaiphieu"].ToString() == "dondatmua")
                        {
                            loaiphieu = DocumentType.DuyetMuaHang;
                        }
                        if (p.Data["loaiphieu"].ToString() == "denghithanhtoan")
                        {
                            loaiphieu = DocumentType.DuyetThanhToan;
                        }
                        if (p.Data["loaiphieu"].ToString() == "kidientuphutung")
                        {
                            loaiphieu = DocumentType.KiDienTuPhuTung ;
                        }
                        if (p.Data["loaiphieu"].ToString() == "kidientuthietbi")
                        {
                            loaiphieu = DocumentType.KiDienTuThietBi;
                        }
                        if (p.Data["loaiphieu"].ToString() == "duyettrathietbi")
                        {
                            loaiphieu = DocumentType.DuyetTraThietBi;
                        }
                        if (p.Data["loaiphieu"].ToString() == "duyetyeucauthuethietbi")
                        {
                            loaiphieu = DocumentType.DuyetYeuCauThueThietBi;
                        }
                        if (loaiphieu == DocumentType.DuyetDatMuaPhuTung)
                            await Navigation.PushAsync(new DuyetChungTuPhuTung_Line(item, loaiphieu));
                        else if (loaiphieu == DocumentType.KiDienTuPhuTung)
                            await Navigation.PushAsync(new DuyetKiDienTuPhuTungLine(new Models.KiDienTuPhuTung.DuyetKiDienTuPhuTungModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else if (loaiphieu == DocumentType.DuyetYeuCauThueThietBi)
                            await Navigation.PushAsync(new DuyetYeuCauThueThietBiPage_Line(new DuyetChungTuModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else if (loaiphieu == DocumentType.KiDienTuThietBi)
                            await Navigation.PushAsync(new DuyetKiDienTuThietBiLine(new Models.KiDienTuPhuTung.DuyetKiDienTuPhuTungModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else if (loaiphieu == DocumentType.DuyetTraThietBi)
                            await Navigation.PushAsync(new DuyetTraThietBiLine_Page(new Models.KiDienTuPhuTung.DuyetKiDienTuPhuTungModel { No_ = p.Data["sochungtu"].ToString() }, loaiphieu));
                        else
                            await Navigation.PushAsync(new DuyetChungTu_Line(item, loaiphieu));
                    }

                }
                catch
                {
                }
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
                        var res = await BaseViewModel.RunHttpClientGet<object>("api/qltb/getUser?username=" + Preferences.Get(Config.User, "") + "&password=" + Preferences.Get(Config.Password, ""));
                        dynamic body = res.Lists;
                        isDuyetDonDatMua = Convert.ToBoolean(BaseViewModel.ISDBNULL(body[0].DON_DAT_MUA.Value, false));
                        OnPropertyChanged(nameof(isDuyetDonDatMua));
                        IsDanhMucThietBi = Convert.ToBoolean(BaseViewModel.ISDBNULL(body[0].DANH_MUC_THIET_BI.Value, false));
                        OnPropertyChanged(nameof(IsDanhMucThietBi));
                        IsLichXichBaoTri = Convert.ToBoolean(BaseViewModel.ISDBNULL(body[0].LICH_XICH_BAO_TRI.Value, false));
                        OnPropertyChanged(nameof(IsLichXichBaoTri));
                        IsDuyetDatPhuTung = Convert.ToBoolean(BaseViewModel.ISDBNULL(body[0].DUYET_DAT_HANG_PHU_TUNG.Value, false));
                        OnPropertyChanged(nameof(IsDuyetDatPhuTung));
                        IsDuyetLCPFOB = Convert.ToBoolean(BaseViewModel.ISDBNULL(body[0].LCP_FOB.Value, false));
                        OnPropertyChanged(nameof(IsDuyetLCPFOB));
                        IsDuyetLCPGC = Convert.ToBoolean(BaseViewModel.ISDBNULL(body[0].LCP_GIA_CONG.Value, false));
                        OnPropertyChanged(nameof(IsDuyetLCPGC));
                        IsMainThietBi = Convert.ToBoolean(BaseViewModel.ISDBNULL(body[0].THIET_BI_CONG_NGHE.Value, false));
                        OnPropertyChanged(nameof(IsMainThietBi));
                        IsMainDonHang = Convert.ToBoolean(BaseViewModel.ISDBNULL(body[0].KY_DIEN_TU.Value, false));
                        OnPropertyChanged(nameof(IsMainDonHang));
                        IsKiDienTuPhuTung = Convert.ToBoolean(BaseViewModel.ISDBNULL(body[0].KY_DIEN_TU_XUAT_PHU_TUNG.Value, false));
                        OnPropertyChanged(nameof(IsKiDienTuPhuTung));
                        IsKiDienTuThietBi = Convert.ToBoolean(BaseViewModel.ISDBNULL(body[0].KY_DIEN_TU_XUAT_THIET_BI.Value, false));
                        OnPropertyChanged(nameof(IsKiDienTuThietBi));
                        IsDuyetYeuCauThueThietBi = Convert.ToBoolean(BaseViewModel.ISDBNULL(body[0].DUYET_YEU_CAU_THUE_THIET_BI.Value, false));
                        OnPropertyChanged(nameof(IsDuyetYeuCauThueThietBi));
                        IsDuyetPhieuTraThietBi = Convert.ToBoolean(BaseViewModel.ISDBNULL(body[0].DUYET_PHIEU_TRA_THIET_BI.Value, false));
                        OnPropertyChanged(nameof(IsDuyetPhieuTraThietBi));
                        IsKiemKeThietBi = Convert.ToBoolean(BaseViewModel.ISDBNULL(body[0].KIEM_KE_THIET_BI.Value, false));
                        OnPropertyChanged(nameof(IsKiemKeThietBi));
                        System.Diagnostics.Debug.WriteLine("đang chay task LoadDuyeDonHang");
                        string url = $"api/DuyetChungTu/getDonDatMua?username={Preferences.Get(Config.User, "")}";
                        var a = await BaseViewModel.RunHttpClientGet<object>(url);
                        NofiDuyetDatMua = a.Lists.Count;
                        OnPropertyChanged(nameof(NofiDuyetDatMua));

                        System.Diagnostics.Debug.WriteLine("đang chay task LoadLCP_FOB");
                        url = $"api/DuyetChungTu/getLenhCapPhat?username={Preferences.Get(Config.User, "")}";
                        var b = await BaseViewModel.RunHttpClientGet<object>(url);
                        NofiLCP_FOB = b.Lists.Count();
                        OnPropertyChanged(nameof(NofiLCP_FOB));

                        System.Diagnostics.Debug.WriteLine("đang chay task LoadLCP_GC");
                        url = $"api/DuyetChungTu/getLenhCapPhat_GC?username={Preferences.Get(Config.User, "")}";
                        var c = await BaseViewModel.RunHttpClientGet<object>(url);
                        NofiLCP_GC = c.Lists.Count();
                        OnPropertyChanged(nameof(NofiLCP_GC));

                        System.Diagnostics.Debug.WriteLine("đang chay task đặt mua phụ tùng");
                        url = $"api/DuyetChungTu/getDatMuaPhuTung?username={Preferences.Get(Config.User, "")}";
                        var d = await BaseViewModel.RunHttpClientGet<object>(url);
                        NofiDuyetDatMuaPhuTung = d.Lists.Count();
                        OnPropertyChanged(nameof(NofiDuyetDatMuaPhuTung));

                        System.Diagnostics.Debug.WriteLine("đang chay task đề nghi thanh toán");
                        url = $"api/DuyetChungTu/getDeNghiThanhToan?username={Preferences.Get(Config.User, "")}";
                        var e = await BaseViewModel.RunHttpClientGet<object>(url);
                        NofiDeNghiTT = e.Lists.Count();
                        OnPropertyChanged(nameof(NofiDeNghiTT));

                        System.Diagnostics.Debug.WriteLine("đang chay task  lấy lịch xich bao trì");
                        url = "api/qltb/getKeHoachBaoTri?user=" + Preferences.Get(Config.User, "") + "&nam=" + DateTime.Now.Year.ToString();
                        var f = await BaseViewModel.RunHttpClientGet<object>(url);
                        NofiLichXich = f.Lists.Count();
                        OnPropertyChanged(nameof(NofiLichXich));

                        System.Diagnostics.Debug.WriteLine("đang chay task đặt lấy danh  mục thiết bi");
                        url = "api/qltb/getThietBi?nhamay=" + Preferences.Get(Config.NhaMay, "");
                        var g = await BaseViewModel.RunHttpClientGet<object>(url);
                        NofiDanhMucThietBi = g.Lists.Count();
                        OnPropertyChanged(nameof(NofiDanhMucThietBi));

                        System.Diagnostics.Debug.WriteLine("đang chay task đặt lấy danh sách kí điện tử  phụ tùng");
                        url = $"api/DuyetChungTu/getKiDienTuPhuTung?nhamay={Preferences.Get(Config.NhaMay, "")}";
                        var h = await BaseViewModel.RunHttpClientGet<object>(url);
                        NofiKidienTuPhuTung = h.Lists.Count();
                        OnPropertyChanged(nameof(NofiKidienTuPhuTung));

                        System.Diagnostics.Debug.WriteLine("đang chay task đặt lấy danh sách kí điện tử  thietbi");
                        url = $"api/DuyetChungTu/getKiDienTuThietBi?nhamay={Preferences.Get(Config.NhaMay, "")}";
                        h = await BaseViewModel.RunHttpClientGet<object>(url);
                        NofiKidienTuThietBi = h.Lists.Count();
                        OnPropertyChanged(nameof(NofiKidienTuThietBi));

                        System.Diagnostics.Debug.WriteLine("đang chay task đặt lấy danh sách yêu cầu thuê thiết bị");
                        url = $"api/DuyetChungTu/getYeuCauThueThietBi?username={Preferences.Get(Config.User, "")}";
                        h = await BaseViewModel.RunHttpClientGet<object>(url);
                        NofiYeuCauThueThietBi = h.Lists.Count();
                        OnPropertyChanged(nameof(NofiYeuCauThueThietBi));

                        System.Diagnostics.Debug.WriteLine("đang chay task lấy danh sách trả thiết bị");
                        url = $"api/DuyetChungTu/geDuyetTraThietBi?username={Preferences.Get(Config.User, "")}";
                        h = await BaseViewModel.RunHttpClientGet<object>(url);
                        NofiTraThietBi = h.Lists.Count();
                        OnPropertyChanged(nameof(NofiTraThietBi));
                        BaseViewModel.HideLoading();
                    }
                    catch (Exception ex)
                    {
                        BaseViewModel.HideLoading();
                    }

                }).ConfigureAwait(false);
            }
            catch (Exception)
            {                
            }
        }

        #region "Button click"
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Danh_Muc_Thiet_Bi());
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new KeHoachBaoTri());
        }

        DuyetChungTu.DuyetChungTuPhuTung_Header duyetChungTuPhuTung_Header;
        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            if (duyetChungTuPhuTung_Header == null)
            {
                duyetChungTuPhuTung_Header = new DuyetChungTu.DuyetChungTuPhuTung_Header(DocumentType.DuyetDatMuaPhuTung);
            }
            await duyetChungTuPhuTung_Header.LoadData();
            await Navigation.PushAsync(duyetChungTuPhuTung_Header);

        }

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Login();
        }

        Tabpage.DuyetDonDatMua DuyetDonDatMua;
        private async void duyetdonhang_Tapped(object sender, EventArgs e)
        {
            if (DuyetDonDatMua == null)
            {
                DuyetDonDatMua = new Tabpage.DuyetDonDatMua();
            }
            await DuyetDonDatMua.LoadData();
            await Navigation.PushAsync(DuyetDonDatMua);

        }

        Tabpage.DuyetLCP_FOB duyetLCP_FOB;
        private async void duyetLCPFOB_Tapped(object sender, EventArgs e)
        {
            if (duyetLCP_FOB == null)
            {
                duyetLCP_FOB = new Tabpage.DuyetLCP_FOB();
            }
            await duyetLCP_FOB.LoadData();
            await Navigation.PushAsync(duyetLCP_FOB);
        }
        Tabpage.DuyetLCP_GIACONG DuyetLCP_GIACONG;
        private async void duyetLCP_GC_Tapped(object sender, EventArgs e)
        {
            if (DuyetLCP_GIACONG == null)
            {
                DuyetLCP_GIACONG = new Tabpage.DuyetLCP_GIACONG();
            }
            await DuyetLCP_GIACONG.LoadData();
            await Navigation.PushAsync(DuyetLCP_GIACONG);
        }
        Tabpage.DuyetDeNghiThanhToanTabpage DuyetDeNghiThanhToanTabpage;
        private async void DuyetDeNghiThanhToan_Tapped(object sender, EventArgs e)
        {
            if (DuyetDeNghiThanhToanTabpage == null)
            {
                DuyetDeNghiThanhToanTabpage = new Tabpage.DuyetDeNghiThanhToanTabpage();
            }
            await DuyetDeNghiThanhToanTabpage.LoadData();
            await Navigation.PushAsync(DuyetDeNghiThanhToanTabpage);
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            BackButtonPressed();
            return true;
        }
        public async Task BackButtonPressed()
        {
            var ok = await new MessageYesNo("Bạn có muốn thoát chương trình không?").Show();
            if (ok == DialogReturn.OK)
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }
        #endregion
        async void NewVersion()
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
        private void btnRefresh_Tapped(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => Load().Wait());
        }

        private async void btnKiDienTuXuatPT_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DuyetKiDienTuPhuTung_Page(DocumentType.KiDienTuPhuTung));
        }

        private async void btnKiDienTuXuatThietBi_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DuyetKiDienTuThietBi_Page(DocumentType.KiDienTuThietBi));
        }

        private async  void btnYeuCauThueThietBi_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DuyetYeuCauThueThietBiPage_Header(DocumentType.DuyetYeuCauThueThietBi));
        }

        private async void btnDuyetPhieuTraThietBi_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DuyetTraThietBi_Page(DocumentType.DuyetTraThietBi ));
        }

        private async void btnKiemKeThietBi_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Kiem_Ke_Thiet_Bi_Header_Page());
        }
    }
}