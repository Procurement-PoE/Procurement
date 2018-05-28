using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POEApi.TestHelpers.Builders;

namespace POEApi.Model.Tests
{
    [TestClass]
    public class GearTests
    {
        [TestMethod]
        public void Item_WithNullSockets_ShouldBeProperlyConstructed()
        {
            JSONProxy.Item proxyItem = Build.A.JsonProxyItem
                                              .WithTypeLine("Koam's Heart")
                                              .WithoutSockets();

            Gear gear = new Gear(proxyItem);

            gear.NumberOfSockets().Should().Be(0);
            gear.SocketedItems.Should().BeEmpty();
            gear.Sockets.Should().BeEmpty();
        }

        [TestMethod]
        public void Item_TypeLineSubstringOfOtherTypeLine_ShouldFindCorrectBaseType()
        {
            JSONProxy.Item tricorneProxyItem = Build.A.JsonProxyItem.WithTypeLine("Tricorne");
            JSONProxy.Item NobleTricorneProxyItem = Build.A.JsonProxyItem.WithTypeLine("Noble Tricorne");
            JSONProxy.Item SinnerTricorneProxyItem = Build.A.JsonProxyItem.WithTypeLine("Sinner Tricorne");

            var tricorneItem = new Gear(tricorneProxyItem);
            tricorneItem.BaseType.Should().Be("Tricorne");
            tricorneItem.GearType.Should().Be(GearType.Helmet);
            tricorneItem.DescriptiveName.Should().Be("Tricorne");

            var nobleTricorneItem = new Gear(NobleTricorneProxyItem);
            nobleTricorneItem.BaseType.Should().Be("Noble Tricorne");
            nobleTricorneItem.GearType.Should().Be(GearType.Helmet);
            nobleTricorneItem.DescriptiveName.Should().Be("Noble Tricorne");

            var sinnerTricorneItem = new Gear(SinnerTricorneProxyItem);
            sinnerTricorneItem.BaseType.Should().Be("Sinner Tricorne");
            sinnerTricorneItem.GearType.Should().Be(GearType.Helmet);
            sinnerTricorneItem.DescriptiveName.Should().Be("Sinner Tricorne");
        }

        [TestMethod]
        public void Item_IdentifiedItems_DescriptiveNameProducesCorrectDescription()
        {
            JSONProxy.Item normalProxyItem = Build.A.JsonProxyItem.WithFrameType((int)Rarity.Normal).WithItemLevel(10)
                .WithTypeLine("Tricorne").WithQuality(9);
            JSONProxy.Item magicProxyItem = Build.A.JsonProxyItem.WithFrameType((int)Rarity.Magic).WithItemLevel(15)
                .WithTypeLine("Noble Tricorne").ThatIsIdentified(false);
            JSONProxy.Item rareProxyItem = Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20)
                .WithTypeLine("Sinner Tricorne").WithQuality(0).ThatIsIdentified(false).WithName("Fantastic Voyage");
            JSONProxy.Item uniqueProxyItem = Build.A.JsonProxyItem.WithFrameType((int)Rarity.Unique).WithItemLevel(30)
                .WithTypeLine("Sinner Tricorne").WithQuality(20).ThatIsIdentified(false)
                .WithName("Kender's Confidence");

            var normalItem = new Gear(normalProxyItem);
            normalItem.Quality.Should().Be(9);
            normalItem.IsQuality.Should().BeTrue();
            normalItem.Rarity.Should().Be(Rarity.Normal);
            normalItem.BaseType.Should().Be("Tricorne");
            normalItem.GearType.Should().Be(GearType.Helmet);
            normalItem.DescriptiveName.Should().Be("Tricorne, +9% Quality, i10");

            var magicItem = new Gear(magicProxyItem);
            magicItem.Quality.Should().Be(0);
            magicItem.IsQuality.Should().BeFalse();
            magicItem.Rarity.Should().Be(Rarity.Magic);
            magicItem.BaseType.Should().Be("Noble Tricorne");
            magicItem.GearType.Should().Be(GearType.Helmet);
            magicItem.DescriptiveName.Should().Be("Unidentified Magic Noble Tricorne, i15");

            var rareItem = new Gear(rareProxyItem);
            rareItem.Quality.Should().Be(0);
            rareItem.IsQuality.Should().BeTrue();
            rareItem.Rarity.Should().Be(Rarity.Rare);
            rareItem.BaseType.Should().Be("Sinner Tricorne");
            rareItem.GearType.Should().Be(GearType.Helmet);
            rareItem.DescriptiveName.Should().Be("Unidentified Rare Sinner Tricorne, +0% Quality, i20");

            var uniqueItem = new Gear(uniqueProxyItem);
            uniqueItem.Quality.Should().Be(20);
            uniqueItem.IsQuality.Should().BeTrue();
            uniqueItem.Rarity.Should().Be(Rarity.Unique);
            uniqueItem.BaseType.Should().Be("Sinner Tricorne");
            uniqueItem.GearType.Should().Be(GearType.Helmet);
            uniqueItem.DescriptiveName.Should().Be("Unidentified Unique Sinner Tricorne, +20% Quality, i30");
        }

        [TestMethod]
        public void Item_UnidentifiedItems_DescriptiveNameProducesCorrectDescription()
        {
            JSONProxy.Item identifiedMagicProxyItem = Build.A.JsonProxyItem.WithFrameType((int)Rarity.Magic)
                .WithItemLevel(17).WithQuality(11).WithTypeLine("Noble Tricorne of Wanderlust").ThatIsIdentified(true);
            JSONProxy.Item identifiedRareProxyItem = Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare)
                .WithItemLevel(25).WithTypeLine("Sinner Tricorne").ThatIsIdentified(true)
                .WithName("Fantastic Voyage").WithQuality(1);
            JSONProxy.Item identifiedUniqueProxyItem = Build.A.JsonProxyItem.WithFrameType((int)Rarity.Unique)
                .WithItemLevel(35).WithTypeLine("Sinner Tricorne").ThatIsIdentified(true)
                .WithName("Kender's Confidence");

            var identifiedMagicItem = new Gear(identifiedMagicProxyItem);
            identifiedMagicItem.Quality.Should().Be(11);
            identifiedMagicItem.IsQuality.Should().BeTrue();
            identifiedMagicItem.Rarity.Should().Be(Rarity.Magic);
            identifiedMagicItem.BaseType.Should().Be("Noble Tricorne");
            identifiedMagicItem.GearType.Should().Be(GearType.Helmet);
            identifiedMagicItem.DescriptiveName.Should().Be("Noble Tricorne of Wanderlust, +11% Quality, i17");

            var identifiedRareItem = new Gear(identifiedRareProxyItem);
            identifiedRareItem.Quality.Should().Be(1);
            identifiedRareItem.IsQuality.Should().BeTrue();
            identifiedRareItem.Rarity.Should().Be(Rarity.Rare);
            identifiedRareItem.BaseType.Should().Be("Sinner Tricorne");
            identifiedRareItem.GearType.Should().Be(GearType.Helmet);
            identifiedRareItem.DescriptiveName.Should().Be(
                "\"Fantastic Voyage\", Rare Sinner Tricorne, +1% Quality, i25");

            var identifiedUniqueItem = new Gear(identifiedUniqueProxyItem);
            identifiedUniqueItem.Quality.Should().Be(0);
            identifiedUniqueItem.IsQuality.Should().BeFalse();
            identifiedUniqueItem.Rarity.Should().Be(Rarity.Unique);
            identifiedUniqueItem.BaseType.Should().Be("Sinner Tricorne");
            identifiedUniqueItem.GearType.Should().Be(GearType.Helmet);
            identifiedUniqueItem.DescriptiveName.Should().Be("\"Kender's Confidence\", Unique Sinner Tricorne, i35");
        }

        [TestMethod]
        public void Item_WithMissingExpectedComponents_DescriptiveNameProducesCorrectDescription()
        {
            JSONProxy.Item withoutItemLevelProxyItem = Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare)
                .WithTypeLine("Sinner Tricorne").ThatIsIdentified(true)
                .WithName("Fantastic Voyage");

            var withoutItemLevelItem = new Gear(withoutItemLevelProxyItem);
            withoutItemLevelItem.Quality.Should().Be(0);
            withoutItemLevelItem.IsQuality.Should().BeFalse();
            withoutItemLevelItem.Rarity.Should().Be(Rarity.Rare);
            withoutItemLevelItem.BaseType.Should().Be("Sinner Tricorne");
            withoutItemLevelItem.GearType.Should().Be(GearType.Helmet);
            withoutItemLevelItem.DescriptiveName.Should().Be("\"Fantastic Voyage\", Rare Sinner Tricorne");

            JSONProxy.Item withoutItemNameProxyItem = Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare)
                .WithItemLevel(35).WithTypeLine("Sinner Tricorne").ThatIsIdentified(true)
                .WithName(null);

            var withoutItemNameItem = new Gear(withoutItemNameProxyItem);
            withoutItemNameItem.Quality.Should().Be(0);
            withoutItemNameItem.IsQuality.Should().BeFalse();
            withoutItemNameItem.Rarity.Should().Be(Rarity.Rare);
            withoutItemNameItem.BaseType.Should().Be("Sinner Tricorne");
            withoutItemNameItem.GearType.Should().Be(GearType.Helmet);
            withoutItemNameItem.DescriptiveName.Should().Be("Rare Sinner Tricorne, i35");
        }
    }
}
