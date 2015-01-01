using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel
{
    public class AdvancedSearchCategory
    {
        public AdvancedSearchCategory(string key, string value)
        {
            this.Key = key;
            this.Value = value;
            this.IsChecked = false;
            this.IsVisible = true;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsChecked { get; set; }
        public bool IsVisible { get; set; }
        public System.Windows.Visibility IsVisibleString
        {
            get
            {
                return IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; 
            }
        }
    }
}