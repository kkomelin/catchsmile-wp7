using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;

namespace CatchSmile.Tools
{
    public class IsoFileConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return FileStorage.ReadBitmapFromIsolatedStorage(value.ToString());
            }

            throw new NotSupportedException();
        }

       /* public string ImageToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {

                // Convert Image to byte[]

               // image.Save(ms, format);

                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String

               // string base64String = Convert.ToBase64String(imageBytes);

              //  return base64String;

            }

        }*/

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            throw new NotSupportedException();

        }

    }

}
