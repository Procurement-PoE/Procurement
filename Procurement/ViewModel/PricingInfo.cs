using POEApi.Infrastructure;
using System.Globalization;
using System.Windows.Input;
using Procurement.View.ViewModel;

namespace Procurement.ViewModel
{
    public class PricingInfo : ObservableBase
    {
        private string value;
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }
        private string orb;
        public string Orb
        {
            get { return orb; }
            set
            {
                this.orb = value;
                OnPropertyChanged();
            }
        }
        private bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                setDefaults();
                OnPropertyChanged();
            }
        }

        private void setDefaults()
        {
            if (string.IsNullOrEmpty(Value))
                Value = "1";

            if (string.IsNullOrEmpty(Orb))
                Orb = "Chaos Orb";
        }

        public ICommand IncreaseValue => new RelayCommand(x => updateValue(1));
        public ICommand DecreaseValue => new RelayCommand(x => updateValue(-1));

        public PricingInfo()
        {
            value = string.Empty;
            orb = string.Empty;
            enabled = false;
        }

        private void updateValue(int difference)
        {
            var buyout = getCurrentBuyout();
            buyout += difference;

            if (buyout < 1)
                buyout = 1;

            Value = buyout.ToString();
        }

        private double getCurrentBuyout()
        {
            double buyout;

            if (double.TryParse(this.value, NumberStyles.Any, CultureInfo.InvariantCulture, out buyout))
                return buyout;

            return 1;
        }

        public void Update(string info)
        {
            if (string.IsNullOrEmpty(info))
                return;

            var valueOrbPair = info.Split(' ');
            value = valueOrbPair[0];
            orb = CurrencyAbbreviationMap.Instance.FromAbbreviation(valueOrbPair[1]);

            if (string.IsNullOrEmpty(orb))
                orb = valueOrbPair[1];

            enabled = !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(orb);
        }

        public string GetSaveText()
        {
            if (!enabled)
                return string.Empty;

            string abbreviation = CurrencyAbbreviationMap.Instance.FromCurrency(orb);

            if (string.IsNullOrEmpty(abbreviation))
                return $"{value} {orb}";

            return $"{value} {abbreviation}";
        }
    }
}
