using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using POEApi.Model;
using Procurement.Controls;

namespace Procurement.ViewModel
{
    public class LinkPath
    {
        public Image image;
        public int row { get; set; }
        public int col { get; set; }
    }


    public class ItemDisplayViewModel
    {
        public Item Item { get; set; }
        private static Dictionary<string, BitmapImage> imageCache = new Dictionary<string, BitmapImage>();
        public ItemDisplayViewModel(Item item)
        {
            this.Item = item;
        }

        public Image getImage()
        {
            Image img = new Image();

            if (!imageCache.ContainsKey(Item.IconURL))
            {
                using (var stream = ApplicationState.Model.GetImage(Item))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    imageCache.Add(Item.IconURL, bitmap);
                }
            }

            img.Source = imageCache[Item.IconURL];
            var itemhover = new ItemHover() { DataContext = ItemHoverViewModelFactory.Create(Item) };

            Popup popup = new Popup();
            popup.AllowsTransparency = true;
            popup.PopupAnimation = PopupAnimation.Fade;
            popup.StaysOpen = true;
            popup.Child = itemhover;
            popup.PlacementTarget = img;
            img.Stretch = Stretch.None;
            img.MouseEnter += (o, e) => { popup.IsOpen = true; };
            img.MouseLeave += (o, e) => { popup.IsOpen = false; };
            return img;
        }

        public UIElement getSocket()
        {
            Gear gear = Item as Gear;
            if (gear == null)
                return null;

            if (gear.Sockets.Count == 0)
                return null;

            Grid masterpiece = new Grid();

            masterpiece.HorizontalAlignment = HorizontalAlignment.Center;
            masterpiece.VerticalAlignment = VerticalAlignment.Center;

            List<Tuple<int, int>> myLittleDesign = getSocketTree(gear.W, gear.H).ToList();
            int rows = myLittleDesign.Max(t => t.Item2) + 1;
            int columns = myLittleDesign.Max(t => t.Item1) + 1;

            for (int i = 0; i < rows; i++)
                masterpiece.RowDefinitions.Add(new RowDefinition() { Height = i % 2 == 0 ? GridLength.Auto : new GridLength(0) });

            for (int i = 0; i < columns; i++)
                masterpiece.ColumnDefinitions.Add(new ColumnDefinition() { Width = i % 2 == 0 ? GridLength.Auto : new GridLength(0)  });

            //masterpiece.ShowGridLines = true;


            IEnumerator<Tuple<int, int>> sockets = getEveryOther(myLittleDesign, 0).GetEnumerator();
            IEnumerator<Tuple<int, int>> links = getEveryOther(myLittleDesign, 1).GetEnumerator();
            int currentGroup = -1;
            for (int i = 0; i < gear.Sockets.Count; i++)
			{
                Socket socket = gear.Sockets[i];
			
                sockets.MoveNext();
                var currentSocketPosition = sockets.Current;

                if (i > 0)
                {
                    links.MoveNext();
                    var link = links.Current;

                    if (currentGroup == socket.Group)
                    {
                        Image img = getLink(currentSocketPosition, link);
                        img.SetValue(Grid.RowProperty, link.Item2);
                        img.SetValue(Grid.ColumnProperty, link.Item1);
                        masterpiece.Children.Add(img);
                    }
                }

                if (!isSocketed(currentSocketPosition, socket, i, gear))
                {
                    Image img = getSocket(currentSocketPosition, socket, string.Empty);
                    img.SetValue(Grid.RowProperty, currentSocketPosition.Item2);
                    img.SetValue(Grid.ColumnProperty, currentSocketPosition.Item1);
                    masterpiece.Children.Add(img);
                }
                else
                {
                    string suffix = "-socketed";
                    Gem g = gear.SocketedItems.Find(si => si.Socket == i && (socket.Attribute == si.Color || socket.Attribute == "G" || si.Color == "G"));
                    if (g.Color == "G")
                        suffix += "-white";
                    Image img = getSocket(currentSocketPosition, socket, suffix);
                    img.SetValue(Grid.RowProperty, currentSocketPosition.Item2);
                    img.SetValue(Grid.ColumnProperty, currentSocketPosition.Item1);
                    getMouseOverImage(img, getSocketItemAt(currentSocketPosition, socket, i, gear));
                    masterpiece.Children.Add(img);
                }

                currentGroup = socket.Group;
            }
            
            return masterpiece;
        }

        private List<Tuple<int, int>> getEveryOther(List<Tuple<int, int>> myLittleDesign, int start)
        {
            List<Tuple<int, int>> ret = new List<Tuple<int, int>>();
            for (int i = start; i < myLittleDesign.Count; i = i + 2)
                ret.Add(myLittleDesign[i]);

            return ret;
        }

        private Image getLink(Tuple<int, int> currentSocket, Tuple<int, int> currentLink)
        {
            Image img = new Image();
            string linkFormat = "pack://application:,,,/Images/Sockets/{0}.png";
            string link = null;
            if (currentSocket.Item1 != currentLink.Item1)
            {
                link = "link-horizontal";
                img.SetValue(Grid.ColumnSpanProperty, 3);
                img.Margin = new Thickness(-19, 0, 0, 0);
                img.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                link = "link-vertical";
                img.SetValue(Grid.RowSpanProperty, 3);
                img.Margin = new Thickness(0, -20, 0, 0);
                img.VerticalAlignment = VerticalAlignment.Top;

            }

            img.SetValue(Panel.ZIndexProperty, 1);

            var bitmap = new BitmapImage(new Uri(string.Format(linkFormat, link), UriKind.Absolute));

            img.Stretch = System.Windows.Media.Stretch.None;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.Freeze();
            img.Source = bitmap;

            return img;
        }

        private bool isSocketed(Tuple<int, int> nextAvail, Socket socket, int socketIndex, Gear item)
        {
            if (item.SocketedItems == null || item.SocketedItems.Count == 0)
                return false;

            return item.SocketedItems.Exists(i => i.Socket == socketIndex && (socket.Attribute == i.Color || socket.Attribute == "G" || i.Color == "G"));
        }

        private Gem getSocketItemAt(Tuple<int, int> nextAvail, Socket socket, int socketIndex, Gear item)
        {
            return item.SocketedItems.First(i => i.Socket == socketIndex && (socket.Attribute == i.Color || socket.Attribute == "G" || i.Color == "G"));
        }

        private Image getSocket(Tuple<int, int> current, Socket socket, string suffix)
        {
            string socketFormat = "pack://application:,,,/Images/Sockets/{0}.png";
            string color = null;
            switch (socket.Attribute)
            {
                case "D":
                    color = "green" + suffix;
                    break;
                case "I":
                    color = "blue" + suffix;
                    break;
                case "S":
                    color = "red" + suffix;
                    break;
                default:
                    color = "white" + suffix;
                    break;
            }

            Image img = new Image();

            var bitmap = new BitmapImage(new Uri(string.Format(socketFormat, color), UriKind.Absolute));
            img.Stretch = System.Windows.Media.Stretch.None;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.Freeze();
            img.Source = bitmap;

            return img;
        }



        private Image getMouseOverImage(Image img, Item item)
        {
            var itemhover = new ItemHover() { DataContext = ItemHoverViewModelFactory.Create(item)};

            Popup popup = new Popup();
            popup.PopupAnimation = PopupAnimation.Fade;
            popup.StaysOpen = true;
            popup.Child = itemhover;
            popup.PlacementTarget = img;
            img.MouseEnter += (o, e) => { popup.IsOpen = true; };
            img.MouseLeave += (o, e) => { popup.IsOpen = false; };

            return img;
        }

        public IEnumerable<Tuple<int, int>> getSocketTree(int W, int H)
        {
            int maxWidth = W;
            if (W > 1)
                maxWidth = 3;

            int maxHeight = H;
            if (H == 2)
                maxHeight = 3;

            if (H == 3)
                maxHeight = 5;

            if (H == 4)
                maxHeight = 6;

            List<Tuple<int, int>> possible = new List<Tuple<int, int>>();

            possible.Add(new Tuple<int, int>(0, 0)); //Top Left
            possible.Add(new Tuple<int, int>(1, 0)); //Link
            possible.Add(new Tuple<int, int>(2, 0)); //Top Right
            possible.Add(new Tuple<int, int>(2, 1)); //Link
            if (W == 1)
                possible.Add(new Tuple<int, int>(0, 1)); //Not valid for everything but single column rapiers / wands meh.
 
            possible.Add(new Tuple<int, int>(2, 2)); //Middle Right
            possible.Add(new Tuple<int, int>(1, 2)); //Link
            possible.Add(new Tuple<int, int>(0, 2)); //Middle Left
            possible.Add(new Tuple<int, int>(0, 3)); //Link
            possible.Add(new Tuple<int, int>(0, 4)); //Bottom Left
            possible.Add(new Tuple<int, int>(1, 4)); //Link
            possible.Add(new Tuple<int, int>(2, 4)); //Bottom Right

            foreach (var turple in possible)
                if (turple.Item1 < maxWidth && turple.Item2 < maxHeight)
                    yield return turple;
        }
    }
}
