using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models
{
    public class DocumentBase 
    {
        public int RowID { get; set; } 
        public DateTime PostingDate { get; set; }
        public string ChungTuNgoai { get; set; }
        public string No_ { get; set; }
        public string Description { get; set; }
        public string TenNhaMay { get; set; }
        public string CustomerName { get; set; }
        public string UserID { get; set; }
    }
}
