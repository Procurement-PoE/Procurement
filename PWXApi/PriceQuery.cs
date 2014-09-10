using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Infrastructure;
using POEApi.Model;
using System.Net;
using System.IO;

namespace PWXApi
{

    //Accepts GET arguments:

    //league = [string] must match poe.xyz.is league parameter (default: standard)
    //s = [string] name search phrase
    //v = [#] version of macro being used, if this doesn't have v=currentversion [3] it adds a little note saying to upgrade the version
    //lvl = [#] level of item to search for (for maps or gems)
    //qual = [#] quality of item to search for (for gems only, will search -2 to +2 of specified qual)
    //type = [Gem|Map|undefined] type of item being searched for
    //link = [pwx|xyz] what link to show in results (pwx = my graphs on prices, xyz = poe.xyz.is results)
    //show = [#] number of results to show in each currency (default: 2)
    //i = [string] this is for the custom interactive search, has some weird options, probably wouldn't use it (there is post-parsing for things like "level=X" etc.)
    //oo = [undef/anything] show on-line seller prices only
    //equiv = [undef/anything] show chaos equivalent prices (poe.pwx.me/rates for current rates)
    //d = [undef/anything] show some additional debug information in results
    public class PriceQuery
    {
        private const string searchURL = @"http://poe.pwx.me/decode-text?v={0}&equiv={1}&link={2}&show={3}&league={4}&s={5}&qual={6}&type={7}&oo={8}";

        public String League { get; set; }
        public String SearchPhrase { get; set; }
        public String MacroVersion { get; set; }
        public String Level { get; set; }
        public String Quality { get; set; }
        public String Type { get; set; }
        public String Showlink { get; set; }
        public String ResultCount { get; set; }
        public String OnlineOnly { get; set; }
        public String Equivalent { get; set; }

        //"i"
        public String CustomSearch { get; set; }

        public PriceQuery()
        {
            
        }
        public PriceQuery(Item item, bool showEquivalent, bool onlineOnly)
            : this(item.League, (item.ItemType != ItemType.Gem) ? item.Name + " " + item.TypeLine: item.TypeLine, "", item.Quality.ToString(), (item.ItemType == ItemType.Gem) ? item.ItemType.ToString() : "", showEquivalent, onlineOnly){}

        public PriceQuery(String league, String searchphrase, String level, String quality, String type, bool showEquivalent, bool onlineOnly)
        {
            this.League = league;
            this.SearchPhrase = searchphrase;
            this.MacroVersion = "3";
            this.Level = level;
            this.Quality = quality;
            this.Type = type;
            this.Showlink = "pwx";
            this.ResultCount = "3";
            if (showEquivalent) this.Equivalent = "yes";
            if (onlineOnly) this.OnlineOnly = "y";
        }
        
        public String getURL()
        {
            return String.Format(searchURL, MacroVersion, Equivalent, Showlink, ResultCount, Uri.EscapeDataString(League), Uri.EscapeDataString(SearchPhrase), Quality, Uri.EscapeDataString(Type), OnlineOnly);
        }

        public String getResultString()
        {
            try
            {
                string response = "";
                using(Stream s = ((HttpWebResponse)(((HttpWebRequest)WebRequest.Create(getURL())).GetResponse())).GetResponseStream())
	            {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        response = sr.ReadToEnd();
                        sr.Close();
                    }
                    s.Close();
	            }
                return response;
            }
            catch (Exception ex)
            {
                Logger.Log("Error getting price from PWX: " + ex.ToString());
                return "error - try again later";
            }
        }
    }
}
