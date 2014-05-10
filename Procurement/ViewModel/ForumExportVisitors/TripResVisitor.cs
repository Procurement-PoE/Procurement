using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class TripResVisitor : VisitorBase
    {
        private const string TOKEN = "{TripRes}";

        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<TripleResistance>(items.OrderBy(i => i.H).ThenBy(i => i.IconURL)));
        }
    }
}
