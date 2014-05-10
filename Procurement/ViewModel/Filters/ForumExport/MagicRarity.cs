using POEApi.Model;
namespace Procurement.ViewModel.Filters
{
    internal class MagicRarity : RarityFilter
    {
        public MagicRarity()
            : base(Rarity.Magic)
        { }
    }
}
