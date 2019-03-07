using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class FracturedItemVisitor : VisitorBase
    {
        private const string TOKEN = "{FracturedItems}";

        public override string Visit(IEnumerable<Item> items, string current)
        {
            if (current.IndexOf(TOKEN) < 0)
                return current;

            return current.Replace(TOKEN, runFilter<FracturedItemFilter>(items.OrderBy(i => i.H)));
        }
    }
}