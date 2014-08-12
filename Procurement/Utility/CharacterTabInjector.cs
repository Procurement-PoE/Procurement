using POEApi.Infrastructure;
using POEApi.Model;
using System.Collections.Generic;

namespace Procurement.Utility
{
    public class CharacterTabInjector
    {
        private const string tabImageCenter = @"http://webcdn.pathofexile.com/gen/image/YTozOntpOjA7aToyNDtp/OjE7czozMjoiMDJhMTk3/N2QxZDAzNDQzNmU3NzM5/ZjgzZDEzYjIwN2YiO2k6/MjthOjI6e2k6MDtpOjI7/aToxO2E6Mzp7czoxOiJ0/IjtpOjI7czoxOiJuIjtz/OjA6IiI7czoxOiJjIjtp/Oi0xMzQ4NzU2Njt9fX0,/a2619ce769/Stash_TabC.png";
        private const string tabImageLeft = @"http://webcdn.pathofexile.com/gen/image/YTozOntpOjA7aToyNDtp/OjE7czozMjoiMDJhMTk3/N2QxZDAzNDQzNmU3NzM5/ZjgzZDEzYjIwN2YiO2k6/MjthOjI6e2k6MDtpOjI7/aToxO2E6Mzp7czoxOiJ0/IjtpOjE7czoxOiJuIjtz/OjA6IiI7czoxOiJjIjtp/Oi0xMzQ4NzU2Njt9fX0,/6628828a0d/Stash_TabL.png";
        private const string tabImageRight = @"http://webcdn.pathofexile.com/gen/image/YTozOntpOjA7aToyNDtp/OjE7czozMjoiMDJhMTk3/N2QxZDAzNDQzNmU3NzM5/ZjgzZDEzYjIwN2YiO2k6/MjthOjI6e2k6MDtpOjI7/aToxO2E6Mzp7czoxOiJ0/IjtpOjM7czoxOiJuIjtz/OjA6IiI7czoxOiJjIjtp/Oi0xMzQ4NzU2Njt9fX0,/83ee9c99fb/Stash_TabR.png";
        public static void Inject(Character character, IEnumerable<Item> inventory)
        {
            List<Item> characterItems = new List<Item>();

            var tabID = ApplicationState.Stash[character.League].NumberOfTabs;
            var inventoryID = tabID + 1;
            ApplicationState.Stash[character.League].NumberOfTabs++;

            foreach (var item in inventory)
            {
                var clone = item.Clone() as Item;
                clone.InventoryId = "Stash" + inventoryID;
                clone.Character = character.Name;

                if (item.InventoryId != "MainInventory")
                    UpdatePosition(item, clone);

                characterItems.Add(clone);
            }

            Tab characterTab = new Tab
            {
                IsFakeTab = true,
                Name = character.Name,
                i = tabID,
                srcC = tabImageCenter,
                srcL = tabImageLeft,
                srcR = tabImageRight,

            };

            ApplicationState.Stash[character.League].AddCharacterTab(characterTab, characterItems);
        }

        private static void UpdatePosition(Item item, Item clone)
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
                    Logger.Log(string.Format("Unknown character inventoryId '{0}' found for {1}" + item.InventoryId, item.TypeLine));
                    break;
            }
        }
    }
}
