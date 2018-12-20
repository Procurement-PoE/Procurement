using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using POEApi.Model;
using Procurement.Interfaces;
using Procurement.ViewModel.Filters;

namespace Procurement.Controls
{
    public abstract class AbstractStashControl : UserControl, IStashControl
    {
        public virtual void RefreshTab(string accountName)
        {
            ApplicationState.Stash[ApplicationState.CurrentLeague].RefreshTab(ApplicationState.Model,
                ApplicationState.CurrentLeague, TabNumber, accountName);
        }

        public int TabNumber { get; set; }

        public int FilterResults { get; set; }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(IEnumerable<IFilter>), typeof(StashControl), null);

        public List<Item> Stash { get; set; }

        public List<IFilter> Filter
        {
            get { return (List<IFilter>) GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public void SetPremiumTabBorderColour()
        {
            var tab = ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs[TabNumber];

            if (Border == null)
            {
                return;
            }

            if (tab.Colour == null)
            {
                return;
            }

            var color = tab.Colour.WpfColor;
            
            Border.BorderBrush = new SolidColorBrush(color);
        }

        //Subsequent controls will give me a border
        public abstract Border Border { get; }

        public abstract void ForceUpdate();
    }
}