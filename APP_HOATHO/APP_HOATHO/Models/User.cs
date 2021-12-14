using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models
{
    public class User
    {       
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? Role { get; set; }
        public int? Active { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string SourceCode { get; set; }
        public string MaXuong { get; set; }
    }
}
