using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using POEApi.Model;

namespace Procurement.ViewModel
{
    internal class NamePlusValueStrategy : DisplayModeStrategyBase
    {
        public NamePlusValueStrategy(Property property)
            : base(property)
        { }

        public override Block Get()
        {
            Paragraph ret = new Paragraph();
            if (property.Values.Count == 0)
            {
                if (property.Name.StartsWith("Requires <unmet>{"))
                {
                    ret.Inlines.Add(new Run("Requires ") { Foreground = Brushes.Gray });
                    ret.Inlines.Add(new Run(property.Name[17].ToString()) { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D20000")) });
                    ret.Inlines.Add(new Run(property.Name.Substring(19)) { Foreground = Brushes.Gray });
                }
                else
                {
                    ret.Inlines.Add(new Run(property.Name) { Foreground = Brushes.Gray });
                }
            }
            else
            {
                ret.Inlines.Add(new Run(property.Name + ":") { Foreground = Brushes.Gray });

                for (int i = 0; i < property.Values.Count; i++)
                {
                    ret.Inlines.Add(new Run(" " + (property.Values[i].Item1)) { Foreground = base.displayColorMappings[property.Values[i].Item2]});
                    if (i != property.Values.Count - 1)
                        ret.Inlines.Add(new Run(", ") { Foreground = Brushes.Gray });
                }
            }
            return ret;
        }
    }
}
