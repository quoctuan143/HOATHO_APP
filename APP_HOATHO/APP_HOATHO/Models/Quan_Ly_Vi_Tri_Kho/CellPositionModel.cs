using APP_HOATHO.Models.Nha_May_Soi;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_HOATHO.Models.Quan_Ly_Vi_Tri_Kho
{
    public class CellPositionModel : Bindable
    {
        public string Code { get; set; }
        string _positionId;
        public string PositionId { get => _positionId; set => SetProperty(ref _positionId ,value); }
    }
}
