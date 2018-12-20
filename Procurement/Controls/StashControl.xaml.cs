using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using POEApi.Infrastructure;
using POEApi.Model;
using Procurement.ViewModel;

namespace Procurement.Controls
{
    public partial class StashControl : AbstractStashControl
    {
        private const int NORMAL_SPACING = 12;
        private const int QUAD_SPACING = 24;
        
        private TabType tabType;

        public StashControl(int tabNumber) : base(tabNumber)
        {
            InitializeComponent();

            Loaded += StashControl_Loaded;
            ApplicationState.LeagueChanged += ApplicationState_LeagueChanged;

            TabNumber = tabNumber;

            SetPremiumTabBorderColour();
        }

        public override void ForceUpdate()
        {
            if (Ready == false && Stash == null)
                refresh();

            base.ForceUpdate();
        }

        public override Border Border => LocalBorder;

        public override void RefreshTab(string accountName)
        {
            base.RefreshTab(accountName);

            refresh();
        }

        private void ApplicationState_LeagueChanged(object sender, PropertyChangedEventArgs e)
        {
            Ready = false;
        }

        private void StashControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Ready)
            {
                return;
            }

            refresh();
        }

        private void refresh()
        {
            Stash = ApplicationState.Stash[ApplicationState.CurrentLeague].GetItemsByTab(TabNumber);
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
            StashByLocation.Clear();

            foreach (var item in Stash)
            {
                var entry = StashByLocation.Keys.FirstOrDefault(x => x.X == item.X
                                                                     && x.Y == item.Y);

                if (entry != null)
                {
                    continue;
                }

                StashByLocation.Add(item, getImage(item));
            }
        }

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

            for (var i = 0; i < columns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                for (var j = 0; j < rows; j++)
                {
                    if (i == 0)
                    {
                        grid.RowDefinitions.Add(new RowDefinition());
                    }

                    var childGrid = new Grid();

                    var currentKey = new Tuple<int, int>(i, j);
                    // Divination cards always have Y == 0, and X has a unique value for each type of card.  Map the
                    // card's "real" location to a fake one on the grid, so it can be displayed.
                    var stashLocation = tabType == TabType.DivinationCard ? new Tuple<int, int>(j * columns + i, 0) : currentKey;

                    var key = StashByLocation.Keys.FirstOrDefault(item => item.X == stashLocation.Item1
                                                                          && item.Y == stashLocation.Item2);
                    if (key == null)
                    {
                        continue;
                    }

                    var gearAtLocation = StashByLocation[key];

                    childGrid.Children.Add(gearAtLocation);

                    Grid.SetColumn(childGrid, i);
                    Grid.SetRow(childGrid, j);
                    if (gearAtLocation.ViewModel.Item.H > 1)
                    {
                        Grid.SetRowSpan(childGrid, gearAtLocation.ViewModel.Item.H);
                    }

                    if (gearAtLocation.ViewModel.Item.W > 1)
                    {
                        Grid.SetColumnSpan(childGrid, gearAtLocation.ViewModel.Item.W);
                    }

                    grid.Children.Add(childGrid);
                }
            }

            Ready = true;
        }

        private ItemDisplay getImage(Item item)
        {
            return new ItemDisplay
            {
                ViewModel = new ItemDisplayViewModel(item),
                IsQuadStash = tabType == TabType.Quad || tabType == TabType.DivinationCard
            };
        }
    }
}