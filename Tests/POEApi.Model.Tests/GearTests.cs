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
    }
}
