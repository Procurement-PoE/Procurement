using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POEApi.Transport;
using Procurement.ViewModel.Filters;
using Procurement.ViewModel.Filters.ForumExport;
using static POEApi.Model.Tests.UnitTestHelper;

namespace POEApi.Model.Tests
{
    [TestClass]
    public class IFilterTests
    {
        private Mock<ITransport> _mockTransport;
        private POEModel _model;
        private IFilter filter;

        [TestInitialize]
        public void TestSetup()
        {
            _mockTransport = new Mock<ITransport>();
            _model = new POEModel { Transport = _mockTransport.Object };
        }

        [TestMethod]
        public void AreEssencesApplicable()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithRemnantOfCorruption);
            filter = new EssenceFilter();

            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);

                var essences = stash.GetItemsByTab(6);

                Assert.IsTrue(essences.All(x => x is Essence));

                //All the items, which are essences in the first place, should make it into the forum template
                Assert.IsTrue(essences.TrueForAll(filter.Applicable));
            }
        }

        [TestMethod]
        public void AreDivineVesselsApplicable()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithDivineVessel);
            filter = new DivineVesselFilter();

            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);

                var vessel = stash.GetItemsByTab(1).Where(x => x is DivineVessel);

                Assert.AreEqual(1, vessel.Count());

                Assert.IsTrue(filter.Applicable(vessel.First()));
            }
        }
    }
}