﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Tabpage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuyetLCP_GIACONG : TabbedPage
    {
        public DuyetLCP_GIACONG()
        {
            InitializeComponent();
            ContentPage navigationPage = new DuyetChungTu.DuyetLenhCapPhatGiaCong_Page(Global.DocumentType.DuyetLCP_GC);
            navigationPage.IconImageSource = "baseline_check_circle_black.png";
            navigationPage.Title = "Chờ Duyệt";

            ContentPage content = new MoLaiChungTu.MoLaiLenhCapPhatGiaCong_Page(Global.DocumentType.MoLaiLCP_GC);
            content.Title = "Mở Lại";
            content.IconImageSource = "baseline_open_in_browser_black.png";
            Children.Add(navigationPage);
            Children.Add(content);
        }
        public async Task LoadData()
        {
            DuyetChungTu.DuyetLenhCapPhatGiaCong_Page list = Children[0] as DuyetChungTu.DuyetLenhCapPhatGiaCong_Page;
            list.viewModel.IsBusy = false;
            list.viewModel.LoadCommand.Execute(null);
        }
    }
}