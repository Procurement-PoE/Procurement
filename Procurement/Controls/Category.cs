using System.Collections.ObjectModel;
using Procurement.ViewModel;
using Procurement.ViewModel.Filters;

namespace Procurement.Controls
{
    internal class Category : ObservableBase
    {
        public Category()
        {
            filters = new ObservableCollection<IFilter>();
            filters.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(filters_CollectionChanged);
        }

        void filters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Filters));
        }
        public string Name { get; set; }
        private ObservableCollection<IFilter> filters;
        public ObservableCollection<IFilter> Filters
        {
            get { return filters; }
            set { filters = value; }
        }
    }
}
