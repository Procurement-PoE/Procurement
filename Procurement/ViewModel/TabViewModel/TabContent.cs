using System.Windows.Controls;
using Procurement.Interfaces;

namespace Procurement.ViewModel
{
    internal class TabContent
    {
        public int Index { get; set; }
        public TabItem TabItem { get; set; }
        public IStashControl Stash { get; set; }

        public TabContent(int index, TabItem tabItem, IStashControl stash)
        {
            Index = index;
            TabItem = tabItem;
            Stash = stash;
        }
    }
}