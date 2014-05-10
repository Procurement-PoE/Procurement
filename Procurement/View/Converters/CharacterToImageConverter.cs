using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using POEApi.Model;

namespace Procurement.View
{
    public class CharacterToImageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string avatarFormat = "pack://application:,,,/Images/Avatars/{0}.png";
            string avatar = (value as Character).Class;

            var bitmap = new BitmapImage(new Uri(string.Format(avatarFormat, avatar), UriKind.Absolute));
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.Freeze();

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
