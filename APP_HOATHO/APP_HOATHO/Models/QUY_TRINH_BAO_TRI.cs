using APP_HOATHO.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace APP_HOATHO.Models
{
   public class QUY_TRINH_BAO_TRI : BaseViewModel

    {
        public string Code { get; set; }
        public string Name { get; set; } 
        string _description;
        public string Description { get => _description; set { _description = value;
                if (_description !=  null)
                {
                    _description = _description.Replace("rn", System.Environment.NewLine);
                    OnPropertyChanged("Description");
                }    
            } }
        public bool? IsCheck { get; set; } 
    }
}
