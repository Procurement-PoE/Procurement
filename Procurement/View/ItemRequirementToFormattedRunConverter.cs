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
    public class ItemRequirementToFormattedRunConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Requirement requirement = value as Requirement;

            Run name = new Run(requirement.Name) { Foreground = Brushes.Gray };
            Run v = new Run(requirement.Value) { Foreground = Brushes.White, FontWeight = FontWeights.Bold };

            Paragraph paragraph = new Paragraph();

            List<Run> runs = new List<Run>();
            runs.Add(name);
            runs.Add(new Run(" ") { Foreground = Brushes.Gray });
            runs.Add(v);

            if (!requirement.NameFirst)
                runs.Reverse();

            paragraph.Inlines.Add(new Run("Requires ") { Foreground = Brushes.Gray });
            paragraph.Inlines.AddRange(runs);

            return new FlowDocument(paragraph);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
