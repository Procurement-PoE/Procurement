using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using POEApi.Model;
using System.Windows.Media;

namespace Procurement.View
{
    public class ItemToColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Gear gear = value as Gear;
            double opacity = 1;
            if (parameter != null)
                opacity = double.Parse(parameter.ToString());
            if (gear != null)
            {
                switch (gear.Quality)
                {
                    case Quality.White:
                        return new SolidColorBrush(Colors.White) { Opacity = opacity };
                    case Quality.Magic:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8888F1")) { Opacity = opacity };
                    case Quality.Rare:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1FF77")) { Opacity = opacity };
                    case Quality.Legendary:
                        return new SolidColorBrush(Colors.Orange) { Opacity = opacity };
                }
            }

            Currency currency = value as Currency;
            if (currency != null)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDF8E")) { Opacity = opacity };

            Gem gem = value as Gem;
            if (gem != null)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1BA29B")) { Opacity = opacity };

            Map map = value as Map;
            if (map != null)
                return new SolidColorBrush(Colors.PaleGreen) { Opacity = opacity };

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
