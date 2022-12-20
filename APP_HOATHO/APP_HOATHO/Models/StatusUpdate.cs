using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models
{
    public class StatusUpdate
    {
       public  enum Status 
        {
            None =0,
            Add = 1,
            Remove = 2,
            Modified = 3
        }
    }
}
