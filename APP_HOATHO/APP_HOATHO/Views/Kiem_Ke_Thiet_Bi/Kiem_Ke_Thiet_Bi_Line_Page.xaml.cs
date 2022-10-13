﻿using APP_HOATHO.Models.Kiem_Ke_Thiet_Bi;
using APP_HOATHO.ViewModels.Kiem_Ke_Thiet_Bi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Kiem_Ke_Thiet_Bi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Kiem_Ke_Thiet_Bi_Line_Page : ContentPage
    {
        Kiem_Ke_Thiet_Bi_Line_ViewModel viewModel;
        public Kiem_Ke_Thiet_Bi_Line_Page(Kiem_Ke_Thiet_Bi_Header_Model item)
        {
            InitializeComponent();
            BindingContext = viewModel = new Kiem_Ke_Thiet_Bi_Line_ViewModel(item);
            viewModel.navigation = Navigation;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListItem.Count == 0)
            {
                viewModel.LoadCommand.Execute(null);
            }    
        }
    }
}