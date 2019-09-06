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

            if (gear.FracturedMods != null)
                foreach (var mod in gear.FracturedMods)
                    if (mod.Contains(keyword))
                        return true;

            if (gear.CraftedMods != null)
                foreach (var mod in gear.CraftedMods)
                    if (mod.Contains(keyword))
                        return true;

            if (gear.EnchantMods != null)
                foreach (var mod in gear.EnchantMods)
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
