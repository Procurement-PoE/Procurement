using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class UnknownItemVisitor : VisitorBase
    {
        private const string TOKEN = "{UnknownItems}";

        public override string Visit(IEnumerable<Item> items, string current)
        {
            if (current.IndexOf(TOKEN) < 0)
                return current;

            return current.Replace(TOKEN, runFilter<UnknownItemFilter>(items.OrderBy(i => i.H)));
        }
    }
}
