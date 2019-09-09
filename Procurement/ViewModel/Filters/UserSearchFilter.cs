using POEApi.Model;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Procurement.ViewModel.Filters
{
    public class UserSearchFilter : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        private string filter;
        public UserSearchFilter(string filter)
        {
            this.filter = filter;
        }
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return "User search"; }
        }

        public string Help
        {
            get { return "Matches user search on name/typeline, geartype, mods and texts"; }
        }

        public bool Applicable(Item item)
        {
            if (string.IsNullOrEmpty(filter))
                return false;

            if (containsMatchedCosmeticMod(item) || isMatchedGear(item))
                return true;
            
            var words = filter.ToLowerInvariant().Split('"')
                .Select((element, index) => index % 2 == 0
                    ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    : new string[] { element })
                .SelectMany(element => element).ToList();

            int count = 0;
            
            var gear = item as Gear;

            foreach (var splitword in words)
            {
                var word = splitword;

                bool dontmatch = false;

                if (word.StartsWith("-"))
                {
                    dontmatch = true;
                    word = word.Remove(0, 1);
                }

                if (item.TypeLine.ToLowerInvariant().Contains(word))
                    goto End;

                if (item.Name.ToLowerInvariant().Contains(word))
                    goto End;

                if (item.Explicitmods != null)
                    foreach (var mod in item.Explicitmods)
                        if (mod.ToLowerInvariant().Contains(word))
                            goto End;

                if (item.Implicitmods != null)
                    foreach (var mod in item.Implicitmods)
                        if (mod.ToLowerInvariant().Contains(word))
                            goto End;

                if (item.FracturedMods != null)
                    foreach (var mod in item.FracturedMods)
                        if (mod.ToLowerInvariant().Contains(word))
                            goto End;

                if (item.CraftedMods != null)
                    foreach (var mod in item.CraftedMods)
                        if (mod.ToLowerInvariant().Contains(word))
                            goto End;

                if (item.EnchantMods != null)
                    foreach (var mod in item.EnchantMods)
                        if (mod.ToLowerInvariant().Contains(word))
                            goto End;

                if (item.FlavourText != null)
                    foreach (var flavourtext in item.FlavourText)
                        if (flavourtext.ToLowerInvariant().Contains(word))
                            goto End;

                if (item.DescrText != null)
                    if (item.DescrText.ToLowerInvariant().Contains(word))
                        goto End;

                if (item.SecDescrText != null)
                    if (item.SecDescrText.ToLowerInvariant().Contains(word))
                        goto End;

                if (item.ProphecyText != null)
                    if (item.ProphecyText.ToLowerInvariant().Contains(word))
                        goto End;

                if (item.Properties != null)
                {
                    foreach (var property in item.Properties)
                    {
                        string proptext = null;
                        if (property.DisplayMode == 0)
                        {
                            if (property.Values.Count == 0)
                                proptext = property.Name;
                            else
                            {
                                proptext = property.Name + ":";
                                for (int i = 0; i < property.Values.Count; i++)
                                {
                                    proptext += " " + property.Values[i].Item1;
                                    if (i != property.Values.Count - 1)
                                        proptext += ", ";
                                }
                            }
                        }
                        else if (property.DisplayMode == 1)
                        {
                            proptext = property.Values[0].Item1 + " " + property.Name;
                        }
                        else if (property.DisplayMode == 3)
                        {
                            var parts = property.Name.Split('%');

                            proptext += parts[0] + property.Values[0].Item1 + parts[1].Substring(1);

                            if (property.Values.Count > 1)
                                proptext += property.Values[1].Item1 + parts[2].Substring(1);
                        }

                        if (proptext != null)
                            if (proptext.ToLowerInvariant().Contains(word))
                                goto End;
                    }
                }

                if (gear != null && gear.Requirements != null)
                {
                    string reqtext = "Requires ";
                    int reqcount = 1;
                    foreach (var requirement in gear.Requirements)
                    {
                        if (requirement.NameFirst)
                            reqtext += requirement.Name + " " + requirement.Value;
                        else
                            reqtext += requirement.Value + " " + requirement.Name;

                        if (reqcount < gear.Requirements.Count)
                        {
                            reqtext += ", ";
                            reqcount++;
                        }
                    }
                    if (reqtext.ToLowerInvariant().Contains(word))
                        goto End;
                }

                if (item.IncubatedDetails != null)
                {
                    List<string> inctexts = new List<string>();

                    inctexts.Add(item.IncubatedDetails.Progress.ToString());
                    inctexts.Add(item.IncubatedDetails.Total.ToString());
                    inctexts.Add($"Incubating {item.IncubatedDetails.Name}");
                    inctexts.Add($"Level {item.IncubatedDetails.Level}+ Monster Kills");

                    foreach (string inctext in inctexts)
                        if (inctext.ToLowerInvariant().Contains(word))
                            goto End;
                }

                if (item is Map || gear != null)
                {
                    string rarity = null;

                    if (item is Map
                        || (!gear.GearType.Equals(GearType.Breachstone)
                        && !gear.GearType.Equals(GearType.DivinationCard)
                        && !gear.TypeLine.StartsWith("Sacrifice at ")
                        && !gear.TypeLine.StartsWith("Mortal ")
                        && !gear.TypeLine.StartsWith("Fragment of the ")
                        && !gear.TypeLine.EndsWith(" Key")))
                    {
                        if (item.Rarity == Rarity.Normal)
                            rarity = "normal";
                        else if (item.Rarity == Rarity.Magic)
                            rarity = "magic";
                        else if (item.Rarity == Rarity.Rare)
                            rarity = "rare";
                        else if (item.Rarity == Rarity.Unique)
                            rarity = "unique";
                        else if (item.Rarity == Rarity.Relic)
                            rarity = "relic";

                        if (rarity.Contains(word))
                            goto End;
                    }
                }

                string text = null;
                if (item.EnchantMods != null && item.EnchantMods.Count() > 0)
                {
                    text = "enchanted";
                    if (text.Contains(word))
                        goto End;
                }
                if (item.CraftedMods != null && item.CraftedMods.Count() > 0)
                {
                    text = "crafted";
                    if (text.Contains(word))
                        goto End;
                }
                if (item.Fractured)
                {
                    text = "fractured";
                    if (text.Contains(word))
                        goto End;
                }
                if (item.Corrupted)
                {
                    text = "corrupted";
                    if (text.Contains(word))
                        goto End;
                }
                if (!item.Identified)
                {
                    text = "unidentified";
                    if (text.Contains(word))
                        goto End;
                }
                if (item.ItemLevel > 0)
                {
                    text = item.ItemLevel.ToString();
                    if (text.Contains(word))
                        goto End;
                }
                if (item.StackSize > 0)
                {
                    text = item.StackSize.ToString();
                    if (text.Contains(word))
                        goto End;
                }
                if (item.MaxStackSize > 0)
                {
                    text = item.MaxStackSize.ToString();
                    if (text.Contains(word))
                        goto End;
                }

                if (word.StartsWith("tier:") && item is Map)
                {
                    int tier;
                    bool greaterthan = false;
                    bool lessthan = false;
                    word = word.Remove(0, 5);
                    if (word.EndsWith("+"))
                    {
                        word = word.Remove(word.Length - 1);
                        greaterthan = true;
                    }
                    else if (word.EndsWith("-"))
                    {
                        word = word.Remove(word.Length - 1);
                        lessthan = true;
                    }
                    
                    int.TryParse(word, out tier);
                    if (tier >= 1 && tier <= 16)
                    {
                        var map = item as Map;
                        if (map != null)
                        {
                            if (greaterthan && tier <= map.MapTier)
                                goto End;
                            else if (lessthan && tier >= map.MapTier)
                                goto End;
                            else if (tier == map.MapTier)
                                goto End;
                        }
                    }
                }

                if (word.StartsWith("ilvl:") && item.ItemLevel > 0)
                {
                    int ilvl;
                    bool greaterthan = false;
                    bool lessthan = false;
                    word = word.Remove(0, 5);
                    if (word.EndsWith("+"))
                    {
                        word = word.Remove(word.Length - 1);
                        greaterthan = true;
                    }
                    else if (word.EndsWith("-"))
                    {
                        word = word.Remove(word.Length - 1);
                        lessthan = true;
                    }
                    int.TryParse(word, out ilvl);
                    if (ilvl >= 1 && ilvl <= 100)
                    {
                        if (greaterthan && ilvl <= item.ItemLevel)
                            goto End;
                        else if (lessthan && ilvl >= item.ItemLevel)
                            goto End;
                        else if (ilvl == item.ItemLevel)
                            goto End;
                    }
                }
                
                if (dontmatch)
                {
                    count++;
                    continue;
                }
                
                break;

                End:
                    if (!dontmatch)
                        count++;
                    else
                        break;
            }

            if (words.Count == count)
                return true;

            if (gear != null && gear.SocketedItems.Any(x => Applicable(x)))
                return true;

            return false;
        }

        private bool containsMatchedCosmeticMod(Item item)
        {
            return item.Microtransactions.Any(x => x.ToLowerInvariant().Contains(filter.ToLowerInvariant()));
        }

        private bool isMatchedGear(Item item)
        {
            Gear gear = item as Gear;

            if (gear == null)
                return false;

            return gear.GearType.ToString().ToLowerInvariant().Contains(filter.ToLowerInvariant());
        }
    }
}
