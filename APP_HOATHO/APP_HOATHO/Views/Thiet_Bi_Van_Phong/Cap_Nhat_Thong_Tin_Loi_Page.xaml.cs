using APP_HOATHO.Global;
using APP_HOATHO.Interface;
using APP_HOATHO.Models;
using APP_HOATHO.Models.Thiet_Bi_Van_Phong;
using APP_HOATHO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Views.Thiet_Bi_Van_Phong
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cap_Nhat_Thong_Tin_Loi_Page : ContentPage
    {
        BaseViewModel BaseView { get; set; }
        List<PhanNhomLoi> PhanNhomLoi;
        public   DevicesMaintenanceHistory Item { get; set; }
        public Cap_Nhat_Thong_Tin_Loi_Page(DevicesMaintenanceHistory item)
        {
            InitializeComponent();
            BaseView = new BaseViewModel();
            Item = item;
            PhanNhomLoi = new List<PhanNhomLoi> {
                new Thiet_Bi_Van_Phong.PhanNhomLoi { Loi = 0, TenLoi = "Lỗi Phần Cứng"},
                new Thiet_Bi_Van_Phong.PhanNhomLoi { Loi = 1, TenLoi = "Lỗi Phần Mềm" },
                new Thiet_Bi_Van_Phong.PhanNhomLoi { Loi = 2, TenLoi = "Lỗi Mạng"},
                new Thiet_Bi_Van_Phong.PhanNhomLoi { Loi = 3, TenLoi = "Quản Trị SX"}
            };
            cbLoaiLoi.DataSource = PhanNhomLoi;
            foreach (var i in PhanNhomLoi)
            {
                if (i.Loi == (Item.PhanLoaiLoi == null ? 0 : Item.PhanLoaiLoi))
                {
                    cbLoaiLoi.SelectedValue = i.Loi;
                }
            }           
            BindingContext = this;
        }

        private async void GuiYeuCau_Clicked(object sender, EventArgs e)
        {
            if (Item.NguyenNhan == "")
            {
                DependencyService.Get<IMessage>().LongAlert("Vui lòng nhập nguyên nhân gây ra lỗi");
                return;
            }
            if (Item.CachXuLy == "")
            {
                DependencyService.Get<IMessage>().LongAlert("Vui lòng nhập cách xử lý");
                return;
            }
            if (Item.PhanLoaiLoi == null )
            {
                DependencyService.Get<IMessage>().LongAlert("Vui lòng chọn phân loại lỗi");
                return;
            }
            var ok = await BaseView.RunHttpClientPost("api/qltb/CapNhatThongTinSauKhiSuaLoi", Item);
            if (ok.IsSuccessStatusCode)
            {
                DependencyService.Get<IMessage>().LongAlert("Cập nhật xử lý lỗi thành công");
                await Navigation.PopAsync();
                MessagingCenter.Send(this, "capnhatxulyloi", Item.RowID);
                MessagingCenter.Send(this, "langngheduyet", DocumentType.DanhSachChoITXuLy);
            }    
                
            else
                DependencyService.Get<IMessage>().LongAlert(ok.Content.ReadAsStringAsync().Result);
        }

        private async void tiepnhan_Clicked(object sender, EventArgs e)
        {
            await new IT_Tiep_Nhan_Yeu_Cau(Item).Show();
        }
    }
    internal class PhanNhomLoi
    {
        public int Loi { get; set; }
        public string TenLoi { get; set; }
    }
}