namespace Procurement.ViewModel.Filters
{
    public class VaalUberFragmentFilter : TypeLineFilter
    {
        public VaalUberFragmentFilter()
            : base("Mortal Grief", "Mortal Rage", "Mortal Hope", "Mortal Ignorance")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                this.keywords = new string[] { "Смертное уныние", "Смертный гнев", "Смертная надежда", "Смертное невежество" };
            }
        }
        public override bool CanFormCategory
        {
            get { return true; }
        }

        public override string Keyword
        {
            get
            {
                if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                {
                    return "Уникальные фрагменты Ваал";
                }
                else
                {
                    return "Uber Vaal Fragments";
                }
            }
        }

        public override string Help
        {
            get
            {
                if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                {
                    return "Уникальные фрагменты Ваал";
                }
                else
                {
                    return "Uber Vaal Fragments";
                }
            }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.VaalFragments; }
        }
    }
}
