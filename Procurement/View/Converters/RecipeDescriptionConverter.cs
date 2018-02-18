using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using Procurement.ViewModel.Recipes;

namespace Procurement.View
{
    public class RecipeDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            KeyValuePair<string, List<RecipeResult>> item = (KeyValuePair<string, List<RecipeResult>>)value;
            int numCompleteResults = item.Value.Count(i => i.PercentMatch == 100);
            return string.Format("{0} ({1}/{2} Results)", item.Key, numCompleteResults, item.Value.Count);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
