using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel
{
    public class CurrencyStashViewModel : INotifyPropertyChanged
    {
        private readonly List<Item> _stash;

        public CurrencyStashViewModel(List<Item> stash)
        {
            _stash = stash;
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

        public event PropertyChangedEventHandler PropertyChanged;

        private ItemDisplayViewModel GetItemAtPosition(int x, int y)
        {
            var item = _stash.FirstOrDefault(i => i.X == x && i.Y == y);

            return new ItemDisplayViewModel(item);
        }

        private ItemDisplayViewModel GetSextant(SextantType sextanType)
        {
            foreach (var item in _stash)
            {
                var curency = item as Sextant;

                if (curency?.Type == sextanType)
                    return new ItemDisplayViewModel(item);
            }

            return new ItemDisplayViewModel(null);
        }
        
        private ItemDisplayViewModel GetCurrencyItem(OrbType orbType)
        {
            foreach (var item in _stash)
            {
                var curency = item as Currency;

                if (curency?.Type == orbType)
                    return new ItemDisplayViewModel(item);
            }

            return new ItemDisplayViewModel(null);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}