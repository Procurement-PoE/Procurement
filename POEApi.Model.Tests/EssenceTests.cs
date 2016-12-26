using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace POEApi.Model.Tests
{
    [TestClass]
    public class EssenceTests
    {
        [TestMethod]
        //Small test to generate the proper orbtypes (I don't want to type it all out by hand...)
        public void AreAllEssencesGenerated()
        {
            //See: https://hydra-media.cursecdn.com/pathofexile.gamepedia.com/d/d5/Essences.png?version=04d25abf2ea2220d05be3f20aa2cc17a
            //First 4 have all tiers, next only 6 etc...
            var totalEssences = 4 * 3 + 4 * 4 + 4 * 5 + 4 * 6 + 4 * 7 + 5;

            Assert.AreEqual(totalEssences, Essence.EssenceGenerator().Count);

            Essence.EssenceGenerator().ForEach(x => Debug.WriteLine(x));
        }


    }
}