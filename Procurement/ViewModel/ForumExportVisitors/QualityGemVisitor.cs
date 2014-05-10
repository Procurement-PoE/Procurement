using System.Collections.Generic;
using POEApi.Model;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class QualityGemVisitor : VisitorBase
    {
        private const string TOKEN = "{QualityGems}";
        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<QualityGemFilter>(items));
        }
    }
}
