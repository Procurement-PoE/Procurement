using System;
using System.Collections.Generic;
using System.Windows.Data;
using Procurement.ViewModel.Recipes;

namespace Procurement.View
{
    public class RecipeDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            KeyValuePair<string, List<RecipeResult>> item = (KeyValuePair<string, List<RecipeResult>>)value;
            return string.Format("{0} ({1} Results)", item.Key, item.Value.Count);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
