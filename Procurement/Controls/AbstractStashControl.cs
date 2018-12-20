using System.Collections.Generic;
using System.Linq;
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
        public bool Ready;

        public virtual void RefreshTab(string accountName)
        {
            ApplicationState.Stash[ApplicationState.CurrentLeague].RefreshTab(ApplicationState.Model,
                ApplicationState.CurrentLeague, TabNumber, accountName);
        }

        public int TabNumber { get; set; }

        public int FilterResults { get; set; }

        public readonly Dictionary<Item, ItemDisplay> StashByLocation = new Dictionary<Item, ItemDisplay>();

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

        public virtual void ForceUpdate()
        {
            FilterResults = !Filter.Any() ? -1 : 0;

            foreach (var item in StashByLocation)
            {
                if (search(item.Key))
                {
                    item.Value.ViewModel.IsItemInFilter = true;
                    FilterResults++;
                }
                else
                {
                    item.Value.ViewModel.IsItemInFilter = false;
                }
            }

            UpdateLayout();
        }

        private bool search(Item item)
        {
            if (Filter.Count() == 0)
            {
                return false;
            }

            return Filter.All(filter => filter.Applicable(item));
        }
    }
}