using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models
{
    public class FileUpload
    {
        public int imageid
        {
            get;
            set;
        }
        public string imagename
        {
            get;
            set;
        }
        public byte[] imagedata
        {
            get;
            set;
        } //this prop
    }
}
