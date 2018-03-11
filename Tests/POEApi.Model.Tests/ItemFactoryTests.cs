using Microsoft.VisualStudio.TestTools.UnitTesting;
using POEApi.Model.Tests.Builders;
using FluentAssertions;
using System.Linq;

namespace POEApi.Model.Tests
{
    [TestClass]
    public class ItemFactoryTests
    {
        [TestMethod]
        public void ItemFactory_GivenItemWithAbyssJewel_ShouldConstructItem()
        {
            var abyssJewel = Build.A.JsonProxyItem
                                    .ThatIsAnAbyssJewel();

            var item = Build.A.JsonProxyItem
                              .WithSocketedItem(abyssJewel);

            Gear result = ItemFactory.Get(item) as Gear;

            result.Should().NotBeNull();
            result.SocketedItems.Single().Should().BeOfType<AbyssJewel>();
        }

        [TestMethod]
        public void ItemFactory_GivenBreakingItem_ShouldReturnUnknownItem()
        {
            JSONProxy.Item item = new JSONProxy.Item();

            Item result = ItemFactory.Get(item);

            result.Should().NotBeNull();
            result.Should().BeOfType<UnknownItem>();
        }
    }
}
