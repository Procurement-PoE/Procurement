using POEApi.Infrastructure;
using System.Collections.Generic;

namespace POEApi.Model
{
    public class CharacterStashBuilder
    {
        public static List<Item> GetCharacterStashItems(string characterName, IEnumerable<Item> inventory, int inventoryID)
        {
            List<Item> characterItems = new List<Item>();
            foreach (var item in inventory)
            {
                var clone = item.Clone() as Item;
                clone.InventoryId = "Stash" + inventoryID;
                clone.Character = characterName;

                if (item.InventoryId != "MainInventory")
                    UpdatePosition(item, clone);

                if (item.InventoryId != "Map")
                    characterItems.Add(clone);
            }
            return characterItems;
        }

        public static void UpdatePosition(Item item, Item clone)
        {
            switch (item.InventoryId)
            {
                case "Flask":
                    clone.Y = 5;
                    break;
                case "Helm":
                    clone.X = 5;
                    clone.Y = 5;
                    break;
                case "Gloves":
                    clone.X = 7;
                    clone.Y = 5;
                    break;
                case "Boots":
                    clone.X = 9;
                    clone.Y = 5;
                    break;
                case "Ring":
                    clone.X = 11;
                    clone.Y = 5;
                    break;
                case "Ring2":
                    clone.X = 11;
                    clone.Y = 6;
                    break;
                case "Weapon":
                    clone.X = 0;
                    clone.Y = 7;
                    break;
                case "Offhand":
                    clone.X = 2;
                    clone.Y = 7;
                    break;
                case "Weapon2":
                    clone.X = 4;
                    clone.Y = 7;
                    break;
                case "Offhand2":
                    clone.X = 6;
                    clone.Y = 7;
                    break;
                case "Belt":
                    clone.X = 9;
                    clone.Y = 7;
                    break;
                case "Amulet":
                    clone.X = 11;
                    clone.Y = 7;
                    break;
                case "BodyArmour":
                    clone.X = 9;
                    clone.Y = 8;
                    break;
                default:
                    Logger.Log(string.Format("Unknown character inventoryId '{0}' found for {1}", clone.InventoryId, clone.TypeLine));
                    break;
            }
        }
    }
}
