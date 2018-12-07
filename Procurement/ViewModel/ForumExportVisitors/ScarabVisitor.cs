using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
     class ScarabVisitor : VisitorBase
    {
        private const string TOKEN = "{Scarab}";

        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<ScarabFilter>(items.OrderBy(i => i.TypeLine)));
        }
    }
}
