﻿using APP_HOATHO.Dialog;
using APP_HOATHO.Global;
using APP_HOATHO.Models;
using APP_HOATHO.Views;
using FFImageLoading.Helpers.Exif;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_HOATHO.ViewModels
{
 public   class KeHoachBaoTriViewModel : BaseViewModel
    {
        
       
        ObservableCollection <Models.KeHoachBaoTri> _kehoachbaotri;
        public ObservableCollection<Models.KeHoachBaoTri> KE_HOACH_BAO_TRI 
        {   get => _kehoachbaotri; 
            set 
            {
                _kehoachbaotri = value;
                OnPropertyChanged(nameof(KE_HOACH_BAO_TRI));
            } 
        }
        public ICommand LoadKeHoachBaoTri { get; set; }  
        public KeHoachBaoTriViewModel() 
        {
           
           
            Title ="Kế hoạch bảo trì";
            KE_HOACH_BAO_TRI = new ObservableCollection<Models.KeHoachBaoTri>();           
            LoadKeHoachBaoTri = new Command<Tuple< string,int>>(async (a) => await ExcuteKeHoachBaoTri(a));
            MessagingCenter.Subscribe<TaoLichSuBaoTri, LICH_SU_BAO_TRI>(this, "AddLichSuBaoTri", (ojb, lsu) => {
                try
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            if (lsu != null)
                            {
                               
                                foreach (Models.KeHoachBaoTri kh in KE_HOACH_BAO_TRI)
                                {
                                    if (kh.No_ == lsu.MA_THIET_BI && kh.Thang == lsu.THANG && kh.Code == lsu.NHOM_BAO_TRI)
                                    {
                                        kh.Da_Bao_Tri = true;
                                        OnPropertyChanged(nameof(KE_HOACH_BAO_TRI));
                                    }
                                }
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

        async Task ExcuteKeHoachBaoTri(Tuple<string, int> input)
        {
            var year = string.IsNullOrEmpty( input.Item1) ? DateTime.Now.Year.ToString() : input.Item1;
            var thang = input.Item2;
            IsBusy = true;
            IsRunning = true;
            try
            {
                KE_HOACH_BAO_TRI.Clear();
                var _json =await Config.client.GetStringAsync(Config.URL + $"api/qltb/getKeHoachBaoTri?user={Preferences.Get(Config.User ,"")}&nam={year}&thang={thang}");
                await Task.Delay(500);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    KE_HOACH_BAO_TRI = JsonConvert.DeserializeObject<ObservableCollection<Models.KeHoachBaoTri>>(result);                    
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
