using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class OfferingVisitor : VisitorBase
    {
        private const string TOKEN = "{Offering}";

        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<OfferingFilter>(items.OrderBy(i => i.TypeLine)));
        }
    }
}