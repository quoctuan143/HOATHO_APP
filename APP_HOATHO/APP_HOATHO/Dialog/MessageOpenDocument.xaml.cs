using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_HOATHO.Global;
using APP_HOATHO.Models.DuyetChungTu;
using APP_HOATHO.Views.DuyetChungTu;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Dialog
{
	[XamlCompilation(XamlCompilationOptions.Compile)]	
    public partial class MessageOpenDocument : PopupPage
    {
        TaskCompletionSource<DialogReturn> _tsk = null;
        string _sochungtu;
        DocumentType _loaiphieu;
        public MessageOpenDocument(string noidung, string sochungtu,DocumentType loaiphieu)
        {
            InitializeComponent();
            lblNoiDung.Text = noidung;
            _sochungtu = sochungtu;
            _loaiphieu = loaiphieu;
        }
        private async void btnOK_Clicked(object sender, EventArgs e)
        {
            DuyetChungTuModel item = new DuyetChungTuModel { No_ = _sochungtu  };
            if (_loaiphieu == DocumentType.DuyetDatMuaPhuTung)
               await Navigation.PushAsync(new DuyetChungTuPhuTung_Line(item, _loaiphieu ));
            else
              await  Navigation.PushAsync(new DuyetChungTu_Line(item , _loaiphieu));            
            await Navigation.PopPopupAsync();
            _tsk.SetResult(DialogReturn.OK);
        }
        public async Task<DialogReturn> Show()
        {
            _tsk = new TaskCompletionSource<DialogReturn>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }
    }
}