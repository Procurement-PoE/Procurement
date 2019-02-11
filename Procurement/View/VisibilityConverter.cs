using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Procurement.View
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = Visibility.Visible;

            if (value == null)
            {
                result = Visibility.Hidden;
            }
            else
            {
                var str = value as string;

                if (str != null)
                {
                    result = str.Length > 0 ? Visibility.Visible : Visibility.Hidden;
                }
                else
                {
                    var enumerable = value as IEnumerable;

                    if (enumerable != null)
                    {
                        result = enumerable.Cast<object>().Any() ? Visibility.Visible : Visibility.Hidden;
                    }
                    else  if (value is int)
                    {
                        result = ((int)value) > 0 ? Visibility.Visible : Visibility.Hidden;
                    }
                    else if (value is double)
                    {
                        result = ((double)value) > 0 ? Visibility.Visible : Visibility.Hidden;
                    }
                    else if (value is bool)
                    {
                        result = ((bool) value) ? Visibility.Visible : Visibility.Hidden;

                        if (parameter != null && parameter.ToString() == "Invert")
                        {
                            if (result == Visibility.Visible)
                                result = Visibility.Collapsed;
                            if (result == Visibility.Hidden)
                                result = Visibility.Visible;
                        }
                    }
                }
            }

            if (parameter != null && parameter.ToString() == "CollapseWhenFalse" && result == Visibility.Hidden)
            {
                result = Visibility.Collapsed;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
