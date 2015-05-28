using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using POEApi.Infrastructure;
using System.Resources;

namespace POEApi.Model
{
    internal class ProxyMapper
    {
        internal static string STACKSIZE;
        internal const string STASH = "Stash";
        public static string QUALITY;
        private static Regex qualityRx = new Regex("[+]{1}(?<quality>[0-9]{1,2}).*");
        private static Dictionary<string, OrbType> orbMap;
  
        static ProxyMapper()
        {
            STACKSIZE = POEApi.Model.ServerTypeRes.StackSizeText;
            QUALITY = POEApi.Model.ServerTypeRes.QualityText;
            orbMap = new Dictionary<string, OrbType>()
        {
            { POEApi.Model.ServerTypeRes.OrbTypeChaos, OrbType.Chaos },
            { POEApi.Model.ServerTypeRes.OrbTypeDivine, OrbType.Divine },
            { POEApi.Model.ServerTypeRes.OrbTypeRegal, OrbType.Regal },
            { POEApi.Model.ServerTypeRes.OrbTypeAugmentation, OrbType.Augmentation },
            { POEApi.Model.ServerTypeRes.OrbTypeOrbOfAlchemy, OrbType.Alchemy },
            { POEApi.Model.ServerTypeRes.OrbTypeAlchemyShard, OrbType.AlchemyShard },
            { POEApi.Model.ServerTypeRes.OrbTypeChromatic, OrbType.Chromatic },
            { POEApi.Model.ServerTypeRes.OrbTypeTransmutation, OrbType.Transmutation },
            { POEApi.Model.ServerTypeRes.OrbTypeScouring, OrbType.Scouring },
            { POEApi.Model.ServerTypeRes.OrbTypeGlassblower,OrbType.GlassblowersBauble },
            { POEApi.Model.ServerTypeRes.OrbTypeCartographer, OrbType.Chisel },
            { POEApi.Model.ServerTypeRes.OrbTypeGemcutterPrism, OrbType.GemCutterPrism },
            { POEApi.Model.ServerTypeRes.OrbTypeAlteration, OrbType.Alteration },
            { POEApi.Model.ServerTypeRes.OrbTypeChance, OrbType.Chance },
            { POEApi.Model.ServerTypeRes.OrbTypeRegret, OrbType.Regret },
            { POEApi.Model.ServerTypeRes.OrbTypeExalted, OrbType.Exalted },
            { POEApi.Model.ServerTypeRes.OrbTypeArmourerScrap, OrbType.ArmourersScrap },
            { POEApi.Model.ServerTypeRes.OrbTypeBlessed, OrbType.Blessed},
            { POEApi.Model.ServerTypeRes.OrbTypeBlacksmithWhetstone, OrbType.BlacksmithsWhetstone },
            { POEApi.Model.ServerTypeRes.OrbTypeScrollFragment, OrbType.ScrollFragment },
            { POEApi.Model.ServerTypeRes.OrbTypeJewellerOrb, OrbType.JewelersOrb },
            { POEApi.Model.ServerTypeRes.OrbTypeScrollOfWisdom, OrbType.ScrollofWisdom },
            { POEApi.Model.ServerTypeRes.OrbTypeOrbOfFusing, OrbType.Fusing },
            { POEApi.Model.ServerTypeRes.OrbTypePortalScroll, OrbType.PortalScroll },
            { POEApi.Model.ServerTypeRes.OrbTypeAlbinoRhoaFeather, OrbType.AlbinaRhoaFeather},
            { POEApi.Model.ServerTypeRes.OrbTypeMirror, OrbType.Mirror },
            { POEApi.Model.ServerTypeRes.OrbTypeEternalOrb, OrbType.Eternal},
            { POEApi.Model.ServerTypeRes.OrbTypeImprint, OrbType.Imprint },
            { POEApi.Model.ServerTypeRes.OrbTypeVaalOrb, OrbType.VaalOrb }
        };
        }

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
