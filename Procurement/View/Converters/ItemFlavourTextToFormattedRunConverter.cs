using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using POEApi.Model;
using System.Windows.Documents;
using Procurement.ViewModel;
using System.Windows.Media;
using System.Windows;
using System.Text.RegularExpressions;

namespace Procurement.View
{
    public class ItemFlavourTextToFormattedRunConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var viewModel = value as ItemHoverViewModel;

            if (viewModel == null || string.IsNullOrEmpty(viewModel.FlavourText))
                return null;

            string flavourtext = viewModel.FlavourText;
            Paragraph paragraph = new Paragraph();

            if (flavourtext.StartsWith("<size:") || flavourtext.StartsWith("<smaller>{"))
                flavourtext = flavourtext.TrimEnd('}').Substring(10);

            if (flavourtext.Contains("<d"))
            {
                Match match = Regex.Match(flavourtext, "(.*?)<default>{(.+?)}(.*?)");

                if (match.Success)
                {
                    paragraph.Inlines.Add(new Run(match.Groups[1].Value) { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AF6025")), BaselineAlignment = BaselineAlignment.Center, FontStyle = FontStyles.Italic });
                    paragraph.Inlines.Add(new Run(match.Groups[2].Value) { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F7F7F")), BaselineAlignment = BaselineAlignment.Center, FontStyle = FontStyles.Italic });
                    paragraph.Inlines.Add(new Run(match.Groups[3].Value) { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AF6025")), BaselineAlignment = BaselineAlignment.Center, FontStyle = FontStyles.Italic });

                    return new FlowDocument(paragraph);
                }
            }

            return new FlowDocument(new Paragraph(new Run(flavourtext) { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AF6025")), BaselineAlignment = BaselineAlignment.Center, FontStyle = FontStyles.Italic }));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
