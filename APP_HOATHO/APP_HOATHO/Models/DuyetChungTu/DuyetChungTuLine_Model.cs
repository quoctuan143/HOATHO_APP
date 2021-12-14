using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.DuyetChungTu
{
    public class DuyetChungTuLine_Model
    {
        public string No_ { get; set; }
        public string Name { get; set; }
        public double   Quantity { get; set; }
        public string Width { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Art { get; set; }
        public string UnitOfMeasure { get; set; }
        public double? Amount { get; set; }

    }
}
