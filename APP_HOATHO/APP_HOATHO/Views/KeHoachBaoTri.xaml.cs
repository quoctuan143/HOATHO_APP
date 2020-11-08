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
            entryThang.SelectedIndex = 0;
            entryNam.Text = DateTime.Now.Year.ToString();
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
                await Navigation.PushAsync(new TaoLichSuBaoTri(item ));
            }    
           
        }

        Boolean filter;
        int thang;
        public bool FilterRecords(object o)
        {

            var item = o as APP_HOATHO.Models.KeHoachBaoTri;
            thang = entryThang.SelectedIndex;          
            if (item != null)            {
                
                
                if (radChuaBaoTri.IsChecked == true)
                {
                    
                    filter = false;
                    
                }
                else if (radDaBaoTri.IsChecked == true)
                {
                    filter = true;                    
                }
                
                if (thang == 0)
                {                  
                    if (radTatCa.IsChecked == true )
                        return true;
                    else if (item.Da_Bao_Tri == filter) return true;
                }   
                else if (radTatCa.IsChecked == true)
                {
                    if (item.Thang == thang) return true;
                }    
                else
                {
                    if (item.Da_Bao_Tri == filter && item.Thang == thang ) return true;
                }
               
            }
            return false;
        }
        private void SfRadioGroup_CheckedChanged(object sender, Syncfusion.XForms.Buttons.CheckedChangedEventArgs e)
        {
            try
            {
                listThietBi.View.Filter = FilterRecords;
                listThietBi.View.RefreshFilter();
            }
            catch (Exception)
            {

               
            }
            
        }

        private void entryNam_TextChanged(object sender, TextChangedEventArgs e)
        {
             
           
        }

        private void entryThang_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            listThietBi.View.Filter = FilterRecords;
            listThietBi.View.RefreshFilter();
        }

        private void entryNam_Unfocused(object sender, FocusEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.LoadKeHoachBaoTri.Execute(entryNam.Text );
            }
        }
    }
}