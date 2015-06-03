using POEApi.Model;
using System;
using System.Diagnostics;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public abstract class ExplicitModBase : IFilter
    {
        internal string keyword;
        public ExplicitModBase(string keyword)
        {
            this.keyword = keyword;
        }

        public bool Applicable(Item item)
        {
            if (item.Explicitmods == null || !(item is Gear))
                return false;

            foreach (var mod in item.Explicitmods)
                if (mod.Contains(keyword))
                    return true;

            return false;
        }

        public abstract bool CanFormCategory
        {
            get;
        }

        public abstract string Keyword
        {
            get;
        }

        public abstract string Help
        {
            get;
        }

        public abstract FilterGroup Group
        {
            get;
        }
    }
}
