using System.Collections.Generic;
using System.Linq;
using Procurement.ViewModel.Recipes;
using System.ComponentModel;
using POEApi.Model;

namespace Procurement.ViewModel
{
    public class RecipeResultViewModel : ObservableBase
    {
        private Dictionary<string, List<RecipeResult>> results;
        private RecipeManager manager;

        public Dictionary<string, List<RecipeResult>> Results
        {
            get { return results; }
            set 
            { 
                results = value;

                OnPropertyChanged();
            }
        }

        private Item selectedItem;

        public Item SelectedItem
        {
            get { return selectedItem; }
            set 
            { 
                selectedItem = value;

                OnPropertyChanged();
            }
        }
        

        public RecipeResultViewModel()
        {
            manager = new RecipeManager();
            ApplicationState.LeagueChanged += ApplicationState_LeagueChanged;
            updateResults();
        }

        public void RadioButtonSelected(Item item)
        {
            SelectedItem = item;
        }

        Dictionary<Tab, List<Item>> GetUsableCurrentLeagueItemsByTab()
        {
            Dictionary<Tab, List<Item>> itemsByTab = new Dictionary<Tab, List<Item>>();
            Stash stash = ApplicationState.Stash[ApplicationState.CurrentLeague];

            var usableTabs = stash.Tabs.Where(t => !Settings.Lists["IgnoreTabsInRecipes"].Contains(t.Name)).ToList();
            foreach (var tab in usableTabs)
            {
                itemsByTab.Add(tab, stash.GetItemsByTab(tab.i));
            }

            return itemsByTab;
        }

        private void updateResults()
        {
            Dictionary<Tab, List<Item>> itemsByTab = GetUsableCurrentLeagueItemsByTab();
            Results = manager.Run(itemsByTab);
            if (Results.Count > 0)
                SelectedItem = Results.Values.First().First().MatchedItems[0];

            ItemFilterUpdater.UpdateLootFilters();
        }

        public void RefreshRecipes()
        {
            updateResults();
        }

        void ApplicationState_LeagueChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            updateResults();
        }
    }
}
