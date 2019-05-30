using POEApi.Model;
using Procurement.ViewModel.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.ViewModel
{
    class LootFilterUpdater
    {
        protected static Dictionary<Tab, List<Item>> GetUsableCurrentLeagueItemsByTab()
        {
            Dictionary<Tab, List<Item>> itemsByTab = new Dictionary<Tab, List<Item>>();
            Stash stash = ApplicationState.Stash[ApplicationState.CurrentLeague];

            var usableTabs = stash.Tabs.Where(t => !Settings.Lists["IgnoreTabsInRecipes"].Contains(t.Name)).ToList();
            foreach (var tab in usableTabs)
            {
                itemsByTab.Add(tab, stash.GetItemsByTab(tab.i));
            }

            return itemsByTab;
        }

        protected static Dictionary<string, List<RecipeResult>> GetRelaxedSameBaseTypeResults()
        {
            var itemsByTab = GetUsableCurrentLeagueItemsByTab();
            SameBaseTypeRecipe sameBaseTypeRecipe = new SameBaseTypeRecipe(30);
            return sameBaseTypeRecipe.Matches(itemsByTab)
                .GroupBy(r => r.Name)
                .Select(group => new
                {
                    Name = group.Key,
                    RecipeGroup = group.OrderByDescending(recipe => recipe.PercentMatch)
                })
                .ToDictionary(g => g.Name, g => g.RecipeGroup.ToList());
        }

        public static void UpdateLootFilter()
        {
            if (!bool.Parse(Settings.UserSettings["UpdateItemFilter"]))
                return;

            string inputFilePath = Settings.UserSettings["ItemFilterFileInputPath"];
            string outputFilePath = Settings.UserSettings["ItemFilterFileOutputPath"];
            if (string.IsNullOrWhiteSpace(inputFilePath) || string.IsNullOrWhiteSpace(outputFilePath))
            {
                // TODO: Log an error/message.
                return;
            }

            string itemFilterText = System.IO.File.ReadAllText(inputFilePath);

            if (bool.Parse(Settings.UserSettings["UseMissingSameBaseTypeItemFilters"]))
            {
                itemFilterText = AddMissingSameBaseTypeItemFilters(itemFilterText);
            }

            if (bool.Parse(Settings.UserSettings["UseChancingBasesItemFilters"]))
            {
                itemFilterText = AddChancingBasesItemFilters(itemFilterText);
            }

            System.IO.File.WriteAllText(outputFilePath, itemFilterText);
        }

        protected static string AddMissingSameBaseTypeItemFilters(string itemFilterText)
        {
            var relaxedResults = GetRelaxedSameBaseTypeResults();

            var resultsMissingQualityBaseTypeItems = relaxedResults.SelectMany(category => category.Value)
                .Where(result => result.Instance is SameBaseTypeRecipe)
                .Where(result => result.PercentMatch < 100)
                .Where(result => result.Name.Contains("20%"));

            var normalQualityItemsNeeded = resultsMissingQualityBaseTypeItems
                .Where(result => result.MatchedItems.All(item => (item as Gear).Rarity != Rarity.Normal))
                .Select(result => result.MatchedItems.First() as Gear)
                .GroupBy(gear => new { gear.BaseType, gear.GearType })
                .Select(group => group.First())
                .OrderBy(gear => gear.BaseType);

            var magicQualityItemsNeeded = resultsMissingQualityBaseTypeItems
                .Where(result => result.MatchedItems.All(item => (item as Gear).Rarity != Rarity.Magic))
                .Select(result => result.MatchedItems.First() as Gear)
                .GroupBy(gear => new { gear.BaseType, gear.GearType })
                .Select(group => group.First())
                .OrderBy(gear => gear.BaseType);

            var rareQualityItemsNeeded = resultsMissingQualityBaseTypeItems
                .Where(result => result.MatchedItems.All(item => (item as Gear).Rarity != Rarity.Rare))
                .Select(result => result.MatchedItems.First() as Gear)
                .GroupBy(gear => new { gear.BaseType, gear.GearType })
                .Select(group => group.First())
                .OrderBy(gear => gear.BaseType);

            StringBuilder normalQualityItemsNeededFilter = new StringBuilder();
            if (normalQualityItemsNeeded.Count() > 0)
            {
                StringBuilder baseTypeLine = new StringBuilder();
                foreach (var item in normalQualityItemsNeeded)
                {
                    baseTypeLine.AppendFormat(" \"{0}\"", item.BaseType);
                }
                normalQualityItemsNeededFilter.AppendFormat(
@"#-------------------------------------------------
#   [0294] Missing Normal Rarity Items With Quality
#-------------------------------------------------

# Normal items needed to compete the same-base-type recipes with quality.

Show # Missing normal rarity gear
	BaseType{0}
	Quality >= 10
	Rarity = Normal
	ElderItem False
	ShaperItem False
	SetFontSize 40
	SetTextColor 255 255 255 255
	SetBorderColor 255 255 255 200
	SetBackgroundColor 75 75 75

", baseTypeLine.ToString());
            }

            StringBuilder magicQualityItemsNeededFilter = new StringBuilder();
            if (magicQualityItemsNeeded.Count() > 0)
            {
                StringBuilder baseTypeLine = new StringBuilder();
                foreach (var item in magicQualityItemsNeeded)
                {
                    baseTypeLine.AppendFormat(" \"{0}\"", item.BaseType);
                }
                magicQualityItemsNeededFilter.AppendFormat(
@"#------------------------------------------------
#   [0295] Missing Magic Rarity Items With Quality
#------------------------------------------------

# Magic items needed to compete the same-base-type recipes with quality.

Show # Missing magic rarity gear
	BaseType{0}
	Quality >= 14
	Rarity = Magic
	ElderItem False
	ShaperItem False
	SetFontSize 40
	#SetTextColor 25 95 235 255  # blue-ish
	SetBorderColor 255 255 255 200
	SetBackgroundColor 75 75 75

", baseTypeLine.ToString());
            }

            StringBuilder rareQualityItemsNeededFilter = new StringBuilder();
            if (rareQualityItemsNeeded.Count() > 0)
            {
                StringBuilder baseTypeLine = new StringBuilder();
                foreach (var item in rareQualityItemsNeeded)
                {
                    baseTypeLine.AppendFormat(" \"{0}\"", item.BaseType);
                }
                rareQualityItemsNeededFilter.AppendFormat(
@"#-----------------------------------------------
#   [0296] Missing Rare Rarity Items With Quality
#-----------------------------------------------

# Rare items needed to compete the same-base-type recipes with quality.

Show # Missing rare rarity gear
	BaseType{0}
	Quality >= 16
	Rarity = Rare
	ElderItem False
	ShaperItem False
	SetFontSize 40
	#SetTextColor 210 178 135 255
	SetBorderColor 255 255 255 200
	SetBackgroundColor 75 75 75

", baseTypeLine.ToString());
            }

            var resultsMissingNonQualityBaseTypeItems = relaxedResults.SelectMany(category => category.Value)
                .Where(result => result.Instance is SameBaseTypeRecipe)
                .Where(result => result.PercentMatch < 100)
                .Where(result => result.Name.Contains("(U)"))
                .Where(result => !result.Name.Contains("20%"));

            var normalItemsNeeded = resultsMissingNonQualityBaseTypeItems
                .Where(result => result.MatchedItems.All(item => (item as Gear).Rarity != Rarity.Normal))
                .Select(result => result.MatchedItems.First() as Gear)
                .GroupBy(gear => new { gear.BaseType, gear.GearType })
                .Select(group => group.First())
                .OrderBy(gear => gear.BaseType);

            var magicItemsNeeded = resultsMissingNonQualityBaseTypeItems
                .Where(result => result.MatchedItems.All(item => (item as Gear).Rarity != Rarity.Magic))
                .Select(result => result.MatchedItems.First() as Gear)
                .GroupBy(gear => new { gear.BaseType, gear.GearType })
                .Select(group => group.First())
                .OrderBy(gear => gear.BaseType);

            var rareItemsNeeded = resultsMissingNonQualityBaseTypeItems
                .Where(result => result.MatchedItems.All(item => (item as Gear).Rarity != Rarity.Rare))
                .Select(result => result.MatchedItems.First() as Gear)
                .GroupBy(gear => new { gear.BaseType, gear.GearType })
                .Select(group => group.First())
                .OrderBy(gear => gear.BaseType);

            StringBuilder normalItemsNeededFilter = new StringBuilder();
            if (normalItemsNeeded.Count() > 0)
            {
                StringBuilder baseTypeLine = new StringBuilder();
                foreach (var item in normalItemsNeeded)
                {
                    baseTypeLine.AppendFormat(" \"{0}\"", item.BaseType);
                }
                normalItemsNeededFilter.AppendFormat(
@"#------------------------------------
#   [0298] Missing Normal Rarity Items
#------------------------------------
# Normal items needed to compete the same-base-type recipes.

Show # Missing normal rarity gear
	BaseType{0}
	Quality = 0
	Rarity = Normal
	ElderItem False
	ShaperItem False
	SetFontSize 36
	SetTextColor 210 178 135 255
	SetBorderColor 213 159 100 200
	SetBackgroundColor 75 75 75

", baseTypeLine.ToString());
            };

            StringBuilder magicItemsNeededFilter = new StringBuilder();
            if (magicItemsNeeded.Count() > 0)
            {
                StringBuilder baseTypeLine = new StringBuilder();
                foreach (var item in magicItemsNeeded)
                {
                    baseTypeLine.AppendFormat(" \"{0}\"", item.BaseType);
                }
                magicItemsNeededFilter.AppendFormat(
@"#------------------------------------
#   [0298b] Missing Magic Rarity Items
#------------------------------------
# Magic items needed to compete the same-base-type recipes.

Show # Missing magic rarity gear
	BaseType{0}
	Quality = 0
	Rarity = Magic
	ElderItem False
	ShaperItem False
	SetFontSize 36
	#SetTextColor 210 178 135 255
	SetBorderColor 213 159 100 200
	SetBackgroundColor 75 75 75

", baseTypeLine.ToString());
            };

            StringBuilder rareItemsNeededFilter = new StringBuilder();
            if (rareItemsNeeded.Count() > 0)
            {
                StringBuilder baseTypeLine = new StringBuilder();
                foreach (var item in rareItemsNeeded)
                {
                    baseTypeLine.AppendFormat(" \"{0}\"", item.BaseType);
                }
                rareItemsNeededFilter.AppendFormat(
@"#------------------------------------
#   [0298c] Missing Rare Rarity Items
#------------------------------------
# Rare items needed to compete the same-base-type recipes.

Show # Missing Rare rarity gear
	BaseType{0}
	Quality = 0
	Rarity = Rare
	ElderItem False
	ShaperItem False
	SetFontSize 36
	#SetTextColor 210 178 135 255
	SetBorderColor 213 159 100 200
	SetBackgroundColor 75 75 75

", baseTypeLine.ToString());
            };

            /*
string allFilters = string.Format("{0}\n{1}", normalItemsNeededFilter.ToString(),
    chanceItemsFilter.ToString());
    */
            string allFilters = string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", normalQualityItemsNeededFilter.ToString(),
                magicQualityItemsNeededFilter.ToString(), rareQualityItemsNeededFilter.ToString(),
                normalItemsNeededFilter.ToString(), magicItemsNeededFilter.ToString(),
                rareItemsNeededFilter.ToString());

            const string customOverridesSearchString = @"#===============================================================================================================
# [[0300]]";

            var matchLocation = itemFilterText.IndexOf(customOverridesSearchString);
            if (matchLocation < 0)
            {
                matchLocation = 0;
            }
            return itemFilterText.Insert(matchLocation, allFilters);
        }

        protected static string AddChancingBasesItemFilters(string itemFilterText)
        {
            // TODO: Automate setting these items.
            List<string> chanceItems;
            switch (ApplicationState.CurrentLeague)
            {
                case "SSF Synthesis":
                    chanceItems = new List<string>
                    {
                        // Enki's Arc Witch
                        // TODO: Have to check what's actually required or
                        // recommended.  Lots of dense information in the guide.
                        "Sadist Garb",  // Inpulsa's Broken Heart
                        "Royal Skean",  // Heartbreaker (optional)

                        // Bella's Dancing Duo Occultist
                        // https://www.pathofexile.com/forum/view-thread/2235258
                        // "Reaver Sword",  // The Dancing Duo (obtained)
                        "Occultist's Vestment",  // Shavronne's Wrappings
                        "Riveted Gloves",  // Command of the Pit
                        "Hallowed Hybrid Flask",  // The Writhing Jar (x3)
                        "Deicide Mask",  // Heretic's Veil (optional)
                        "Onyx Amulet",  // Aul's Uprising, Envy Edition (optional)

                        // Popcorn Skeletons
                        "Grinning Fetish",  // Earendel's Embrace (x2) (obtained 1/2)
                        "Compound Spiked Shield",  // Maligaro's Lense (optional)
                        "Bone Armour",  // Bloodbond (optional?)
                        "Soldier Boots",  // Alberon's Warpath
                        "Embroidered Gloves",  // Vixen's Entrapment

                        // Whispering Ice CI Elequisitor
                        "Vile Staff",  // The Whispering Ice (required!)
                        "Onyx Amulet",  // Astramentis
                        "Leather Belt",  // Cyclopean Coil
                        "Cobalt Jewel",  // Fertile Mind (x2)
                        "Crimson Jewel",  // Brute Force Solution
                        "Crimson Jewel",  // Izaro's Turmoil
                        "Viridian Jewel",  // Pure Talent

                        // Mjolner Discharge
                        "Gavel",  // Mjölner
                        "Conquest Chainmail",  // Kingsguard
                        // "Titan Greaves",  // The Red Trail (Breach exclusive; upgrade from The Infinite Pursuit)
                        "Diamond Ring",  // Romira's Banquet
                        // "Agate Amulet",  // Voll's Devotion (Anarchy/Onslaught exclusive)
                        "Viridian Jewel",  // The Golden Rule
                    };
                    break;
                case "Synthesis":
                    chanceItems = new List<string>
                    {
                        // Toasters HC Occultist Support – 7 Auras + 4 Curses
                        // https://www.pathofexile.com/forum/view-thread/2126677
                        "Lacquered Garb",  // Victario's Influence (required)
                        "Archon Kite Shield",  // Prism Guardian (required)
                        "Prophet Crown",  // The Broken Crown (required)
                        "Paua Ring",  // Doedre’s Damning
                        "Onyx Amulet",  // Impresence
                        "War Hammer",  // Brightbeak (optional)
                        "Stealth Boots",  // Sin Trek (optional)
                        "Samite Gloves",  // Kalista's Grace (optional)
                    };
                    break;
                case "SSF Betrayal":
                    chanceItems = new List<string>
                    {
                        "Ranger Bow",  // Null's Inclination (required)
                        "Lacquered Garb",  // Victario's Influence (required)
                        "Prophet Crown",  // Speaker's Wreath (required)
                        // "Riveted Gloves",  // Command of the Pit (drop-only from a delve boss?)

                        // Enki's Arc Witch
                        // TODO: Have to check what's actually required or
                        // recommended.  Lots of dense information in the guide.
                        "Sadist Garb",  // Inpulsa's Broken Heart

                        // Lightning Tendrils CwC Arc
                        // "Harlequin Mask",  // Mind of the Council (only from Unbearable Whispers prophecy chain).
                        "Sadist Garb",  // Inpulsa's Broken Heart
                        "Lapis Amulet",  // Choir of the Storm (optional)

                        // Incinerate/Fireball Ignite Elementalist
                        "Lathi",  // The Searing Touch
                        "Samite Helmet",  // Hrimnor's Resolve
                        "Glorious Plate",  // Kaom's Heart
                        "Ruby Ring",  // Emberwake (x2)
                        "Onyx Amulet",  // Impresence
                        "Bismuth Flask",  // The Wise Oak
                        "Viridian Jewel",  // Grand Spectrum (x3)

                        // Whispering Ice CI Elequisitor
                        // "Vile Staff",  // The Whispering Ice (required!) (obtained)
                        "Onyx Amulet",  // Astramentis
                        "Leather Belt",  // Cyclopean Coil
                        "Cobalt Jewel",  // Fertile Mind (x2)
                        "Crimson Jewel",  // Brute Force Solution
                        "Crimson Jewel",  // Izaro's Turmoil
                        "Viridian Jewel",  // Pure Talent

                        // Mjolner Discharge
                        "Gavel",  // Mjölner
                        // "Conquest Chainmail",  // Kingsguard (obtained)
                        // "Titan Greaves",  // The Red Trail (Breach exclusive; upgrade from The Infinite Pursuit)
                        "Diamond Ring",  // Romira's Banquet
                        // "Agate Amulet",  // Voll's Devotion (Anarchy/Onslaught exclusive)
                        "Viridian Jewel",  // The Golden Rule
                    };
                    break;
                case "SSF Delve":
                    chanceItems = new List<string>
                    {
                        // Triple Herald Blade Vortex Elementalist
                        "Sadist Garb",  // Inpulsa's Broken Heart
                        // "Silken Hood",  // Starkonja's Head [obtained]
                        "Goathide Gloves",  // Hrimsorrow
                        // "Amethyst Flask",  // Atziri's Promise (can not be chanced)
                        "Sapphire Flask",  // Taste of Hate

                        // Enki's Arc Witch
                        // TODO: Have to check what's actually required or
                        // recommended.  Lots of dense information in the guide.

                        // Whispering Ice CI Elequisitor
                        // "Vile Staff",  // The Whispering Ice (required!) [obtained]
                        "Onyx Amulet",  // Astramentis
                        "Leather Belt",  // Cyclopean Coil
                        "Cobalt Jewel",  // Fertile Mind (x2)
                        "Crimson Jewel",  // Brute Force Solution
                        "Crimson Jewel",  // Izaro's Turmoil
                        "Viridian Jewel",  // Pure Talent

                        // Mjolner Discharge
                        "Gavel",  // Mjölner
                        // "Conquest Chainmail",  // Kingsguard [obtained]
                        // "Titan Greaves",  // The Red Trail (Breach exclusive; upgrade from The Infinite Pursuit)
                        "Diamond Ring",  // Romira's Banquet
                        // "Agate Amulet",  // Voll's Devotion (Anarchy/Onslaught exclusive)
                        "Viridian Jewel",  // The Golden Rule
                    };
                    break;
                case "SSF Incursion":
                    chanceItems = new List<string>
                    {
                        // Mjolner Discharge
                        "Gavel",  // Mjölner
                        "Conquest Chainmail",  // Kingsguard
                        // "Titan Greaves",  // The Red Trail (Breach exclusive; upgrade from The Infinite Pursuit)
                        "Diamond Ring",  // Romira's Banquet
                        // "Agate Amulet",  // Voll's Devotion (Anarchy/Onslaught exclusive)
                        "Viridian Jewel",  // The Golden Rule

                        // Elemental Wander
                        "Tarnished Spirit Shield",  // Brinerot Flag (optional)
                        "Destiny Leather",  // Queen of the Forest
                        "Ursine Pelt",  // Rat's Nest
                        "Topaz Flask",  // Vessel of Vinktar
                        "Ruby Flask",  // Dying Sun
                        "Bismuth Flask",  // The Wise Oak
                        "Viridian Jewel",  // Static Electricity
                        "Cobalt Jewel",  // Conqueror's Potency

                        // CI Caustic Arrow
                        "Spike-Point Arrow Quiver",  // Soul Strike
                        "Onyx Amulet",  // Impresence
                        "Stibnite Flask",  // Witchfire Brew
                        "Ruby Flask",  // Dying Sun

                        // Golemancer
                        "Prismatic Jewel",  // The Anima Stone (also from vendor recipe using other uniques)
                        "Cobalt Jewel",  // Primordial Harmony (x9)
                        "Crimson Jewel",  // Primordial Might
                        "Viridian Jewel",  // Primordial Eminence (x1, for The Anima Stone recipe)
                        "Rock Breaker",  // Clayshaper (0/2 obtained)
                        "Vaal Mask",  // The Vertex
                        // "Unset Ring",  // Redblade Band (Warbands exclusive)
                        // "Full Wyrmscale",  // Belly of the Beast (not needed in this build)

                        // SRS/Zombies Necromancer
                        "Elegant Ringmail",  // Geofri's Sanctuary
                        "Crusader Gloves",  // Shaper's Touch
                        "Close Helmet",  // The Baron
                        "Void Sceptre",  // Mon'tregul's Grasp
                        "Onyx Amulet",  // Astramentis
                        "Soldier Boots",  // Alberon's Warpath
                        "Cobalt Jewel",  // Violent Dead (x2)
                        "Crimson Jewel",  // Efficient Training (x3)
                        "Crimson Jewel",  // Brawn (x2-3)
                    };
                    break;
                case "SSF Standard":
                    chanceItems = new List<string>
                    {
                        // SR/RF Totems
                        "Spidersilk Robe",  // Soul Mantle
                        // "Vaal Sceptre",  // Doryani's Catalyst (can not be chanced)
                        "Pinnacle Tower Shield",  // Lioneye's Remorse
                        // "Samite Helmet",  // Hrimnor's Resolve (obtained)
                        "Topaz Ring",  // Kikazaru (2x)
                        "Crimson Jewel",  // Spire of Stone
                        "Viridian Jewel",  // Self-Flagellation

                        // Elemental Wander
                        "Tarnished Spirit Shield",  // Brinerot Flag (optional)
                        // "Destiny Leather",  // Queen of the Forest (obtained)
                        "Ursine Pelt",  // Rat's Nest
                        "Topaz Flask",  // Vessel of Vinktar
                        "Ruby Flask",  // Dying Sun
                        // "Bismuth Flsk",  // The Wise Oak (obtained)
                        "Viridian Jewel",  // Static Electricity
                        "Cobalt Jewel",  // Conqueror's Potency
                    };
                    break;
                case "SSF Bestiary":
                    chanceItems = new List<string>
                    {
                        // Frost Wind Ascendant
                        "Imperial Skean",  // White Wind
                        "Nubuck Gloves",  // Oskarm
                        "Full Wyrmscale",  // Belly of the Beast

                        // Magma Orb MoM Caster
                        "Paua Ring",  // Doedre's Damning
                        "Blue Pearl Amulet",  // Gloomfang
                        // "Amethyst Flask",  // Atziri's Promise (can not be chanced)
                        // "Cobalt Jewel",  // Inevitability (2/2 obtained)

                        // SRS/Zombies Necromancer
                        "Spidersilk Robe",  // The Covenant
                        // "Close Helmet",  // The Baron (obtained)
                        "Void Sceptre",  // Mon'tregul's Grasp
                        "Onyx Amulet",  // Astramentis
                        "Colossal Tower Shield",  // Ahn's Heritage
                        "Crimson Jewel",  // Fragility (x2)
                        "Soldier Boots",  // Alberon's Warpath
                        "Crimson Jewel",  // Efficient Training (x2)
                        "Cobalt Jewel",  // Violent Dead (x2)
                        "Laminated Kite Shield",  // Victario's Charity (Necromantic Aegis variant)
                        "Rawhide Tower Shield",  // Lycosidae (Necromantic Aegis variant)
                    };
                    break;
                case "Bestiary":
                    chanceItems = new List<string>
                    {
                        // Golemancer
                        "Prismatic Jewel",  // The Anima Stone (also from vendor recipe using other uniques)
                        "Cobalt Jewel",  // Primordial Harmony (x9)
                        "Crimson Jewel",  // Primordial Might
                        "Viridian Jewel",  // Primordial Eminence (x1, for The Anima Stone recipe)
                        "Rock Breaker",  // Clayshaper (1/2 obtained)
                        "Secutor Helm",  // Skullhead (expensive alternative)
                        // "Unset Ring",  // Redblade Band (Warbands exclusive)
                        "Full Wyrmscale",  // Belly of the Beast (very much optional)
                    };
                    break;
                default:
                    chanceItems = new List<string>();
                    break;
            }
            chanceItems = chanceItems.Distinct().ToList();

            StringBuilder chanceItemsFilter = new StringBuilder();
            if (chanceItems.Count() > 0)
            {
                StringBuilder baseTypeLine = new StringBuilder();
                foreach (var item in chanceItems)
                {
                    baseTypeLine.AppendFormat(" \"{0}\"", item);
                }
                chanceItemsFilter.AppendFormat(@"

Show # $Chancing B-T1-Chancing %D4
	BaseType{0}
	Rarity Normal
	Quality = 0
	ElderItem False
	ShaperItem False
	Corrupted False
	SetFontSize 38
	SetTextColor 255 255 255 255         # TEXTCOLOR:	 Normal Items: Strong Highlight
	SetBorderColor 0 210 0 210           # BORDERCOLOR:	 T1 Chancing
	PlayAlertSound 7 300                 # DROPSOUND:	 T1 chancing items
", baseTypeLine.ToString());
            }

            const string chancingBasesSearchString = @"#   [0710] Chancing items
#------------------------------------
# You can add your own items here. The items in this section have a dropsound";
            var matchLocation = itemFilterText.IndexOf(chancingBasesSearchString);
            if (matchLocation < 0)
            {
                matchLocation = 0;
            }
            return itemFilterText.Insert(matchLocation + chancingBasesSearchString.Length,
                chanceItemsFilter.ToString());
        }
    }
}
