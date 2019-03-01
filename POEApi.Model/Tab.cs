using System;
using System.Diagnostics;
namespace POEApi.Model
{
    public enum TabType
    {
        Normal,
        Premium,
        Currency,
        DivinationCard,
        Essence,
        Quad,
        Map,
        Fragment,

        Unknown
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Tab
    {
        public bool IsFakeTab { get; set; }
        public TabType Type { get; set; }
        public string Name { get; set; }
        public int i { get; set; }
        public Colour Colour { get; set; }
        public string srcL { get; set; }
        public string srcC { get; set; }
        public string srcR { get; set; }
        public bool Hidden { get; set; }        
        
        public Tab()
        { }

        public Tab(JSONProxy.Tab t)
        {
            Colour = new Colour() { b = t.colour.b, g = t.colour.g, r = t.colour.r };
            i = t.i;
            Name = t.n;
            srcR = GetUrl(t.srcR);
            srcC = GetUrl(t.srcC);
            srcL = GetUrl(t.srcL); 
            Hidden = t.hidden;
            Type = ProxyMapper.GetTabType(t.type);
        }

        private string GetUrl(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                return url;

            return "http://webcdn.pathofexile.com" + url;
        }

        private string DebuggerDisplay
        {
            get { return string.Format("{0} Tab - \"{1}\"", Type, Name); }
        }
    }
}
