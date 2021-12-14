using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using APP_HOATHO.Models;
using APP_HOATHO.Services;
using APP_HOATHO.Global;
using APP_HOATHO.Interface;

namespace APP_HOATHO.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged 
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
        public virtual void OnAppearing()
        {
            // No default implementation. 
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
