using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Media;
using POEApi.Model;

namespace Procurement.ViewModel
{
    internal abstract class DisplayModeStrategyBase : IDisplayModeStrategy
    {
        protected Dictionary<int, SolidColorBrush> displayColorMappings;
        protected Property property { get; set; }
        public DisplayModeStrategyBase(Property property)
        {
            this.property = property;
            displayColorMappings = new Dictionary<int, SolidColorBrush>();
            displayColorMappings.Add(0, Brushes.White);
            displayColorMappings.Add(1, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8888F1")));
            displayColorMappings.Add(4, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#960003"))); //Red fire
            displayColorMappings.Add(5, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2943c6"))); //Blue cold
            displayColorMappings.Add(6, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f2bc01"))); //Yellow lightning
            displayColorMappings.Add(7, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D02090"))); //Pink chaos
            displayColorMappings.Add(8, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"))); //Imprint White
            displayColorMappings.Add(9, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"))); //White
        }

        public abstract Block Get();
    }
}
