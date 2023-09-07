using APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.ViewModels.Quan_Ly_Vi_Tri_Kho
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ThongTinChiTietCayVai_Page : ContentPage, INotifyPropertyChanged
	{
		public ThongTinChiTietCayVai_Model Item { get; set; }
        public ThongTinChiTietCayVai_Page (ThongTinChiTietCayVai_Model item)
		{
			InitializeComponent ();
			Item = item;
			BindingContext = this;
		}
	}
}