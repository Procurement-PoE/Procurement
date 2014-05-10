using System.Collections.ObjectModel;
using System.ComponentModel;
using Procurement.ViewModel.Filters;

namespace Procurement.Controls
{
    internal class Category : INotifyPropertyChanged
    {
        public Category()
        {
            filters = new ObservableCollection<IFilter>();
            filters.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(filters_CollectionChanged);
        }

        void filters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            onPropertyChanged("Filters");
        }
        public string Name { get; set; }
        private ObservableCollection<IFilter> filters;
        public ObservableCollection<IFilter> Filters
        {
            get { return filters; }
            set { filters = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
