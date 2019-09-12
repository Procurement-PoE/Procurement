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
        private bool OrMatch;
        private bool HasSpace;
        public UserSearchFilter(string filter, bool OrMatch, bool HasSpace)
        {
            this.filter = filter;
            this.OrMatch = OrMatch;
            this.HasSpace = HasSpace;
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
            get { return "Matches user search on name/typeline, geartype, mods, properties and texts"; }
        }

        public bool Applicable(Item item)
        {
            if (string.IsNullOrEmpty(filter))
                return false;

            if (!OrMatch)
            {
                if (!HasSpace)
                {
                    if (hasMatch(filter.Trim('"'), item))
                        return true;
                }
                else
                {
                    var words = filter.Split('"')
                                         .Select((element, index) => index % 2 == 0
                                             ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                             : new string[] { element })
                                         .SelectMany(element => element).ToList();

                    if (words.All(x => hasMatch(x, item)))
                    {
                        return true;
                    }
                }
            }
            else
            {
                var words = filter.Split('|');

                foreach (var word in words)
                {
                    if (!HasSpace)
                    {
                        if (hasMatch(word.Trim('"'), item))
                            return true;
                    }
                    else
                    {
                        var words1 = word.Split('"')
                                             .Select((element, index) => index % 2 == 0
                                                 ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                 : new string[] { element })
                                             .SelectMany(element => element).ToList();

                        if (words1.All(x => hasMatch(x, item)))
                            return true;
                    }
                }
            }

            var gear = item as Gear;

            if (gear != null && gear.SocketedItems.Any(x => Applicable(x)))
                return true;

            return false;
        }

        private bool hasMatch(string word, Item item)
        {
            var gear = item as Gear;
            var map = item as Map;

            bool dontmatch = false;

            if (word.StartsWith("-"))
            {
                word = word.Remove(0, 1);

                if (string.IsNullOrEmpty(word))
                    goto End;

                dontmatch = true;
            }

            if (item.TypeLine.ToLowerInvariant().Contains(word))
                goto End;

            if (!string.IsNullOrEmpty(item.Name) && item.Name.ToLowerInvariant().Contains(word))
                goto End;

            if (gear != null && gear.GearType.ToString().ToLowerInvariant().Contains(word))
                goto End;

            if (item.Microtransactions != null && item.Microtransactions.Count > 0)
                if (item.Microtransactions.Any(x => x.ToLowerInvariant().Contains(word)))
                    goto End;

            if (item.Explicitmods != null)
                if (item.Explicitmods.Any(x => x.ToLowerInvariant().Contains(word)))
                    goto End;

            if (item.Implicitmods != null)
                if (item.Implicitmods.Any(x => x.ToLowerInvariant().Contains(word)))
                    goto End;

            if (item.FracturedMods != null && item.FracturedMods.Count > 0)
                if (item.FracturedMods.Any(x => x.ToLowerInvariant().Contains(word)))
                    goto End;

            if (item.CraftedMods != null && item.CraftedMods.Count > 0)
                if (item.CraftedMods.Any(x => x.ToLowerInvariant().Contains(word)))
                    goto End;

            if (item.EnchantMods != null && item.EnchantMods.Count > 0)
                if (item.EnchantMods.Any(x => x.ToLowerInvariant().Contains(word)))
                    goto End;

            if (item.FlavourText != null)
                if (item.FlavourText.Any(x => x.ToLowerInvariant().Contains(word)))
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

            if (item.Requirements != null)
            {
                string reqtext = "Requires ";
                int reqcount = 1;
                foreach (var requirement in item.Requirements)
                {
                    if (requirement.NameFirst)
                        reqtext += requirement.Name + " " + requirement.Value;
                    else
                        reqtext += requirement.Value + " " + requirement.Name;

                    if (reqcount < item.Requirements.Count)
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
                List<string> inctext = new List<string>();

                inctext.Add(item.IncubatedDetails.Progress.ToString());
                inctext.Add(item.IncubatedDetails.Total.ToString());
                inctext.Add($"Incubating {item.IncubatedDetails.Name}");
                inctext.Add($"Level {item.IncubatedDetails.Level}+ Monster Kills");

                if (inctext.Any(x => x.ToLowerInvariant().Contains(word)))
                    goto End;
            }

            string text = "";

            if (item.ItemLevel > 0 && !(item is Incubator))
            {
                text = "rarity: " + item.Rarity.ToString().ToLowerInvariant();
                if (text.Contains(word))
                    goto End;
            }

            if (item.ItemLevel > 0)
            {
                text = "item level: " + item.ItemLevel.ToString();
                if (text.Contains(word))
                    goto End;
            }

            if (item.Implicitmods != null)
            {
                if (item.Implicitmods.Count == 1)
                {
                    text = "implicit";
                    if (text.Contains(word))
                        goto End;
                }
                else if (item.Implicitmods.Count == 2)
                {
                    text = "two-implicit";
                    if (text.Contains(word))
                        goto End;
                    text = "double implicit";
                    if (text.Contains(word))
                        goto End;
                }
            }

            if (item.IsMirrored)
            {
                text = "mirrored";
                if (text.Contains(word))
                    goto End;
            }

            if (item.EnchantMods != null && item.EnchantMods.Count > 0)
            {
                text = "enchanted";
                if (text.Contains(word))
                    goto End;
            }

            if (item.CraftedMods != null && item.CraftedMods.Count > 0)
            {
                text = "crafted";
                if (text.Contains(word))
                    goto End;
            }

            if (item.VeiledMods != null && item.VeiledMods.Count > 0)
            {
                text = "veiled";
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

            if (item is FullBestiaryOrb)
            {
                text = "captured beast";
                if (text.Contains(word))
                    goto End;
            }

            if (item is Gem)
            {
                text = "gem";
                if (text.Contains(word))
                    goto End;
            }

            if (gear != null && gear.GearType.Equals(GearType.Unknown)
                && (gear.TypeLine.StartsWith("Sacrifice at ")
                 || gear.TypeLine.StartsWith("Mortal ")
                 || gear.TypeLine.StartsWith("Fragment of the ")
                 || gear.TypeLine.EndsWith(" Key")))
            {
                text = "map fragment";
                if (text.Contains(word))
                    goto End;
            }

            if (map != null && word.StartsWith("tier:"))
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
                    if (greaterthan && tier <= map.MapTier)
                        goto End;
                    else if (lessthan && tier >= map.MapTier)
                        goto End;
                    else if (tier == map.MapTier)
                        goto End;
                }
            }

            if (item.ItemLevel > 0 && word.StartsWith("ilvl:"))
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
                return true;
            }
                
            return false;

            End:
                if (!dontmatch)
                    return true;
                else
                    return false;
        }
    }
}
