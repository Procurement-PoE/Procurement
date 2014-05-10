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
            var viewModel = value as ItemHoverViewModel;

            if (viewModel == null || !viewModel.HasRequirements)
            {
                return null;
            }

            var paragraph = new Paragraph();

            foreach (var requirement in viewModel.Requirements)
            {
                var runs = new List<Run>
                {
                    new Run(requirement.Name) {Foreground = Brushes.Gray},
                    new Run(" ") {Foreground = Brushes.Gray},
                    new Run(requirement.Value) {Foreground = Brushes.White}
                };

                if (!requirement.NameFirst)
                    runs.Reverse();

                if (paragraph.Inlines.Count > 0)
                {
                    paragraph.Inlines.Add(new Run(", ") { Foreground = Brushes.Gray });
                }

                paragraph.Inlines.AddRange(runs);
            }

            paragraph.Inlines.InsertBefore(paragraph.Inlines.FirstInline, new Run("Requires ") { Foreground = Brushes.Gray });

            return new FlowDocument(paragraph);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
