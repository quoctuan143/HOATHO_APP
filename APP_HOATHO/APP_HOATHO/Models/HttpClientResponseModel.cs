using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace APP_HOATHO.Models
{
    /// <summary>
    /// chức năng này dùng để trả về dữ liệu khi thực hiện API
    /// </summary>
   public class HttpClientResponseModel<T> where T : class 
    {
        public HttpResponseMessage Status { get; set; } 
        public ObservableCollection<T> Lists { get; set; }
    }
}
