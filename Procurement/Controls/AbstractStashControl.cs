using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
            throw new NotImplementedException();
        }

        public abstract void ForceUpdate();
    }
}