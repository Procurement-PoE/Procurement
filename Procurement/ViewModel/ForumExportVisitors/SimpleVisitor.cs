using System.Collections.Generic;
using POEApi.Model;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal abstract class SimpleVisitor : VisitorBase
    {
        private readonly IFilter filter;
        private readonly string token;

        internal SimpleVisitor(string token, IFilter filter)
        {
            this.token = token;
            this.filter = filter;
        }

        public override string Visit(IEnumerable<Item> items, string current)
        {
            if (current.IndexOf(token) < 0)
            {
                return current;
            }

            return current.Replace(token, runFilter(filter, items));
        }
    }
}