using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using POEApi.Infrastructure;
using POEApi.Model;
using Procurement.Utility;
using Procurement.ViewModel;

namespace Procurement.Controls
{
    public partial class ItemDisplay : UserControl
    {
        private static readonly List<Popup> annoyed = new List<Popup>();
        private bool contexted;
        private Image itemImage;

        private TextBlock textblock;
        private ItemDisplayViewModel _viewModel;

        public static readonly DependencyProperty HeightScaleProperty = DependencyProperty.Register(
            "HeightScale", typeof(double?), typeof(ItemDisplay), new PropertyMetadata(null));

        public double? HeightScale
        {
            get { return (double?) GetValue(HeightScaleProperty); }
            set { SetValue(HeightScaleProperty, value); }
        }

        public static readonly DependencyProperty WidthScaleProperty = DependencyProperty.Register(
            "WidthScale", typeof(double?), typeof(ItemDisplay), new PropertyMetadata(null));

        public double? WidthScale
        {
            get { return (double?) GetValue(WidthScaleProperty); }
            set { SetValue(WidthScaleProperty, value); }
        }

        public ItemDisplay()
        {
            InitializeComponent();
        }

        public ItemDisplayViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;

                if (DataContext == null)
                {
                    DataContext = _viewModel;
                }
            }
        }

        public bool IsQuadStash { get; set; }

        private void ItemDisplay_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!contexted)
            {
                itemImage.ContextMenu = getContextMenu();
                contexted = true;
            }

            //Reset the export to pob button incase user needs to export again
            (buyoutControl.DataContext as SetBuyoutViewModel).IsDataInClipboard = false;
        }

        public static void ClosePopups()
        {
            closeOthersButNot(new Popup());
        }

        private void ItemDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ItemDisplayViewModel;
            if (vm != null)
            {
                var i = vm.getImage();
                itemImage = i;

                if (vm.Item != null && vm.Item.IsGear && itemImage != null)
                    RenderOptions.SetBitmapScalingMode(itemImage, BitmapScalingMode.NearestNeighbor);

                if (i != null)
                {
                    //See: https://github.com/Stickymaddness/Procurement/issues/966
                    if (HeightScale.HasValue && WidthScale.HasValue)
                    {
                        itemImage.Height = HeightScale.Value;
                        itemImage.Width = WidthScale.Value;
                    }

                    MainGrid.Children.Add(i);

                    if (vm.HasSocket)
                    {
                        BindSocketPopup(vm);
                    }

                    Height = i.Height;
                    Width = i.Width;
                }
            }

            Loaded -= ItemDisplay_Loaded;

            resyncText();
        }

        private void resyncText()
        {
            var vm = DataContext as ItemDisplayViewModel;
            if (vm != null)
            {
                var item = vm.Item;
                if (item == null)
                {
                    return;
                }

                var pricingInfo = string.Empty;

                if (!string.IsNullOrWhiteSpace(item.Id) && Settings.Buyouts.ContainsKey(item.Id))
                {
                    pricingInfo = Settings.Buyouts[item.Id].Buyout;

                    if (pricingInfo == string.Empty)
                    {
                        pricingInfo = Settings.Buyouts[item.Id].Price;
                    }
                }

                if (textblock != null)
                {
                    MainGrid.Children.Remove(textblock);
                }

                textblock = new TextBlock();
                textblock.Text = pricingInfo;

                if (item is Currency)
                {
                    textblock.VerticalAlignment = VerticalAlignment.Bottom;
                }


                textblock.IsHitTestVisible = false;
                textblock.Margin = new Thickness(1, 1, 0, 0);
                MainGrid.Children.Add(textblock);
            }
        }

        private void BindSocketPopup(ItemDisplayViewModel vm)
        {
            UIElement socket = null;
            var isKeyPressed = false;

            Action DisplaySocket = () =>
            {
                if (socket == null)
                {
                    socket = vm.GetSocket(IsQuadStash);
                }

                if (!MainGrid.Children.Contains(socket))
                {
                    MainGrid.Children.Add(socket);
                }
            };

            MainGrid.MouseEnter += (o, ev) => { DisplaySocket(); };

            MainGrid.MouseLeave += (o, ev) =>
            {
                if (!isKeyPressed)
                {
                    MainGrid.Children.Remove(socket);
                }
            };

            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow.KeyDown += (o, ev) =>
            {
                if (ev.SystemKey == Key.LeftAlt ||
                    ev.SystemKey == Key.RightAlt)
                {
                    isKeyPressed = true;

                    DisplaySocket();
                }
            };

            mainWindow.KeyUp += (o, ev) =>
            {
                if ((Keyboard.GetKeyStates(Key.LeftAlt) == KeyStates.None || Keyboard.GetKeyStates(Key.LeftAlt) == KeyStates.Toggled) &&
                    (Keyboard.GetKeyStates(Key.RightAlt) == KeyStates.None || Keyboard.GetKeyStates(Key.RightAlt) == KeyStates.Toggled))
                {
                    isKeyPressed = false;

                    if (!MainGrid.IsMouseOver)
                    {
                        MainGrid.Children.Remove(socket);
                    }
                }
            };
        }

        private SetBuyoutView buyoutControl;

        private ContextMenu getContextMenu()
        {
            var vm = DataContext as ItemDisplayViewModel;
            var item = vm.Item;

            var menu = new ContextMenu();
            menu.Background = Brushes.Black;

            menu.Resources = Resources["ExpressionDarkGrid"] as ResourceDictionary;

            var setBuyout = new MenuItem();
            setBuyout.StaysOpenOnClick = true;

            buyoutControl = new SetBuyoutView(item);

            if (Settings.Buyouts.ContainsKey(item.Id))
            {
                buyoutControl.SetBuyoutInfo(Settings.Buyouts[item.Id]);
            }

            setBuyout.Header = buyoutControl;
            buyoutControl.Update += buyoutControl_Update;
            buyoutControl.SaveImageClicked += buyoutControl_SaveImageClicked;
            menu.Items.Add(setBuyout);

            return menu;
        }

        private void buyoutControl_Update(ItemTradeInfo info)
        {
            updateBuyout(info);
            Settings.SaveBuyouts();

            resyncText();
            itemImage.ContextMenu.IsOpen = false;
        }

        private void updateBuyout(ItemTradeInfo info)
        {
            var vm = DataContext as ItemDisplayViewModel;
            var item = vm.Item;

            if (info.IsEmpty)
            {
                Settings.Buyouts.Remove(item.Id);
                return;
            }

            Settings.Buyouts[item.Id] = info;
        }

        private void buyoutControl_SaveImageClicked()
        {
            ItemHoverRenderer.SaveToDisk((DataContext as ItemDisplayViewModel).Item, Dispatcher);
        }

        private void buyoutView_SaveClicked(string amount, string orbType)
        {
            var abbreviation = CurrencyAbbreviationMap.Instance.FromCurrency(orbType);

            if (string.IsNullOrEmpty(abbreviation))
                abbreviation = orbType;

            var vm = DataContext as ItemDisplayViewModel;
            var item = vm.Item;

            Settings.Buyouts[item.Id].Buyout = string.Format("{0} {1}", amount, abbreviation);
            Settings.SaveBuyouts();

            resyncText();
        }

        public static void closeOthersButNot(Popup current)
        {
            var others = annoyed.Where(p => p.IsOpen && !ReferenceEquals(current, p)).ToList();
            Task.Factory.StartNew(() => others.ToList().ForEach(p => p.Dispatcher.Invoke(() => { p.IsOpen = false; })));
        }
    }
}