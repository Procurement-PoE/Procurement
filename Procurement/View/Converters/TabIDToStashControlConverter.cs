using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using POEApi.Model;
using Procurement.Controls;
using Procurement.Utility;
using Procurement.ViewModel.Filters;
using Procurement.ViewModel;

namespace Procurement.View
{
    public class TabIDToStashControlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TabInfo item = value as TabInfo;
            Grid g = new Grid();
            g.Children.Add(new StashControl() { TabNumber = item.ID });
            return g;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TabIDToStashControlFiltered : IValueConverter
    {
        public static Dictionary<string, Grid> cache;
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            cache = cache ?? new Dictionary<string, Grid>();

            Item item = value as Item;
            string key = getKey(item);
            if (cache.ContainsKey(key))
                return cache[key];

            int inventoryId = int.Parse(item.inventoryId.Replace("Stash", "")) - 1;
            Grid g = new Grid();

            StashControl control = new StashControl() { TabNumber = inventoryId };
            Tab tab = ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs.Find(t => t.i == inventoryId);
            Image tabImage = getImage(tab, true);

            control.SetValue(StashControl.FilterProperty, new List<IFilter>() { new ItemFilter(item) });
            control.ForceUpdate();
            RowDefinition imageRow = new RowDefinition();
            imageRow.Height = new GridLength(26);
            g.RowDefinitions.Add(imageRow);
            g.RowDefinitions.Add(new RowDefinition());
            tabImage.SetValue(Grid.RowProperty, 0);
            control.SetValue(Grid.RowProperty, 1);
            g.Children.Add(tabImage);
            g.Children.Add(control);
            cache.Add(key, g);

            return g;
        }

        private string getKey(Item item)
        {
            return string.Concat(item.inventoryId, ":", item.X, ":", item.Y, ":", ApplicationState.CurrentLeague);
                    
        }

        private Image getImage(Tab tab, bool mouseOver)
        {
            return StashHelper.GenerateTabImage(tab, mouseOver);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}