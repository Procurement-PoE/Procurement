using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents;

namespace Procurement.View
{
    public class ResultMatchConverter : IValueConverter
    {
        private static Dictionary<string, List<GradientStop>> gradientStops;
        public ResultMatchConverter()
        {
            if (gradientStops == null)
            {
                gradientStops = new Dictionary<string, List<GradientStop>>();
                gradientStops.Add("Perfect", new List<GradientStop>());
                gradientStops.Add("Partial", new List<GradientStop>());

                gradientStops["Perfect"].Add(new GradientStop((Color)ColorConverter.ConvertFromString("#002D19"), 0));
                gradientStops["Perfect"].Add(new GradientStop((Color)ColorConverter.ConvertFromString("#004C2A"), 0.5));
                gradientStops["Perfect"].Add(new GradientStop((Color)ColorConverter.ConvertFromString("#007F46"), 1));

                gradientStops["Partial"].Add(new GradientStop((Color)ColorConverter.ConvertFromString("#260C00"), 0));
                gradientStops["Partial"].Add(new GradientStop((Color)ColorConverter.ConvertFromString("#913700"), 0.5));
                gradientStops["Partial"].Add(new GradientStop((Color)ColorConverter.ConvertFromString("#CE5200"), 1));
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal match = (decimal)value;

            Border border = new Border();
            border.BorderThickness = new System.Windows.Thickness(1);
            border.Background = getBrush(match);

            TextBlock tb = new TextBlock();
            tb.FontSize = 14;
            tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAB9066"));
            tb.Background = Brushes.Transparent;
            tb.Inlines.Add(new Bold(new Run(string.Format("Match Percentage: {0}", match.ToString("0.00")))));
            tb.VerticalAlignment = VerticalAlignment.Center;

            border.Child = tb;

            return border;
        }

        private LinearGradientBrush getBrush(decimal match)
        {
            List<GradientStop> stops = match >= 100 ? gradientStops["Perfect"] : gradientStops["Partial"];
            return new LinearGradientBrush(new GradientStopCollection(stops), new Point(0, 0), new Point(1, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
