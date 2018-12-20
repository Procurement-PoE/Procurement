using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using POEApi.Model;
using Procurement.ViewModel;
using POEApi.Infrastructure;
using System.Windows.Media.Imaging;

namespace Procurement.Controls
{
    public partial class StashControl : AbstractStashControl
    {
        private bool initialized = false;
        private TabType tabType;

        private Dictionary<Tuple<int, int>, Item> stashByLocation;
        private Dictionary<Tuple<int, int>, Border> borderByLocation;

        public override void ForceUpdate()
        {
            if (initialized == false && Stash == null)
                refresh();

            FilterResults = !Filter.Any() ? -1 : 0;

            foreach (var item in Stash)
            {
                Tuple<int, int> index;
                if (tabType == TabType.DivinationCard)
                {
                    // Divination cards always have Y == 0, and X has a unique value for each type of card.  Map the
                    // card's "real" location to a fake one on the grid, so it can be displayed.
                    int remainder = 0;
                    int row = Math.DivRem(item.X, QUAD_SPACING, out remainder);
                    index = Tuple.Create<int, int>(remainder, row);
                }
                else
                {
                    index = Tuple.Create<int, int>(item.X, item.Y);
                }

                // Currency tab does not have the standard 12x12 grid
                // so we have to check each column exists before attempting to access it
                if (borderByLocation.ContainsKey(index))
                {
                    updateResult(borderByLocation[index], search(item));
                }
            }

            this.UpdateLayout();
        }

        private void updateResult(Border border, bool isResult)
        {
            if (isResult)
            {
                FilterResults++;
                border.BorderBrush = Brushes.Yellow;
                border.Background = Brushes.Black;
                return;
            }

            border.BorderBrush = Brushes.Transparent;
            border.Background = Brushes.Transparent;
        }

        public override void RefreshTab(string accountName)
        {
            base.RefreshTab(accountName);

            refresh();
        }

        public StashControl(int tabNumber)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(StashControl_Loaded);
            ApplicationState.LeagueChanged += ApplicationState_LeagueChanged;
            stashByLocation = new Dictionary<Tuple<int, int>, Item>();

            TabNumber = tabNumber;

            SetPremiumTabBorderColour();
        }

        void ApplicationState_LeagueChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            initialized = false;
        }

        void StashControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (initialized)
                return;

            refresh();
        }

        private void refresh()
        {
            this.Stash = ApplicationState.Stash[ApplicationState.CurrentLeague].GetItemsByTab(TabNumber);
            tabType = GetTabType();

            updateStashByLocation();
            render(tabType);
        }

        private TabType GetTabType()
        {
            try
            {
                return ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs[TabNumber].Type;
            }
            catch (Exception ex)
            {
                Logger.Log("Error in StashControl.GetTabType: " + ex);
                return TabType.Normal;
            }
        }

        private void updateStashByLocation()
        {
            stashByLocation.Clear();

            foreach (var item in this.Stash)
            {
                var key = Tuple.Create<int, int>(item.X, item.Y);

                if (stashByLocation.ContainsKey(key))
                    continue;

                stashByLocation.Add(key, item);
            }
        }

        private const int NORMAL_SPACING = 12;
        private const int QUAD_SPACING = 24;

        private void render(TabType tabType)
        {
            int columns = NORMAL_SPACING, rows = NORMAL_SPACING;

            // Force divination card tabs to use quad tab spacing so there is enough room to show all the different
            // cards.  A normal tab only has 144 slots, but there are >200 divination cards.
            if (tabType == TabType.Quad || tabType == TabType.DivinationCard)
            {
                columns = QUAD_SPACING;
                rows = QUAD_SPACING;

                gridOuter.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/stash-quad-grid.png")));
            }

            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            grid.Children.Clear();

            borderByLocation = new Dictionary<Tuple<int, int>, Border>();

            for (int i = 0; i < columns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                for (int j = 0; j < rows; j++)
                {
                    if (i == 0)
                        grid.RowDefinitions.Add(new RowDefinition());

                    Grid childGrid = new Grid();

                    Tuple<int, int> currentKey = new Tuple<int, int>(i, j);
                    // Divination cards always have Y == 0, and X has a unique value for each type of card.  Map the
                    // card's "real" location to a fake one on the grid, so it can be displayed.
                    Tuple<int, int> stashLocation = tabType == TabType.DivinationCard ?
                        new Tuple<int, int>(j * columns + i, 0) : currentKey;

                    if (!stashByLocation.ContainsKey(stashLocation))
                        continue;

                    Item gearAtLocation = stashByLocation[stashLocation];

                    setBackround(childGrid, gearAtLocation);
                    //if (search(gearAtLocation))
                    Border border = getBorder();
                    borderByLocation[currentKey] = border;
                    childGrid.Children.Add(border);

                    childGrid.Children.Add(getImage(gearAtLocation));

                    Grid.SetColumn(childGrid, i);
                    Grid.SetRow(childGrid, j);
                    if (gearAtLocation.H > 1)
                        Grid.SetRowSpan(childGrid, gearAtLocation.H);

                    if (gearAtLocation.W > 1)
                        Grid.SetColumnSpan(childGrid, gearAtLocation.W);

                    grid.Children.Add(childGrid);
                }
            }

            initialized = true;
        }

        private UIElement getImage(Item item)
        {
            return new ItemDisplay()
            {
                DataContext = new ItemDisplayViewModel(item),
                IsQuadStash = tabType == TabType.Quad || tabType == TabType.DivinationCard,
            };
        }

        void popup_LostMouseCapture(object sender, MouseEventArgs e)
        {
            (sender as Popup).IsOpen = false;
        }

        private Border getBorder()
        {
            Border b = new Border();
            b.BorderBrush = Brushes.Transparent;
            b.BorderThickness = new Thickness(2);
            return b;
        }

        private void setBackround(Grid childGrid, Item item)
        {
            if (item is Gear && (item as Gear).Rarity != Rarity.Normal && (item as Gear).Explicitmods == null)
                childGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#88001D"));
            else
                childGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#21007F"));

            childGrid.Background.Opacity = 0.3;
        }

        private bool search(Item item)
        {
            if (Filter.Count() == 0)
                return false;

            return Filter.All(filter => filter.Applicable(item));
        }

        public override Border Border => LocalBorder;
    }
}
