using System;
using System.Globalization;
using System.Windows.Data;

namespace Hirdavatci
{
    public class BarcodeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is string barcode ? barcode.GenerateBarCodeImage() : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
