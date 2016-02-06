using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POEApi.Transport;

namespace POEApi.Model.Tests
{
    [TestClass]
    public class PoeModelTests
    {
        //[TestMethod]
        //public void GetStashTest()
        //{
        //    POEModel model = new POEModel();

        //    Mock<ITrans
        //    model.GetStash("Talisman", "d347hm4n");
        //}

        [TestMethod]
        public void GetAccountNameTest()
        {
            var mockTransport = new Mock<ITransport>();
            
            var fakeAccountNameResponse = "{\"accountName\":\"fakeAccountName\"}";

            using (var stream = GenerateStreamFromString(fakeAccountNameResponse))
            {
                mockTransport.Setup(m => m.GetAccountName()).Returns(stream);

                var model = new POEModel {Transport = mockTransport.Object};

                var account = model.GetAccountName();

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
