using System.Collections.Generic;
using POEApi.Model;
using Procurement.Controls;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel
{
    public static class TabFactory
    {
        public static AbstractStashTabControl GenerateTab(Tab tab, List<IFilter> filters)
        {
            AbstractStashTabControl stashTab;

            switch (tab.Type)
            {
                case TabType.Currency:
                    stashTab = new CurrencyStashTab(tab.i, filters);
                    break;
                case TabType.Essence:
                    stashTab = new EssenceStashTab(tab.i, filters);
                    break;
                case TabType.Fragment:
                    stashTab = new FragmentStashTab(tab.i, filters);
                    break;
                default:
                    stashTab = new StashTabControl(tab.i, filters);
                    break;
            }

            return stashTab;
        }
    }
}