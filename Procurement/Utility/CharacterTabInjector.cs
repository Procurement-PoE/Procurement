using System.Linq;
using POEApi.Infrastructure;
using POEApi.Model;
using System.Collections.Generic;
using System;

namespace Procurement.Utility
{
    public class CharacterInventory
    {
        public Character Character { get; set; }
        public List<Item> Inventory { get; set; }

        public CharacterInventory(Character character, List<Item> inventory)
        {
            Character = character;
            Inventory = inventory;
        }
    }
    
    public class CharacterTabInjector
    {
        private const string tabImageCenter = @"http://webcdn.pathofexile.com/gen/image/YTozOntpOjA7aToyNDtp/OjE7czozMjoiMDJhMTk3/N2QxZDAzNDQzNmU3NzM5/ZjgzZDEzYjIwN2YiO2k6/MjthOjI6e2k6MDtpOjI7/aToxO2E6Mzp7czoxOiJ0/IjtpOjI7czoxOiJuIjtz/OjA6IiI7czoxOiJjIjtp/Oi0xMzQ4NzU2Njt9fX0,/a2619ce769/Stash_TabC.png";
        private const string tabImageLeft = @"http://webcdn.pathofexile.com/gen/image/YTozOntpOjA7aToyNDtp/OjE7czozMjoiMDJhMTk3/N2QxZDAzNDQzNmU3NzM5/ZjgzZDEzYjIwN2YiO2k6/MjthOjI6e2k6MDtpOjI7/aToxO2E6Mzp7czoxOiJ0/IjtpOjE7czoxOiJuIjtz/OjA6IiI7czoxOiJjIjtp/Oi0xMzQ4NzU2Njt9fX0,/6628828a0d/Stash_TabL.png";
        private const string tabImageRight = @"http://webcdn.pathofexile.com/gen/image/YTozOntpOjA7aToyNDtp/OjE7czozMjoiMDJhMTk3/N2QxZDAzNDQzNmU3NzM5/ZjgzZDEzYjIwN2YiO2k6/MjthOjI6e2k6MDtpOjI7/aToxO2E6Mzp7czoxOiJ0/IjtpOjM7czoxOiJuIjtz/OjA6IiI7czoxOiJjIjtp/Oi0xMzQ4NzU2Njt9fX0,/83ee9c99fb/Stash_TabR.png";

        private List<CharacterInventory> characterInventories;

        public CharacterTabInjector()
        {
            characterInventories = new List<CharacterInventory>();
        }

        public void Add(Character character, List<Item> inventory)
        {
            characterInventories.Add(new CharacterInventory(character, inventory));
        }

        internal void Inject()
        {
            foreach (var poison in characterInventories)
                inject(poison.Character, poison.Inventory);
        }
        
        private void inject(Character character, IEnumerable<Item> inventory)
        {
            ApplicationState.Stash[character.League].NumberOfTabs++;

            var tabID = getTabID(character);
            var inventoryID = tabID + 1;

            List<Item> characterItems = CharacterStashBuilder.GetCharacterStashItems(character.Name, inventory, inventoryID);

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

        private static int getTabID(Character character)
        {
            try
            {
                return ApplicationState.Stash[character.League].Tabs.OrderByDescending(t => t.i).First().i + 1;
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("Error getting tabId for character {0} in league '{1}', exception details: {2}", character.Name, character.League, ex.ToString()));
                return ApplicationState.Stash[character.League].NumberOfTabs;
            }
        }
    }
}
