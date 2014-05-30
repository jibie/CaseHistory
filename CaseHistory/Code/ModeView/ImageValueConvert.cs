using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace OfIllness.ModeView
{
    public class ImageValueConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strPicUrl = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, value);
            BitmapImage bi = new BitmapImage(new Uri(strPicUrl, UriKind.Absolute));
            bi.Freeze();
            return bi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
