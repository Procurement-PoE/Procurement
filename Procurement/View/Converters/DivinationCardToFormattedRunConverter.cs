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
    public class DivinationCardToFormattedRunConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var viewModel = value as ItemHoverViewModel;

            if (viewModel == null || !viewModel.IsDivinationCard || viewModel.ExplicitMods.Count == 0)
                return null;

            var mods = viewModel.ExplicitMods;

            Paragraph paragraph = new Paragraph();

            Match match = Regex.Match(mods.First(), "<(augmented|corrupted|currencyitem|default|divination|gemitem|magicitem|normal|prophecy|rareitem|uniqueitem|whiteitem)>{(.+?)}(\r\n)?");

            while (match.Success)
            {
                string colortext = match.Groups[1].Value;
                string color = "";

                if (colortext.Equals("augmented"))
                    color = "#8888FF";
                else if (colortext.Equals("corrupted"))
                    color = "#D20000";
                else if (colortext.Equals("currencyitem"))
                    color = "#AA9E82";
                else if (colortext.Equals("default"))
                    color = "#7F7F7F";
                else if (colortext.Equals("divination"))
                    color = "#AAE6E6";
                else if (colortext.Equals("gemitem"))
                    color = "#1BA29B";
                else if (colortext.Equals("magicitem"))
                    color = "#8888FF";
                else if (colortext.Equals("normal"))
                    color = "#C8C8C8";
                else if (colortext.Equals("prophecy"))
                    color = "#B54BFF";
                else if (colortext.Equals("rareitem"))
                    color = "#FFFF77";
                else if (colortext.Equals("uniqueitem"))
                    color = "#AF6025";
                else if (colortext.Equals("whiteitem"))
                    color = "#C8C8C8";

                paragraph.Inlines.Add(new Run(match.Groups[2].Value) { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)), BaselineAlignment = BaselineAlignment.Center });

                if (match.Groups[3].Value.Equals("\r\n"))
                    paragraph.Inlines.Add(new Run("\r\n"));

                match = match.NextMatch();
            }

            return new FlowDocument(paragraph);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
