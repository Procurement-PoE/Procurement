using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using POEApi.Model;

namespace Procurement.ViewModel
{
    internal class StringFormatStrategy : DisplayModeStrategyBase
    {
        public StringFormatStrategy(Property property)
            : base(property)
        { }

        public override Block Get()
        {
            Paragraph ret = new Paragraph();

            var parts = property.Name.Split('%');
            ret.Inlines.Add(new Run(parts[0]) { Foreground = Brushes.Gray });
            ret.Inlines.Add(new Run(property.Values[0].Item1) { Foreground = Brushes.White } );

            // Heist contracts (and possibly other items now, too) appear to
            // have only one part parsed from the property.
            if (parts.Length == 1)
                return ret;

            ret.Inlines.Add(new Run(parts[1].Substring(1)) { Foreground = Brushes.Gray });

            if (property.Values.Count == 1)
                return ret;

            ret.Inlines.Add(new Run(property.Values[1].Item1) { Foreground = Brushes.White } );
            ret.Inlines.Add(new Run(parts[2].Substring(1)) { Foreground = Brushes.Gray });

            return ret;
        }
    }
}