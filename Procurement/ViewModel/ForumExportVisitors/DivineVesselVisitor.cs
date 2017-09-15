using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class DivineVesselVisitor : VisitorBase
    {
        private const string TOKEN = "{DivineVessel}";

        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<DivineVesselFilter>(items.OrderBy(i => i.TypeLine)));
        }
    }
}