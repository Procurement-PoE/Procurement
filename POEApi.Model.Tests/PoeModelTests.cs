using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POEApi.Transport;
using static POEApi.Model.Tests.UnitTestHelper;

namespace POEApi.Model.Tests
{
    [TestClass]
    public class PoeModelTests
    {
        private Mock<ITransport> _mockTransport;
        private POEModel _model;

        [TestInitialize]
        public void TestSetup()
        {
            _mockTransport = new Mock<ITransport>();
            _model = new POEModel {Transport = _mockTransport.Object};
        }

        [TestMethod]
        public void GetInventoryTest()
        {
            string fakeCharacterInfo = Encoding.UTF8.GetString(Files.SampleInventory);

            using (var stream = GenerateStreamFromString(fakeCharacterInfo))
            {
                _mockTransport.Setup(m => m.GetInventory("", false, "")).Returns(stream);

                var inventory = _model.GetInventory("", false, "");

                Assert.IsNotNull(inventory);
            }
        }

        [TestMethod]
        public void GetCharactersTest()
        {
            string fakeCharacterInfo = Encoding.UTF8.GetString(Files.SampleCharacter);

            using (var stream = GenerateStreamFromString(fakeCharacterInfo))
            {
                _mockTransport.Setup(m => m.GetCharacters()).Returns(stream);

                var characters = _model.GetCharacters();

                Assert.IsNotNull(characters);

                Assert.AreEqual(5, characters.Count);
            }
        }

        [TestMethod]
        public void GetStashTest()
        {
            //Nasty little gotcha See: http://blogs.msdn.com/b/cie/archive/2014/03/19/encountered-unexpected-character-239-error-serializing-json.aspx
            //You have to remove the first three bytes from the file.
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStash);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);

                Assert.AreEqual(17, stash.Tabs.Count);
            }
        }

        [TestMethod]
        public void GetEssenceStashTest()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithEssences);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);

                Assert.AreEqual(83, stash.Tabs.Count);

                var items = stash.GetItemsByTab(14);

                Assert.IsTrue(items.Any(x => x is Essence));
            }
        }

        [TestMethod]
        public void GetRelicStashTest()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithRelic);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);

                Assert.AreEqual(27, stash.Tabs.Count);

                var items = stash.GetItemsByTab(7);

                Assert.AreEqual(1, items.OfType<Gear>().Count(x => x.Rarity == Rarity.Relic));
            }
        }

        [TestMethod]
        public void GetLitheBladeStashTest()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithLitheBlade);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);

                Assert.AreEqual(39, stash.Tabs.Count);

                var items = stash.GetItemsByTab(12);

                Assert.AreEqual(1, items.OfType<Gear>().Count(x => x.TypeLine == "Lithe Blade" && x.GearType == GearType.Sword));
            }
        }

        [TestMethod]
        public void GetSaintlyChainmailStashTest()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithSaintlyChainmail);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);

                Assert.AreEqual(39, stash.Tabs.Count);

                var items = stash.GetItemsByTab(19);

                Assert.AreEqual(1, items.OfType<Gear>().Count(x => x.TypeLine == "Saintly Chainmail" && x.GearType == GearType.Chest));
            }
        }

        [TestMethod]
        public void GetAccountNameTest()
        {
            var fakeAccountNameResponse = "{\"accountName\":\"fakeAccountName\"}";

            using (var stream = GenerateStreamFromString(fakeAccountNameResponse))
            {
                _mockTransport.Setup(m => m.GetAccountName()).Returns(stream);

                var account = _model.GetAccountName();

                Assert.AreEqual("fakeAccountName", account);
            }
        }

        [TestMethod]
        public void GetShardCurrencyStashTest()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleCurrencyTabWithShards);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);

                Assert.AreEqual(26, stash.Tabs.Count);

                var items = stash.GetItemsByTab(0);

                Assert.AreEqual(1, items.OfType<Currency>().Count(x => x.Type == OrbType.EngineersOrb));
                Assert.AreEqual(1, items.OfType<Currency>().Count(x => x.Type == OrbType.BindingOrb));
                Assert.AreEqual(1, items.OfType<Currency>().Count(x => x.Type == OrbType.BindingShard));
            }
        }

        [TestMethod]
        public void GetChargeTest()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithLeagueStoneChargeInfo);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);

                Assert.AreEqual(361, stash.Tabs.Count);

                var items = stash.GetItemsByTab(5);

                var leagueStones = items.OfType<Leaguestone>();
                
                Assert.IsTrue(leagueStones.All(x => x.Charges.ToString() == "5/5"));
            }
        }
    }
}
