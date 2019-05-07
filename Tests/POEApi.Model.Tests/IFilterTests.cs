using System.Collections.Generic;
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

        [TestMethod]
        public void AreOfferingsApplicable()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithDivineVessel);
            filter = new OfferingFilter();

            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);

                var offerings = stash.GetItemsByTab(1).Where(x => x is Offering);

                Assert.AreEqual(3, offerings.Count());

                Assert.IsTrue(offerings.All(filter.Applicable));
            }
        }

        [TestMethod]
        public void AreScarabsApplicable()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithScarab);
            filter = new ScarabFilter();

            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);
                var test = stash.GetItemsByTab(1);
                var scarab = stash.GetItemsByTab(1).Where(x => x is Scarab);

                Assert.AreEqual(1, scarab.Count());

                Assert.IsTrue(filter.Applicable(scarab.First()));
            }
        }

        [TestMethod]
        public void AreRareMapsApplicable()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithMaps);
            filter = new MapFilter();
            var rareFilter = new RarityFilter(Rarity.Rare);

            var filters = new[] {filter, rareFilter};
            

            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);

                var maps = stash.GetItemsByTab(4).Where(x => x is Map);

                Assert.AreEqual(88, maps.Count());

                Assert.IsTrue(maps.All(filter.Applicable));

                var rareMaps = new List<Item>();

                foreach (var map in maps)
                {
                    if(filters.All(x => x.Applicable(map)))
                    {
                        rareMaps.Add(map);
                    }
                }
                
                Assert.IsTrue(rareMaps.Count > 0);
                Assert.IsTrue(maps.Count() > rareMaps.Count);
            }
        }

    }
}