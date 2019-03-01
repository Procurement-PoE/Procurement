using System.Collections.Generic;
using System;

namespace POEApi.Infrastructure
{
    //WTB Bi-directional Dictionary in .NET

    public sealed class CurrencyAbbreviationMap
    {
        private static volatile CurrencyAbbreviationMap _instance;
        private static object _syncRoot = new Object();

        private static Dictionary<string, string> _currencyToAbbreviation = new Dictionary<string, string>();
        private static Dictionary<string, string> _abbreviationToCurrency = new Dictionary<string, string>();

        private CurrencyAbbreviationMap()
        {
            AddItem("Chromatic Orb", "chrom");
            AddItem("Orb of Alteration", "alt");
            AddItem("Jeweller's Orb", "jew");
            AddItem("Orb of Chance", "chance");
            AddItem("Cartographer's Chisel", "chisel");
            AddItem("Orb of Fusing", "fuse");
            AddItem("Orb of Alchemy", "alch");
            AddItem("Orb of Scouring", "scour");
            AddItem("Blessed Orb", "blessed");
            AddItem("Chaos Orb", "chaos");
            AddItem("Orb of Regret", "regret");
            AddItem("Regal Orb", "regal");
            AddItem("Gemcutter's Prism", "gcp");
            AddItem("Divine Orb", "divine");
            AddItem("Exalted Orb", "exa");
            AddItem("Vaal Orb", "vaal");
        }

        private static void AddItem(string currency, string abbreviation)
        {
            _currencyToAbbreviation.Add(currency, abbreviation);
            _abbreviationToCurrency.Add(abbreviation, currency);
        }

        public string FromAbbreviation(string abbreviation)
        {
            if (!_abbreviationToCurrency.ContainsKey(abbreviation))
                return string.Empty;
            
            return _abbreviationToCurrency[abbreviation];
        }

        public string FromCurrency(string currency)
        {
            if (!_currencyToAbbreviation.ContainsKey(currency))
                return string.Empty;

            return _currencyToAbbreviation[currency];
        }

        public static CurrencyAbbreviationMap Instance
        {
            get
            {
                if (_instance == null)
                    lock (_syncRoot)
                        if (_instance == null)
                            _instance = new CurrencyAbbreviationMap();

                return _instance;
            }
        }
    }
}
