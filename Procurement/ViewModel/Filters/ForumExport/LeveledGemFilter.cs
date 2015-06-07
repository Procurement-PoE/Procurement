using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal class LeveledGemFilter : IFilter
    {
        private readonly int level;

        public LeveledGemFilter()
            : this(0)
        { }

        public LeveledGemFilter(int level)
        {
            this.level = level;
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }

        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get
            {
                if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                {
                    return (this.level == 0) ? "Камни с уровнем" : "Камни " + level.ToString() + " уровня";
                }
                else
                {
                    return (this.level == 0) ? "Leveled Gems" : "Level " + level.ToString() + " Gems";
                }
            }
        }

        public string Help
        {
            get { return (this.level == 0) ? "Leveled Gems" : "Level " + level.ToString() + " Gems"; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            Gem gem = item as Gem;
            if (gem == null)
                return false;

            if (this.level == 0)
                return gem.Level > 1;

            return gem.Level == this.level;
        }
    }
}
