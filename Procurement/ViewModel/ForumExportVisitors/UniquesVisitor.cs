using System;
using System.Collections.Generic;
using POEApi.Model;
using Procurement.ViewModel.Filters;
using System.Linq;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class UniquesVisitor : VisitorBase
    {
        private const string TOKEN = "{Uniques}";
        public override string Visit(IEnumerable<Item> items, string current)
        {
            if (current.IndexOf(TOKEN) < 0)
                return current;

            return current.Replace(TOKEN, runFilter<UniqueRarity>(items.OrderBy(i => i.H).ThenBy(i => i.IconURL)));
        }
    }
}
