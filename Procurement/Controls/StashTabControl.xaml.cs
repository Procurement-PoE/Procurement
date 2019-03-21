using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using POEApi.Model;
using Procurement.ViewModel;
using Procurement.ViewModel.Filters;

namespace Procurement.Controls
{
    public partial class StashTabControl : AbstractStashTabControl
    {
        private const int NormalSpacing = 12;
        private const int QuadSpacing = 24;
        
        public StashTabControl(int tabNumber) : base(tabNumber)
        {
            InitializeComponent();
            Refresh();
            Ready = true;

            SetPremiumTabBorderColour();
        }

        public StashTabControl(int tabNumber, List<IFilter> list) : this(tabNumber)
        {
            Filters = list;
        }

        public override void ForceUpdate()
        {
            if (Ready == false && Stash == null)
                Refresh();

            base.ForceUpdate();
        }

        public override Border Border => LocalBorder;

        public override void RefreshTab(string accountName)
        {
            base.RefreshTab(accountName);

            Refresh();
        }

        public override void Refresh()
        {
            base.Refresh();

            Render();
        }

        private void Render()
        {
            int columns = NormalSpacing, rows = NormalSpacing;

            // Force divination card tabs to use quad tab spacing so there is enough room to show all the different
            // cards.  A normal tab only has 144 slots, but there are >200 divination cards.
            if (TabType == TabType.Quad || TabType == TabType.DivinationCard)
            {
                columns = QuadSpacing;
                rows = QuadSpacing;

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
                    var stashLocation = TabType == TabType.DivinationCard ? new Tuple<int, int>(j * columns + i, 0) : currentKey;

                    var keyItem = TabItemsToViewModels.Keys.FirstOrDefault(item => item.X == stashLocation.Item1
                                                                             && item.Y == stashLocation.Item2);
                    if (keyItem == null)
                    {
                        continue;
                    }

                    ItemDisplayViewModel itemViewModel = TabItemsToViewModels[keyItem];

                    var itemDisplay = new ItemDisplay
                    {
                        ViewModel = itemViewModel,
                        IsQuadStash = TabType == TabType.Quad || TabType == TabType.DivinationCard
                    };

                    childGrid.Children.Add(itemDisplay);

                    SetBackground(childGrid, keyItem);
                    
                    Grid.SetColumn(childGrid, i);
                    Grid.SetRow(childGrid, j);
                    if (itemViewModel.Item.H > 1)
                    {
                        Grid.SetRowSpan(childGrid, itemViewModel.Item.H);
                    }

                    if (itemViewModel.Item.W > 1)
                    {
                        Grid.SetColumnSpan(childGrid, itemViewModel.Item.W);
                    }

                    grid.Children.Add(childGrid);
                }
            }

            Ready = true;
        }
    }
}