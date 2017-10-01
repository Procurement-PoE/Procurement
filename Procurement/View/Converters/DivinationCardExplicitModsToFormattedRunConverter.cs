using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using Procurement.ViewModel;
using System.Windows.Media;
using System.Windows.Data;
using System.Text.RegularExpressions;

namespace Procurement.View
{
    class DivinationCardExplicitModsToFormattedRunConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var mod = value as String;

            var paragraph = new Paragraph();

            var runs = new List<Run>();

            var count = 0;
            
            foreach (Match part in Regex.Matches(mod, "<(?<type>.*?)>\\{(?<text>.*?)\\}"))
            {
                switch (part.Groups["type"].Value)
                {
                    case "default":
                        runs.Add(new Run(part.Groups["text"].Value + (count > 0 ? " " : "")) { Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#7f7f7f") });
                        break;
                    case "uniqueitem":
                        runs.Add(new Run(part.Groups["text"].Value + (count > 0 ? " " : "")) { Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#af6025") });
                        break;
                    case "rareitem":
                        runs.Add(new Run(part.Groups["text"].Value + (count > 0 ? " " : "")) { Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffff77") });
                        break;
                    case "magicitem":
                        runs.Add(new Run(part.Groups["text"].Value + (count > 0 ? " " : "")) { Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#8888ff") });
                        break;
                    case "whiteitem":
                        runs.Add(new Run(part.Groups["text"].Value + (count > 0 ? " " : "")) { Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#c8c8c8") });
                        break;
                    case "gemitem":
                        runs.Add(new Run(part.Groups["text"].Value + (count > 0 ? " " : "")) { Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#1ba29b") });
                        break;
                    case "currencyitem":
                        runs.Add(new Run(part.Groups["text"].Value + (count > 0 ? " " : "")) { Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#aa9e82") });
                        break;
                    case "questitem":
                        runs.Add(new Run(part.Groups["text"].Value + (count > 0 ? " " : "")) { Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#4ae63a") });
                        break;
                    case "crafted":
                        runs.Add(new Run(part.Groups["text"].Value + (count > 0 ? " " : "")) { Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#b4b4ff") });
                        break;
                    case "divination":
                        runs.Add(new Run(part.Groups["text"].Value + (count > 0 ? " " : "")) { Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#aae6e6") });
                        break;
                    case "corrupted":
                        runs.Add(new Run(part.Groups["text"].Value + (count > 0 ? " " : "")) { Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#d20000") });
                        break;
                    case "normal":
                        runs.Add(new Run(part.Groups["text"].Value + (count > 0 ? " " : "")) { Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#c8c8c8") });
                        break;
                    default:
                        runs.Add(new Run(part.Groups["text"].Value + (count > 0 ? " " : "")) { Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#c8c8c8") });
                        break;
                }

                count++;


            }

            paragraph.Inlines.AddRange(runs);

            return new FlowDocument(paragraph);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
