using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using POEApi.Model;
using Procurement.Interfaces;
using Procurement.ViewModel;
using Procurement.ViewModel.Filters;

namespace Procurement.Controls
{
    /// <summary>
    /// Interaction logic for CurencyStash.xaml
    /// </summary>
    public partial class CurencyStash : UserControl, IStashControl
    {
        public CurrencyStashViewModel viewModel;
        private List<IFilter> list;

        public CurencyStash(int tabNumber)
        {


            TabNumber = tabNumber;
            var stash = ApplicationState.Stash[ApplicationState.CurrentLeague].GetItemsByTab(tabNumber);
            //TabType tabType = GetTabType();
            
            viewModel = new CurrencyStashViewModel(stash);

            DataContext = viewModel;

            InitializeComponent();
        }

        public CurencyStash(int tabNumber, List<IFilter> list) : this(tabNumber)
        {
            this.list = list;
        }

        public void RefreshTab(string accountName)
        {
            throw new NotImplementedException();
        }

        public int TabNumber { get; set; }
        public int FilterResults { get; set; }
        public List<IFilter> Filter { get; set; }

        public void ForceUpdate()
        {
            //throw new NotImplementedException();
        }
    }

    public class CurrencyStashViewModel : INotifyPropertyChanged
    {
        private readonly List<Item> _stash;

        public CurrencyStashViewModel(List<Item> stash)
        {
            _stash = stash;
        }

        public ItemDisplayViewModel Exalted
        {
            get { return GetCurrencyItem(OrbType.Exalted); }
        }

        public ItemDisplayViewModel Chaos
        {
            get { return GetCurrencyItem(OrbType.Chaos); }
        }

        public ItemDisplayViewModel WisdomScraps
        {
            get { return GetCurrencyItem(OrbType.ScrollofWisdom); }
        }

        public ItemDisplayViewModel WisdomScrolls
        {
            get { return GetCurrencyItem(OrbType.WisdomScroll); }
        }

        public ItemDisplayViewModel TownPortalScrolls
        {
            get { return GetCurrencyItem(OrbType.PortalScroll); }
        }

        public ItemDisplayViewModel BlacksmithsWhetstone
        {
            get { return GetCurrencyItem(OrbType.BlacksmithsWhetstone); }
        }

        public ItemDisplayViewModel ArmourersScrap
        {
            get { return GetCurrencyItem(OrbType.ArmourersScrap); }
        }

        private ItemDisplayViewModel GetCurrencyItem(OrbType orbType)
        {
            foreach (var item in _stash)
            {
                if (item.ItemType == ItemType.Currency)
                {
                    var curency = item as Currency;

                    if (curency != null)
                    {
                        if (curency.Type == orbType)
                        {
                            return new ItemDisplayViewModel(item);
                        }
                    }
                }
            }

            return new ItemDisplayViewModel(null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
