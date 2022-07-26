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

namespace APP_HOATHO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : ContentPage, INotifyPropertyChanged
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
            InitializeComponent();
            DependencyService.Get<ISetStatusBarColor>().SetColoredStatusBar("#06264c");            
           FullName = Preferences.Get(Config.FullName, "");
            OnPropertyChanged(nameof(FullName));

            if (Preferences.Get(Config.Role, "") == "0")
            {
                IsDanhMucThietBi = true;
                IsLichXichBaoTri = true;
                IsDuyetDatPhuTung = true;
                isDuyetDonDatMua = true;
                IsDuyetLCPFOB = true;
                IsDuyetLCPGC = true;
                IsMainThietBi = true;
                IsMainDonHang = true;
                IsKiDienTuPhuTung = true;
                IsKiDienTuThietBi = true;
                IsDuyetYeuCauThueThietBi = true;
                IsDuyetPhieuTraThietBi = true;
            }
            if (Preferences.Get(Config.Role, "") == "1")
            {                
                isDuyetDonDatMua = true;
                IsDuyetLCPFOB = true;
                IsDuyetLCPGC = true;                
                IsMainDonHang = true;                
            }
            if (Preferences.Get(Config.Role, "") == "2")
            {
                IsDanhMucThietBi = true;
                IsLichXichBaoTri = true;
                IsDuyetDatPhuTung = true; 
                IsMainThietBi = true;               
                IsDuyetYeuCauThueThietBi = true  ;
                IsDuyetPhieuTraThietBi = true  ;
            }    
            if (Preferences.Get(Config.Role, "") == "3")
                {
                    IsDanhMucThietBi = true;
                    IsLichXichBaoTri = true;                   
                    IsMainThietBi = true;
            }
            if (Preferences.Get(Config.Role, "") == "4")
            {               
                IsDuyetDatPhuTung = true;               
                IsMainThietBi = true;              
                IsKiDienTuPhuTung = true ;
                IsKiDienTuThietBi = true;
               
            }

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

            Task.Factory.StartNew(() => Load().Wait());
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

                        //await DependencyService.Get<IProcessLoader>().Show("Đang tải vui lòng đợi");
                        // await Task.Delay(1000);

                        //Action LoadDuyeDonHang = async() =>
                        // {
                        System.Diagnostics.Debug.WriteLine("đang chay task LoadDuyeDonHang");
                        string url = $"api/DuyetChungTu/getDonDatMua?username={Preferences.Get(Config.User, "")}";
                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Config.URL);
                            HttpResponseMessage respon = await client.GetAsync(url);
                            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string _json = await respon.Content.ReadAsStringAsync();
                                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                                if (_json.Contains("[]") == false)
                                {
                                    Int32 from = _json.IndexOf("[");
                                    Int32 to = _json.IndexOf("]");
                                    string result = _json.Substring(from, to - from + 1);
                                    NofiDuyetDatMua = JsonConvert.DeserializeObject<ObservableCollection<DuyetChungTuModel>>(result).Count;
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        OnPropertyChanged(nameof(NofiDuyetDatMua));
                                    });
                                }
                            }
                        }
                        // };
                        //Task task1 = new Task(LoadDuyeDonHang);
                        //task1.Start();

                        //duyệt LCP FOB
                        //Action LoadLCP_FOB = async () =>
                        //{
                        System.Diagnostics.Debug.WriteLine("đang chay task LoadLCP_FOB");
                         url = $"api/DuyetChungTu/getLenhCapPhat?username={Preferences.Get(Config.User, "")}";
                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Config.URL);
                            HttpResponseMessage respon = await client.GetAsync(url);
                            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string _json = await respon.Content.ReadAsStringAsync();
                                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                                if (_json.Contains("[]") == false)
                                {     
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Int32 from = _json.IndexOf("[");
                                        Int32 to = _json.IndexOf("]");
                                        string result = _json.Substring(from, to - from + 1);
                                        NofiLCP_FOB = JsonConvert.DeserializeObject<ObservableCollection<DuyetChungTuModel>>(result).Count;
                                        OnPropertyChanged(nameof(NofiLCP_FOB));
                                    });
                                }
                            }
                        }
                       
                        System.Diagnostics.Debug.WriteLine("đang chay task LoadLCP_GC");
                        url = $"api/DuyetChungTu/getLenhCapPhat_GC?username={Preferences.Get(Config.User, "")}";
                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Config.URL);
                            HttpResponseMessage respon = await client.GetAsync(url);
                            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string _json = await respon.Content.ReadAsStringAsync();
                                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                                if (_json.Contains("[]") == false)
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Int32 from = _json.IndexOf("[");
                                        Int32 to = _json.IndexOf("]");
                                        string result = _json.Substring(from, to - from + 1);
                                        NofiLCP_GC = JsonConvert.DeserializeObject<ObservableCollection<DuyetChungTuModel>>(result).Count;
                                        OnPropertyChanged(nameof(NofiLCP_GC));
                                    });
                                }

                            }
                        }
                        
                        System.Diagnostics.Debug.WriteLine("đang chay task đặt mua phụ tùng");
                        url = $"api/DuyetChungTu/getDatMuaPhuTung?username={Preferences.Get(Config.User, "")}";
                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Config.URL);
                            HttpResponseMessage respon = await client.GetAsync(url);
                            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string _json = await respon.Content.ReadAsStringAsync();
                                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                                if (_json.Contains("[]") == false)
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Int32 from = _json.IndexOf("[");
                                        Int32 to = _json.IndexOf("]");
                                        string result = _json.Substring(from, to - from + 1);
                                        NofiDuyetDatMuaPhuTung = JsonConvert.DeserializeObject<ObservableCollection<DuyetChungTuModel>>(result).Count;
                                        OnPropertyChanged(nameof(NofiDuyetDatMuaPhuTung));
                                    });
                                }
                            }
                        }

                        System.Diagnostics.Debug.WriteLine("đang chay task đề nghi thanh toán");
                        url = $"api/DuyetChungTu/getDeNghiThanhToan?username={Preferences.Get(Config.User, "")}";
                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Config.URL);
                            HttpResponseMessage respon = await client.GetAsync(url);
                            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string _json = await respon.Content.ReadAsStringAsync();
                                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                                if (_json.Contains("[]") == false)
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Int32 from = _json.IndexOf("[");
                                        Int32 to = _json.IndexOf("]");
                                        string result = _json.Substring(from, to - from + 1);
                                        NofiDeNghiTT = JsonConvert.DeserializeObject<ObservableCollection<DeNghiThanhToanHeader_Model>>(result).Count;
                                        OnPropertyChanged(nameof(NofiDeNghiTT));
                                    });
                                }
                            }
                        }
                       
                        System.Diagnostics.Debug.WriteLine("đang chay task  lấy lịch xich bao trì");

                        url = "api/qltb/getKeHoachBaoTri?user=" + Preferences.Get(Config.User, "") + "&nam=" + DateTime.Now.Year.ToString();

                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Config.URL);
                            HttpResponseMessage respon = await client.GetAsync(url);
                            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string _json = await respon.Content.ReadAsStringAsync();
                                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                                if (_json.Contains("[]") == false)
                                {     
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Int32 from = _json.IndexOf("[");
                                        Int32 to = _json.IndexOf("]");
                                        string result = _json.Substring(from, to - from + 1);
                                        NofiLichXich = JsonConvert.DeserializeObject<ObservableCollection<Models.KeHoachBaoTri>>(result).Where(p => p.Thang == DateTime.Now.Month && p.Da_Bao_Tri == false).ToList().Count;
                                        OnPropertyChanged(nameof(NofiLichXich));
                                    });
                                }
                            }
                        }

                        
                        System.Diagnostics.Debug.WriteLine("đang chay task đặt lấy danh  mục thiết bi");
                        url = "api/qltb/getThietBi?nhamay=" + Preferences.Get(Config.NhaMay, "");

                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Config.URL);
                            HttpResponseMessage respon = await client.GetAsync(url);
                            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string _json = await respon.Content.ReadAsStringAsync();
                                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                                if (_json.Contains("[]") == false)
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Int32 from = _json.IndexOf("[");
                                        Int32 to = _json.IndexOf("]");
                                        string result = _json.Substring(from, to - from + 1);
                                        NofiDanhMucThietBi = JsonConvert.DeserializeObject<ObservableCollection<DanhMuc_ThietBi>>(result).Count;
                                        OnPropertyChanged(nameof(NofiDanhMucThietBi));
                                    });
                                }
                            }
                        }

                        //await DependencyService.Get<IProcessLoader>().Hide();
                        System.Diagnostics.Debug.WriteLine("đang chay task đặt lấy danh sách kí điện tử  phụ tùng");
                        url = $"api/DuyetChungTu/getKiDienTuPhuTung?nhamay={Preferences.Get(Config.NhaMay, "")}";

                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Config.URL);
                            HttpResponseMessage respon = await client.GetAsync(url);
                            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string _json = await respon.Content.ReadAsStringAsync();
                                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                                if (_json.Contains("[]") == false)
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Int32 from = _json.IndexOf("[");
                                        Int32 to = _json.IndexOf("]");
                                        string result = _json.Substring(from, to - from + 1);
                                        NofiKidienTuPhuTung = JsonConvert.DeserializeObject<ObservableCollection<DuyetKiDienTuPhuTungModel>>(result).Count;
                                        OnPropertyChanged(nameof(NofiKidienTuPhuTung));
                                    });
                                }
                            }
                        }

                        //await DependencyService.Get<IProcessLoader>().Hide();
                        System.Diagnostics.Debug.WriteLine("đang chay task đặt lấy danh sách kí điện tử  thietbi");
                        url = $"api/DuyetChungTu/getKiDienTuThietBi?nhamay={Preferences.Get(Config.NhaMay, "")}";

                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Config.URL);
                            HttpResponseMessage respon = await client.GetAsync(url);
                            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string _json = await respon.Content.ReadAsStringAsync();
                                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                                if (_json.Contains("[]") == false)
                                {    
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Int32 from = _json.IndexOf("[");
                                        Int32 to = _json.IndexOf("]");
                                        string result = _json.Substring(from, to - from + 1);
                                        NofiKidienTuThietBi = JsonConvert.DeserializeObject<ObservableCollection<DuyetKiDienTuPhuTungModel>>(result).Count;
                                        OnPropertyChanged(nameof(NofiKidienTuThietBi));
                                    });
                                }
                            }
                        }

                        System.Diagnostics.Debug.WriteLine("đang chay task đặt lấy danh sách yêu cầu thuê thiết bị");
                        url = $"api/DuyetChungTu/getYeuCauThueThietBi?username={Preferences.Get(Config.User, "")}";

                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Config.URL);
                            HttpResponseMessage respon = await client.GetAsync(url);
                            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string _json = await respon.Content.ReadAsStringAsync();
                                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                                if (_json.Contains("[]") == false)
                                {                                    
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Int32 from = _json.IndexOf("[");
                                        Int32 to = _json.IndexOf("]");
                                        string result = _json.Substring(from, to - from + 1);
                                        NofiYeuCauThueThietBi = JsonConvert.DeserializeObject<ObservableCollection<DuyetChungTuModel>>(result).Count;

                                        OnPropertyChanged(nameof(NofiYeuCauThueThietBi ));
                                    });
                                }
                            }
                        }

                        System.Diagnostics.Debug.WriteLine("đang chay task lấy danh sách trả thiết bị");
                        url = $"api/DuyetChungTu/geDuyetTraThietBi?username={Preferences.Get(Config.User, "")}";

                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Config.URL);
                            HttpResponseMessage respon = await client.GetAsync(url);
                            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string _json = await respon.Content.ReadAsStringAsync();
                                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                                if (_json.Contains("[]") == false)
                                {        
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Int32 from = _json.IndexOf("[");
                                        Int32 to = _json.IndexOf("]");
                                        string result = _json.Substring(from, to - from + 1);
                                        NofiTraThietBi = JsonConvert.DeserializeObject<ObservableCollection<DuyetKiDienTuPhuTungModel>>(result).Count;
                                        OnPropertyChanged(nameof(NofiTraThietBi));
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //await DependencyService.Get<IProcessLoader>().Hide();
                        // await new MessageBox(ex.Message).Show();
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
    }
}