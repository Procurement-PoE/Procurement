using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Infrastructure;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    public abstract class Recipe
    {
        public abstract string Name { get; }
        protected bool ReturnsPartialMatches { get; private set; }
        protected decimal ReturnMatchesGreaterThan { get; private set; }

        public Recipe()
        {
            ReturnsPartialMatches = false;
        }
        public Recipe(decimal returnMatchesGreaterThan)
        {
            this.ReturnsPartialMatches = true;
            this.ReturnMatchesGreaterThan = returnMatchesGreaterThan;
        }

        public virtual string GetResultName(RecipeResult result)
        {
            return Name;
        }

        public abstract IEnumerable<RecipeResult> Matches(IEnumerable<Item> items);

        public virtual IEnumerable<RecipeResult> Matches(IDictionary<Tab, List<Item>> items)
        {
            List<Item> flatItems = new List<Item>();
            foreach (var tab in items)
            {
                flatItems.AddRange(tab.Value);
            }
            return Matches(flatItems);
        }

        protected bool GetShouldUseShortRecipeDescriptions()
        {
            bool result = false;
            bool.TryParse(Settings.UserSettings.GetEntry("UseShortRecipeDisplayDescriptions"), out result);
            // result is false if the parsing attempt failed.
            return result;
        }
    }
}
