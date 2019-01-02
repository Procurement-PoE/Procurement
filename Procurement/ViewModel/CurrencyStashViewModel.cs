using System.Collections.Generic;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel
{
    public class CurrencyStashViewModel : ObservableBase
    {
        private readonly Dictionary<Item, ItemDisplayViewModel> _stash;

        public CurrencyStashViewModel(Dictionary<Item, ItemDisplayViewModel> stashByLocation)
        {
            _stash = stashByLocation;
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

        public ItemDisplayViewModel Annulment  => GetCurrencyItem(OrbType.AnnulmentOrb);
        public ItemDisplayViewModel AnnulmentShard => GetCurrencyItem(OrbType.AnnulmentOrb);
        public ItemDisplayViewModel ExaltedShard => GetCurrencyItem(OrbType.ExaltedShard);
        public ItemDisplayViewModel MirrorShard => GetCurrencyItem(OrbType.AnnulmentShard);

        private ItemDisplayViewModel GetItemAtPosition(int x, int y)
        {
            ItemDisplayViewModel rtnViewModel = null;

            foreach (var item in _stash)
            {
                if (item.Key.X == x & item.Key.Y == y)
                {
                    rtnViewModel = new ItemDisplayViewModel(item.Key);

                    _stash[item.Key] = rtnViewModel;
                    break;
                }
            }

            return rtnViewModel ?? (rtnViewModel = new ItemDisplayViewModel(null));
        }

        private ItemDisplayViewModel GetSextant(SextantType sextantType)
        {
            ItemDisplayViewModel rtnViewModel = null;

            foreach (var item in _stash)
            {
                var sextant = item.Key as Sextant;

                if (sextant?.Type == sextantType)
                {
                    rtnViewModel = new ItemDisplayViewModel(sextant);

                    _stash[sextant] = rtnViewModel;
                    break;
                }
            }

            return rtnViewModel ?? (rtnViewModel = new ItemDisplayViewModel(null));
        }
        
        private ItemDisplayViewModel GetCurrencyItem(OrbType orbType)
        {
            ItemDisplayViewModel rtnViewModel = null;

            foreach (var item in _stash)
            {
                var currency = item.Key as Currency;

                if (currency?.Type == orbType)
                {
                    rtnViewModel = new ItemDisplayViewModel(currency);

                    _stash[currency] = rtnViewModel;
                    break;
                }
            }

            return rtnViewModel ?? (rtnViewModel = new ItemDisplayViewModel(null));
        }
    }
}