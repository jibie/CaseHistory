using System;
using System.Windows.Data;
using System.IO;
using System.Windows.Media.Imaging;
namespace OfIllness.ModeView
{
    public class ImageThumbnail : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapSource bitSource = null;
            string picPath = value.ToString();
            if (File.Exists(picPath))
            {
                Uri picUri = new Uri(picPath);
                BitmapDecoder bd = BitmapDecoder.Create(picUri, BitmapCreateOptions.DelayCreation, BitmapCacheOption.Default);

                if (bd.Frames[0].Thumbnail != null)
                {
                    bitSource = bd.Frames[0];
                }
                else
                {//todo 可以研究 BitmapImage.Create(
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.UriSource = picUri;
                    bi.EndInit();
                    bitSource = bi;

                }
                //bitSource.Freeze();
            }
            return bitSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
