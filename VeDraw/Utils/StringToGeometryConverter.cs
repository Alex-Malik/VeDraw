using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace VeDraw.Utils
{
    class StringToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return Geometry.Parse(value as string);
            }
            else
            {
                return Geometry.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Geometry)
            {
                return ((Geometry)value).ToString();
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
