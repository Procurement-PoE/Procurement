using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class MatchedSet
    {
        private static Dictionary<string, PropertyInfo> properties;
        public Gear Amulet { get; set; }
        public Gear Belt { get; set; }
        public Gear Helm { get; set; }
        public Gear RingLeft { get; set; }
        public Gear RingRight { get; set; }
        public Gear Weapon { get; set; }
        public Gear Offhand { get; set; }
        public Gear Boots { get; set; }
        public Gear Armour { get; set; }
        public Gear Gloves { get; set; }

        public MatchedSet()
        {
            properties = properties ??
                         GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(p => p.Name);
        }

        public decimal Match()
        {
            decimal notNull = properties.Values.Count(p => p.GetValue(this, null) != null);
            return (notNull / properties.Values.Count) * 100;
        }

        public List<Gear> GetAll()
        {
            return properties.Values.Select(p => p.GetValue(this, null))
                             .Where(v => v != null)
                             .Distinct()
                             .Cast<Gear>()
                             .ToList();
        }

        public List<string> GetMissing()
        {
            return properties.Values.Select(p => new {name = p.Name, value = p.GetValue(this, null)})
                             .Where(at => at.value == null)
                             .Select(at => at.name)
                             .ToList();
        }
    }
}