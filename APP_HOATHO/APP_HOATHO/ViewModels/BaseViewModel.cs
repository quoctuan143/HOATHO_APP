using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using APP_HOATHO.Models;
using APP_HOATHO.Services;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using APP_HOATHO.Dialog;
using Plugin.Connectivity;

namespace APP_HOATHO.ViewModels
{
    public class BaseViewModel :  INotifyPropertyChanged
    {


        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
            set { SetProperty(ref isRunning, value); }
        }
        public object  ISDBNULL (object inp , object output)
        {
            if (inp == null) return  output;
            return inp;
        }
        public void ShowLoading(string title)
        {
            DependencyService.Get<IProcessLoader>().Show(title);
        }
        public void HideLoading()
        {
            DependencyService.Get<IProcessLoader>().Hide();
        }
        public void ShortAlert(string title)
        {
            DependencyService.Get<IMessage>().ShortAlert(title);
        }
        protected virtual void OnAppearing()
        {
            // No default implementation. 
        }
        public async Task<HttpClientResponseModel<T>> RunHttpClientGet <T>(string apiUrl ) where T : class 
        {
            try
            {                
                var respon = await Config.client.GetAsync(apiUrl);
                HttpClientResponseModel<T> values = new HttpClientResponseModel<T>();
                values.Status = respon;
                values.ErrorString = respon.Content.ToString();
                values.Lists = new ObservableCollection<T>();
                if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string _json = await respon.Content.ReadAsStringAsync();
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("[]") == false)
                    {
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string result = _json.Substring(from, to - from + 1);
                        values.Lists = JsonConvert.DeserializeObject<ObservableCollection<T>>(result);
                    }                    
                }                
                return values;
            }
            catch (Exception ex)
            {
                return new HttpClientResponseModel<T> { Status = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest }, ErrorString = ex.Message};
            }

        }
        public async Task<HttpClientResponseModel<T>> RunHttpClientGet<T>(string apiUrl, object value) where T : class
        {
            try
            {
                var respon = await Config.client.PostAsJsonAsync(apiUrl,value);
                HttpClientResponseModel<T> values = new HttpClientResponseModel<T>();
                values.Status = respon;
                values.ErrorString = respon.Content.ToString();
                values.Lists = new ObservableCollection<T>();
                if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string _json = await respon.Content.ReadAsStringAsync();
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("[]") == false)
                    {
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string result = _json.Substring(from, to - from + 1);
                        values.Lists = JsonConvert.DeserializeObject<ObservableCollection<T>>(result);
                    }
                }
                return values;
            }
            catch (Exception ex)
            {
                return new HttpClientResponseModel<T> { Status = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest }, ErrorString = ex.Message };
            }

        }
        public async Task<HttpResponseMessage > RunHttpClientPost(string apiUrl , object  Value) 
        {
            try
            {                
                HttpResponseMessage Status = await Config.client.PostAsJsonAsync(apiUrl, Value); 
                return Status;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage  { StatusCode = System.Net.HttpStatusCode.BadRequest };
            }

        }
        /// <summary>
        /// Called when the view model is disappearing. View Model clean-up should be performed here.
        /// </summary>
        public virtual void OnDisappearing()
        {
            // No default implementation. 
        }
        public void LongAlert(string title)
        {
            DependencyService.Get<IMessage>().LongAlert(title);
        }
        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion

    }
}
