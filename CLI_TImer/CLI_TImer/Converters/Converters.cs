using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CLI_Timer.Converters
{
    public class HexToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType = null, object parameter = null, CultureInfo cultureInfo = null) 
        {
            var hexColor = value as string;
            if (string.IsNullOrEmpty(hexColor)) return null;

            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            var Color = (value as SolidColorBrush).Color.ToString();

            return Color;
        }
    }

    public class HexToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            var hexColor = value as string;
            if (string.IsNullOrEmpty(hexColor)) return null;

            var rgb = System.Convert.ToInt32(hexColor.Remove(0, 1), 16);

            return Color.FromRgb((byte)((rgb >> 16) & 0xFF), (byte)((rgb>> 8) & 0xFF), (byte)(rgb& 0xFF));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }
}
