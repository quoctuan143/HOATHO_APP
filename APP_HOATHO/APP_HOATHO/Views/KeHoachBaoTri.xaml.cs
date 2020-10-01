using APP_HOATHO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_HOATHO.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.XForms.Buttons;

namespace APP_HOATHO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KeHoachBaoTri : ContentPage
    {
        KeHoachBaoTriViewModel viewModel;
        public KeHoachBaoTri()
        {
            InitializeComponent();
            BindingContext = viewModel = new KeHoachBaoTriViewModel();
        }

        private void Nam_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (IsBusy == true) return;
            if (viewModel.KE_HOACH_BAO_TRI.Count == 0)
                viewModel.LoadKeHoachBaoTri.Execute(DateTime.Now.Year.ToString ());
            
        }

        private async  void ToolbarItem_Clicked(object sender, EventArgs e)
        {
          APP_HOATHO.Models.KeHoachBaoTri  item = listThietBi.SelectedItem as APP_HOATHO.Models.KeHoachBaoTri;
            if (item != null )
            {
                await Navigation.PushAsync(new TaoLichSuBaoTri(item.No_));
            }    
           
        }

        Boolean filter;
        public bool FilterRecords(object o)
        {

            var item = o as APP_HOATHO.Models.KeHoachBaoTri;

            if (item != null)
            {
                if (radChuaBaoTri.IsChecked == true)
                {
                    filter = false;
                    
                }
                if (radDaBaoTri.IsChecked == true)
                {
                    filter = true;
                    
                }
                if (radTatCa.IsChecked == true)
                {
                    return true;
                }
                if (item.Da_Bao_Tri == filter )
                    return true;
            }
            return false;
        }
        private void SfRadioGroup_CheckedChanged(object sender, Syncfusion.XForms.Buttons.CheckedChangedEventArgs e)
        {
            try
            {
                if (radChuaBaoTri.IsChecked == true)
                {
                    filter = false;
                    listThietBi.View.Filter = FilterRecords;
                    listThietBi.View.RefreshFilter();
                }
                if (radDaBaoTri.IsChecked == true)
                {
                    filter = true;
                    listThietBi.View.Filter = FilterRecords;
                    listThietBi.View.RefreshFilter();
                }
                if (radTatCa.IsChecked == true)
                {
                    listThietBi.View.RefreshFilter();
                }
            }
            catch (Exception)
            {

               
            }
            
        }
    }
}