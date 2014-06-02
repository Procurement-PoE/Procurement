using System.Collections.Generic;
using System;

namespace POEApi.Infrastructure
{
    //WTB Bi-directional Dictionary in .NET

    public sealed class CurrencyAbbreviationMap
    {
        private static volatile CurrencyAbbreviationMap instance;
        private static object syncRoot = new Object();

        private static Dictionary<string, string> currencyToAbbreviation = new Dictionary<string, string>();
        private static Dictionary<string, string> abbreviationToCurrency = new Dictionary<string, string>();

        private CurrencyAbbreviationMap()
        {
            addItem("Chromatic Orb", "chrom");
            addItem("Orb of Alteration", "alt");
            addItem("Jeweller's Orb", "jewel");
            addItem("Orb of Chance", "chance");
            addItem("Cartographer's Chisel", "chisel");
            addItem("Orb of Fusing", "fuse");
            addItem("Orb of Alchemy", "alch");
            addItem("Orb of Scouring", "scour");
            addItem("Blessed Orb", "blessed");
            addItem("Chaos Orb", "chaos");
            addItem("Orb of Regret", "regret");
            addItem("Regal Orb", "regal");
            addItem("Gemcutter's Prism", "gcp");
            addItem("Divine Orb", "divine");
            addItem("Exalted Orb", "exa");
            addItem("Vaal Orb", "vaal");
        }

        private static void addItem(string currency, string abbreviation)
        {
            currencyToAbbreviation.Add(currency, abbreviation);
            abbreviationToCurrency.Add(abbreviation, currency);
        }

        public string FromAbbreviation(string abbreviation)
        {
            if (!abbreviationToCurrency.ContainsKey(abbreviation))
                return string.Empty;
            
            return abbreviationToCurrency[abbreviation];
        }

        public string FromCurrency(string currency)
        {
            if (!currencyToAbbreviation.ContainsKey(currency))
                return string.Empty;

            return currencyToAbbreviation[currency];
        }

        public static CurrencyAbbreviationMap Instance
        {
            get
            {
                if (instance == null)
                    lock (syncRoot)
                        if (instance == null)
                            instance = new CurrencyAbbreviationMap();

                return instance;
            }
        }
    }
}
