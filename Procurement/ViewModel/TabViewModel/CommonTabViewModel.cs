using System;
using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using POEApi.Model.Interfaces;

namespace Procurement.ViewModel
{
    public class CommonTabViewModel : ObservableBase
    {
        private readonly Dictionary<Item, ItemDisplayViewModel> _stash;

        protected CommonTabViewModel(Dictionary<Item, ItemDisplayViewModel> stashByLocation)
        {
            _stash = stashByLocation;
        }

        protected ItemDisplayViewModel GetItemAtPosition(int x, int y)
        {
            var itemDisplayViewModel = new ItemDisplayViewModel(null);

            foreach (var item in _stash)
            {
                if (item.Key.X == x & item.Key.Y == y)
                {
                    itemDisplayViewModel = new ItemDisplayViewModel(item.Key);

                    _stash[item.Key] = itemDisplayViewModel;
                    break;
                }
            }

            return itemDisplayViewModel;
        }

        protected ItemDisplayViewModel GetItemCalled(string name)
        {
            var itemDisplayViewModel = new ItemDisplayViewModel(null);

            foreach (var item in _stash)
            {
                if (item.Key.TypeLine.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    itemDisplayViewModel = new ItemDisplayViewModel(item.Key);

                    _stash[item.Key] = itemDisplayViewModel;
                    break;
                }
            }

            return itemDisplayViewModel;
        }

        protected ItemDisplayViewModel GetSextant(SextantType sextantType)
        {
            var itemDisplayViewModel = new ItemDisplayViewModel(null);

            foreach (var item in _stash)
            {
                var sextant = item.Key as Sextant;

                if (sextant?.Type == sextantType)
                {
                    itemDisplayViewModel = new ItemDisplayViewModel(sextant);

                    _stash[sextant] = itemDisplayViewModel;
                    break;
                }
            }

            return itemDisplayViewModel;
        }

        protected ItemDisplayViewModel GetCurrencyItem(OrbType orbType)
        {
            var itemDisplayViewModel = new ItemDisplayViewModel(null);

            foreach (var item in _stash)
            {
                var currency = item.Key as Currency;

                if (currency?.Type == orbType)
                {
                    itemDisplayViewModel = new ItemDisplayViewModel(currency);

                    _stash[currency] = itemDisplayViewModel;
                    break;
                }
            }

            return itemDisplayViewModel;
        }

        protected ItemDisplayViewModel GetEssenceItem(EssenceType essenceType)
        {
            var itemDisplayViewModel = new ItemDisplayViewModel(null);

            foreach (var item in _stash)
            {
                var essence = item.Key as Essence;

                if (essence?.Type == essenceType)
                {
                    itemDisplayViewModel = new ItemDisplayViewModel(essence);

                    _stash[essence] = itemDisplayViewModel;
                    break;
                }
            }

            return itemDisplayViewModel;
        }

        protected ItemDisplayViewModel GetBreach<T>(BreachType breachType) where T : IBreachCurrency
        {
            var itemDisplayViewModel = new ItemDisplayViewModel(null);

            foreach (var breach in _stash.Keys.OfType<T>())
            {
                var breachItem = breach as Item;

                if (breach.Type == breachType)
                {
                    itemDisplayViewModel = new ItemDisplayViewModel(breachItem);

                    _stash[breachItem] = itemDisplayViewModel;
                    break;
                }
            }

            return itemDisplayViewModel;
        }

        protected ItemDisplayViewModel GetScarab(ScarabRank rank, ScarabEffect effect)
        {
            var itemDisplayViewModel = new ItemDisplayViewModel(null);

            foreach (var scarab in _stash.Keys.OfType<Scarab>())
            {
                Item scarabItem = scarab;

                if (scarab.ScarabRank == rank && scarab.ScarabEffect == effect)
                {
                    itemDisplayViewModel = new ItemDisplayViewModel(scarabItem);

                    _stash[scarabItem] = itemDisplayViewModel;
                    break;
                }
            }

            return itemDisplayViewModel;
        }

        protected ItemDisplayViewModel GetOffering()
        {
            var itemDisplayViewModel = new ItemDisplayViewModel(null);

            foreach (var offering in _stash.Keys.OfType<Offering>())
            {
                itemDisplayViewModel = new ItemDisplayViewModel(offering);

                _stash[offering] = itemDisplayViewModel;
                break;
            }

            return itemDisplayViewModel;
        }
    }
}