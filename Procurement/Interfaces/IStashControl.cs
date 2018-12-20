using System.Collections.Generic;
using System.Windows.Controls;
using Procurement.ViewModel.Filters;

namespace Procurement.Interfaces
{
    public interface IStashControl
    {
        void RefreshTab(string accountName);
        int TabNumber { get; set; }
        int FilterResults { get; set; }
        List<IFilter> Filter { get; set; }

        void ForceUpdate();
    }
}