using System.ComponentModel;

namespace Procurement.ViewModel
{
    public class TabInfo : ObservableBase
    {
        public string Name { get; set; }
        public decimal AvailableSpace { get; set; }
        public string Url { get; set; }
        public int ID { get; set; }
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;

                OnPropertyChanged();
            }
        }

        public void FixName()
        {
            int id = -1;
            if (int.TryParse(Name, out id))
                Name = "Tab " + Name;
        }
    }
}
