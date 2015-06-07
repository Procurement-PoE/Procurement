using System.Text.RegularExpressions;
using System.Linq;
namespace Procurement.ViewModel.Filters.ForumExport
{
    class StrengthFilter : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.CharacterAttributes; }
        }

        public StrengthFilter()
            : base("Increased Strength", "Strength", "Strength")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "к силе" };
                this.keyword = "Повышение силы";
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }


    class IntelligenceFilter : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.CharacterAttributes; }
        }

        public IntelligenceFilter()
            : base("Increased Intelligence", "Intelligence", "Intelligence")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "к интеллекту" };
                this.keyword = "Повышение интеллекта";
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }

    class DexterityFilter : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.CharacterAttributes; }
        }

        public DexterityFilter()
            : base("Increased Dexterity", "Increased Dexterity", "Dexterity")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "к ловкости" };
                this.keyword = "Повышение ловкости";
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}
