using APP_HOATHO.Models.Nha_May_Soi;

namespace APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho
{
    public class PurchaseLine_Model : Bindable
    {
        public string No_ { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public double Quantity { get; set; } 
    }
}