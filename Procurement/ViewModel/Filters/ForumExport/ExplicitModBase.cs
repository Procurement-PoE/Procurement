using POEApi.Model;
using System;
using System.Diagnostics;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public abstract class ExplicitModBase : IFilter
    {
        private readonly string keyword;
        public ExplicitModBase(string keyword)
        {
            this.keyword = keyword;
        }

        public bool Applicable(Item item)
        {
            Gear gear = item as Gear;
            if (gear == null)
                return false;

            if (gear.Explicitmods != null)
                foreach (var mod in gear.Explicitmods)
                    if (mod.Contains(keyword))
                        return true;

            if (gear.Fracturedmods != null)
                foreach (var mod in gear.Fracturedmods)
                    if (mod.Contains(keyword))
                        return true;

            if (gear.Craftedmods != null)
                foreach (var mod in gear.Craftedmods)
                    if (mod.Contains(keyword))
                        return true;

            if (gear.Enchantmods != null)
                foreach (var mod in gear.Enchantmods)
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
