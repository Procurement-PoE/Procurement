using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class IncursionVialVisitor : VisitorBase
    {
        private const string TOKEN = "{IncursionVial}";

        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<IncursionVialsFilter>(items.OrderBy(i => i.TypeLine)));
        }
    }
}