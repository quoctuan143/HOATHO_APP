using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models;
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
 public   class KeHoachBaoTriViewModel : BaseViewModel
    {
        
       
        ObservableCollection <KeHoachBaoTri> _kehoachbaotri;
        public ObservableCollection<KeHoachBaoTri> KE_HOACH_BAO_TRI 
        {   get => _kehoachbaotri; 
            set 
            {
                _kehoachbaotri = value;
                OnPropertyChanged(nameof(KE_HOACH_BAO_TRI));
            } 
        }
        public Command<string> LoadKeHoachBaoTri { get; set; }  
        public KeHoachBaoTriViewModel() 
        {
           
           
            Title ="Kế hoạch bảo trì";
            KE_HOACH_BAO_TRI = new ObservableCollection<KeHoachBaoTri>();           
            LoadKeHoachBaoTri = new Command<string>(async  (p) => await  ExcuteKeHoachBaoTri(p));
           
        }

        async Task ExcuteKeHoachBaoTri(string year  )
        {
            if (year == null) year = DateTime.Now.Year.ToString();
            IsBusy = true;
            IsRunning = true;
            try
            {
                KE_HOACH_BAO_TRI.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getKeHoachBaoTri?manhamay=" + Preferences.Get(Config.NhaMay ,"") + "&nam=" + year ).Result;
                await Task.Delay(3000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    KE_HOACH_BAO_TRI = JsonConvert.DeserializeObject<ObservableCollection<KeHoachBaoTri>>(result);                    
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
