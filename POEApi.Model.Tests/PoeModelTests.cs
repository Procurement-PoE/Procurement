using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POEApi.Transport;

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

        public Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
