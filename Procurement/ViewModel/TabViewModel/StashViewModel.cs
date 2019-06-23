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
        private List<TabContent> tabsAndContent;
        private StashView stashView;
        private List<IFilter> categoryFilter;
        private TabItem selectedTab { get; set; }
        private ResourceDictionary expressionDark;
        private OrbType configuredOrbType;
        private bool currencyDistributionUsesCount;
        private string filter;

        private const string _enableTabRefreshOnLocationChangedConfigName = "EnableTabRefreshOnLocationChanged";

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
                item.Stash.Filters = allfilters;
                item.Stash.ForceUpdate();
                if (item.Stash.ItemsMatchingFiltersCount == 0)
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

        public string Total => "Total " + configuredOrbType + " in Orbs";

        public string TotalOrbValue => ApplicationState.Stash[ApplicationState.CurrentLeague].GetTotal(configuredOrbType).ToString();

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
        });

        public ICommand RefreshUsedCommand => new RelayCommand(x =>
        {
            ScreenController.Instance.LoadRefreshViewUsed();
        });

        public static DateTime LastAutomaticRefresh { get; protected set; }
        public void OnClientLogFileChanged(object sender, ClientLogFileEventArgs e)
        {
            // All actions currently taken when the log file changes relate to refreshing staash tabs.  This checks
            // first that we are logged in, and quits early if we are not.
            if (!LoggedIn)
                return;

            lock (this)
            {
                if ((DateTime.Now - LastAutomaticRefresh).TotalSeconds <= 120)
                    return;
                LastAutomaticRefresh = DateTime.Now;

                Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        if (ScreenController.Instance.ButtonsVisible)
                        {
                            ScreenController.Instance.LoadRefreshViewUsed();
                        }
                    }));
            }
        }

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

            ApplicationState.Model.StashLoading += ApplicationState_StashLoading;

            if (Settings.UserSettings.Keys.Contains(_enableTabRefreshOnLocationChangedConfigName))
            {
                var enabled = false;
                if (bool.TryParse(Settings.UserSettings[_enableTabRefreshOnLocationChangedConfigName], out enabled)
                    && enabled)
                {
                    ClientLogFileWatcher.ClientLogFileChanged -= OnClientLogFileChanged;
                    ClientLogFileWatcher.ClientLogFileChanged += OnClientLogFileChanged;
                }
            }
        }

        private void ApplicationState_StashLoading(POEModel sender, POEApi.Model.Events.StashLoadedEventArgs e)
        {
            if (e.State != POEApi.Model.Events.POEEventState.AfterEvent)
                return;

            foreach (var tabAndContent in tabsAndContent)
            {
                if (tabAndContent.Index == e.StashID && tabAndContent.Stash is AbstractStashTabControl)
                {
                    // This tab has been refreshed.  We mark it as not ready, so it is refreshed the next time it is
                    // selected.
                    (tabAndContent.Stash as AbstractStashTabControl).Ready = false;
                    break;
                }
            }
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
            tabsAndContent.Clear();

            getAvailableItems();
            stashView.tabControl.SelectionChanged -= new SelectionChangedEventHandler(tabControl_SelectionChanged);
            stashView.tabControl.Items.Clear();
            stashView.tabControl.SelectionChanged += new SelectionChangedEventHandler(tabControl_SelectionChanged);
            stashView_Loaded(sender, null);
            OnPropertyChanged(nameof(AvailableItems));
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(TotalOrbValue));
            OnPropertyChanged(nameof(TotalDistibution));
            OnPropertyChanged(nameof(GemDistribution));
        }

        public void GetTabList(object o)
        {
            Button selector = o as Button;
            ScrollViewer scrollViewer = selector.TemplatedParent as ScrollViewer;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            TabControl tabControl = scrollViewer.TemplatedParent as TabControl;

            selector.ContextMenu = GetContextMenu(selector, tabControl);
            selector.ContextMenu.Height = 550;
            selector.ContextMenu.IsOpen = true;
        }

        private ContextMenu GetContextMenu(Button target, TabControl tabControl)
        {
            ContextMenu menu = new ContextMenu();
            menu.PlacementTarget = target;
            menu.Resources = expressionDark;

            foreach (TabItem item in tabControl.Items)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Tag = item;
                TabVisuals tabVisuals = item.Tag as TabVisuals;
                menuItem.Header = tabVisuals.Name;

                if (tabVisuals.Colour != null)
                {
                    menuItem.Style = new Style();
                    menuItem.BorderThickness = new Thickness(0);
                    menuItem.Background = new SolidColorBrush(tabVisuals.Colour.WpfColor);
                    menuItem.Foreground = new SolidColorBrush(ContrastColor(tabVisuals.Colour));
                }

                menuItem.Click += (o, e) => { closeAndSelect(menu, menuItem); };
                menu.Items.Add(menuItem);
            }

            return menu;
        }

        private Color ContrastColor(Colour color)
        {
            // Counting the perceptive luminance - human eye favors green color...
            double luminance = (0.299 * color.r + 0.587 * color.g + 0.114 * color.b) / 255;

            if (luminance > 0.5)
                return Color.FromRgb((byte)59, (byte)44, (byte)27);

            return Color.FromRgb((byte)255, (byte)192, (byte)119);
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
            var stash = ApplicationState.Stash[ApplicationState.CurrentLeague];
            for (var i = 1; i <= ApplicationState.Stash[ApplicationState.CurrentLeague].NumberOfTabs; i++)
            {
                var currentTab = stash.Tabs[i - 1];

                var item = new TabItem
                {
                    Header = StashHelper.GenerateTabImage(currentTab, false),
                    Tag = new TabVisuals()
                    {
                        Name = currentTab.Name,
                        Colour = currentTab.Colour
                    },
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent
                };

                var stashTab = TabFactory.GenerateTab(currentTab, getUserFilter(string.Empty));

                CraftTabAndContent(item, stashTab, i);

                stashView.tabControl.Items.Add(item);
            }

            stashView.Loaded -= stashView_Loaded;
        }

        private void CraftTabAndContent(TabItem item, AbstractStashTabControl stashTab, int i)
        {
            item.Content = stashTab;

            addContextMenu(item, stashTab);

            tabsAndContent.Add(new TabContent(i - 1, item, stashTab));
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