using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Hirdavatci
{
    public class MalzemeValueToProgressBarColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Malzeme malzeme)
            {
                double oran = (double)malzeme.KalanAdet / malzeme.ToplamAdet;
                if (oran > 0.66)
                {
                    return Brushes.Green;
                }
                if (oran is < 0.66 and > 0.33)
                {
                    return Brushes.Orange;
                }
                if (oran < 0.33)
                {
                    return Brushes.Red;
                }
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}