using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;
using System.Collections.Generic;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class CurrencyVisitor : VisitorBase
    {
        private const string TOKEN = "{Currency}";

        public override string Visit(IEnumerable<Item> items, string current)
        {                     
            return current.Replace(TOKEN, runFilter<CurrencyFilter>(items.OrderBy(i => i.TypeLine)));
        }
    }
}
