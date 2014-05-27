using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using POEApi.Model;
using Procurement.ViewModel;
using POEApi.Infrastructure;
using Procurement.Utility;

namespace Procurement.Controls
{
    public partial class ItemDisplay : UserControl
    {
        private static List<Popup> annoyed = new List<Popup>();
        private static ResourceDictionary expressionDarkGrid;
        private Image itemImage;
        private bool contexted = false;

        private TextBlock textblock;

        public ItemDisplay()
        {
            MainGrid = new Grid();
            AddChild(MainGrid);

            expressionDarkGrid = expressionDarkGrid ?? Application.LoadComponent(new Uri("/Procurement;component/Controls/ExpressionDarkGrid.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;

            this.Loaded += new RoutedEventHandler(ItemDisplay_Loaded);
            this.MouseRightButtonUp += ItemDisplay_MouseRightButtonUp;
        }

        void ItemDisplay_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!contexted)
            {
                itemImage.ContextMenu = getContextMenu();
                contexted = true;
            }
        }

        public static void ClosePopups()
        {
            closeOthersButNot(new Popup());
        }

        void ItemDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            ItemDisplayViewModel vm = this.DataContext as ItemDisplayViewModel;
            Image i = vm.getImage();
            itemImage = i;

            this.MainGrid.Children.Add(i);

            if (vm.HasSocket)
                BindSocketPopup(vm, i);

            this.Height = i.Height;
            this.Width = i.Width;
            this.Loaded -= new RoutedEventHandler(ItemDisplay_Loaded);

            resyncText();
        }

        private void resyncText()
        {
            ItemDisplayViewModel vm = this.DataContext as ItemDisplayViewModel;
            Item item = vm.Item;

            if ((item is Currency))
                return;

            MenuItem setBuyout = new MenuItem();
            string buyoutValue = string.Empty;

            if (Settings.Buyouts.ContainsKey(item.UniqueIDHash))
                buyoutValue = Settings.Buyouts[item.UniqueIDHash];

            if (textblock != null)
                this.MainGrid.Children.Remove(textblock);

            textblock = new TextBlock();
            textblock.Text = buyoutValue;
            textblock.IsHitTestVisible = false;
            textblock.Margin = new Thickness(1, 1, 0, 0);
            this.MainGrid.Children.Add(textblock);
        }

        private void doSocketAlwaysOver(UIElement socket)
        {
            this.MainGrid.Children.Add(socket);
        }

        private void BindSocketPopup(ItemDisplayViewModel vm, Image i)
        {
            UIElement socket = null;

            MainGrid.MouseEnter += (o, ev) =>
            {
                if (socket == null)
                    socket = vm.GetSocket();

                MainGrid.Children.Add(socket);
            };
            
            MainGrid.MouseLeave += (o, ev) => MainGrid.Children.Remove(socket);
        }

        private ContextMenu getContextMenu()
        {
            ItemDisplayViewModel vm = this.DataContext as ItemDisplayViewModel;
            Item item = vm.Item;

            ContextMenu menu = new ContextMenu();
            menu.Background = Brushes.Black;         
            
            menu.Resources = expressionDarkGrid;

            if (!(item is Currency))
            {
                MenuItem setBuyout = new MenuItem();

                var buyoutControl = new SetBuyoutView();

                if (Settings.Buyouts.ContainsKey(item.UniqueIDHash))
                {
                    var price = Settings.Buyouts[item.UniqueIDHash].Split(' ');
                    buyoutControl.SetValue(price[0], CurrencyAbbreviationMap.Instance.FromAbbreviation(price[1]));
                }

                setBuyout.Header = buyoutControl;
                buyoutControl.SaveClicked += new SetBuyoutView.BuyoutHandler(buyoutView_SaveClicked);
                buyoutControl.RemoveClicked += new SetBuyoutView.BuyoutHandler(buyoutControl_RemoveClicked);
                buyoutControl.SaveImageClicked += buyoutControl_SaveImageClicked;
                menu.Items.Add(setBuyout);
            }

            return menu;
        }

        void buyoutControl_SaveImageClicked()
        {
            ItemHoverRenderer.SaveToDisk((this.DataContext as ItemDisplayViewModel).Item, Dispatcher);
        }

        void buyoutControl_RemoveClicked(string amount, string orbType)
        {
            ItemDisplayViewModel vm = this.DataContext as ItemDisplayViewModel;
            Item item = vm.Item;

            Settings.Buyouts.Remove(item.UniqueIDHash);
            Settings.Save();

            resyncText();
        }

        void buyoutView_SaveClicked(string amount, string orbType)
        {
            var abbreviation = CurrencyAbbreviationMap.Instance.FromCurrency(orbType);
                       
            ItemDisplayViewModel vm = this.DataContext as ItemDisplayViewModel;
            Item item = vm.Item;

            Settings.Buyouts[item.UniqueIDHash] = string.Format("{0} {1}", amount, abbreviation);

            Settings.SaveBuyouts();

            resyncText();
        }

        public static void closeOthersButNot(Popup current)
        {
            List<Popup> others = annoyed.Where(p => p.IsOpen && !object.ReferenceEquals(current, p)).ToList();
            Task.Factory.StartNew(() => others.ToList().ForEach(p => p.Dispatcher.Invoke((Action)(() => { p.IsOpen = false; }))));
        }
    }
}
