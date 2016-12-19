using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using POEApi.Infrastructure;

namespace POEApi.Model
{
    internal class ProxyMapper
    {
        internal const string STACKSIZE = "Stack Size";
        internal const string STASH = "Stash";
        public const string QUALITY = "Quality";
        private static Regex qualityRx = new Regex("[+]{1}(?<quality>[0-9]{1,2}).*");

        #region   Orb Types  

        private static readonly Dictionary<string, OrbType> orbMap = new Dictionary<string, OrbType>()
        {
            {"Chaos", OrbType.Chaos},
            {"Divine", OrbType.Divine},
            {"Regal", OrbType.Regal},
            {"Augmentation", OrbType.Augmentation},
            {"Orb of Alchemy", OrbType.Alchemy},
            {"Alchemy Shard", OrbType.AlchemyShard},
            {"Chromatic", OrbType.Chromatic},
            {"Transmutation", OrbType.Transmutation},
            {"Scouring", OrbType.Scouring},
            {"Glassblower", OrbType.GlassblowersBauble},
            {"Cartographer", OrbType.Chisel},
            {"Gemcutter's Prism", OrbType.GemCutterPrism},
            {"Alteration", OrbType.Alteration},
            {"Chance", OrbType.Chance},
            {"Regret", OrbType.Regret},
            {"Exalted", OrbType.Exalted},
            {"Armourer's Scrap", OrbType.ArmourersScrap},
            {"Blessed", OrbType.Blessed},
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
            {"Whispering Essence of Greed", OrbType.WhisperingGreed},
            {"Whispering Essence of Contempt", OrbType.WhisperingContempt},
            {"Whispering Essence of Hatred", OrbType.WhisperingHatred},
            {"Whispering Essence of Woe", OrbType.WhisperingWoe},
            {"Muttering Essence of Greed", OrbType.MutteringGreed},
            {"Muttering Essence of Contempt", OrbType.MutteringContempt},
            {"Muttering Essence of Hatred", OrbType.MutteringHatred},
            {"Muttering Essence of Woe", OrbType.MutteringWoe},
            {"Muttering Essence of Fear", OrbType.MutteringFear},
            {"Muttering Essence of Anger", OrbType.MutteringAnger},
            {"Muttering Essence of Torment", OrbType.MutteringTorment},
            {"Muttering Essence of Sorrow", OrbType.MutteringSorrow},
            {"Weeping Essence of Greed", OrbType.WeepingGreed},
            {"Weeping Essence of Contempt", OrbType.WeepingContempt},
            {"Weeping Essence of Hatred", OrbType.WeepingHatred},
            {"Weeping Essence of Woe", OrbType.WeepingWoe},
            {"Weeping Essence of Fear", OrbType.WeepingFear},
            {"Weeping Essence of Anger", OrbType.WeepingAnger},
            {"Weeping Essence of Torment", OrbType.WeepingTorment},
            {"Weeping Essence of Sorrow", OrbType.WeepingSorrow},
            {"Weeping Essence of Rage", OrbType.WeepingRage},
            {"Weeping Essence of Suffering", OrbType.WeepingSuffering},
            {"Weeping Essence of Wrath", OrbType.WeepingWrath},
            {"Weeping Essence of Doubt", OrbType.WeepingDoubt},
            {"Wailing Essence of Greed", OrbType.WailingGreed},
            {"Wailing Essence of Contempt", OrbType.WailingContempt},
            {"Wailing Essence of Hatred", OrbType.WailingHatred},
            {"Wailing Essence of Woe", OrbType.WailingWoe},
            {"Wailing Essence of Fear", OrbType.WailingFear},
            {"Wailing Essence of Anger", OrbType.WailingAnger},
            {"Wailing Essence of Torment", OrbType.WailingTorment},
            {"Wailing Essence of Sorrow", OrbType.WailingSorrow},
            {"Wailing Essence of Rage", OrbType.WailingRage},
            {"Wailing Essence of Suffering", OrbType.WailingSuffering},
            {"Wailing Essence of Wrath", OrbType.WailingWrath},
            {"Wailing Essence of Doubt", OrbType.WailingDoubt},
            {"Wailing Essence of Loathing", OrbType.WailingLoathing},
            {"Wailing Essence of Zeal", OrbType.WailingZeal},
            {"Wailing Essence of Anguish", OrbType.WailingAnguish},
            {"Wailing Essence of Spite", OrbType.WailingSpite},
            {"Screaming Essence of Greed", OrbType.ScreamingGreed},
            {"Screaming Essence of Contempt", OrbType.ScreamingContempt},
            {"Screaming Essence of Hatred", OrbType.ScreamingHatred},
            {"Screaming Essence of Woe", OrbType.ScreamingWoe},
            {"Screaming Essence of Fear", OrbType.ScreamingFear},
            {"Screaming Essence of Anger", OrbType.ScreamingAnger},
            {"Screaming Essence of Torment", OrbType.ScreamingTorment},
            {"Screaming Essence of Sorrow", OrbType.ScreamingSorrow},
            {"Screaming Essence of Rage", OrbType.ScreamingRage},
            {"Screaming Essence of Suffering", OrbType.ScreamingSuffering},
            {"Screaming Essence of Wrath", OrbType.ScreamingWrath},
            {"Screaming Essence of Doubt", OrbType.ScreamingDoubt},
            {"Screaming Essence of Loathing", OrbType.ScreamingLoathing},
            {"Screaming Essence of Zeal", OrbType.ScreamingZeal},
            {"Screaming Essence of Anguish", OrbType.ScreamingAnguish},
            {"Screaming Essence of Spite", OrbType.ScreamingSpite},
            {"Screaming Essence of Scorn", OrbType.ScreamingScorn},
            {"Screaming Essence of Envy", OrbType.ScreamingEnvy},
            {"Screaming Essence of Misery", OrbType.ScreamingMisery},
            {"Screaming Essence of Dread", OrbType.ScreamingDread},
            {"Shrieking Essence of Greed", OrbType.ShriekingGreed},
            {"Shrieking Essence of Contempt", OrbType.ShriekingContempt},
            {"Shrieking Essence of Hatred", OrbType.ShriekingHatred},
            {"Shrieking Essence of Woe", OrbType.ShriekingWoe},
            {"Shrieking Essence of Fear", OrbType.ShriekingFear},
            {"Shrieking Essence of Anger", OrbType.ShriekingAnger},
            {"Shrieking Essence of Torment", OrbType.ShriekingTorment},
            {"Shrieking Essence of Sorrow", OrbType.ShriekingSorrow},
            {"Shrieking Essence of Rage", OrbType.ShriekingRage},
            {"Shrieking Essence of Suffering", OrbType.ShriekingSuffering},
            {"Shrieking Essence of Wrath", OrbType.ShriekingWrath},
            {"Shrieking Essence of Doubt", OrbType.ShriekingDoubt},
            {"Shrieking Essence of Loathing", OrbType.ShriekingLoathing},
            {"Shrieking Essence of Zeal", OrbType.ShriekingZeal},
            {"Shrieking Essence of Anguish", OrbType.ShriekingAnguish},
            {"Shrieking Essence of Spite", OrbType.ShriekingSpite},
            {"Shrieking Essence of Scorn", OrbType.ShriekingScorn},
            {"Shrieking Essence of Envy", OrbType.ShriekingEnvy},
            {"Shrieking Essence of Misery", OrbType.ShriekingMisery},
            {"Shrieking Essence of Dread", OrbType.ShriekingDread},
            {"Deafening Essence of Greed", OrbType.DeafeningGreed},
            {"Deafening Essence of Contempt", OrbType.DeafeningContempt},
            {"Deafening Essence of Hatred", OrbType.DeafeningHatred},
            {"Deafening Essence of Woe", OrbType.DeafeningWoe},
            {"Deafening Essence of Fear", OrbType.DeafeningFear},
            {"Deafening Essence of Anger", OrbType.DeafeningAnger},
            {"Deafening Essence of Torment", OrbType.DeafeningTorment},
            {"Deafening Essence of Sorrow", OrbType.DeafeningSorrow},
            {"Deafening Essence of Rage", OrbType.DeafeningRage},
            {"Deafening Essence of Suffering", OrbType.DeafeningSuffering},
            {"Deafening Essence of Wrath", OrbType.DeafeningWrath},
            {"Deafening Essence of Doubt", OrbType.DeafeningDoubt},
            {"Deafening Essence of Loathing", OrbType.DeafeningLoathing},
            {"Deafening Essence of Zeal", OrbType.DeafeningZeal},
            {"Deafening Essence of Anguish", OrbType.DeafeningAnguish},
            {"Deafening Essence of Spite", OrbType.DeafeningSpite},
            {"Deafening Essence of Scorn", OrbType.DeafeningScorn},
            {"Deafening Essence of Envy", OrbType.DeafeningEnvy},
            {"Deafening Essence of Misery", OrbType.DeafeningMisery},
            {"Deafening Essence of Dread", OrbType.DeafeningDread},
            {"Essence of Insanity", OrbType.Insanity},
            {"Essence of Horror", OrbType.Horror},
            {"Essence of Delirium", OrbType.Delirium},
            {"Essence of Hysteria", OrbType.Hysteria},
            {"Essence of Corruption", OrbType.Corruption}
        };
        #endregion

        private static string getPropertyByName(List<JSONProxy.Property> properties, string name)
        {
            JSONProxy.Property prop = properties.Find(p => p.Name == name);

            if (prop == null)
                return string.Empty;

            return (prop.Values[0] as object[])[0].ToString();
        }
        
        internal static OrbType GetOrbType(JSONProxy.Item item)
        {
            return GetOrbType(item.TypeLine);
        }

        internal static OrbType GetOrbType(string name)
        {
            try
            {
                return orbMap.First(m => name.Contains(m.Key)).Value;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                Logger.Log("ProxyMapper.GetOrbType Failed! ItemType = " + name);
                return OrbType.Unknown;
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
            JSONProxy.Property stackSize = list.Find(p => p.Name == STACKSIZE);
            if (stackSize == null)
                return new StackInfo(1, 1);

            string[] stackInfo = getPropertyByName(list, STACKSIZE).Split('/');

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
                Logger.Log("Error in ProxyMapper.GetTabs: " + ex.ToString());
                throw;
            }
        }
    }
}
