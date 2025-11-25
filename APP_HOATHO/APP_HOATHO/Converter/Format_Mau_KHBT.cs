using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace APP_HOATHO.Converter
{
    //for mat màu của grid kế hoạch bảo trì, nếu chưa thì đỏ, rồi thì xanh
    public class Format_Mau_KHBT : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool _value = (bool)value;          
            if (_value == false )
                return Color.Red;
            else if (_value == true)
                return Color.Green;
            return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Format_Mau_XuatKhoCayVai : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StatusFormatColor  _value = (StatusFormatColor)value;
            if (_value == StatusFormatColor.ChuaHoanThanh)
                return Color.Black;
            else if (_value ==  StatusFormatColor.DoDang)
                return Color.Red;
            else if (_value == StatusFormatColor.HoanThanh)
                return Color.Green;
            return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OpacityDisable : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool _value = (bool)value;
            if (_value == false)
                return 0.3;
            else
                return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public enum StatusFormatColor
    {
        TatCa =-1,
        ChuaHoanThanh = 0,
        DoDang =1,
        HoanThanh=2,
        DaDinhVi= 3,
        ChuaDinhVi=4
    }
}
