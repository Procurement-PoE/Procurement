using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using POEApi.Model;
using Procurement.View.ViewModel;

namespace Procurement.ViewModel
{
    public class MainPageViewModel : ObservableBase
    {
        private List<Item> stashItems;
        private bool isBusy;

        public MainPageViewModel()
        {
            IsBusy = false;
            var stash = ApplicationState.Stash[ApplicationState.CurrentLeague];
            stashItems = stash.GetItemsByTab(0);
        }

        public void getTabs(object o)
        {
            Button selector = o as Button;
            ScrollViewer scrollViewer = selector.TemplatedParent as ScrollViewer;
            TabControl tabControl = scrollViewer.TemplatedParent as TabControl;

            selector.ContextMenu = getContextMenu(selector, tabControl);
            selector.ContextMenu.IsOpen = true;
        }

        private ContextMenu getContextMenu(Button target, TabControl tabControl)
        {
            ContextMenu menu = new ContextMenu();
            menu.PlacementTarget = target;

            foreach (TabItem item in tabControl.Items)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Tag = item;
                menuItem.Header = ((item.Header as TextBlock).Inlines.FirstInline as Run).Text;
                menuItem.Click += (o, e) => { CloseAndSelect(menu, menuItem); };
                menu.Items.Add(menuItem);
            }

            return menu;
        }
        
        public ICommand GetTabs => new RelayCommand(getTabs);

        public void CloseAndSelect(ContextMenu menu, MenuItem menuItem)
        {
            menu.IsOpen = false;
            TabItem newCurrent = menuItem.Tag as TabItem;
            newCurrent.BringIntoView();
            newCurrent.IsSelected = true;
        }
        public List<Item> StashItems
        {
            get { return stashItems; }
            set 
            { 
                stashItems = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set 
            { 
                isBusy = value;
                OnPropertyChanged();
            }
        }
    }
}