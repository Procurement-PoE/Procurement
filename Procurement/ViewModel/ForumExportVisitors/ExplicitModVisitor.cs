using POEApi.Model;
using Procurement.ViewModel.Filters;
using Procurement.ViewModel.Filters.ForumExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.ForumExportVisitors
{
    class ExplicitModVisitor : VisitorBase
    {

        private Dictionary<string, IFilter> tokens;
        public ExplicitModVisitor()
        {
            tokens = new Dictionary<string, IFilter>();
            tokens.Add("{Life}", new LifeFilter());
            tokens.Add("{LifeRegen}", new LifeRegenFilter());
            tokens.Add("{CritChance}", new CritChanceFilter());
            tokens.Add("{GlobalCritChance}", new GlobalCritChanceFilter());
            tokens.Add("{GlobalCritMultiplier}", new GlobalCritMultiplierFilter());
            tokens.Add("{SpellDamage}", new SpellDamageFilter());
            tokens.Add("{Mana}", new ManaFilter());
            tokens.Add("{ManaRegen}", new ManaRegenFilter());
        }
        public override string Visit(IEnumerable<Item> items, string current)
        {
            string updated = current;
            var sorted = items.OrderBy(i => i.H).ThenBy(i => i.IconURL);

            foreach (var token in tokens)
            {
                if (updated.IndexOf(token.Key) < 0)
                    continue;

                updated = updated.Replace(token.Key, runFilter(token.Value, sorted));
            }

            return updated;
        }
    }
}
