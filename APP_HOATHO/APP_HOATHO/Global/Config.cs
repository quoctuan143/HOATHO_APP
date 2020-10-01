using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APP_HOATHO.Global
{
    public class Config
    {
        public static string URL = "http://thietbi.hoatho.com.vn/";
        public static string User = "User";
        public static string Password = "Password";
        public static string Role = "Role";
        public static string PhoneNumber = "PhoneNumber";
        public static string Token { get; set; }
        public static string NhaMay = "NhaMay";
        public static string MaXuong = "MaXuong";
        public static System.Net.Http.HttpClient client;
      
        public Config()
        {
            if (client == null)
            {
                client = new System.Net.Http.HttpClient();
            }
            
        }
        

    }

    public enum DialogReturn
    {
        OK = 0,
        Cancel = 1,
        Repeat = 2,
        Stop = 3
    }
}
