using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class DualResVisitor : VisitorBase
    {
        private const string TOKEN = "{DualRes}";

        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<DualResistances>(items.OrderBy(i => i.H).ThenBy(i => i.IconURL)));
        }
    }
}
