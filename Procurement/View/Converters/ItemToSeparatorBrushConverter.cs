using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

using POEApi.Model;

namespace Procurement.View
{
    public class ItemToSeparatorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as Item;
            string color;

            if (item is Gem)
                color = "305C56";
            else if (item is Prophecy)
                color = "822B76";
            else if (item is QuestItem)
                color = "34532B";
            else if (item is Currency || item is Sextant || item is Essence || item is Fossil || item is Resonator || item is BreachSplinter || item is LegionSplinter)
                color = "5F573F";
            else if (item.Rarity == Rarity.Unique)
                color = "85442B";
            else if (item.Rarity == Rarity.Rare)
                color = "85622B";
            else if (item.Rarity == Rarity.Magic)
                color = "443470";
            else
                color = "52463A";

            return new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0),
                GradientStops = new GradientStopCollection()
                {
                    new GradientStop((Color)ColorConverter.ConvertFromString("#00" + color), 0.25),
                    new GradientStop((Color)ColorConverter.ConvertFromString("#FF" + color), 0.50),
                    new GradientStop((Color)ColorConverter.ConvertFromString("#00" + color), 0.75)
                }
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}