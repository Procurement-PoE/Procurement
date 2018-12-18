using FluentAssertions;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POEApi.Transport;
using Procurement.Controls;
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
        public void GetExpiredNameCharactersTest()
        {
            string fakeCharacterInfo = Encoding.UTF8.GetString(Files.SampleCharacterExpiredName);

            using (var stream = GenerateStreamFromString(fakeCharacterInfo))
            {
                _mockTransport.Setup(m => m.GetCharacters()).Returns(stream);

                var characters = _model.GetCharacters();

                Assert.IsNotNull(characters);

                Assert.AreEqual(characters.Count, 2);
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
        public void GetFragmentStashTest()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleFragmentStash);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);

                Assert.AreEqual(1, stash.Tabs.Count);

                var items = stash.GetItemsByTab(12);
                
                var viewModel = new FragmentStashViewModel(items); 

                Assert.AreEqual(4, viewModel.ChayulaSplinter.Item.StackSize);
                Assert.AreEqual(6, viewModel.Offering.Item.StackSize);
                Assert.AreEqual(11, viewModel.Dawn.Item.StackSize);
                Assert.AreEqual(8, viewModel.TulSplinter.Item.StackSize);
                Assert.AreEqual(11, viewModel.XophSplinter.Item.StackSize);
                Assert.AreEqual(17, viewModel.EshSplinter.Item.StackSize);
                Assert.AreEqual(4, viewModel.ChayulaSplinter.Item.StackSize);
                Assert.AreEqual(1, viewModel.DivineVessel.Item.StackSize);
                Assert.IsNull(viewModel.Yriel.Item);
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

        [TestMethod]
        public void GetPantheonSoulInventoryTest()
        {
            string fakeInventoryInfo = Encoding.UTF8.GetString(Files.SampleInventoryWithPantheonSoul);
            using (var stream = GenerateStreamFromString(fakeInventoryInfo))
            {
                _mockTransport.Setup(m => m.GetInventory(string.Empty, false, string.Empty)).Returns(stream);
                var inventory = _model.GetInventory(string.Empty, false, string.Empty);

                inventory.Should().NotBeNull();
                inventory.Should().HaveCount(3);
                Item item = inventory[2];

                item.Should().NotBeNull();
                Currency pantheonSoul = item as Currency;

                pantheonSoul.Should().NotBeNull();
                pantheonSoul.Name.Should().BeEmpty();
                pantheonSoul.TypeLine.Should().Be("Captured Soul of The Forgotten Soldier");
                pantheonSoul.Type.Should().Be(OrbType.PantheonSoul);
                pantheonSoul.ItemType.Should().Be(ItemType.Currency);

                pantheonSoul.StackInfo.Should().NotBeNull();
                pantheonSoul.StackInfo.Amount.Should().Be(1);
                pantheonSoul.StackInfo.MaxSize.Should().Be(1);
            }
        }

        [TestMethod]
        public void GetNetsStashTest()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithNets);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, string.Empty, string.Empty, false)).Returns(stream);

                var stash = _model.GetStash(0, string.Empty, string.Empty);
                stash.Should().NotBeNull();
                stash.Tabs.Should().HaveCount(1);

                var items = stash.GetItemsByTab(5);
                items.Should().NotBeNull();
                items.Should().HaveCount(5);

                items.Should().AllBeAssignableTo<Currency>();

                var simpleRopeNet = items[1] as Net;
                simpleRopeNet.Should().NotBeNull();
                simpleRopeNet.Name.Should().BeEmpty();
                simpleRopeNet.TypeLine.Should().Be("Simple Rope Net");
                simpleRopeNet.NetTier.Should().Be(1);

                var thaumaturgicalNet = items[3] as Net;
                thaumaturgicalNet.Should().NotBeNull();
                thaumaturgicalNet.Name.Should().BeEmpty();
                thaumaturgicalNet.TypeLine.Should().Be("Thaumaturgical Net");
                thaumaturgicalNet.NetTier.Should().Be(10);

                var necromancyNet = items[4] as Net;
                necromancyNet.Should().NotBeNull();
                necromancyNet.Name.Should().BeEmpty();
                necromancyNet.TypeLine.Should().Be("Necromancy Net");
                necromancyNet.NetTier.Should().Be(0);
            }
        }

        [TestMethod]
        public void GetMirroredItemsStashTest()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithMirroredItems);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, string.Empty, string.Empty, false)).Returns(stream);
                var stash = _model.GetStash(0, string.Empty, string.Empty);
                stash.Should().NotBeNull();
                stash.Tabs.Should().HaveCount(1);

                var items = stash.GetItemsByTab(54);
                items.Should().NotBeNull();
                items.Should().HaveCount(8);

                var mirroredItems = items.Where(i => i.IsMirrored).ToList();
                var normalItems = items.Where(i => !i.IsMirrored).ToList();
                mirroredItems.Should().HaveCount(4);

                var mirroredItemsTypeLines = mirroredItems.Select(i => i.TypeLine).ToList();
                var normalItemsTypeLines = normalItems.Select(i => i.TypeLine).ToList();
                mirroredItemsTypeLines.Should().BeEquivalentTo(normalItemsTypeLines);

                var mirroredItemsTypes = mirroredItems.Select(i => i.GetType()).ToList();
                var normalItemsTypes = normalItems.Select(i => i.GetType()).ToList();
                mirroredItemsTypes.Should().BeEquivalentTo(normalItemsTypes);
            }
        }

        [TestMethod]
        public void GetInventoryWithQuestItemsTest()
        {
            string fakeCharacterInfo = Encoding.UTF8.GetString(Files.SampleInventoryWithQuestItems);

            using (var stream = GenerateStreamFromString(fakeCharacterInfo))
            {
                _mockTransport.Setup(m => m.GetInventory("", false, "")).Returns(stream);

                var inventory = _model.GetInventory("", false, "");

                Assert.IsNotNull(inventory);

                Assert.AreEqual(2, inventory.Count);
                Assert.AreEqual(true, inventory.All(x => x is QuestItem));
            }
        }
    }
}
