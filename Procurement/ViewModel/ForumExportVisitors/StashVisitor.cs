using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class StashVisitor : VisitorBase
    {
        private const string TOKENSTART = "{Stash:";
        public override string Visit(IEnumerable<Item> items, string current)
        {
            List<int> tokenLocations = getTokenLocations(current);
            if (tokenLocations.Count == 0)
                return current;

            foreach (int location in tokenLocations)
            {
                var nameToken = getToken(location, current);
                Regex replacer = new Regex(nameToken.Item2);
                var tabs = ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs.FindAll(t => t.Name == nameToken.Item1);
                if (tabs == null)
                {
                    current = replacer.Replace(current, string.Empty, 1, location);
                    continue;
                }

                string sItems = "";
                foreach (Tab tab in tabs)
                    sItems  = sItems + getItems(ApplicationState.Stash[ApplicationState.CurrentLeague].GetItemsByTab(tab.i).OrderBy(i => i.H).ThenBy(i => i.IconURL));

                current = current.Replace(nameToken.Item2, sItems);
            }

            return current;
        }

        private Tuple<string, string> getToken(int location, string current)
        {
            int startLength = TOKENSTART.Length;
            int tokenEnd = current.IndexOf("}", location);
            int nameStart = location + startLength;

            string name = current.Substring(nameStart, tokenEnd - nameStart);
            string token = current.Substring(location, tokenEnd - (location - 1));

            return new Tuple<string,string>(name, token);
        }

        private string getItems(IEnumerable<Item> items)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in items)
                builder.Append(getLinkItem(item));

            return builder.ToString();
        }

        private List<int> getTokenLocations(string current)
        {
            List<int> tokenLocations = new List<int>();

            int index = current.IndexOf(TOKENSTART, 0);

            while (index > -1)
            {
                tokenLocations.Add(index);
                index = current.IndexOf(TOKENSTART, (index + 1));
            }

            tokenLocations.Reverse();
            return tokenLocations;
        }
    }
}
