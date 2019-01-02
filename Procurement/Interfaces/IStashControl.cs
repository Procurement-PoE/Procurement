using System.Collections.Generic;
using Procurement.ViewModel.Filters;

namespace Procurement.Interfaces
{
    public interface IStashControl
    {
        void RefreshTab(string accountName);
        int TabNumber { get; set; }
        int ItemsMatchingFiltersCount { get; set; }
        List<IFilter> Filters { get; set; }

        void ForceUpdate();
    }
}