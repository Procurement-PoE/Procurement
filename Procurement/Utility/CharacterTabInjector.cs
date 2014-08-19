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

            var tabID = ApplicationState.Stash[character.League].NumberOfTabs;
            var inventoryID = tabID + 1;
            ApplicationState.Stash[character.League].NumberOfTabs++;

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
    }
}
