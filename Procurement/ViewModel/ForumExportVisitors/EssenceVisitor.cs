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

    internal class LeagueStoneVisitor : VisitorBase
    {
        private const string TOKEN = "{Leaguestone}";

        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<LeagestoneFilter>(items.OrderBy(i => i.TypeLine)));
        }
    }
}