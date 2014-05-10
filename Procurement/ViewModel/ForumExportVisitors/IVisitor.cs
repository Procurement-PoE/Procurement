using System.Collections.Generic;
using POEApi.Model;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal interface IVisitor
    {
        string Visit(IEnumerable<Item> items, string current);
    }
}
