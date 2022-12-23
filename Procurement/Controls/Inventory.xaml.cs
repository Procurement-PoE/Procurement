using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using POEApi.Model;
using Procurement.ViewModel;

namespace Procurement.Controls
{
    public partial class Inventory : UserControl
    {
        public Inventory()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Inventory_Loaded);
        }

        public string Character
        {
            get { return (string)GetValue(CharacterProperty); }
            set
            {
                SetValue(CharacterProperty, value);
                render();
            }
        }

        public static readonly DependencyProperty CharacterProperty =
            DependencyProperty.Register("Character", typeof(string), typeof(Inventory), new PropertyMetadata(OnCharacterChanged));

        void Inventory_Loaded(object sender, RoutedEventArgs e)
        {
            if (initialized)
                return;

            refresh(ApplicationState.AccountName);
        }

        private void refresh(string accountName)
        {
            // TODO: Get the correct tabId to use (instead of -1).
            this.Invent = ApplicationState.Model.GetInventory(Character, -1, false, accountName,
                ApplicationState.CurrentRealm).Where(i => i.InventoryId == "MainInventory").ToList();
            inventByLocation = Invent.ToDictionary(item => new Tuple<int, int>(item.X, item.Y));
            render();
        }

        public static void OnCharacterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Inventory instance = d as Inventory;
            if (instance.Invent == null)
                return;

            instance.initialized = false;
            instance.refresh(ApplicationState.AccountName);
        }

        private bool initialized = false;

        private Dictionary<Tuple<int, int>, Item> inventByLocation;
        public List<Item> Invent { get; set; }

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set 
            { 
                SetValue(FilterProperty, value);
                render();
            }
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(string), typeof(Inventory), null);

        private void render()
        {
            const int columns = 12, rows = 5;

            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            grid.Children.Clear();

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

                    if (!inventByLocation.ContainsKey(currentKey))
                        continue;

                    Item gearAtLocation = inventByLocation[currentKey];

                    setBackround(childGrid, gearAtLocation);

                    Border border = getBorder();
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

        private Border getBorder()
        {
            Border b = new Border();
            b.BorderBrush = Brushes.Transparent;
            b.BorderThickness = new Thickness(1);
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
            if (string.IsNullOrEmpty(Filter))
                return false;

            return item.TypeLine.ToLower().Contains(Filter.ToLower()) || item.Name.ToLower().Contains(Filter.ToLower());
        }
    }
}
