using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    public static class CategoryManager
    {
        private static Dictionary<string, IEnumerable<IFilter>> categories;
        private static List<IFilter> availableFilters;

        static CategoryManager()
        {
            categories = new Dictionary<string, IEnumerable<IFilter>>();
            availableFilters = getAvailableFilters();

            initializeBaseCategories();
            initializeUserCategories();
        }

        public static Dictionary<string, string> GetAvailableCategories()
        {
            return categories.ToDictionary(k => k.Key, k => string.Join(Environment.NewLine, k.Value.Select(filter => filter.Help)));
        }

        public static IEnumerable<IFilter> GetCategory(string category)
        {
            return categories[category];
        }

        private static void initializeUserCategories()
        {
            //For Testing and Illustration
            categories.Add("Craftables", new List<IFilter>() { new NormalRarity(), new OrFilter(new FourLink(), new FiveLink()) });
        }

        public static List<IFilter> GetAvailableFilters()
        {
            availableFilters = availableFilters ?? getAvailableFilters();
            return availableFilters;
        }
        private static List<IFilter> getAvailableFilters()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                                                  .Where(t => !(t.IsAbstract || t.IsInterface) && typeof(IFilter).IsAssignableFrom(t) && t.Name != typeof(OrFilter).Name)
                                                  .Where(t => t.GetConstructor(new Type[] { }) != null)
                                                  .OrderBy(t => t.Name)
                                                  .Select(t => Activator.CreateInstance(t) as IFilter)
                                                  .OrderBy(i => i.Group)
                                                  .ToList();
        }

        private static void initializeBaseCategories()
        {
            availableFilters.ForEach(f => categories.Add(f.Keyword, new List<IFilter>() { f }));
        }
    }
}
