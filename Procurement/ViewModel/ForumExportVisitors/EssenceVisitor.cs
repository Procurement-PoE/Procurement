using System.Collections.Generic;
using System.Linq;

using POEApi.Model;

using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class EssenceVisitor : VisitorBase
    {
        private const string TOKEN = "{Essence}";

        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<EssenceFilter>(items.OrderBy(i => i.TypeLine)));
        }
    }
}