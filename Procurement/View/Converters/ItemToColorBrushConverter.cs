using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

using POEApi.Model;

namespace Procurement.View
{
    public class ItemToColorBrushConverter : IValueConverter
    {
        private static readonly Color NormalItemColor = Colors.White;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Gear gear = value as Gear;
            double opacity = 1;
            if(parameter != null)
                opacity = double.Parse(parameter.ToString(), CultureInfo.InvariantCulture);
            if(gear != null)
            {
                switch(gear.Rarity)
                {
                    case Rarity.Normal:
                        return new SolidColorBrush(NormalItemColor) {Opacity = opacity};
                    case Rarity.Magic:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8888F1")) { Opacity = opacity };
                    case Rarity.Rare:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1FF77")) { Opacity = opacity };
                    case Rarity.Unique:
                        return new SolidColorBrush(Colors.Orange) {Opacity = opacity};
                    case Rarity.Relic:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6AAD6A")) { Opacity = opacity};
                }
            }

            Currency currency = value as Currency;
            if(currency != null)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDF8E")) {Opacity = opacity};

            Gem gem = value as Gem;
            if(gem != null)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1BA29B")) {Opacity = opacity};

            Map map = value as Map;
            if(map != null)
                return new SolidColorBrush(Colors.PaleGreen) {Opacity = opacity};

            Prophecy prophecy = value as Prophecy;
            if(prophecy != null)
                return new SolidColorBrush(Colors.Purple) { Opacity = opacity };

            //This was throwing an exception and killing the application - this is not ideal - I will default to normal item color when handle=ing new types
            return new SolidColorBrush(NormalItemColor) {Opacity = opacity};
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}