using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using POEApi.Model;

namespace Procurement.View
{
    public class CharacterToLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Character c = value as Character;
            return string.Format("Level {0} {1}", c.Level, c.Class);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
