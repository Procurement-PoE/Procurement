using POEApi.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Procurement.View
{
    public class CurrencyCraftingSlotScalingConverter : IValueConverter
    {
        private const double WidthScale = 38.3388;
        private const double HeightScale = 38.89066666666667;

        private const string Width = "width";
        private const string Height = "height";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Item item = value as Item;

            if (item == null)
                return 0;

            if (parameter?.ToString() == Width)
                return item.W * WidthScale;

            if (parameter?.ToString() == Height)
                return item.H * HeightScale;

            return 47; // 1h1w
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
