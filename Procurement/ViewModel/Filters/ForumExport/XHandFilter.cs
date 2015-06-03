using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public abstract class XHandFilter : IFilter
    {
        private string handed;

        public XHandFilter(string handed)
        {
            this.handed = handed;
        }
        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return string.Concat(handed, " ", "Handed Weapon"); }
        }

        public string Help
        {
            get { return string.Concat("Returns all", handed, "(s)"); }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public bool Applicable(Item item)
        {
            Gear gear = item as Gear;
            if (gear == null)
                return false;

            if (gear.Properties == null)
                return false;

            //TODO: RU strings "Одноручн","Двуручн"
            return gear.Properties.Any(p => p.Name.Contains(string.Concat(handed, " ", "Handed")));
        }
    }
}
