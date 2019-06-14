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
        // TODO(20190613): Dynamically determine what URLs to use, based on data for other [real] tabs.
        private const string tabImageCenter = @"https://web.poecdn.com/gen/image/WzIzLDEseyJ0IjoibSIsImMiOi0xMzQ4NzU2Nn1d/4470380632/Stash_TabL.png";
        private const string tabImageLeft = @"https://web.poecdn.com/gen/image/WzIzLDEseyJ0IjoibCIsImMiOi0xMzQ4NzU2Nn1d/0ce8f75b7c/Stash_TabL.png";
        private const string tabImageRight = @"https://web.poecdn.com/gen/image/WzIzLDEseyJ0IjoiciIsImMiOi0xMzQ4NzU2Nn1d/b85f086896/Stash_TabL.png";

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
