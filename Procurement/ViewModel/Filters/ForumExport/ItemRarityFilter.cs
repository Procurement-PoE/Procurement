namespace Procurement.ViewModel.Filters
{
    internal class ItemRarityFilter : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.MagicFind; }
        }

        public ItemRarityFilter()
            : base("Item Rarity", "Item with the Item Rarity stat", "INCREASED RARITY")
        { }
    }
}
