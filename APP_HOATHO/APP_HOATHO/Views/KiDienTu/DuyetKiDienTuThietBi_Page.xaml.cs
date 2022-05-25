﻿using APP_HOATHO.Global;
using APP_HOATHO.Models.KiDienTuPhuTung;
using APP_HOATHO.ViewModels.DuyetChungTu;
using APP_HOATHO.Views.DuyetChungTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.KiDienTu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuyetKiDienTuThietBi_Page : ContentPage
    {
       
        public DuyetKiDienTuThietBi_ViewModel viewModel;
        public DuyetKiDienTuThietBi_Page(DocumentType type)
        {
            InitializeComponent();
            BindingContext = viewModel = new DuyetKiDienTuThietBi_ViewModel(type);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListItem.Count == 0)
            viewModel.LoadCommand.Execute(null);
        }
        private void listChiTiet_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            DuyetKiDienTuPhuTungModel _selectItem = listChiTiet.SelectedItem as DuyetKiDienTuPhuTungModel;
            if (_selectItem == null) return;
            Navigation.PushAsync(new DuyetKiDienTuThietBiLine(_selectItem, viewModel._documentType)); 
        }
    }

}