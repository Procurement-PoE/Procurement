using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;
using System.Collections.Generic;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class SixSocketVisitor : VisitorBase
    {
        private const string TOKEN = "{SixSocket}";
        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<SixSocketFilter>(items));
        }
    }
}
