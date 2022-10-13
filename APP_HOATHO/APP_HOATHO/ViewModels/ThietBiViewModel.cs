using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using APP_HOATHO.Models;
using APP_HOATHO.Views;
using APP_HOATHO.Global;
using Xamarin.Essentials;
using Newtonsoft.Json;
using APP_HOATHO.Dialog;
using Syncfusion.SfDataGrid.XForms;
using System.Linq;

namespace APP_HOATHO.ViewModels
{
    public class ThietBiViewModel : BaseViewModel
    {
        int size = 0;      
        ObservableCollection<DanhMuc_ThietBi> _items;
        public ObservableCollection<DanhMuc_ThietBi> Items { get =>  _items; set { _items = value; OnPropertyChanged(nameof(Items)); }}

        public ObservableCollection<DanhMuc_ThietBi> ketqua = new ObservableCollection<DanhMuc_ThietBi>();
        public Command LoadItemsCommand { get; set; }
        public Command LoadMoreCommand { get; set; } 
        public ThietBiViewModel() 
        {
            Title = "Browse";
            Items = new ObservableCollection<DanhMuc_ThietBi>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadMoreCommand = new Command(async () => await ExecuteLoadMoreCommand());
        }

     async   Task  ExecuteLoadMoreCommand()
        {
            IsRunning = true;
           if (size + 100 < ketqua.Count )
            {
                for(int i= size; i< size + 100;i++)
                {
                    Items.Add(ketqua[i]);                    
                }
                size = Items.Count;
            }    
           else
            {
                for (int i = size; i < ketqua.Count ; i++)
                {
                    Items.Add(ketqua[i]);
                }
                size = Items.Count;
            }
            IsRunning = false;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            IsRunning = true;
            try
            {
                Items.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getThietBi?nhamay=" + Preferences.Get(Config.NhaMay ,"")).Result;
                await Task.Delay(3000);
                var a = await RunHttpClientGet<DanhMuc_ThietBi>("api/qltb/getThietBi?nhamay=" + Preferences.Get(Config.NhaMay, ""));
                Items = a.Lists;                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }

        public bool FilterRecords(object o)
        {
            string filterText = "Germany";
            var item = o as DanhMuc_ThietBi;

            if (item != null)
            {

                if (item.No_2.Equals(filterText))
                    return true;
            }
            return false;
        }
    }

 
}