using System.Windows.Data;
using System.Collections.Generic;
using Procurement.ViewModel.Filters;
using System.Linq;

namespace Procurement.View
{
    public class FilterListToNameConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable<IFilter> filters = (IEnumerable<IFilter>)value;
            return string.Join(", ", filters.Select(f => f.Keyword).ToArray());
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
