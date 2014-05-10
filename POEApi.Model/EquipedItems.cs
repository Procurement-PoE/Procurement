using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace POEApi.Model
{
    public class EquipedItems
    {
        private Dictionary<string, PropertyInfo> properties;
        private Dictionary<string, string> propertyMapping;

        public Item Amulet { get; set; }
        public Item Belt { get; set; }
        public Item Helm { get; set; }
        public Item RingLeft { get; set; }
        public Item RingRight { get; set; }
        public Item Flask0 { get; set; }
        public Item Flask1 { get; set; }
        public Item Flask2 { get; set; }
        public Item Flask3 { get; set; }
        public Item Flask4 { get; set; }
        public Item Weapon { get; set; }
        public Item Offhand { get; set; }
        public Item AltWeapon { get; set; }
        public Item AltOffhand { get; set; }
        public Item Boots { get; set; }
        public Item Armour { get; set; }
        public Item Gloves { get; set; }

        private Dictionary<string, Item> mapping;

        public EquipedItems(IEnumerable<Item> items)
        {
            propertyMapping = new Dictionary<string, string>();
            properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(p => p.Name);
            propertyMapping.Add("Ring", "RingLeft");
            propertyMapping.Add("Ring2", "RingRight");
            propertyMapping.Add("Weapon2", "AltWeapon");
            propertyMapping.Add("Offhand2", "AltOffhand");
            propertyMapping.Add("BodyArmour", "Armour");

            foreach (var item in items)
                setProperty(item);
        }   

        private void setProperty(Item item)
        {
            string target = item.inventoryId;

            if (propertyMapping.ContainsKey(item.inventoryId))
                target = propertyMapping[item.inventoryId];

            if (item.inventoryId == "Flask")
                target = item.inventoryId + item.X;

            properties[target].SetValue(this, item, null);
        }

        public Dictionary<string, Item> GetItems()
        {
            return properties.Keys.ToDictionary(prop => prop, prop => (Item)properties[prop].GetValue(this, null));
        }
    }
}
