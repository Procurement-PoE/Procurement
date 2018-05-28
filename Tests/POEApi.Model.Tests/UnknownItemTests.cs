using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POEApi.TestHelpers.Builders;

namespace POEApi.Model.Tests
{
    [TestClass]
    public class UnknownItemTests
    {
        [TestMethod]
        public void UnknownItem_EmptyConstructor()
        {
            var item = new UnknownItem();
            item.Quality.Should().Be(0);
            item.IsQuality.Should().BeFalse();
            item.TypeLine.Should().BeNull();
            item.DescriptiveName.Should().Be("[Unknown Item]");
            item.ItemSource.Should().BeNull();
            item.ErrorInformation.Should().BeNull();
        }

        [TestMethod]
        public void UnknownItem_IdentifiedItems_DescriptiveNameProducesCorrectDescription()
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

            var normalItem = new UnknownItem(normalProxyItem);
            normalItem.Quality.Should().Be(9);
            normalItem.IsQuality.Should().BeTrue();
            normalItem.TypeLine.Should().Be("Tricorne");
            normalItem.DescriptiveName.Should().Be("[Unknown Item] Tricorne, +9% Quality, i10");
            normalItem.ItemSource.Should().Be(normalProxyItem);
            normalItem.ErrorInformation.Should().BeNull();

            var magicItem = new UnknownItem(magicProxyItem);
            magicItem.Quality.Should().Be(0);
            magicItem.IsQuality.Should().BeFalse();
            magicItem.TypeLine.Should().Be("Noble Tricorne");
            magicItem.DescriptiveName.Should().Be("[Unknown Item] Noble Tricorne, i15");
            magicItem.ItemSource.Should().Be(magicProxyItem);
            magicItem.ErrorInformation.Should().BeNull();

            var rareItem = new UnknownItem(rareProxyItem);
            rareItem.Quality.Should().Be(0);
            rareItem.IsQuality.Should().BeTrue();
            rareItem.TypeLine.Should().Be("Sinner Tricorne");
            rareItem.DescriptiveName.Should().Be("[Unknown Item] Sinner Tricorne, +0% Quality, i20");
            rareItem.ItemSource.Should().Be(rareProxyItem);
            rareItem.ErrorInformation.Should().BeNull();

            var uniqueItem = new UnknownItem(uniqueProxyItem);
            uniqueItem.Quality.Should().Be(20);
            uniqueItem.IsQuality.Should().BeTrue();
            uniqueItem.TypeLine.Should().Be("Sinner Tricorne");
            uniqueItem.DescriptiveName.Should().Be("[Unknown Item] Sinner Tricorne, +20% Quality, i30");
            uniqueItem.ItemSource.Should().Be(uniqueProxyItem);
            uniqueItem.ErrorInformation.Should().BeNull();
        }
    }
}
