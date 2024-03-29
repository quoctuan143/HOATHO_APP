﻿using APP_HOATHO.Models.Kiem_Ke_Thiet_Bi;
using APP_HOATHO.Models.Nha_May_Soi;
using APP_HOATHO.ViewModels.Kiem_Ke_Thiet_Bi;
using APP_HOATHO.ViewModels.Nha_May_Soi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Nha_May_Soi.Xuat_Kien_NVL
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Xuat_Kien_Header_Page : ContentPage
    {
        public Xuat_Kien_Header_ViewModel viewModel;
        public Xuat_Kien_Header_Page()
        {
            InitializeComponent();
            viewModel = new Xuat_Kien_Header_ViewModel();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListItem.Count == 0)
                viewModel.LoadCommand.Execute(null);
        }
        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            Xuat_Kien_Header_Model _selectItem = listChiTiet.SelectedItem as Xuat_Kien_Header_Model;
            if (_selectItem == null) return;
            Navigation.PushAsync(new Xuat_Kien_Line_Page(_selectItem));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Xuat_Kien_Header_Model _selectItem = listChiTiet.SelectedItem as Xuat_Kien_Header_Model;
            if (_selectItem == null) return;
            viewModel.KiemTraChatLuongCommand.Execute(_selectItem.No_);
        }
    }
}