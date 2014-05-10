using POEApi.Model;
namespace Procurement.ViewModel.Filters
{
    internal class UniqueRarity : RarityFilter
    {
        public UniqueRarity()
            : base(Rarity.Unique)
        { }
    }
}
