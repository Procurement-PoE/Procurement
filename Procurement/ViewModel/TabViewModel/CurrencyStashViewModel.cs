using System.Collections.Generic;
using POEApi.Model;

namespace Procurement.ViewModel
{
    public class CurrencyStashViewModel : CommonTabViewModel
    {
        public CurrencyStashViewModel(Dictionary<Item, ItemDisplayViewModel> stashByLocation) : base(stashByLocation)
        {

        }

        public ItemDisplayViewModel Exalted => GetCurrencyItem(OrbType.Exalted);
        public ItemDisplayViewModel Chaos => GetCurrencyItem(OrbType.Chaos);
        public ItemDisplayViewModel ChaosShard => GetCurrencyItem(OrbType.ChaosShard);
        public ItemDisplayViewModel ScrollFragment => GetCurrencyItem(OrbType.ScrollFragment);
        public ItemDisplayViewModel WisdomScrolls => GetCurrencyItem(OrbType.ScrollofWisdom);
        public ItemDisplayViewModel TownPortalScrolls => GetCurrencyItem(OrbType.PortalScroll);
        public ItemDisplayViewModel BlacksmithsWhetstone => GetCurrencyItem(OrbType.BlacksmithsWhetstone);
        public ItemDisplayViewModel ArmourersScrap => GetCurrencyItem(OrbType.ArmourersScrap);
        public ItemDisplayViewModel GlassblowersBauble => GetCurrencyItem(OrbType.GlassblowersBauble);
        public ItemDisplayViewModel GemCuttersPrism => GetCurrencyItem(OrbType.GemCutterPrism);
        public ItemDisplayViewModel Chisel => GetCurrencyItem(OrbType.Chisel);
        public ItemDisplayViewModel Transmutation => GetCurrencyItem(OrbType.Transmutation);
        public ItemDisplayViewModel Alteration => GetCurrencyItem(OrbType.Alteration);
        public ItemDisplayViewModel Augmentation => GetCurrencyItem(OrbType.Augmentation);
        public ItemDisplayViewModel Mirror => GetCurrencyItem(OrbType.Mirror);
        public ItemDisplayViewModel Alchemy => GetCurrencyItem(OrbType.Alchemy);
        public ItemDisplayViewModel Chance => GetCurrencyItem(OrbType.Chance);
        public ItemDisplayViewModel TransmutationShard => GetCurrencyItem(OrbType.TransmutationShard);
        public ItemDisplayViewModel AlterationShard => GetCurrencyItem(OrbType.AlterationShard);
        public ItemDisplayViewModel Regal => GetCurrencyItem(OrbType.Regal);
        public ItemDisplayViewModel RegalShard => GetCurrencyItem(OrbType.RegalShard);
        public ItemDisplayViewModel AlchemyShard => GetCurrencyItem(OrbType.AlchemyShard);
        public ItemDisplayViewModel Blessed => GetCurrencyItem(OrbType.Blessed);
        public ItemDisplayViewModel Divine => GetCurrencyItem(OrbType.Divine);
        public ItemDisplayViewModel Jewlers => GetCurrencyItem(OrbType.JewelersOrb);
        public ItemDisplayViewModel Fuse => GetCurrencyItem(OrbType.Fusing);
        public ItemDisplayViewModel Chromatic => GetCurrencyItem(OrbType.Chromatic);
        public ItemDisplayViewModel Scour => GetCurrencyItem(OrbType.Scouring);
        public ItemDisplayViewModel Regret => GetCurrencyItem(OrbType.Regret);
        public ItemDisplayViewModel Vaal => GetCurrencyItem(OrbType.VaalOrb);
        public ItemDisplayViewModel Perandus => GetCurrencyItem(OrbType.PerandusCoin);
        public ItemDisplayViewModel Silver => GetCurrencyItem(OrbType.SilverCoin);
        public ItemDisplayViewModel CraftingSection => GetItemAtPosition(28, 0);
        public ItemDisplayViewModel Slot1 => GetItemAtPosition(30, 0);
        public ItemDisplayViewModel Slot2 => GetItemAtPosition(31, 0);
        public ItemDisplayViewModel Slot3 => GetItemAtPosition(32, 0);
        public ItemDisplayViewModel Slot4 => GetItemAtPosition(33, 0);
        public ItemDisplayViewModel Slot5 => GetItemAtPosition(34, 0);
        public ItemDisplayViewModel Slot6 => GetItemAtPosition(40, 0);
        public ItemDisplayViewModel Slot7 => GetItemAtPosition(41, 0);
        public ItemDisplayViewModel Slot8 => GetItemAtPosition(42, 0);
        public ItemDisplayViewModel Slot9 => GetItemAtPosition(43, 0);
        public ItemDisplayViewModel Slot10 => GetItemAtPosition(44, 0);
        public ItemDisplayViewModel Slot11 => GetItemAtPosition(45, 0);
        public ItemDisplayViewModel Slot12 => GetItemAtPosition(46, 0);
        public ItemDisplayViewModel Slot13 => GetItemAtPosition(47, 0);
        public ItemDisplayViewModel Slot14 => GetItemAtPosition(48, 0);
        public ItemDisplayViewModel Apprentice => GetSextant(SextantType.Apprentice);
        public ItemDisplayViewModel Journey => GetSextant(SextantType.Journeyman);
        public ItemDisplayViewModel Master => GetSextant(SextantType.Master);

        public ItemDisplayViewModel Annulment => GetCurrencyItem(OrbType.AnnulmentOrb);
        public ItemDisplayViewModel AnnulmentShard => GetCurrencyItem(OrbType.AnnulmentShard);
        public ItemDisplayViewModel ExaltedShard => GetCurrencyItem(OrbType.ExaltedShard);
        public ItemDisplayViewModel MirrorShard => GetCurrencyItem(OrbType.MirrorShard);

        public bool HasCraftingItem => CraftingSection.Item != null;
        public bool HasExalted => Exalted.Item != null;
        public bool HasChaos => Chaos.Item != null;
        public bool HasChaosShard => ChaosShard.Item != null;
        public bool HasScrollFragment => ScrollFragment.Item != null;
        public bool HasWisdomScrolls => WisdomScrolls.Item != null;
        public bool HasTownPortalScrolls => TownPortalScrolls.Item != null;
        public bool HasBlacksmithsWhetstone => BlacksmithsWhetstone.Item != null;
        public bool HasArmourersScrap => ArmourersScrap.Item != null;
        public bool HasGlassblowersBauble => GlassblowersBauble.Item != null;
        public bool HasGemCuttersPrism => GemCuttersPrism.Item != null;
        public bool HasChisel => Chisel.Item != null;
        public bool HasTransmutation => Transmutation.Item != null;
        public bool HasAlteration => Alteration.Item != null;
        public bool HasAugmentation => Augmentation.Item != null;
        public bool HasMirror => Mirror.Item != null;
        public bool HasAlchemy => Alchemy.Item != null;
        public bool HasChance => Chance.Item != null;
        public bool HasTransmutationShard => TransmutationShard.Item != null;
        public bool HasAlterationShard => AlterationShard.Item != null;
        public bool HasRegal => Regal.Item != null;
        public bool HasRegalShard => RegalShard.Item != null;
        public bool HasAlchemyShard => AlchemyShard.Item != null;
        public bool HasBlessed => Blessed.Item != null;
        public bool HasDivine => Divine.Item != null;
        public bool HasJewlers => Jewlers.Item != null;
        public bool HasFuse => Fuse.Item != null;
        public bool HasChromatic => Chromatic.Item != null;
        public bool HasScour => Scour.Item != null;
        public bool HasRegret => Regret.Item != null;
        public bool HasVaal => Vaal.Item != null;
        public bool HasPerandus => Perandus.Item != null;
        public bool HasSilver => Silver.Item != null;
        public bool HasSlot1 => Slot1.Item != null;
        public bool HasSlot2 => Slot2.Item != null;
        public bool HasSlot3 => Slot3.Item != null;
        public bool HasSlot4 => Slot4.Item != null;
        public bool HasSlot5 => Slot5.Item != null;
        public bool HasSlot6 => Slot6.Item != null;
        public bool HasSlot7 => Slot7.Item != null;
        public bool HasSlot8 => Slot8.Item != null;
        public bool HasSlot9 => Slot9.Item != null;
        public bool HasSlot10 => Slot10.Item != null;
        public bool HasSlot11 => Slot11.Item != null;
        public bool HasSlot12 => Slot12.Item != null;
        public bool HasSlot13 => Slot13.Item != null;
        public bool HasSlot14 => Slot14.Item != null;
        public bool HasApprentice => Apprentice.Item != null;
        public bool HasJourney => Journey.Item != null;
        public bool HasMaster => Master.Item != null;
        public bool HasAnnulment => Annulment.Item != null;
        public bool HasAnnulmentShard => AnnulmentShard.Item != null;
        public bool HasExaltedShard => ExaltedShard.Item != null;
        public bool HasMirrorShard => MirrorShard.Item != null;
    }
}