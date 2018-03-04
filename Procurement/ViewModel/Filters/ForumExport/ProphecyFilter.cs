using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class AbyssJewelFilter : IFilter
    {
        public bool Applicable(Item item)
        {
            return item is AbyssJewel;
        }

        public FilterGroup Group
        {
            get
            {
                return FilterGroup.Default;
            }
        }

        public bool CanFormCategory
        {
            get
            {
                return true;
            }
        }

        public string Keyword
        {
            get
            {
                return "Abyss Jewel";
            }
        }

        public string Help
        {
            get
            {
                return "All Abyss Jewels";
            }
        }

    }

    public class ProphecyFilter : IFilter
    {
        public bool CanFormCategory
        {
            get
            {
                return false;
            }
        }

        public string Keyword
        {
            get
            {
                return "Prophecy";
            }
        }

        public string Help
        {
            get
            {
                return "All Prophecies";
            }
        }

        public FilterGroup Group
        {
            get
            {
                return FilterGroup.Default;
            }
        }

        public bool Applicable(Item item)
        {
            return item is Prophecy;
        }
    }
}