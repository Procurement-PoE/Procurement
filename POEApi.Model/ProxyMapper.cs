using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using POEApi.Infrastructure;

namespace POEApi.Model
{
    internal class ProxyMapper
    {
        internal const string STACKSIZE = "Stack Size";
        internal const string CHARGES = "Currently has %0 of %1 Charges";
        internal const string STASH = "Stash";
        public const string QUALITY = "Quality";
        public const string NETTIER = "Net Tier";
        public const string GENUS = "Genus";
        public const string GROUP = "Group";
        public const string FAMILY = "Family";
        private static readonly Regex qualityRx = new Regex("[+]{1}(?<quality>[0-9]{1,2}).*");

        #region   Orb Types  

        private static readonly Dictionary<string, OrbType> orbMap = new Dictionary<string, OrbType>
        {
            {"Chaos Orb", OrbType.Chaos},
            {"Divine Orb", OrbType.Divine},
            {"Regal Orb", OrbType.Regal},
            {"Orb of Augmentation", OrbType.Augmentation},
            {"Orb of Alchemy", OrbType.Alchemy},
            {"Alchemy Shard", OrbType.AlchemyShard},
            {"Chromatic Orb", OrbType.Chromatic},
            {"Orb of Transmutation", OrbType.Transmutation},
            {"Transmutation Shard", OrbType.TransmutationShard},
            {"Orb of Scouring", OrbType.Scouring},
            {"Glassblower's Bauble", OrbType.GlassblowersBauble},
            {"Cartographer's Chisel", OrbType.Chisel},
            {"Gemcutter's Prism", OrbType.GemCutterPrism},
            {"Orb of Alteration", OrbType.Alteration},
            {"Alteration Shard", OrbType.AlterationShard},
            {"Orb of Chance", OrbType.Chance},
            {"Orb of Regret", OrbType.Regret},
            {"Exalted Orb", OrbType.Exalted},
            {"Armourer's Scrap", OrbType.ArmourersScrap},
            {"Blessed Orb", OrbType.Blessed},
            {"Blacksmith's Whetstone", OrbType.BlacksmithsWhetstone},
            {"Scroll Fragment", OrbType.ScrollFragment},
            {"Jeweller's Orb", OrbType.JewelersOrb},
            {"Scroll of Wisdom", OrbType.ScrollofWisdom},
            {"Orb of Fusing", OrbType.Fusing},
            {"Portal Scroll", OrbType.PortalScroll},
            {"Albino Rhoa Feather", OrbType.AlbinaRhoaFeather},
            {"Mirror", OrbType.Mirror},
            {"Eternal Orb", OrbType.Eternal},
            {"Imprint", OrbType.Imprint},
            {"Vaal Orb", OrbType.VaalOrb},
            {"Perandus Coin", OrbType.PerandusCoin},
            {"Silver Coin", OrbType.SilverCoin},
            {"Ancient Orb", OrbType.AncientOrb},
            {"Ancient Shard", OrbType.AncientShard},
            {"Annulment Shard", OrbType.AnnulmentShard},
            {"Binding Shard", OrbType.BindingShard},
            {"Chaos Shard", OrbType.ChaosShard},
            {"Engineer's Orb", OrbType.EngineersOrb},
            {"Engineer's Shard", OrbType.EngineersShard},
            {"Exalted Shard", OrbType.ExaltedShard},
            {"Harbinger's Orb", OrbType.HarbingersOrb},
            {"Harbinger's Shard", OrbType.HarbingersShard},
            {"Horizon Shard", OrbType.HorizonShard},
            {"Mirror Shard", OrbType.MirrorShard},
            {"Orb of Annulment", OrbType.AnnulmentOrb},
            {"Orb of Binding", OrbType.BindingOrb},
            {"Orb of Horizons", OrbType.HorizonOrb},
            {"Regal Shard", OrbType.RegalShard},
            {"Bestiary Orb", OrbType.BestiaryOrb},
            {"Simple Rope Net", OrbType.SimpleRopeNet},
            {"Reinforced Rope Net", OrbType.ReinforcedRopeNet},
            {"Strong Rope Net", OrbType.StrongRopeNet},
            {"Simple Iron Net", OrbType.SimpleIronNet},
            {"Reinforced Iron Net", OrbType.ReinforcedIronNet},
            {"Strong Iron Net", OrbType.StrongIronNet},
            {"Simple Steel Net", OrbType.SimpleSteelNet},
            {"Reinforced Steel Net", OrbType.ReinforcedSteelNet},
            {"Strong Steel Net", OrbType.StrongSteelNet},
            {"Thaumaturgical Net", OrbType.ThaumaturgicalNet},
            {"Necromancy Net", OrbType.NecromancyNet},
            {"Pantheon Soul", OrbType.PantheonSoul},
        };

        #endregion

        private static readonly Dictionary<string, EssenceType> essenceMap = new Dictionary<string, EssenceType>
        {
            {"Whispering Essence of Greed", EssenceType.WhisperingGreed},
            {"Whispering Essence of Contempt", EssenceType.WhisperingContempt},
            {"Whispering Essence of Hatred", EssenceType.WhisperingHatred},
            {"Whispering Essence of Woe", EssenceType.WhisperingWoe},
            {"Muttering Essence of Greed", EssenceType.MutteringGreed},
            {"Muttering Essence of Contempt", EssenceType.MutteringContempt},
            {"Muttering Essence of Hatred", EssenceType.MutteringHatred},
            {"Muttering Essence of Woe", EssenceType.MutteringWoe},
            {"Muttering Essence of Fear", EssenceType.MutteringFear},
            {"Muttering Essence of Anger", EssenceType.MutteringAnger},
            {"Muttering Essence of Torment", EssenceType.MutteringTorment},
            {"Muttering Essence of Sorrow", EssenceType.MutteringSorrow},
            {"Weeping Essence of Greed", EssenceType.WeepingGreed},
            {"Weeping Essence of Contempt", EssenceType.WeepingContempt},
            {"Weeping Essence of Hatred", EssenceType.WeepingHatred},
            {"Weeping Essence of Woe", EssenceType.WeepingWoe},
            {"Weeping Essence of Fear", EssenceType.WeepingFear},
            {"Weeping Essence of Anger", EssenceType.WeepingAnger},
            {"Weeping Essence of Torment", EssenceType.WeepingTorment},
            {"Weeping Essence of Sorrow", EssenceType.WeepingSorrow},
            {"Weeping Essence of Rage", EssenceType.WeepingRage},
            {"Weeping Essence of Suffering", EssenceType.WeepingSuffering},
            {"Weeping Essence of Wrath", EssenceType.WeepingWrath},
            {"Weeping Essence of Doubt", EssenceType.WeepingDoubt},
            {"Wailing Essence of Greed", EssenceType.WailingGreed},
            {"Wailing Essence of Contempt", EssenceType.WailingContempt},
            {"Wailing Essence of Hatred", EssenceType.WailingHatred},
            {"Wailing Essence of Woe", EssenceType.WailingWoe},
            {"Wailing Essence of Fear", EssenceType.WailingFear},
            {"Wailing Essence of Anger", EssenceType.WailingAnger},
            {"Wailing Essence of Torment", EssenceType.WailingTorment},
            {"Wailing Essence of Sorrow", EssenceType.WailingSorrow},
            {"Wailing Essence of Rage", EssenceType.WailingRage},
            {"Wailing Essence of Suffering", EssenceType.WailingSuffering},
            {"Wailing Essence of Wrath", EssenceType.WailingWrath},
            {"Wailing Essence of Doubt", EssenceType.WailingDoubt},
            {"Wailing Essence of Loathing", EssenceType.WailingLoathing},
            {"Wailing Essence of Zeal", EssenceType.WailingZeal},
            {"Wailing Essence of Anguish", EssenceType.WailingAnguish},
            {"Wailing Essence of Spite", EssenceType.WailingSpite},
            {"Screaming Essence of Greed", EssenceType.ScreamingGreed},
            {"Screaming Essence of Contempt", EssenceType.ScreamingContempt},
            {"Screaming Essence of Hatred", EssenceType.ScreamingHatred},
            {"Screaming Essence of Woe", EssenceType.ScreamingWoe},
            {"Screaming Essence of Fear", EssenceType.ScreamingFear},
            {"Screaming Essence of Anger", EssenceType.ScreamingAnger},
            {"Screaming Essence of Torment", EssenceType.ScreamingTorment},
            {"Screaming Essence of Sorrow", EssenceType.ScreamingSorrow},
            {"Screaming Essence of Rage", EssenceType.ScreamingRage},
            {"Screaming Essence of Suffering", EssenceType.ScreamingSuffering},
            {"Screaming Essence of Wrath", EssenceType.ScreamingWrath},
            {"Screaming Essence of Doubt", EssenceType.ScreamingDoubt},
            {"Screaming Essence of Loathing", EssenceType.ScreamingLoathing},
            {"Screaming Essence of Zeal", EssenceType.ScreamingZeal},
            {"Screaming Essence of Anguish", EssenceType.ScreamingAnguish},
            {"Screaming Essence of Spite", EssenceType.ScreamingSpite},
            {"Screaming Essence of Scorn", EssenceType.ScreamingScorn},
            {"Screaming Essence of Envy", EssenceType.ScreamingEnvy},
            {"Screaming Essence of Misery", EssenceType.ScreamingMisery},
            {"Screaming Essence of Dread", EssenceType.ScreamingDread},
            {"Shrieking Essence of Greed", EssenceType.ShriekingGreed},
            {"Shrieking Essence of Contempt", EssenceType.ShriekingContempt},
            {"Shrieking Essence of Hatred", EssenceType.ShriekingHatred},
            {"Shrieking Essence of Woe", EssenceType.ShriekingWoe},
            {"Shrieking Essence of Fear", EssenceType.ShriekingFear},
            {"Shrieking Essence of Anger", EssenceType.ShriekingAnger},
            {"Shrieking Essence of Torment", EssenceType.ShriekingTorment},
            {"Shrieking Essence of Sorrow", EssenceType.ShriekingSorrow},
            {"Shrieking Essence of Rage", EssenceType.ShriekingRage},
            {"Shrieking Essence of Suffering", EssenceType.ShriekingSuffering},
            {"Shrieking Essence of Wrath", EssenceType.ShriekingWrath},
            {"Shrieking Essence of Doubt", EssenceType.ShriekingDoubt},
            {"Shrieking Essence of Loathing", EssenceType.ShriekingLoathing},
            {"Shrieking Essence of Zeal", EssenceType.ShriekingZeal},
            {"Shrieking Essence of Anguish", EssenceType.ShriekingAnguish},
            {"Shrieking Essence of Spite", EssenceType.ShriekingSpite},
            {"Shrieking Essence of Scorn", EssenceType.ShriekingScorn},
            {"Shrieking Essence of Envy", EssenceType.ShriekingEnvy},
            {"Shrieking Essence of Misery", EssenceType.ShriekingMisery},
            {"Shrieking Essence of Dread", EssenceType.ShriekingDread},
            {"Deafening Essence of Greed", EssenceType.DeafeningGreed},
            {"Deafening Essence of Contempt", EssenceType.DeafeningContempt},
            {"Deafening Essence of Hatred", EssenceType.DeafeningHatred},
            {"Deafening Essence of Woe", EssenceType.DeafeningWoe},
            {"Deafening Essence of Fear", EssenceType.DeafeningFear},
            {"Deafening Essence of Anger", EssenceType.DeafeningAnger},
            {"Deafening Essence of Torment", EssenceType.DeafeningTorment},
            {"Deafening Essence of Sorrow", EssenceType.DeafeningSorrow},
            {"Deafening Essence of Rage", EssenceType.DeafeningRage},
            {"Deafening Essence of Suffering", EssenceType.DeafeningSuffering},
            {"Deafening Essence of Wrath", EssenceType.DeafeningWrath},
            {"Deafening Essence of Doubt", EssenceType.DeafeningDoubt},
            {"Deafening Essence of Loathing", EssenceType.DeafeningLoathing},
            {"Deafening Essence of Zeal", EssenceType.DeafeningZeal},
            {"Deafening Essence of Anguish", EssenceType.DeafeningAnguish},
            {"Deafening Essence of Spite", EssenceType.DeafeningSpite},
            {"Deafening Essence of Scorn", EssenceType.DeafeningScorn},
            {"Deafening Essence of Envy", EssenceType.DeafeningEnvy},
            {"Deafening Essence of Misery", EssenceType.DeafeningMisery},
            {"Deafening Essence of Dread", EssenceType.DeafeningDread},
            {"Essence of Insanity", EssenceType.Insanity},
            {"Essence of Horror", EssenceType.Horror},
            {"Essence of Delirium", EssenceType.Delirium},
            {"Essence of Hysteria", EssenceType.Hysteria},
            {"Remnant of Corruption", EssenceType.RemnantOfCorruption}
        };

        private static readonly Dictionary<string, BreachType> breachMap = new Dictionary<string, BreachType>
        {
            {"Chayula", BreachType.Chayula},
            {"Xoph", BreachType.Xoph},
            {"Esh", BreachType.Esh},
            {"Tul", BreachType.Tul},
            {"Uul-Netol", BreachType.UulNetol}
        };

        private static readonly Dictionary<string, TabType> tabTypeMap = new Dictionary<string, TabType>
        {
            {"NormalStash", TabType.Normal},
            {"PremiumStash", TabType.Premium},
            {"CurrencyStash", TabType.Currency},
            {"DivinationCardStash", TabType.DivinationCard},
            {"EssenceStash", TabType.Essence},
            {"QuadStash", TabType.Quad}
        };

        public static TabType GetTabType(string type)
        {
            try
            {
                return tabTypeMap.First(m => type.Contains(m.Key)).Value;
            }
            catch (Exception)
            {
                return TabType.Unknown;
            }
        }

        private static string getPropertyByName(List<JSONProxy.Property> properties, string name)
        {
            if (properties == null)
                return null;

            var prop = properties.Find(p => p.Name == name);

            if (prop == null)
                return string.Empty;

            return (prop.Values[0] as JArray)[0].ToString();
        }

        internal static OrbType GetOrbType(JSONProxy.Item item)
        {
            return GetOrbType(item.TypeLine);
        }

        internal static OrbType GetOrbType(string name)
        {
            try
            {
                // Collapse all of the "Captured Soul of ..." into a single PantheonSoul OrbType.
                if (name.StartsWith("Captured Soul of ", StringComparison.CurrentCultureIgnoreCase))
                {
                    name = "Pantheon Soul";
                }
                return orbMap.First(m => name.Equals(m.Key, StringComparison.CurrentCultureIgnoreCase)).Value;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                Logger.Log("ProxyMapper.GetOrbType Failed! ItemType = " + name);

                return OrbType.Unknown;
            }
        }

        internal static EssenceType GetEssenceType(JSONProxy.Item item)
        {
            return GetEssenceType(item.TypeLine);
        }

        internal static EssenceType GetEssenceType(string name)
        {
            try
            {
                return essenceMap.First(m => name.Contains(m.Key)).Value;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                Logger.Log("ProxyMapper.GetEssenceType Failed! ItemType = " + name);

                return EssenceType.Unknown;
            }
        }

        public static BreachType GetBreachType(JSONProxy.Item item)
        {
            try
            {
                return breachMap.First(m => item.TypeLine.Contains(m.Key)).Value;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                Logger.Log("ProxyMapper.GetBreachType Failed! ItemType = " + item.TypeLine);

                return BreachType.Unknown;
            }
        }

        internal static List<Property> GetProperties(List<JSONProxy.Property> properties)
        {
            return properties.Select(p => new Property(p)).ToList();
        }

        internal static List<Requirement> GetRequirements(List<JSONProxy.Requirement> requirements)
        {
            if (requirements == null)
                return new List<Requirement>();

            return requirements.Select(r => new Requirement(r)).ToList();
        }

        internal static StackInfo GetStackInfo(List<JSONProxy.Property> list)
        {
            string propertyValue = getPropertyByName(list, STACKSIZE);
            if (string.IsNullOrWhiteSpace(propertyValue))
                return new StackInfo(1, 1);

            var stackInfo = propertyValue.Split('/');

            return new StackInfo(Convert.ToInt32(stackInfo[0]), Convert.ToInt32(stackInfo[1]));
        }

        internal static int GetQuality(List<JSONProxy.Property> properties)
        {
            return Convert.ToInt32(qualityRx.Match(getPropertyByName(properties, QUALITY)).Groups["quality"].Value);
        }

        internal static List<Tab> GetTabs(List<JSONProxy.Tab> tabs)
        {
            try
            {
                return tabs.Select(t => new Tab(t)).ToList();
            }
            catch (Exception ex)
            {
                Logger.Log("Error in ProxyMapper.GetTabs: " + ex);
                throw;
            }
        }

        public static ChargeInfo GetCharges(List<JSONProxy.Property> list)
        {
            try
            {
                var chargeSize = list.Find(p => p.Name == CHARGES);
                if (chargeSize == null)
                    return new ChargeInfo(0, 0);

                var qty = chargeSize.Values[0] as JArray;

                var max = chargeSize.Values[1] as JArray;

                return new ChargeInfo(int.Parse(qty.First.ToString()), int.Parse(max.First.ToString()));
            }
            catch (Exception ex)
            {
                Logger.Log("Error in ProxyMapper.GetCharges: " + ex);
            }

            return new ChargeInfo(1,1);
        }

        public static int GetNetTier(List<JSONProxy.Property> properties)
        {
            return Convert.ToInt32(getPropertyByName(properties, NETTIER));
        }

        public static string GetGenus(List<JSONProxy.Property> properties)
        {
            return getPropertyByName(properties, GENUS);
        }

        public static string GetGroup(List<JSONProxy.Property> properties)
        {
            return getPropertyByName(properties, GROUP);
        }

        public static string GetFamily(List<JSONProxy.Property> properties)
        {
            return getPropertyByName(properties, FAMILY);
        }
    }
}