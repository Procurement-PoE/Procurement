using System.Collections.Generic;
using FluentAssertions;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POEApi.Transport;
using Procurement.Controls;
using Procurement.ViewModel;
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
                _mockTransport.Setup(m => m.GetInventory("", false, "", Realm.PC)).Returns(stream);

                var inventory = _model.GetInventory("", -1, false, "", Realm.PC);

                Assert.IsNotNull(inventory);
            }
        }

        [TestMethod]
        public void GetCharactersTest()
        {
            string fakeCharacterInfo = Encoding.UTF8.GetString(Files.SampleCharacter);

            using (var stream = GenerateStreamFromString(fakeCharacterInfo))
            {
                _mockTransport.Setup(m => m.GetCharacters(Realm.PC)).Returns(stream);

                var characters = _model.GetCharacters(Realm.PC);

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
                _mockTransport.Setup(m => m.GetCharacters(Realm.PC)).Returns(stream);

                var characters = _model.GetCharacters(Realm.PC);

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
                _mockTransport.Setup(m => m.GetStash(0, "", "", Realm.PC, false)).Returns(stream);

                var stash = _model.GetStash(0, "", "", Realm.PC);

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
                _mockTransport.Setup(m => m.GetStash(0, "", "", Realm.PC, false)).Returns(stream);

                var stash = _model.GetStash(0, "", "", Realm.PC);

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
                _mockTransport.Setup(m => m.GetStash(0, "", "", Realm.PC, false)).Returns(stream);

                var stash = _model.GetStash(0, "", "", Realm.PC);

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
                _mockTransport.Setup(m => m.GetStash(0, "", "", Realm.PC, false)).Returns(stream);

                var stash = _model.GetStash(0, "", "", Realm.PC);

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
                _mockTransport.Setup(m => m.GetStash(0, "", "", Realm.PC, false)).Returns(stream);

                var stash = _model.GetStash(0, "", "", Realm.PC);

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
                _mockTransport.Setup(m => m.GetAccountName(Realm.PC)).Returns(stream);

                var account = _model.GetAccountName(Realm.PC);

                Assert.AreEqual("fakeAccountName", account);
            }
        }

        [TestMethod]
        public void GetShardCurrencyStashTest()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleCurrencyTabWithShards);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", Realm.PC, false)).Returns(stream);

                var stash = _model.GetStash(0, "", "", Realm.PC);

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
                _mockTransport.Setup(m => m.GetStash(0, "", "", Realm.PC, false)).Returns(stream);

                var stash = _model.GetStash(0, "", "", Realm.PC);

                Assert.IsNotNull(stash);

                Assert.AreEqual(1, stash.Tabs.Count);

                var items = stash.GetItemsByTab(12);
                var itemViewModelPairing = new Dictionary<Item, ItemDisplayViewModel>();

                foreach (var item in items)
                {
                    itemViewModelPairing.Add(item, null);
                }

                var viewModel = new FragmentStashViewModel(itemViewModelPairing); 

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
                _mockTransport.Setup(m => m.GetStash(0, "", "", Realm.PC, false)).Returns(stream);

                var stash = _model.GetStash(0, "", "", Realm.PC);

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
                _mockTransport.Setup(m => m.GetInventory(string.Empty, false, string.Empty, Realm.PC)).Returns(stream);
                var inventory = _model.GetInventory(string.Empty, -1, false, string.Empty, Realm.PC);

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

                pantheonSoul.StackSize.Should().Be(0);
            }
        }

        [TestMethod]
        public void GetNetsStashTest()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithNets);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, string.Empty, string.Empty, Realm.PC, false)).Returns(stream);

                var stash = _model.GetStash(0, string.Empty, string.Empty, Realm.PC);
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
                _mockTransport.Setup(m => m.GetStash(0, string.Empty, string.Empty, Realm.PC, false)).Returns(stream);
                var stash = _model.GetStash(0, string.Empty, string.Empty, Realm.PC);
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
        public void GetSynthesisedItemsStashTest()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithSynthesisItems);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, string.Empty, string.Empty, Realm.PC, false)).Returns(stream);
                var stash = _model.GetStash(0, string.Empty, string.Empty, Realm.PC);
                stash.Should().NotBeNull();
                stash.Tabs.Should().HaveCount(1);

                var items = stash.GetItemsByTab(1);
                items.Should().NotBeNull();
                items.Should().HaveCount(2);

                var synthesisedItems = items.Where(i => i.Synthesised).ToList();
                synthesisedItems.Should().HaveCount(1);

                var synthesisedItemTypeLine = synthesisedItems[0].TypeLine;
                synthesisedItemTypeLine.Should().NotStartWith("Synthesised");
                synthesisedItemTypeLine.Should().Contain(" Synthesised ");

                var synthesisedGear = synthesisedItems[0] as Gear;
                synthesisedGear.Should().NotBeNull();
                var synthesisedItemImplicitMods = synthesisedGear.Implicitmods;
                synthesisedItemImplicitMods.Should().HaveCount(1);
                synthesisedItemImplicitMods[0].Should().Be("Socketed Gems have 10% reduced Mana Reservation");
                synthesisedGear.Explicitmods.Should().BeNullOrEmpty();
                synthesisedGear.FracturedMods.Should().BeNullOrEmpty();

                var fracturedItems = items.Where(i => i.Fractured).ToList();
                fracturedItems.Should().HaveCount(1);
                fracturedItems[0].TypeLine.Should().Be("Leather Belt");

                var fracturedGear = fracturedItems[0] as Gear;
                fracturedGear.Should().NotBeNull();
                var fracturedGearFracturedMods = fracturedGear.FracturedMods;
                fracturedGearFracturedMods.Should().HaveCount(2);
                fracturedGearFracturedMods.Should().Contain(
                    new List<string>{ "+40 to maximum Energy Shield", "+42% to Cold Resistance" });

                synthesisedItems[0].TypeLine.Should().NotBe(fracturedItems[0].TypeLine);
            }
        }

        [TestMethod]
        public void IsScarabDetected()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithScarab);

            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", Realm.PC, false)).Returns(stream);

                var stash = _model.GetStash(0, "", "", Realm.PC);

                Assert.IsNotNull(stash);
                var scarab = stash.GetItemsByTab(1).OfType<Scarab>()
                            .First(x => x.ScarabRank == ScarabRank.Gilded && x.ScarabEffect == ScarabEffect.Breach);

                Assert.IsNotNull(scarab);
                
                Assert.AreEqual(ScarabRank.Gilded, scarab.ScarabRank);
                Assert.AreEqual(ScarabEffect.Breach, scarab.ScarabEffect);
            }
        }

        [TestMethod]
        public void GetInventoryWithQuestItemsTest()
        {
            string fakeCharacterInfo = Encoding.UTF8.GetString(Files.SampleInventoryWithQuestItems);

            using (var stream = GenerateStreamFromString(fakeCharacterInfo))
            {
                _mockTransport.Setup(m => m.GetInventory("", false, "", Realm.PC)).Returns(stream);

                var inventory = _model.GetInventory("", -1, false, "", Realm.PC);

                Assert.IsNotNull(inventory);

                Assert.AreEqual(2, inventory.Count);
                Assert.AreEqual(true, inventory.All(x => x is QuestItem));
            }
        }
    }
}
