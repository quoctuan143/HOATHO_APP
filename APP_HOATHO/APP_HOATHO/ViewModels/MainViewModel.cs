using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Nha_May_Soi;
using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Models.Thiet_Bi_Van_Phong;
using APP_HOATHO.ViewModels.DuyetChungTu;
using APP_HOATHO.ViewModels.Ki_Dien_Tu_Thiet_Bi;
using APP_HOATHO.ViewModels.Nha_May_Soi.KyLenhXuatHang;
using APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.ViewModels.TBSX;
using APP_HOATHO.Views;
using APP_HOATHO.Views.Barcode;
using APP_HOATHO.Views.DuyetChungTu;
using APP_HOATHO.Views.KiDienTu;
using APP_HOATHO.Views.Kiem_Ke_Thiet_Bi;
using APP_HOATHO.Views.Nha_May_Soi;
using APP_HOATHO.Views.Nha_May_Soi.KyLenhXuatHang;
using APP_HOATHO.Views.Nha_May_Soi.Xuat_Kien_NVL;
using APP_HOATHO.Views.Quan_Ly_Vi_Tri_Kho;
using APP_HOATHO.Views.Tabpage;
using APP_HOATHO.Views.TBSX;
using APP_HOATHO.Views.Thiet_Bi_Van_Phong;
using APP_HOATHO.Views.ThietBi;
using Newtonsoft.Json;
using Plugin.LatestVersion;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using KeHoachBaoTri = APP_HOATHO.Views.KeHoachBaoTri;

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
        public bool IsMainSoi { get; set; }
        public bool IsKiDienTuPhuTung { get; set; }
        public bool IsKiDienTuThietBi { get; set; }
        public bool IsDuyetYeuCauThueThietBi { get; set; }
        public bool IsDuyetPhieuTraThietBi { get; set; }
        public bool IsKiemKeThietBi { get; set; }
        public bool IsXuatKienNVL { get; set; }
        public bool IsThongTinKien { get; set; }
        public bool IsDuyetTongHopThietBiNhaMay { get; set; }
        public bool IsYeuCauXuLyLoi { get => true; set => value = true; }
        public bool IsCapNhatThongTinLoi { get; set; }
        public bool IsDanhSachChoXuLy { get; set; }
        public bool IsPhieuNhapKho { get; set; }
        public bool IsPhieuXuatKho { get; set; }
        public bool IsCapNhatViTriKho { get; set; }
        public bool IsQuanLyViTriKho { get; set; }
        public bool IsCapNhatOChuaVai { get; set; }
        public bool IsTaoPhieuXuatKhoGomVai { get; set; }
        public bool IsNhanHelpDesk { get; set; }
        public bool IsTBSX { get; set; }
        public bool IsTP_KyLenhXuatHangSoi { get; set; }
        public bool IsGD_KyLenhXuatHangSoi { get; set; }
        public bool IsTBSXDC { get; set; }
        public int NofiLCP_FOB { get; set; }
        public int NofiLCP_GC { get; set; }
        public int NofiDuyetDatMua { get; set; }
        public int NofiDuyetDatMuaPhuTung { get; set; }
        public int NofiKidienTuPhuTung { get; set; }
        public int NofiKidienTuThietBi { get; set; }
        public int NofiLichXich { get; set; }
        public int NofiDanhMucThietBi { get; set; }
        public int NofiDanhMucThietBiRoi { get; set; }
        public int NofiDeNghiTT { get; set; }
        public int NofiYeuCauThueThietBi { get; set; }
        public int NofiTraThietBi { get; set; }
        public int NofiDuyetTongHopThietBiNhaMay { get; set; }
        public int NofiDanhSachChoITXuLy { get; set; }
        public int NofiTBSX { get; set; }
        public int NofiTBSXDC { get; set; }
        public int NofiTPKyLenhXuaHang { get; set; }
        public int NofiGDKyLenhXuaHang { get; set; }
        private DuyetChungTuPhuTung_Header duyetChungTuPhuTung_Header;
        private DuyetDonDatMua DuyetDonDatMua;
        private DuyetLCP_FOB duyetLCP_FOB;
        private DuyetLCP_GIACONG DuyetLCP_GIACONG;
        private DuyetDeNghiThanhToanTabpage DuyetDeNghiThanhToanTabpage;

        #endregion "Field"

        #region "Command"

        public ICommand DanhMucThietBiCommand { get; set; }
        public ICommand DanhMucThietBiRoiCommand { get; set; }
        public ICommand LichXichBaoTriCommand { get; set; }
        public ICommand KiemKeThietBiCommand { get; set; }
        public ICommand DuyetYeuCauThueThietBiCommand { get; set; }
        public ICommand DuyetYeuCauTraThietBiCommand { get; set; }
        public ICommand DuyetDatPhuTungCommand { get; set; }
        public ICommand DuyetKyDienTuXuatPhuTung { get; set; }
        public ICommand DuyetKyDienTuXuatThietBi { get; set; }
        public ICommand DuyetDonDatMuaCommand { get; set; }
        public ICommand DuyetLenhCapPhatFOBCommand { get; set; }
        public ICommand DuyetLenhCapPhatGiaCongCommand { get; set; }
        public ICommand DuyetDeNghiThanhToanCommand { get; set; }
        public ICommand LoadDataCommand { get; set; }
        public ICommand DuyetTongHopThietBiThueNhaMayCommand { get; set; }
        public ICommand XuatKienNVLCommand { get; set; }
        public ICommand ThongTinKienCommand { get; set; }
        public ICommand YeuCauXuLyLoiCommand { get; set; }
        public ICommand CapNhatThongTinLoiCommand { get; set; }
        public ICommand DanhSachChoXuLyCommand { get; set; }
        public ICommand PhieuNhapKhoCommand { get; set; }
        public ICommand PhieuXuatKhoCommand { get; set; }
        public ICommand CapNhatViTriKhoCommand { get; set; }
        public ICommand CapNhatViTriChuaOVaiCommand { get; set; }
        public ICommand PhieuXuatKhoGomVaiCommand { get; set; }
        public ICommand TaoPhieuXuatKhoGomVaiCommand { get; set; }
        public ICommand ChuyenVaiTuKeASangBCommand { get; set; }
        public ICommand ThongTinChiTietCayVaiCommand { get; set; }
        public ICommand ThongTinChiTietKeVaiCommand { get; set; }
        public ICommand TBSXCommand { get; set; }
        public ICommand TBSXDCCommand { get; set; }
        public ICommand TP_KyLenhXuatHangSoiCommand { get; set; }
        public ICommand GD_KyLenhXuatHangSoiCommand { get; set; }

        #endregion "Command"

        public MainViewModel()
        {
            ShowLoading("Đang tải dữ liệu");
            LoadDataCommand = new Command(async () => await Load());
            DuyetDeNghiThanhToanCommand = new Command(async () =>
            {
                if (DuyetDeNghiThanhToanTabpage == null)
                {
                    DuyetDeNghiThanhToanTabpage = new DuyetDeNghiThanhToanTabpage();
                }
                await DuyetDeNghiThanhToanTabpage.LoadData();
                await Navigation.PushAsync(DuyetDeNghiThanhToanTabpage);
            });
            DuyetLenhCapPhatGiaCongCommand = new Command(async () =>
            {
                if (DuyetLCP_GIACONG == null)
                {
                    DuyetLCP_GIACONG = new DuyetLCP_GIACONG();
                }
                await DuyetLCP_GIACONG.LoadData();
                await Navigation.PushAsync(DuyetLCP_GIACONG);
            });
            DuyetLenhCapPhatFOBCommand = new Command(async () =>
            {
                if (duyetLCP_FOB == null)
                {
                    duyetLCP_FOB = new DuyetLCP_FOB();
                }
                await duyetLCP_FOB.LoadData();
                await Navigation.PushAsync(duyetLCP_FOB);
            });
            DuyetDonDatMuaCommand = new Command(async () =>
            {
                if (DuyetDonDatMua == null)
                {
                    DuyetDonDatMua = new DuyetDonDatMua();
                }
                await DuyetDonDatMua.LoadData();
                await Navigation.PushAsync(DuyetDonDatMua);
            });
            DuyetTongHopThietBiThueNhaMayCommand = new Command(async () => await Navigation.PushAsync(new Ky_Dien_Tu_Tong_Hop_Thue_Thiet_Bi_Header_Page()));
            DanhMucThietBiCommand = new Command(async () => await Navigation.PushAsync(new Danh_Muc_Thiet_Bi()));
            DanhMucThietBiRoiCommand = new Command(async () => await Navigation.PushAsync(new Danh_Muc_Thiet_Bi_Roi()));
            LichXichBaoTriCommand = new Command(async () => await Navigation.PushAsync(new KeHoachBaoTri()));
            KiemKeThietBiCommand = new Command(async () => await Navigation.PushAsync(new Kiem_Ke_Thiet_Bi_Header_Page()));
            DuyetYeuCauThueThietBiCommand = new Command(async () => await Navigation.PushAsync(new DuyetYeuCauThueThietBiPage_Header(DocumentType.DuyetYeuCauThueThietBi)));
            DuyetYeuCauTraThietBiCommand = new Command(async () => await Navigation.PushAsync(new DuyetTraThietBi_Page(DocumentType.DuyetTraThietBi)));
            DuyetDatPhuTungCommand = new Command(async () =>
            {
                if (duyetChungTuPhuTung_Header == null)
                {
                    duyetChungTuPhuTung_Header = new DuyetChungTuPhuTung_Header(DocumentType.DuyetDatMuaPhuTung);
                }
                await duyetChungTuPhuTung_Header.LoadData();
                await Navigation.PushAsync(duyetChungTuPhuTung_Header);
            });
            DuyetKyDienTuXuatPhuTung = new Command(async () => await Navigation.PushAsync(new DuyetKiDienTuPhuTung_Page(DocumentType.KiDienTuPhuTung)));
            DuyetKyDienTuXuatThietBi = new Command(async () => await Navigation.PushAsync(new DuyetKiDienTuThietBi_Page(DocumentType.KiDienTuThietBi)));
            XuatKienNVLCommand = new Command(async () => await Navigation.PushAsync(new Xuat_Kien_Header_Page()));
            ThongTinKienCommand = new Command(async () =>
            {
                try
                {
                    var scan = new ZXingScannerPage();
                    scan.Title = "Tìm kiếm kiện";
                    Shell.SetTabBarIsVisible(scan, false);
                    await Navigation.PushAsync(scan);
                    scan.OnScanResult += (result) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            //show form lên
                            try
                            {
                                DependencyService.Get<IBeepService>().Beep();
                                if (IsBusy) return;

                                IsBusy = true;
                                string[] arr = result.Text.Split('_');
                                string ma;
                                string item;
                                if (arr.Length == 2)
                                {
                                    ma = result.Text.Split('_')[0];
                                    item = result.Text.Split('_')[1];
                                }
                                else
                                {
                                    ma = result.Text.Split('_')[0];
                                    item = "";
                                }

                                var Item = await RunHttpClientGet<UploadImageKien>($"api/soi/SearchPackingID?packingId={ma}&item={item}");
                                if (Item.Lists.Count > 0)
                                {
                                    await Navigation.PopAsync();
                                    await Navigation.PushAsync(new Thong_Tin_Kien_Page(Item.Lists[0]));
                                }
                                else
                                    DependencyService.Get<IMessage>().LongAlert("Không tìm thấy kiện này trong hệ thống");
                            }
                            catch { }
                            finally { IsBusy = false; }
                        });
                    };
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
            YeuCauXuLyLoiCommand = new Command(async () =>
            {
                try
                {
                    var scan = new ZXingScannerPage();
                    scan.Title = "Tìm kiếm thiết bị";
                    Shell.SetTabBarIsVisible(scan, false);
                    await Navigation.PushAsync(scan);
                    scan.OnScanResult += (result) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            //show form lên
                            try
                            {
                                DependencyService.Get<IBeepService>().Beep();
                                if (IsBusy) return;

                                IsBusy = true;
                                string ma = result.Text.Split('=')[1];
                                var Item = await RunHttpClientGet<DanhMuc_ThietBi>("api/qltb/getTimKiemThietBi?mathietbi=" + ma);
                                if (Item.Lists.Count > 0)
                                {
                                    await Navigation.PopAsync();
                                    DevicesMaintenanceHistory yeucau = new DevicesMaintenanceHistory
                                    {
                                        DocumentNo_ = ma,
                                        NoiDungLoi = "",
                                        NguoiGuiYeuCau = Preferences.Get(Config.FullName, ""),
                                        TenThietBi = Item.Lists[0].Description2
                                    };
                                    await Navigation.PushAsync(new Yeu_Cau_Xu_Ly_Loi_Page(yeucau));
                                }
                                else
                                    DependencyService.Get<IMessage>().LongAlert("Không tìm thấy thiết bị này trong hệ thống");
                            }
                            catch { }
                            finally { IsBusy = false; }
                        });
                    };
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
            CapNhatThongTinLoiCommand = new Command(async () =>
            {
                try
                {
                    var scan = new ZXingScannerPage();
                    scan.Title = "Tìm kiếm thiết bị";
                    Shell.SetTabBarIsVisible(scan, false);
                    await Navigation.PushAsync(scan);
                    scan.OnScanResult += (result) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            //show form lên
                            try
                            {
                                DependencyService.Get<IBeepService>().Beep();
                                if (IsBusy) return;

                                IsBusy = true;
                                string ma = result.Text.Split('=')[1];
                                var Item = await RunHttpClientGet<DevicesMaintenanceHistory>("api/qltb/TimKiemYeuCauXuLy?maThietBi=" + ma);
                                if (Item.Lists.Count > 0)
                                {
                                    await Navigation.PopAsync();
                                    DevicesMaintenanceHistory yeucau = new DevicesMaintenanceHistory
                                    {
                                        DocumentNo_ = ma,
                                        ITXuLy = Preferences.Get(Config.FullName, ""),
                                        TenThietBi = Item.Lists[0].Description2,
                                        NguoiGuiYeuCau = Item.Lists[0].NguoiGuiYeuCau
                                    };
                                    await Navigation.PushAsync(new Cap_Nhat_Thong_Tin_Loi_Page(yeucau));
                                }
                                else
                                    DependencyService.Get<IMessage>().LongAlert("Không tìm thấy thiết bị này trong hệ thống");
                            }
                            catch (Exception ex) { }
                            finally { IsBusy = false; }
                        });
                    };
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
            DanhSachChoXuLyCommand = new Command(async () => await Navigation.PushAsync(new Danh_Sach_Cho_Xu_Ly_Page()));
            PhieuNhapKhoCommand = new Command(async () => await Navigation.PushAsync(new DanhSachPhieuNhapMua()));
            PhieuXuatKhoCommand = new Command(async () => await Navigation.PushAsync(new DanhSachPhieuXuatKho_Page()));
            PhieuXuatKhoGomVaiCommand = new Command(async () => await Navigation.PushAsync(new DanhSachPhieuXuatKhoGomVai_Page()));
            CapNhatViTriKhoCommand = new Command(async () =>
            {
                try
                {
                    ScanBarcode scan = new ScanBarcode(false, "Quét kệ chứa cây vải");
                    scan.ScanBarcodeResult += (s, result) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            CellPositionModel item = new CellPositionModel { Code = result, PositionId = "" };
                            var ok1 = await RunHttpClientPost("api/qltb/PostKiemTraOChuaVai", item);
                            if (!ok1.IsSuccessStatusCode)
                            {
                                await new MessageBox($"Id : {result} này không tồn tại trong hệ thống").Show();
                                return;
                            }
                            //show form lên
                            try
                            {
                                await Navigation.PushAsync(new CapNhatViTriLuuKho_Page(result));
                            }
                            catch
                            {
                                await new MessageBox("Barcode không tồn tại trong hệ thống").Show();
                            }
                        });
                    };
                    await Navigation.PushAsync(scan);
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
            CapNhatViTriChuaOVaiCommand = new Command(async () =>
            {
                try
                {
                    await Navigation.PushAsync(new CapNhatViTriChuaOVai_Page());
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
            TaoPhieuXuatKhoGomVaiCommand = new Command(async () =>
            {
                try
                {
                    await Navigation.PushAsync(new TaoPhieuXuatKhoGomVai_Page());
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });

            ChuyenVaiTuKeASangBCommand = new Command(async () =>
            {
                try
                {
                    await Navigation.PushAsync(new ChuyenKeVaiASangB_Page());
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });

            ThongTinChiTietCayVaiCommand = new Command(async () =>
            {
                try
                {
                    ScanBarcode scan = new ScanBarcode(false,"Quét Id Cây Vải",true);
                    scan.ScanBarcodeResult += (s, result) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            //show form lên
                            try
                            {                              
                                if (IsBusy) return;

                                IsBusy = true;                                

                                var Item = await RunHttpClientGet<ThongTinChiTietCayVai_Model>($"api/qltb/XemThongTinChiTietCayVai?barcodeId={result}");
                                if (Item.Lists.Count > 0)
                                {                                   
                                    await Navigation.PushAsync(new ThongTinChiTietCayVai_Page(Item.Lists[0]));
                                }
                                else
                                   await new MessageBox($"Không tìm thấy cây vải có barcodeId {result} trong hệ thống").Show();
                            }
                            catch { }
                            finally { IsBusy = false; }
                        });
                    };
                    await Navigation.PushAsync(scan);
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });

            ThongTinChiTietKeVaiCommand = new Command(async () =>
            {
                try
                {
                    await Navigation.PushAsync(new Thong_Tin_Chi_Tiet_Ke_Vai_Page());
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });

            TBSXCommand = new Command(async () =>
            {
                try
                {
                    await Navigation.PushAsync(new TBSX_Header_Page());
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
            TBSXDCCommand = new Command(async () =>
            {
                try
                {
                    await Navigation.PushAsync(new TBSX_DC_Header_Page());
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
            TP_KyLenhXuatHangSoiCommand = new Command(async () =>
            {
                try
                {
                    await Navigation.PushAsync(new KyLehnhXuatHangHeader_Page(AppoveType.TruongPhongKinhDoanh));
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
            GD_KyLenhXuatHangSoiCommand = new Command(async () =>
            {
                try
                {
                    await Navigation.PushAsync(new KyLehnhXuatHangHeader_Page(AppoveType.GiamDocNhaMay));
                }
                catch (Exception ex)
                {
                    await new MessageBox(ex.Message).Show();
                }
            });
            Task.Factory.StartNew(async () => await Load());

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
            MessagingCenter.Subscribe<Ki_Dien_Tu_Tong_Hop_Thiet_Bi_Nha_May_Header_ViewModel, DocumentType>(this, "langngheduyet", (obj, item) =>
            {
                NofiDuyetTongHopThietBiNhaMay--;
                OnPropertyChanged(nameof(NofiDuyetTongHopThietBiNhaMay));
            });
            MessagingCenter.Subscribe<Cap_Nhat_Thong_Tin_Loi_Page, DocumentType>(this, "langngheduyet", (obj, item) =>
            {
                if (NofiDanhSachChoITXuLy > 0)
                {
                    NofiDanhSachChoITXuLy--;
                    OnPropertyChanged(nameof(NofiDanhSachChoITXuLy));
                }
            });
            MessagingCenter.Subscribe<TBSX_DC_Header_ViewModel>(this, "langngheduyet", (obj) =>
            {
                if (NofiTBSXDC > 0)
                {
                    NofiTBSXDC--;
                    OnPropertyChanged(nameof(NofiTBSXDC));
                }
            });
            MessagingCenter.Subscribe<TBSX_Header_ViewModel>(this, "langngheduyet", (obj) =>
            {
                if (NofiTBSX > 0)
                {
                    NofiTBSX--;
                    OnPropertyChanged(nameof(NofiTBSX));
                }
            });
            MessagingCenter.Subscribe<KyLenhXuatHangHeader_ViewModel>(this, "langngheduyet", (obj) =>
            {
                if (NofiTPKyLenhXuaHang > 0)
                {
                    NofiTPKyLenhXuaHang--;
                    OnPropertyChanged(nameof(NofiTPKyLenhXuaHang));
                }
                if (NofiGDKyLenhXuaHang > 0)
                {
                    NofiGDKyLenhXuaHang--;
                    OnPropertyChanged(nameof(NofiGDKyLenhXuaHang));
                }
            });
        }

        private async Task Load()
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
                            IsMainSoi = Convert.ToBoolean(ISDBNULL(body[0].NHA_MAY_SOI.Value, false));
                            OnPropertyChanged(nameof(IsMainSoi));
                            IsQuanLyViTriKho = Convert.ToBoolean(ISDBNULL(body[0].QUAN_LY_VI_TRI_KHO.Value, false));
                            OnPropertyChanged(nameof(IsQuanLyViTriKho));
                            IsCapNhatOChuaVai = Convert.ToBoolean(ISDBNULL(body[0].QUAN_LY_VI_TRI_KHO.Value, false));
                            OnPropertyChanged(nameof(IsCapNhatOChuaVai));

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
                                string url = "api/qltb/getThietBi?username=" + Preferences.Get(Config.User, "");
                                var g = await RunHttpClientGet<object>(url);
                                NofiDanhMucThietBi = g.Lists.Count();
                                OnPropertyChanged(nameof(NofiDanhMucThietBi));
                                url = "api/qltb/getThietBiRoi?username=" + Preferences.Get(Config.User, "");
                                var k = await RunHttpClientGet<object>(url);
                                NofiDanhMucThietBiRoi = k.Lists.Count();
                                OnPropertyChanged(nameof(NofiDanhMucThietBiRoi));
                            }
                            

                            IsLichXichBaoTri = Convert.ToBoolean(ISDBNULL(body[0].LICH_XICH_BAO_TRI.Value, false));
                            OnPropertyChanged(nameof(IsLichXichBaoTri));
                            if (IsLichXichBaoTri)
                            {
                                System.Diagnostics.Debug.WriteLine(" đang chay task 3");
                                string url = "api/qltb/getKeHoachBaoTri?user=" + Preferences.Get(Config.User, "") + "&nam=" + DateTime.Now.Year.ToString() + "&thang=" + DateTime.Now.Month.ToString();
                                var f = await RunHttpClientGet<Models.KeHoachBaoTri>(url);
                                NofiLichXich = f.Lists.Where(x => x.Da_Bao_Tri == false).Count();
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
                                string url = $"api/DuyetChungTu/getLenhCapPhat?username={Preferences.Get(Config.User, "")}";
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
                                string url = $"api/DuyetChungTu/getLenhCapPhat_GC?username={Preferences.Get(Config.User, "")}";
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
                            if (IsKiDienTuPhuTung)
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
                            if (IsKiDienTuThietBi)
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
                            if (IsDuyetPhieuTraThietBi)
                            {
                                System.Diagnostics.Debug.WriteLine(" đang chay task 11");
                                string url = $"api/DuyetChungTu/geDuyetTraThietBi?username={Preferences.Get(Config.User, "")}";
                                var h = await RunHttpClientGet<object>(url);
                                NofiTraThietBi = h.Lists.Count();
                                OnPropertyChanged(nameof(NofiTraThietBi));
                                System.Diagnostics.Debug.WriteLine("chay xong task 11");
                            }

                            IsDuyetTongHopThietBiNhaMay = Convert.ToBoolean(ISDBNULL(body[0].DUYET_KY_TONG_HOP_THIET_BI_NHA_MAY.Value, false));
                            OnPropertyChanged(nameof(IsDuyetTongHopThietBiNhaMay));
                            if (IsDuyetTongHopThietBiNhaMay)
                            {
                                System.Diagnostics.Debug.WriteLine(" đang chay task 12");
                                var h = await RunHttpClientGet<object>($"api/DuyetChungTu/getKiDienTuTongHop_NhaMay?username={Preferences.Get(Config.User, "")}");
                                NofiDuyetTongHopThietBiNhaMay = h.Lists.Count();
                                OnPropertyChanged(nameof(NofiDuyetTongHopThietBiNhaMay));
                                System.Diagnostics.Debug.WriteLine("chay xong task 12");
                            }

                            IsKiemKeThietBi = Convert.ToBoolean(ISDBNULL(body[0].KIEM_KE_THIET_BI.Value, false));
                            OnPropertyChanged(nameof(IsKiemKeThietBi));

                            IsXuatKienNVL = Convert.ToBoolean(ISDBNULL(body[0].XUAT_KIEN_NVL.Value, false));
                            OnPropertyChanged(nameof(IsXuatKienNVL));
                            IsThongTinKien = Convert.ToBoolean(ISDBNULL(body[0].THONG_TIN_KIEN.Value, false));
                            OnPropertyChanged(nameof(IsThongTinKien));

                            IsCapNhatThongTinLoi = Convert.ToBoolean(ISDBNULL(body[0].IT_CAP_NHAT_HELPDESK.Value, false));
                            OnPropertyChanged(nameof(IsCapNhatThongTinLoi));

                            IsNhanHelpDesk = Convert.ToBoolean(ISDBNULL(body[0].NHAN_YC_HELPDESK.Value, false));
                            OnPropertyChanged(nameof(IsNhanHelpDesk));

                            IsDanhSachChoXuLy = Convert.ToBoolean(ISDBNULL(body[0].DANH_SACH_HELPDESK.Value, false));
                            OnPropertyChanged(nameof(IsDanhSachChoXuLy));
                            if (IsDanhSachChoXuLy)
                            {
                                var h = await RunHttpClientGet<object>($"api/qltb/DanhSachThietBiChoXuLy");
                                NofiDanhSachChoITXuLy = h.Lists.Count();
                                OnPropertyChanged(nameof(NofiDanhSachChoITXuLy));
                            }
                            IsPhieuNhapKho = Convert.ToBoolean(ISDBNULL(body[0].HOA_DON_NHAP_MUA.Value, false));
                            OnPropertyChanged(nameof(IsPhieuNhapKho));

                            IsPhieuXuatKho = Convert.ToBoolean(ISDBNULL(body[0].PHIEU_XUAT_NPL.Value, false));
                            OnPropertyChanged(nameof(IsPhieuXuatKho));

                            IsCapNhatViTriKho = Convert.ToBoolean(ISDBNULL(body[0].CAP_NHAT_VI_TRI_KHO.Value, false));
                            OnPropertyChanged(nameof(IsCapNhatViTriKho));

                            var tpkd_tbsx = Convert.ToBoolean(ISDBNULL(body[0].TPKD_DUYET_TBSX.Value, false));
                            if (tpkd_tbsx)
                            {
                                IsTBSX = true;
                                IsTBSXDC = true;
                                OnPropertyChanged(nameof(IsTBSX));
                                var h = await RunHttpClientGet<object>($"api/tbsx/getTBSX_Header?userid={Preferences.Get(Config.User, "")}&tongiamdoc={Preferences.Get(Config.Role,1)}");
                                NofiTBSX = h.Lists.Count();
                                OnPropertyChanged(nameof(NofiTBSX));
                                
                                var k = await RunHttpClientGet<object>($"api/tbsx/getPhuLucTBSX_Header?userid={Preferences.Get(Config.User, "")}&tongiamdoc={Preferences.Get(Config.Role, 1)}");
                                NofiTBSXDC = k.Lists.Count();
                                OnPropertyChanged(nameof(NofiTBSXDC));
                            }

                            var tgd_tbsx = Convert.ToBoolean(ISDBNULL(body[0].TGD_DUYET_TBSX.Value, false));
                            if (tgd_tbsx)
                            {
                                IsTBSX = true;
                                IsTBSXDC = true;
                                OnPropertyChanged(nameof(IsTBSX));
                                var h = await RunHttpClientGet<object>($"api/tbsx/getTBSX_Header?userid={Preferences.Get(Config.User, "")}&tongiamdoc={Preferences.Get(Config.Role, 1)}");
                                NofiTBSX = h.Lists.Count();
                                OnPropertyChanged(nameof(NofiTBSX));
                                var k = await RunHttpClientGet<object>($"api/tbsx/getPhuLucTBSX_Header?userid={Preferences.Get(Config.User, "")}&tongiamdoc={Preferences.Get(Config.Role,1)}");
                                NofiTBSXDC = k.Lists.Count();
                                OnPropertyChanged(nameof(NofiTBSXDC));
                            }
                            IsTP_KyLenhXuatHangSoi = Convert.ToBoolean(ISDBNULL(body[0].TP_SOI_KY_LXH.Value, false));
                            OnPropertyChanged(nameof(IsTP_KyLenhXuatHangSoi));
                            if (IsTP_KyLenhXuatHangSoi)
                            {
                                var h = await RunHttpClientGet<object>($"api/Soi/GetLenhXuatHang?userid={Preferences.Get(Config.User, "")}&permision={AppoveType.TruongPhongKinhDoanh}");
                                NofiTPKyLenhXuaHang = h.Lists.Count();
                                OnPropertyChanged(nameof(NofiTPKyLenhXuaHang));
                            }
                            IsGD_KyLenhXuatHangSoi = Convert.ToBoolean(ISDBNULL(body[0].GD_SOI_KY_LXH.Value, false));
                            OnPropertyChanged(nameof(IsGD_KyLenhXuatHangSoi));
                            if (IsGD_KyLenhXuatHangSoi)
                            {
                                var h = await RunHttpClientGet<object>($"api/Soi/GetLenhXuatHang?userid={Preferences.Get(Config.User, "")}&permision={AppoveType.GiamDocNhaMay}");
                                NofiGDKyLenhXuaHang = h.Lists.Count();
                                OnPropertyChanged(nameof(NofiGDKyLenhXuaHang));
                            }    
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

        private async Task NewVersion()
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