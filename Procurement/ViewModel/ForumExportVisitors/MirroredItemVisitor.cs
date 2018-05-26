using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class MirroredItemVisitor : VisitorBase
    {
        private const string TOKEN = "{Mirrored}";

        public override string Visit(IEnumerable<Item> items, string current)
        {
            if (current.IndexOf(TOKEN) < 0)
                return current;

            return current.Replace(TOKEN, runFilter<MirroredItemFilter>(items.OrderBy(i => i.H)));
        }
    }
}
