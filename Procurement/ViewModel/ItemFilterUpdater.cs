using POEApi.Infrastructure;
using POEApi.Model;
using Procurement.ViewModel.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.ViewModel
{
    class ItemFilterUpdater
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

        public static void UpdateLootFilters()
        {
            foreach (var fileConfig in Settings.ItemFilterConfig.FileConfig)
            {
                if (!fileConfig.Disabled)
                    UpdateLootFilter(fileConfig, Settings.ItemFilterConfig);
            }
        }

        public static void UpdateLootFilter(POEApi.Model.Protobuf.FileConfig fileConfig,
            POEApi.Model.Protobuf.ItemFilterConfig itemFilterConfig)
        {
            if (string.IsNullOrWhiteSpace(fileConfig.InputLocation) ||
                string.IsNullOrWhiteSpace(fileConfig.OutputLocation))
            {
                Logger.Log("Must provide both an input and output location for an item filter file config.");
                return;
            }

            string itemFilterText = "";
            if (!System.IO.File.Exists(fileConfig.InputLocation))
            {
                Logger.Log(string.Format(
                    "Input location '{0}' for item filter file confg does not exist; using empty base.",
                    fileConfig.InputLocation));
            }
            else
            {
                itemFilterText = System.IO.File.ReadAllText(fileConfig.InputLocation);
            }

            foreach (var filterRuleConfig in fileConfig.FilterRuleConfig)
            {
                itemFilterText = ApplyFilterRule(itemFilterText, filterRuleConfig, itemFilterConfig);
            }

            System.IO.File.WriteAllText(fileConfig.OutputLocation, itemFilterText);
        }

        protected static string ApplyFilterRule(string itemFilterText,
            POEApi.Model.Protobuf.FilterRuleConfig filterRuleConfig,
            POEApi.Model.Protobuf.ItemFilterConfig itemFilterConfig)
        {
            switch (filterRuleConfig.RuleType)
            {
                case POEApi.Model.Protobuf.FilterRuleConfig.Types.FilterRuleType.MissingSameBaseType:
                    return AddMissingSameBaseTypeItemFilters(itemFilterText, filterRuleConfig, itemFilterConfig);
                case POEApi.Model.Protobuf.FilterRuleConfig.Types.FilterRuleType.ChancingBases:
                    return AddChancingBasesItemFilters(itemFilterText, filterRuleConfig, itemFilterConfig);
                default:
                    Logger.Log(string.Format(
                        "Unknown item filter rule type '{0}' found for rule with name '{1}'; skipping.",
                        filterRuleConfig.RuleType.ToString(), filterRuleConfig.Name));
                    break;
            }

            return itemFilterText;
        }

        protected static string AddMissingSameBaseTypeItemFilters(string itemFilterText,
            POEApi.Model.Protobuf.FilterRuleConfig filterRuleConfig,
            POEApi.Model.Protobuf.ItemFilterConfig itemFilterConfig)
        {

            var additionalConfig = new POEApi.Model.Protobuf.MissingSameBaseTypesFilterRuleConfig();
            if (filterRuleConfig.AdditionalConfig.Is(POEApi.Model.Protobuf.MissingSameBaseTypesFilterRuleConfig.Descriptor))
            {
                additionalConfig = filterRuleConfig.AdditionalConfig.Unpack<
                    POEApi.Model.Protobuf.MissingSameBaseTypesFilterRuleConfig>();
            }

            var relaxedResults = GetRelaxedSameBaseTypeResults();

            var resultsMissingQualityBaseTypeItems = relaxedResults.SelectMany(category => category.Value)
                .Where(result => result.Instance is SameBaseTypeRecipe)
                .Where(result => result.PercentMatch < 100)
                .Where(result => result.Name.Contains("20%"));

            StringBuilder normalQualityItemsNeededFilter = new StringBuilder();
            if (!additionalConfig.DisableMissingNormalQualityItems)
            {
                var normalQualityItemsNeeded = resultsMissingQualityBaseTypeItems
                    .Where(result => result.MatchedItems.All(item => (item as Gear).Rarity != Rarity.Normal))
                    .Select(result => result.MatchedItems.First() as Gear)
                    .GroupBy(gear => new { gear.BaseType, gear.GearType })
                    .Select(group => group.First())
                    .OrderBy(gear => gear.BaseType);

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
            }

            StringBuilder magicQualityItemsNeededFilter = new StringBuilder();
            if (!additionalConfig.DisableMissingMagicQualityItems)
            {
                var magicQualityItemsNeeded = resultsMissingQualityBaseTypeItems
                    .Where(result => result.MatchedItems.All(item => (item as Gear).Rarity != Rarity.Magic))
                    .Select(result => result.MatchedItems.First() as Gear)
                    .GroupBy(gear => new { gear.BaseType, gear.GearType })
                    .Select(group => group.First())
                    .OrderBy(gear => gear.BaseType);

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
            }

            StringBuilder rareQualityItemsNeededFilter = new StringBuilder();
            if (!additionalConfig.DisableMissingRareQualityItems)
            {
                var rareQualityItemsNeeded = resultsMissingQualityBaseTypeItems
                    .Where(result => result.MatchedItems.All(item => (item as Gear).Rarity != Rarity.Rare))
                    .Select(result => result.MatchedItems.First() as Gear)
                    .GroupBy(gear => new { gear.BaseType, gear.GearType })
                    .Select(group => group.First())
                    .OrderBy(gear => gear.BaseType);

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
            }

            var resultsMissingNonQualityBaseTypeItems = relaxedResults.SelectMany(category => category.Value)
                .Where(result => result.Instance is SameBaseTypeRecipe)
                .Where(result => result.PercentMatch < 100)
                .Where(result => result.Name.Contains("(U)"))
                .Where(result => !result.Name.Contains("20%"));

            StringBuilder normalItemsNeededFilter = new StringBuilder();
            if (!additionalConfig.DisableMissingNormalItems)
            {
                var normalItemsNeeded = resultsMissingNonQualityBaseTypeItems
                    .Where(result => result.MatchedItems.All(item => (item as Gear).Rarity != Rarity.Normal))
                    .Select(result => result.MatchedItems.First() as Gear)
                    .GroupBy(gear => new { gear.BaseType, gear.GearType })
                    .Select(group => group.First())
                    .OrderBy(gear => gear.BaseType);

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
            }

            StringBuilder magicItemsNeededFilter = new StringBuilder();
            if (!additionalConfig.DisableMissingMagicItems)
            {
                var magicItemsNeeded = resultsMissingNonQualityBaseTypeItems
                    .Where(result => result.MatchedItems.All(item => (item as Gear).Rarity != Rarity.Magic))
                    .Select(result => result.MatchedItems.First() as Gear)
                    .GroupBy(gear => new { gear.BaseType, gear.GearType })
                    .Select(group => group.First())
                    .OrderBy(gear => gear.BaseType);

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
            }

            StringBuilder rareItemsNeededFilter = new StringBuilder();
            if (!additionalConfig.DisableMissingRareItems)
            {
                var rareItemsNeeded = resultsMissingNonQualityBaseTypeItems
                    .Where(result => result.MatchedItems.All(item => (item as Gear).Rarity != Rarity.Rare))
                    .Select(result => result.MatchedItems.First() as Gear)
                    .GroupBy(gear => new { gear.BaseType, gear.GearType })
                    .Select(group => group.First())
                    .OrderBy(gear => gear.BaseType);

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
            }

            string allFilters = string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", normalQualityItemsNeededFilter.ToString(),
                magicQualityItemsNeededFilter.ToString(), rareQualityItemsNeededFilter.ToString(),
                normalItemsNeededFilter.ToString(), magicItemsNeededFilter.ToString(),
                rareItemsNeededFilter.ToString());

            var matchLocation = itemFilterText.IndexOf(filterRuleConfig.SearchString);
            if (matchLocation < 0)
            {
                matchLocation = 0;
            }
            else if (filterRuleConfig.SearchStringRelation ==
                POEApi.Model.Protobuf.FilterRuleConfig.Types.SearchStringRelation.After)
            {
                matchLocation += filterRuleConfig.SearchString.Length;
            }

            return itemFilterText.Insert(matchLocation, allFilters);
        }

        protected static string AddChancingBasesItemFilters(string itemFilterText,
            POEApi.Model.Protobuf.FilterRuleConfig filterRuleConfig,
            POEApi.Model.Protobuf.ItemFilterConfig itemFilterConfig)
        {
            if (filterRuleConfig.Disabled)
                return itemFilterText;

            List<string> chanceItems = new List<string>();
            foreach (var leagueConfig in itemFilterConfig.LeagueConfig)
            {
                if (!leagueConfig.Disabled && string.Equals(leagueConfig.Name, ApplicationState.CurrentLeague))
                {
                    foreach (var buildConfig in leagueConfig.BuildConfig)
                    {
                        if (buildConfig.Disabled)
                            continue;
                        foreach (var chancingBase in buildConfig.ChancingBase)
                        {
                            if (chancingBase.Disabled)
                                continue;
                            if (chancingBase.QuantityHeld < chancingBase.QuantityNeeded)
                            {
                                chanceItems.Add(chancingBase.BaseTypeName);
                            }
                        }
                    }
                }
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

            var matchLocation = itemFilterText.IndexOf(filterRuleConfig.SearchString);
            if (matchLocation < 0)
            {
                matchLocation = 0;
            }
            else if (filterRuleConfig.SearchStringRelation ==
                POEApi.Model.Protobuf.FilterRuleConfig.Types.SearchStringRelation.After)
            {
                matchLocation += filterRuleConfig.SearchString.Length;
            }
            return itemFilterText.Insert(matchLocation, chanceItemsFilter.ToString());
        }
    }
}
