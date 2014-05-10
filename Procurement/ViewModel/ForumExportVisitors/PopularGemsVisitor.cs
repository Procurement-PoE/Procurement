using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class PopularGemsVisitor : VisitorBase
    {
        private const string TOKEN = "{PopularGems}";
        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<PopularGemsFilter>(items.OfType<Gem>().Where(g => !g.IsQuality)));
        }
    }
}
