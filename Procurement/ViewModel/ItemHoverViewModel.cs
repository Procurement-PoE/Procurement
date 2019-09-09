using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Animation;

using POEApi.Model;

namespace Procurement.ViewModel
{
    public class ItemHoverViewModel
    {
        public Item Item { get; private set; }
        public string TypeLine { get; private set; }
        public ItemType ItemType { get; private set; }
        public List<Property> Properties { get; private set; }
        public List<Requirement> Requirements { get; private set; }
        public bool HasRequirements { get; private set; }

        public string Name { get; private set; }
        public bool HasName { get; private set; }
        public List<string> ExplicitMods { get; private set; }
        public bool IsMirrored { get; private set; }
        public bool HasExplicitMods { get; private set; }
        public List<string> ImplicitMods { get; private set; }
        public bool HasImplicitMods { get; private set; }
        public string DescriptionText { get; private set; }
        public string SecondaryDescriptionText { get; private set; }
        public bool IsCorrupted { get; private set; }
        public List<string> Microtransactions { get; private set; }
        public bool HasMicrotransactions { get; private set; }
        public List<string> EnchantMods { get; private set; }
        public bool HasEnchantMods { get; private set; }
        public string FlavourText { get; private set; }

        public List<string> CraftedMods { get; set; }
        public List<string> VeiledMods { get; set; }
        public List<string> FracturedMods { get; set; }

        public bool HasCraftedMods { get; private set; }
        public bool HasVeiledMods { get; private set; }
        public bool HasFracturedMods { get; private set; }
        public bool IsProphecy { get; set; }
        public string ProphecyText { get; set; }
        public string ProphecyDifficultyText { get; set; }
        public bool IsGear { get; set; }

        public string ItemLevel { get; set; }

        public bool IsGemProgressVisible
        {
            get
            {
                var gem = Item as Gem;

                if (gem == null)
                    return false;

                return gem.HasExperience;
            }
        }

        public double LevelExperienceProgress { get; set; }

        public int ExperienceNumerator { get; }
        public int ExperienceDenominator { get; }

        public int IncubatorNumerator { get; }
        public int IncubatorDenominator { get; }
        public double IncubatorProgress { get; }
        public string Incubating { get; set; }
        public string IncubationLevel { get; set; }

        public bool IsIncubatorProgressVisible
        {
            get
            {
                return Item.IncubatedDetails != null;
            }
        }

        public ItemHoverViewModel(Item item)
        {
            this.Item = item;
            this.Name = item.Name;
            this.TypeLine = item.TypeLine;
            this.HasName = !string.IsNullOrEmpty(item.Name);
            this.ItemType = item.ItemType;
            this.Properties = item.Properties;

            this.Requirements = new List<Requirement>();
            this.ExplicitMods = item.Explicitmods;
            this.IsMirrored = item.IsMirrored;

            this.ImplicitMods = new List<string>();

            this.DescriptionText = item.DescrText;

            this.IsCorrupted = item.Corrupted;

            this.Microtransactions = item.Microtransactions;
            this.HasMicrotransactions = item.Microtransactions.Count > 0;

            this.EnchantMods = item.EnchantMods;

            this.CraftedMods = item.CraftedMods;
            setVeiledMods(item);

            this.FracturedMods = item.FracturedMods;

            SecondaryDescriptionText = item.SecDescrText;
            setTypeSpecificProperties(item);

            if ((item is AbyssJewel || item is FullBestiaryOrb) && item.ItemLevel > 0)
            {
                this.ItemLevel = string.Format("Item Level: {0}", item.ItemLevel);
            }

            var gem = Item as Gem;
            if (gem != null)
            {
                LevelExperienceProgress = gem.LevelExperienceProgress;
                ExperienceNumerator = gem.ExperienceNumerator;
                ExperienceDenominator = gem.ExperienceDenominator;
            }

            if (IsIncubatorProgressVisible)
            {
                IncubatorNumerator = item.IncubatedDetails.Progress;
                IncubatorDenominator = item.IncubatedDetails.Total;
                Incubating = $"Incubating {item.IncubatedDetails.Name}";
                IncubationLevel =  $"Level {item.IncubatedDetails.Level}+ Monster Kills";
                if (Item.IncubatedDetails.Total > 0)
                {
                    IncubatorProgress = Convert.ToDouble(item.IncubatedDetails.Progress) / Convert.ToDouble(item.IncubatedDetails.Total);
                }
            }

            // If an item has crafted mods but no true explicit mods:
            //   In game: the crafted mods are correctly separated from the previous section.
            //   On official web site: there is no seperator added before the previous section.
            this.HasCraftedMods = CraftedMods?.Count > 0;
            this.HasVeiledMods = VeiledMods?.Count > 0;
            this.HasFracturedMods = FracturedMods?.Count > 0;
            this.HasExplicitMods = ExplicitMods?.Count > 0 || HasFracturedMods || HasCraftedMods || IsMirrored;
            this.HasImplicitMods = ImplicitMods?.Count > 0;
            this.HasEnchantMods = item.EnchantMods.Count > 0;
            this.HasRequirements = Requirements?.Count > 0;

            if (item.FlavourText?.Count > 0)
            {
                this.FlavourText = string.Join("", item.FlavourText);
            }

            setProphecyProperties(item);
        }

        private void setVeiledMods(Item item)
        {
            // The text for veiled mods is in the format "(Prefix|Suffix)##" where ## currently can be 01-06.
            VeiledMods = new List<string>();
            foreach (var veiledMod in item.VeiledMods)
            {
                if (veiledMod.StartsWith("Prefix"))
                {
                    VeiledMods.Add("Veiled Prefix");
                }
                else if (veiledMod.StartsWith("Suffix"))
                {
                    VeiledMods.Add("Veiled Suffix");
                }
                else
                {
                    VeiledMods.Add("Veiled Affix");
                }
            }
        }

        private void setProphecyProperties(Item item)
        {
            var prophecy = item as Prophecy;
            if (prophecy == null)
                return;

            this.IsProphecy = true;
            this.ProphecyText = prophecy.ProphecyText;
            this.ProphecyDifficultyText = prophecy.ProphecyDifficultyText;
        }

        private void setTypeSpecificProperties(Item item)
        {
            var gear = item as Gear;

            if (gear != null)
                setGearProperties(item, gear);

            var gem = item as Gem;

            if (gem != null)
                this.Requirements = gem.Requirements;
        }

        private void setGearProperties(Item item, Gear gear)
        {
            this.IsGear = true;
            if (item.ItemLevel > 0)
            {
                this.ItemLevel = string.Format("Item Level: {0}", item.ItemLevel);
            }
            this.Requirements = gear.Requirements;
            this.ImplicitMods = gear.Implicitmods;
        }
    }
}
