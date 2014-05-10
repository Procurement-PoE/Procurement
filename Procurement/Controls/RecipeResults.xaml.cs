using System;
using System.Collections.Generic;
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
using Procurement.ViewModel;
using POEApi.Model;

namespace Procurement.Controls
{
    /// <summary>
    /// Interaction logic for RecipeResults.xaml
    /// </summary>
    public partial class RecipeResults : UserControl
    {
        public RecipeResults()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RecipeResultViewModel vm = this.DataContext as RecipeResultViewModel;
            RadioButton button = sender as RadioButton;
            Item item = button.Tag as Item;
            vm.RadioButtonSelected(item);
        }
    }
}
