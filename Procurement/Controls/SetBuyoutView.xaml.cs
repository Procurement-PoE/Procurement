using System.Linq;
using System.Collections;
using System.Windows.Controls;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Procurement.Controls
{
    public partial class SetBuyoutView : UserControl
    {
        public SetBuyoutView()
        {
            InitializeComponent();
            BuyoutValue.Text = "1";
        }

        public event BuyoutHandler SaveClicked;
        public event BuyoutHandler RemoveClicked;
        public event SaveImageHandler SaveImageClicked;
        public delegate void BuyoutHandler(string amount, string orbType);
        public delegate void SaveImageHandler();

        public void SaveBuyout_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (BuyoutValue.Text == string.Empty)
                BuyoutValue.Text = "0";
            
            SaveClicked(double.Parse(BuyoutValue.Text, CultureInfo.InvariantCulture).ToString(), ((ComboBoxItem)OrbType.SelectedItem).Content.ToString());
        }

        public void SaveImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveImageClicked();
        }

        private void Increase_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            updateValue(1);
        }

        private void Decrease_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            updateValue(-1);
        }

        public void SetValue(string amount, string orbType)
        {
            BuyoutValue.Text = amount;
            OrbType.SelectedItem = OrbType.Items.Cast<ComboBoxItem>().First(i => i.Content.ToString() == orbType);
        }

        private void updateValue(int difference)
        {
            var buyout = double.Parse(BuyoutValue.Text, CultureInfo.InvariantCulture);
            buyout += difference;
            BuyoutValue.Text = buyout.ToString(); 
        }

        private void BuyoutValue_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = validateInput(e.Text);
        }

        private static bool validateInput(string text)
        {
            return new Regex("[^0-9.]+").IsMatch(text);
        }

        private void RemoveBuyout_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RemoveClicked(null, null);
        }
    }
}
