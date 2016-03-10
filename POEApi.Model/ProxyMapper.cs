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
        private static Dictionary<string, OrbType> orbMap = new Dictionary<string, OrbType>()           
        {
            { "Chaos", OrbType.Chaos },
            { "Divine", OrbType.Divine },
            { "Regal", OrbType.Regal },
            { "Augmentation", OrbType.Augmentation },
            { "Orb of Alchemy", OrbType.Alchemy },
            { "Alchemy Shard", OrbType.AlchemyShard },
            { "Chromatic", OrbType.Chromatic },
            { "Transmutation", OrbType.Transmutation },
            { "Scouring", OrbType.Scouring },
            { "Glassblower",OrbType.GlassblowersBauble },
            { "Cartographer", OrbType.Chisel },
            { "Gemcutter's Prism", OrbType.GemCutterPrism },
            { "Alteration", OrbType.Alteration },
            { "Chance", OrbType.Chance },
            { "Regret", OrbType.Regret },
            { "Exalted", OrbType.Exalted },
            { "Armourer's Scrap", OrbType.ArmourersScrap },
            { "Blessed", OrbType.Blessed},
            { "Blacksmith's Whetstone", OrbType.BlacksmithsWhetstone },
            { "Scroll Fragment", OrbType.ScrollFragment },
            { "Jeweller's Orb", OrbType.JewelersOrb },
            { "Scroll of Wisdom", OrbType.ScrollofWisdom },
            { "Orb of Fusing", OrbType.Fusing },
            { "Portal Scroll", OrbType.PortalScroll },
            { "Albino Rhoa Feather", OrbType.AlbinaRhoaFeather},
            { "Mirror", OrbType.Mirror },
            { "Eternal Orb", OrbType.Eternal},
            { "Imprint", OrbType.Imprint },
            { "Vaal Orb", OrbType.VaalOrb },
            { "Perandus Coin", OrbType.PerandusCoin }
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
                var message = "ProxyMapper.GetOrbType Failed! ItemType = " + name;
                Logger.Log(message);
                throw new Exception(message);
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
