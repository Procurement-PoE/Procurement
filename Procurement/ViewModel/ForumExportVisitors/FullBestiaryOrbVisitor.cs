using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    // TODO: Add similar visitors that look for orbs that have exactly one or two Bestiary beast mods, once we can
    // identify which mods those are.

    internal class FullBestiaryOrbVisitor : VisitorBase
    {
        private const string TOKEN = "{FullBestiaryOrbs}";

        public override string Visit(IEnumerable<Item> items, string current)
        {
            if (current.IndexOf(TOKEN) < 0)
                return current;

            return current.Replace(TOKEN, runFilter<FullBestiaryOrbFilter>(items.OrderBy(i => i.H)));
        }
    }
}