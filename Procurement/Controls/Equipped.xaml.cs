using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using POEApi.Model;
using Procurement.ViewModel;

namespace Procurement.Controls
{
    public partial class Equipped : UserControl
    {
        private Dictionary<string, Tuple<int, int>> absolutely;
        private EquipedItems equipped;
        private bool showAlts = false;

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
            DependencyProperty.Register("Character", typeof(string), typeof(Equipped), new PropertyMetadata(OnCharacterChanged));
        
        public Equipped()
        {
            InitializeComponent();
            absolutely = getAbolutePositions();
            this.PreviewKeyDown += new KeyEventHandler(Equipped_PreviewKeyDown);
            this.Loaded += new RoutedEventHandler(Equipped_Loaded);
            
        }

        public static void OnCharacterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Equipped instance = d as Equipped;
            if (instance.equipped == null)
                return;

            instance.render();

        }
        void Equipped_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.X)
                return;
                
            showAlts = !showAlts;
            render();
        }

        private Dictionary<string, Tuple<int, int>> getAbolutePositions()
        {
            Dictionary<string, Tuple<int, int>> ret = new Dictionary<string, Tuple<int, int>>();

            ret.Add("Amulet", new Tuple<int, int>(192, 367));
            ret.Add("Belt", new Tuple<int, int>(357, 250));
            ret.Add("Helm", new Tuple<int, int>(97, 250));
            ret.Add("RingLeft", new Tuple<int, int>(251, 181));
            ret.Add("RingRight", new Tuple<int, int>(251, 367));
            ret.Add("Flask0", new Tuple<int, int>(416, 184));
            ret.Add("Flask1", new Tuple<int, int>(416, 229));
            ret.Add("Flask2", new Tuple<int, int>(416, 277));
            ret.Add("Flask3", new Tuple<int, int>(416, 324));
            ret.Add("Flask4", new Tuple<int, int>(416, 372));
            //Todo: Redo this tab to follow more modern MVVM pattern
            //Scale of mainhand/offhand "Knackered"
            ret.Add("Weapon", new Tuple<int, int>(109, 63));
            ret.Add("Offhand", new Tuple<int, int>(109, 436));
            ret.Add("AltWeapon", new Tuple<int, int>(109, 63));
            ret.Add("AltOffhand", new Tuple<int, int>(109, 436));
            ret.Add("Boots", new Tuple<int, int>(310, 367));
            ret.Add("Armour", new Tuple<int, int>(204, 250));
            ret.Add("Gloves", new Tuple<int, int>(310, 135));

            return ret;
        }
            

        void Equipped_Loaded(object sender, RoutedEventArgs e)
        {
            render();
            this.Loaded -= Equipped_Loaded;
        }

        private void render()
        {
            // TODO: Get the correct tabId to use (instead of -1).
            equipped = new EquipedItems(ApplicationState.Model.GetInventory(Character, -1, false,
                ApplicationState.AccountName, ApplicationState.CurrentRealm).Where(
                i => i.InventoryId != "MainInventory"));
            davinci.Children.Clear();
            Dictionary<string, Item> itemsAtPosition = equipped.GetItems();

            foreach (string key in itemsAtPosition.Keys)
            {
                Grid childGrid = new Grid();
                childGrid.Margin = new Thickness(1);

                Item gearAtLocation = itemsAtPosition[key];
                if (gearAtLocation == null)
                    continue;

                if (key.Contains("Weapon") || key.Contains("Offhand"))
                {
                    bool isAlt = key.StartsWith("Alt");
                    childGrid.Height = 187;
                    childGrid.Width = 93;                    
                    childGrid.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    childGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                    if (!showAlts && isAlt)
                        continue;

                    if (showAlts && !isAlt)
                        continue;
                }


                Border border = getBorder();
                childGrid.Children.Add(border);

                childGrid.Children.Add(getImage(gearAtLocation));

                Canvas.SetTop(childGrid, absolutely[key].Item1);
                Canvas.SetLeft(childGrid, absolutely[key].Item2);

                davinci.Children.Add(childGrid);
            }
            this.davinci.Focus();
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
    }
}
