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

namespace Procurement.View
{
    public class ItemExplicitModsToFormattedRunConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string mod = value as string;
            return new FlowDocument(new Paragraph(new Run(mod) { Foreground = Brushes.Turquoise, FontWeight = FontWeights.Bold, BaselineAlignment = BaselineAlignment.Center }));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

