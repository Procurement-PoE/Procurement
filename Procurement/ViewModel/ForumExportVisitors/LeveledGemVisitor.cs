using System.Collections.Generic;
using POEApi.Model;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class LeveledGemVisitor : VisitorBase
    {
        private const string TOKEN = "{LeveledGems}";
        public override string Visit(IEnumerable<Item> items, string current)
        {
            return current.Replace(TOKEN, runFilter<LeveledGemFilter>(items));
        }
    }
}
