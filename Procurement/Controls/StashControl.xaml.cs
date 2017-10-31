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
using Procurement.ViewModel.Filters;
using System.Diagnostics;
using POEApi.Infrastructure;
using Procurement.Interfaces;

namespace Procurement.Controls
{
    public partial class StashControl : UserControl, IStashControl
    {
        public int TabNumber { get; set; }
        public int FilterResults { get; set; }
        private bool initialized = false;

        private Dictionary<Tuple<int, int>, Item> stashByLocation;
        private Dictionary<Tuple<int, int>, Border> borderByLocation;
        public List<Item> Stash { get; set; }
        //public int FilterResults { get; private set; }

        public List<IFilter> Filter
        {
            get { return (List<IFilter>) GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public void ForceUpdate()
        {
            if (initialized == false && Stash == null)
                refresh();

            FilterResults = !Filter.Any() ? -1 : 0;

            foreach (var item in Stash)
            {
                var index = Tuple.Create<int, int>(item.X, item.Y);

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

        public void RefreshTab(string accountName)
        {
            ApplicationState.Stash[ApplicationState.CurrentLeague].RefreshTab(ApplicationState.Model,
                ApplicationState.CurrentLeague, TabNumber, accountName);
            refresh();
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(IEnumerable<IFilter>), typeof(StashControl), null);

        public StashControl()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(StashControl_Loaded);
            ApplicationState.LeagueChanged += ApplicationState_LeagueChanged;
            stashByLocation = new Dictionary<Tuple<int, int>, Item>();
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
            TabType tabType = GetTabType();

            updateStashByLocation(tabType);
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

        private void updateStashByLocation(TabType tabType)
        {

            stashByLocation.Clear();

            int x = 0;
            int y = 0;

            foreach (var item in this.Stash)
            {
                if (tabType == TabType.DivinationCard)
                {                    
                    var key = Tuple.Create<int, int>(x, y);
                    
                    if (stashByLocation.ContainsKey(key))
                        continue;

                    stashByLocation.Add(key, item);

                    if (x < 12)
                    {
                        x++;
                    }
                    else
                    {
                        x = 0;
                        y++;
                    }
                }
                else
                {
                    var key = Tuple.Create<int, int>(item.X, item.Y);

                    if (stashByLocation.ContainsKey(key))
                        continue;

                    stashByLocation.Add(key, item);
                }
                
            }
        }

        private const int NORMAL_SPACING = 12;
        private const int QUAD_SPACING = 24;

        private void render(TabType tabType)
        {
            int columns = NORMAL_SPACING, rows = NORMAL_SPACING;

            if (tabType == TabType.Quad)
            {
                columns = QUAD_SPACING;
                rows = QUAD_SPACING;
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
                    childGrid.Margin = new Thickness(1);

                    Tuple<int, int> currentKey = new Tuple<int, int>(i, j);

                    if (!stashByLocation.ContainsKey(currentKey))
                        continue;

                    Item gearAtLocation = stashByLocation[currentKey];

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
            return new ItemDisplay() { DataContext = new ItemDisplayViewModel(item) };
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
    }
}
