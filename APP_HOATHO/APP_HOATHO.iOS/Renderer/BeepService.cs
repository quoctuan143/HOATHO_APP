using APP_HOATHO.Interface;
using APP_HOATHO.iOS.Renderer;
using AudioToolbox;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(BeepService))]
namespace APP_HOATHO.iOS.Renderer
{
    public class BeepService : IBeepService
    {
        public void Beep()
        {
            // Sử dụng các API iOS để phát ra âm thanh "beep"
            var beepSound = new SystemSound(1108);
            beepSound.PlaySystemSound();
        }
    }
}