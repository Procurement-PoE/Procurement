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

            var nobleTricorneItem = new Gear(NobleTricorneProxyItem);
            nobleTricorneItem.BaseType.Should().Be("Noble Tricorne");
            nobleTricorneItem.GearType.Should().Be(GearType.Helmet);

            var sinnerTricorneItem = new Gear(SinnerTricorneProxyItem);
            sinnerTricorneItem.BaseType.Should().Be("Sinner Tricorne");
            sinnerTricorneItem.GearType.Should().Be(GearType.Helmet);
        }
    }
}
