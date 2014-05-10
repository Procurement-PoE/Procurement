using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using POEApi.Model;

namespace Procurement.ViewModel
{
    class ValuesPlusNameStrategy : DisplayModeStrategyBase
    {
        public ValuesPlusNameStrategy(Property property)
            : base(property)
        { }

        public override Block Get()
        {
            Paragraph ret = new Paragraph(new Run(property.Values[0].Item1) { Foreground = Brushes.Gray });
            if (property.Values.Count > 0)
                ret.Inlines.Add(new Run(" " + property.Name) { Foreground = Brushes.White});

            return ret;
        }
    }
}
