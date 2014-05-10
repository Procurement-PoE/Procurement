using POEApi.Model;
namespace Procurement.ViewModel.Filters
{
    internal class NormalRarity : RarityFilter
    {
        public NormalRarity()
            : base(Rarity.Normal)
        { }
    }
}
