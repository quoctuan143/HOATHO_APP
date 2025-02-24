using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace APP_HOATHO.Models
{
    public  class LookupValue
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
    }

    public class LookupValueInt
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class NhaMayModel
    {
        public string Code { get; set; }
        public string Name { get; set; }        
    }
    public class XuongModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string WorkCenterCode { get; set; }
    }
    public class ToSanXuatModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string WorkShopCode { get; set; }
    }

    public class NhaMayOfUser
    {
        public ObservableCollection<NhaMayModel> NhaMays { get; set; }
        public ObservableCollection<XuongModel> Xuongs { get; set; }
        public ObservableCollection<ToSanXuatModel> Tos { get; set; }
    }
}
