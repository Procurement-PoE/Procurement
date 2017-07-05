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

                Assert.AreEqual(characters.Count, 5);
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

                Assert.AreEqual(stash.Tabs.Count, 17);
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

                Assert.AreEqual(stash.Tabs.Count, 83);

                var items = stash.GetItemsByTab(14);

                Assert.IsTrue(items.Any(x => x is Essence));
            }
        }

        [TestMethod]
        public void GetRelichStashTest()
        {
            string fakeStashInfo = Encoding.UTF8.GetString(Files.SampleStashWithRelic);
            using (var stream = GenerateStreamFromString(fakeStashInfo))
            {
                _mockTransport.Setup(m => m.GetStash(0, "", "", false)).Returns(stream);

                var stash = _model.GetStash(0, "", "");

                Assert.IsNotNull(stash);

                Assert.AreEqual(stash.Tabs.Count, 27);

                var items = stash.GetItemsByTab(7);

                Assert.AreEqual(items.OfType<Gear>().Count(x => x.Rarity == Rarity.Relic), 1);
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

                Assert.AreEqual(account, "fakeAccountName");
            }
        }
    }
}
