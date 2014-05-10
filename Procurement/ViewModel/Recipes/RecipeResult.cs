using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    public class RecipeResult
    {
        public bool IsMatch { get; set; }
        public decimal PercentMatch { get; set; }
        public Recipe Instance { get; set; }
        public List<Item> MatchedItems { get; set; }
        public List<string> Missing { get; set; }

        public string Name
        {
            get
            {
                if (Instance != null)
                    return Instance.GetResultName(this);

                return "Undocumented recipe.";
            }
        }
    }
}
