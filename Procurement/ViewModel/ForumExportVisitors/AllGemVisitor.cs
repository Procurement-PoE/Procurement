using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;
using System.Collections.Generic;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class AllGemVisitor : VisitorBase
    {
        private const string TOKEN = "{AllGems}";
        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<AllGemsFilter>(items));
        }
    }
}
