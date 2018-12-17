using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using POEApi.Model;
using Procurement.Controls;
using Procurement.Utility;
using Procurement.View;
using Procurement.ViewModel.Filters;
using System.Text;
using POEApi.Infrastructure;
using Procurement.Interfaces;
using Procurement.View.ViewModel;

namespace Procurement.ViewModel
{
    public class StashViewModel : ObservableBase
    {
        private class TabContent
        {
            public int Index { get; set; }
            public TabItem TabItem { get; set; }
            public IStashControl Stash { get; set; }
            public TabContent(int index, TabItem tabItem, IStashControl stash)
            {
                this.Index = index;
                this.TabItem = tabItem;
                this.Stash = stash;
            }
        }

        private List<TabContent> tabsAndContent;
        private StashView stashView;
        private List<IFilter> categoryFilter;
        private TabItem selectedTab { get; set; }
        private ResourceDictionary expressionDark;
        private OrbType configuredOrbType;
        private bool currencyDistributionUsesCount;
        private string filter;

        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                processFilter();
            }
        }

        private void processFilter()
        {
            List<IFilter> allfilters = getUserFilter(filter);
            allfilters.AddRange(categoryFilter);

            foreach (var item in tabsAndContent)
            {
                item.Stash.Filter = allfilters;
                item.Stash.ForceUpdate();
                if (item.Stash.FilterResults == 0)
                {
                    item.TabItem.Visibility = Visibility.Collapsed;
                    (item.TabItem.Content as UIElement).Visibility = Visibility.Collapsed;
                }
                else
                {
                    item.TabItem.Visibility = Visibility.Visible;
                    (item.TabItem.Content as UIElement).Visibility = Visibility.Visible;
                }
            }
            var first = tabsAndContent.Find(w => w.TabItem.Visibility == Visibility.Visible);
            if (first != null)
                first.TabItem.IsSelected = true;
        }

        public bool LoggedIn { get { return !ApplicationState.Model.Offline; } }

        public void SetCategoryFilter(string category, bool? isChecked)
        {
            if (!isChecked.Value)
            {
                var filtersBeGone = CategoryManager.GetCategory(category).Select(f => f.GetType()).ToList();
                categoryFilter.RemoveAll(f => filtersBeGone.Contains(f.GetType()));
                processFilter();
                return;
            }

            categoryFilter.AddRange(CategoryManager.GetCategory(category));
            processFilter();
        }

        public ICommand GetTabs => new RelayCommand(GetTabList);

        public List<AdvancedSearchCategory> AvailableCategories { get; private set; }

        public List<string> Leagues
        {
            get { return ApplicationState.Leagues; }
        }

        public string CurrentLeague
        {
            get { return ApplicationState.CurrentLeague; }
        }

        public string Total
        {
            get { return "Total " + configuredOrbType.ToString() + " in Orbs : " + ApplicationState.Stash[ApplicationState.CurrentLeague].GetTotal(configuredOrbType).ToString(); }
        }

        public Dictionary<OrbType, double> TotalDistibution
        {
            get
            {
                if (currencyDistributionUsesCount)
                    return ApplicationState.Stash[ApplicationState.CurrentLeague].GetTotalCurrencyCount();

                return ApplicationState.Stash[ApplicationState.CurrentLeague].GetTotalCurrencyDistribution(configuredOrbType);
            }
        }

        public SortedDictionary<string, int> GemDistribution
        {
            get
            {
                return ApplicationState.Stash[ApplicationState.CurrentLeague].GetTotalGemDistribution();
            }
        }

        public List<string> AvailableItems { get; private set; }


        public ICommand RefreshCommand => new RelayCommand(x =>
        {
            ScreenController.Instance.LoadRefreshView();
            ScreenController.Instance.InvalidateRecipeScreen();
        });

        public ICommand RefreshUsedCommand => new RelayCommand(x =>
        {
            ScreenController.Instance.LoadRefreshViewUsed();
            ScreenController.Instance.InvalidateRecipeScreen();
        });

        public StashViewModel(StashView stashView)
        {
            this.stashView = stashView;

            categoryFilter = new List<IFilter>();
            AvailableCategories = CategoryManager.GetAvailableCategories();
            tabsAndContent = new List<TabContent>();
            stashView.Loaded += new RoutedEventHandler(stashView_Loaded);

            ApplicationState.LeagueChanged += new PropertyChangedEventHandler(ApplicationState_LeagueChanged);
            stashView.tabControl.SelectionChanged += new SelectionChangedEventHandler(tabControl_SelectionChanged);
            getAvailableItems();
            expressionDark = Application.LoadComponent(new Uri("/Procurement;component/Controls/ExpressionDark.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;

            configuredOrbType = OrbType.Chaos;
            string currencyDistributionMetric = Settings.UserSettings["CurrencyDistributionMetric"];
            if (currencyDistributionMetric.ToLower() == "count")
                currencyDistributionUsesCount = true;
            else
                configuredOrbType = (OrbType)Enum.Parse(typeof(OrbType), currencyDistributionMetric);
        }

        private void getAvailableItems()
        {
            try
            {
                AvailableItems = ApplicationState.Stash[ApplicationState.CurrentLeague].Get<Item>().SelectMany(i => getSearchTerms(i)).Distinct().ToList();
            }
            catch (KeyNotFoundException kex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine(string.Format("Error: attempted to get items for the non existant league '{0}'", ApplicationState.CurrentLeague));
                sb.AppendLine("Current leagues are:");
                foreach (var item in ApplicationState.Leagues)
                    sb.AppendLine(item);
                sb.AppendLine();
                sb.AppendLine("Exception details : ");
                sb.AppendLine(kex.ToString());

                Logger.Log(sb.ToString());

                MessageBox.Show(string.Format("Error getting items for {0} league, are you sure your league settings are correct?", ApplicationState.CurrentLeague), "Error loading items", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private IEnumerable<string> getSearchTerms(Item item)
        {
            yield return item.TypeLine;
            if (!string.IsNullOrEmpty(item.Name))
                yield return item.Name;

            Gear gear = item as Gear;
            if (gear != null)
                yield return gear.GearType.ToString();
        }

        void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectedTab != null)
                unselectPreviousTab(selectedTab);

            var item = stashView.tabControl.SelectedItem as TabItem;
            selectedTab = item;
            Image i = item.Header as Image;
            CroppedBitmap bm = (CroppedBitmap)i.Source;
            Tab tab = (Tab)i.Tag;
            item.Header = StashHelper.GenerateTabImage(tab, true);
        }

        private void unselectPreviousTab(TabItem selectedTab)
        {
            Image i = selectedTab.Header as Image;
            Tab tab = i.Tag as Tab;
            selectedTab.Header = StashHelper.GenerateTabImage(tab, false);
        }

        void ApplicationState_LeagueChanged(object sender, PropertyChangedEventArgs e)
        {
            getAvailableItems();
            stashView.tabControl.SelectionChanged -= new SelectionChangedEventHandler(tabControl_SelectionChanged);
            stashView.tabControl.Items.Clear();
            stashView.tabControl.SelectionChanged += new SelectionChangedEventHandler(tabControl_SelectionChanged);
            stashView_Loaded(sender, null);
            OnPropertyChanged(nameof(AvailableItems));
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(TotalDistibution));
            OnPropertyChanged(nameof(GemDistribution));
        }

        public void GetTabList(object o)
        {
            Button selector = o as Button;
            ScrollViewer scrollViewer = selector.TemplatedParent as ScrollViewer;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            TabControl tabControl = scrollViewer.TemplatedParent as TabControl;

            selector.ContextMenu = getContextMenu(selector, tabControl);
            selector.ContextMenu.Height = 550;
            selector.ContextMenu.IsOpen = true;
        }

        private ContextMenu getContextMenu(Button target, TabControl tabControl)
        {
            ContextMenu menu = new ContextMenu();
            menu.PlacementTarget = target;
            menu.Resources = expressionDark;

            foreach (TabItem item in tabControl.Items)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Tag = item;
                menuItem.Header = item.Tag.ToString();
                menuItem.Click += (o, e) => { closeAndSelect(menu, menuItem); };
                menu.Items.Add(menuItem);
            }

            return menu;
        }

        private void closeAndSelect(ContextMenu menu, MenuItem menuItem)
        {
            menu.IsOpen = false;
            TabItem newCurrent = menuItem.Tag as TabItem;
            newCurrent.BringIntoView();
            newCurrent.IsSelected = true;
        }

        void stashView_Loaded(object sender, RoutedEventArgs e)
        {
            for (var i = 1; i <= ApplicationState.Stash[ApplicationState.CurrentLeague].NumberOfTabs; i++)
            {
                var stash = ApplicationState.Stash[ApplicationState.CurrentLeague];
                var currentTab = stash.Tabs[i - 1];

                var item = new TabItem
                {
                    Header = StashHelper.GenerateTabImage(currentTab, false),
                    Tag = currentTab.Name,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent
                };

                switch (currentTab.Type)
                {
                    case TabType.Currency:
                        var currencyStash = new CurrencyStash(currentTab.i, getUserFilter(string.Empty));

                        item.Content = currencyStash;

                        addContextMenu(item, currencyStash);

                        tabsAndContent.Add(new TabContent(i - 1, item, currencyStash));
                        break;
                    case TabType.Essence:
                        var essenceStash = new EssenceStash(currentTab.i, getUserFilter(string.Empty));

                        item.Content = essenceStash;

                        addContextMenu(item, essenceStash);

                        tabsAndContent.Add(new TabContent(i - 1, item, essenceStash));
                        break;
                    case TabType.Fragment:
                        var fragmentStash = new FragmentStash(currentTab.i, getUserFilter(string.Empty));

                        item.Content = fragmentStash;

                        addContextMenu(item, fragmentStash);

                        tabsAndContent.Add(new TabContent(i - 1, item, fragmentStash));
                        break;
                    default:
                        var itemStash = new StashControl();

                        itemStash.Filter = getUserFilter(string.Empty);
                        itemStash.TabNumber = currentTab.i;

                        item.Content = itemStash;
                        addContextMenu(item, itemStash);

                        tabsAndContent.Add(new TabContent(i - 1, item, itemStash));
                        break;
                }


                stashView.tabControl.Items.Add(item);
                
            }

            stashView.Loaded -= new System.Windows.RoutedEventHandler(stashView_Loaded);
        }

        private void addContextMenu(TabItem item, IStashControl itemStash)
        {
            ContextMenu contextMenu = new ContextMenu();

            if (!ApplicationState.Model.Offline)
                contextMenu.Items.Add(getMenuItem(itemStash, "Refresh", refresh_Click));
            
            contextMenu.Items.Add(getMenuItem(itemStash, "Set Tabwide Buyout", setTabBuyout_Click));

            item.ContextMenu = contextMenu;
        }

        private MenuItem getMenuItem(IStashControl itemStash, string header, RoutedEventHandler handler)
        {
            MenuItem menuItem = new MenuItem() { Header = header };
            menuItem.Tag = itemStash;
            menuItem.Click += new RoutedEventHandler(handler);

            return menuItem;
        }

        private static List<IFilter> getUserFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return new List<IFilter>();

            UserSearchFilter searchCriteria = new UserSearchFilter(filter);
            return new List<IFilter>() { searchCriteria };
        }

        void setTabBuyout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IStashControl stash = getStash(sender);

                var tabName = ApplicationState.Stash[ApplicationState.CurrentLeague].GetTabNameByTabId(stash.TabNumber);

                var pricingInfo = new PricingInfo();

                if (Settings.TabsBuyouts.ContainsKey(tabName))
                    pricingInfo.Update(Settings.TabsBuyouts[tabName]);

                SetTabBuyoutView buyoutView = new SetTabBuyoutView(pricingInfo, tabName);
                buyoutView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                buyoutView.Update += buyoutView_Update;
                buyoutView.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Log("Exception in setTabBuyout_Click: " + ex.ToString());
                MessageBox.Show("Error setting tabwide buyout, error details logged to DebugInfo.log, please open a ticket at https://github.com/Stickymaddness/Procurement/issues", "Error setting tabwide buyout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void buyoutView_Update(PricingInfo buyoutInfo, string tabName)
        {
            if (buyoutInfo.Enabled)
                Settings.TabsBuyouts[tabName] = buyoutInfo.GetSaveText();
            else
                Settings.TabsBuyouts.Remove(tabName);

            Settings.SaveTabBuyouts();
        }
        
        void refresh_Click(object sender, RoutedEventArgs e)
        {                      
            IStashControl stash = getStash(sender);
            stash.RefreshTab(ApplicationState.AccountName);
            ScreenController.Instance.RefreshRecipeScreen();
            ScreenController.Instance.UpdateTrading();
        }

        private static IStashControl getStash(object sender)
        {
            MenuItem source = sender as MenuItem;
            IStashControl stash = source.Tag as IStashControl;

            return stash;
        }
    }
}