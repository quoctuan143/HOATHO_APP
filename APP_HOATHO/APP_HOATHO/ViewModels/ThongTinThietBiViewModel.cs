using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models;
using APP_HOATHO.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels
{
 public   class ThongTinThietBiViewModel : BaseViewModel
    {
        
        public DanhMuc_ThietBi Item { get; set;  }
        ObservableCollection<LICH_SU_BAO_TRI> lichsubaotri;
        public ObservableCollection<LICH_SU_BAO_TRI> lICH_SU_BAO_TRIs { get => lichsubaotri; set { lichsubaotri = value; OnPropertyChanged("lICH_SU_BAO_TRIs"); } }
        ObservableCollection <QUY_TRINH_BAO_TRI> _quytrinhbaotri;
        public ObservableCollection<QUY_TRINH_BAO_TRI> QUY_TRINH_BAO_TRIs 
        {   get => _quytrinhbaotri ; 
            set 
            {                 
                _quytrinhbaotri = value;
                OnPropertyChanged(nameof(QUY_TRINH_BAO_TRIs));
            } 
        }
        ObservableCollection<APP_HOATHO.Models.KeHoachBaoTri> _kehoachbaotris;
       

        public ObservableCollection<APP_HOATHO.Models.KeHoachBaoTri> KE_HOACH_BAO_TRIs
        {
            get => _kehoachbaotris;
            set
            {
                _kehoachbaotris = value;
                OnPropertyChanged(nameof(KE_HOACH_BAO_TRIs));
            }
        }
      
        public Command<string> LoadKeHoachBaoTri { get; set; }
        public Command<string> LoadLichSuBaoTri { get; set; }
        public Command<string> LoadQuyTrinhBaoTri { get; set; }      
        public ThongTinThietBiViewModel(DanhMuc_ThietBi _item) 
        {
           
            Item = _item;
            Title = Item.NameVN;
            KE_HOACH_BAO_TRIs = new ObservableCollection<APP_HOATHO.Models.KeHoachBaoTri>();
            
            lICH_SU_BAO_TRIs = new ObservableCollection<LICH_SU_BAO_TRI>();
            QUY_TRINH_BAO_TRIs = new ObservableCollection<QUY_TRINH_BAO_TRI>();
            LoadLichSuBaoTri = new Command<string>(async (p)  => await  ExcuteLoadLichSuBaoTri(p));
            LoadQuyTrinhBaoTri = new Command<string>(async (p) => await ExcuteLoadQuyTrinhBaoTri(p));
            LoadKeHoachBaoTri = new Command<string>(async (p) => await ExcuteKeHoachBaoTri(p));
           

            MessagingCenter.Subscribe<ThongTinThietBi_Edit, DanhMuc_ThietBi>(this, "UpdateThietBi", (ojb, tbi) => {
                try
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            if (tbi != null)
                            {
                                Item = tbi;
                                OnPropertyChanged(nameof(Item));
                            }
                        }
                        catch (Exception)
                        {


                        }



                    });
                }
                catch (Exception)
                {

                }


            });
        }

        async Task ExcuteLoadQuyTrinhBaoTri(string mathietbi)
        {
            IsBusy = true;
            IsRunning = true;
            try
            {
                QUY_TRINH_BAO_TRIs.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getQuyTrinhBaoTri?mathietbi=" +Item.No_).Result;
              //  await Task.Delay(3000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    QUY_TRINH_BAO_TRIs = JsonConvert.DeserializeObject<ObservableCollection<QUY_TRINH_BAO_TRI>>(result);                    
                }
                
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }

        async Task ExcuteLoadLichSuBaoTri(string mathietbi)
        {
            IsBusy = true;
            IsRunning = true;
            try
            {
                lICH_SU_BAO_TRIs.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getLichSuBaoTri?mathietbi=" + Item.No_).Result;
              //  await Task.Delay(3000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    lICH_SU_BAO_TRIs = JsonConvert.DeserializeObject<ObservableCollection<LICH_SU_BAO_TRI>>(result);
                }
                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }

        async Task ExcuteKeHoachBaoTri(string year)
        {
            IsBusy = true;
            IsRunning = true;
            try
            {
                KE_HOACH_BAO_TRIs.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getKeHoachBaoTri_ThietBi?mathietbi=" + Item.No_  + "&nam=" + year).Result;
               // await Task.Delay(3000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    KE_HOACH_BAO_TRIs = JsonConvert.DeserializeObject<ObservableCollection<APP_HOATHO.Models.KeHoachBaoTri>>(result);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }
    }
}
       