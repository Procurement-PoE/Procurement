using System.Collections.Generic;

namespace Procurement.ViewModel.Filters
{
    public abstract class TypeLineFilter : IFilter
    {
        private IEnumerable<string> keywords;

        public abstract bool CanFormCategory { get; }
        public abstract string Keyword { get; }
        public abstract string Help { get; }
        public abstract FilterGroup Group { get; }      

        public TypeLineFilter(params string[] keyword)
        {
            this.keywords = keyword;
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            foreach (var key in keywords)
                if (item.TypeLine == key)
                    return true;

            return false;
        }
    }
}
