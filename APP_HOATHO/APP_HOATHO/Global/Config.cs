using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace APP_HOATHO.Global
{
    public class Config
    {
        //public static string URL = "http://192.168.1.21:8889/";
        public static string URL =  "https://htapis.hoatho.com.vn:59443/";
        public static string User = "User";
        public static string Password = "Password";
        public static string Role = "Role";
        public static string FullName = "FullName";
        public static string PhoneNumber = "PhoneNumber";
        public static string Email = "Email";
        public static string EmployeeNo = "EmployeeNo";
        public static string Token { get; set; }
        public static string NhaMay = "NhaMay";
        public static string MaXuong = "MaXuong";
        public static System.Net.Http.HttpClient client;       
        public Config()
        {
            //ServicePointManager.SecurityProtocol |= (SecurityProtocolType.Ssl3 |
            //  SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 |
            //                                SecurityProtocolType.Tls);
            //ServicePointManager.DefaultConnectionLimit = 20;
            if (client == null)
            {
                client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(URL);
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

    public enum DocumentType
    {
        DuyetLCP = 0,
        DuyetLCP_GC =1,
        DuyetMuaHang =2,
        DuyetThanhToan=3,
        DuyetDatMuaPhuTung = 4,
        MoLaiDatMua =5,
        MoLaiLCP_FOB =6,
        MoLaiLCP_GC = 7    ,
        KiDienTuPhuTung = 8,
        KiDienTuThietBi = 9,
        DuyetYeuCauThueThietBi = 10,
        DuyetTraThietBi = 11,
        KyDienTuTongHopThietBiNhaMay =12,
        DanhSachChoITXuLy = 13
    }

    public enum AppoveType
    {
        Admin = 0,
        TruongPhongKinhDoanh = 1,
        GiamDocNhaMay = 2,
        CoDien = 3,
        KiThuatDauTu = 4,
        TongGiamDoc = 5
    }
}
