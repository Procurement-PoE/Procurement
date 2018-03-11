using Microsoft.VisualStudio.TestTools.UnitTesting;
using POEApi.TestHelpers.Builders;
using POEApi.Model.JSONProxy;
using Procurement.ViewModel.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POEApi.Model;

namespace Procurement.Tests
{
    [TestClass]
    public class RecipeTests
    {
        public bool IsCloseToValue(decimal expectedValue, decimal epsilon, decimal actualValue)
        {
            decimal difference = Math.Abs(expectedValue - actualValue);
            return difference < epsilon;
        }

        [TestMethod]
        public void Recipe_SameNameRecipe_SameBaseType()
        {
            const string itemsName = "Alpha Beta";
            POEApi.Model.JSONProxy.Item firstProxyItem = Build.A.JsonProxyItem.WithName(itemsName).WithId("1");
            POEApi.Model.JSONProxy.Item secondProxyItem = Build.A.JsonProxyItem.WithName(itemsName).WithId("2");
            IEnumerable<POEApi.Model.Item> items = new List<POEApi.Model.Item> {
                new Gear(firstProxyItem), new Gear(secondProxyItem) };

            SameNameRecipe twoCopiesRecipe = new SameNameRecipe("Two", 2);
            var matches = twoCopiesRecipe.Matches(items);

            Assert.AreEqual(1, matches.Count());

            var match = matches.First();
            Assert.AreEqual(100m, match.PercentMatch);

            var matchedItems = match.MatchedItems;
            Assert.AreEqual(2, matchedItems.Count);
            Assert.AreEqual(itemsName, matchedItems[0].Name);
            Assert.AreNotEqual(matchedItems[0].Id, matchedItems[1].Id);
        }

        [TestMethod]
        public void Recipe_SameNameRecipe_SameBaseType_ThreeItems()
        {
            const string itemsName = "Alpha Beta";
            POEApi.Model.JSONProxy.Item firstProxyItem = Build.A.JsonProxyItem.WithName(itemsName).WithId("1");
            POEApi.Model.JSONProxy.Item secondProxyItem = Build.A.JsonProxyItem.WithName(itemsName).WithId("2");
            IEnumerable<POEApi.Model.Item> items = new List<POEApi.Model.Item> {
                new Gear(firstProxyItem), new Gear(secondProxyItem) };

            SameNameRecipe threeCopiesRecipe = new SameNameRecipe("Three", 3);
            var matches = threeCopiesRecipe.Matches(items);

            Assert.AreEqual(1, matches.Count());

            var match = matches.First();
            Assert.IsTrue(IsCloseToValue(66.66m, 0.1m, match.PercentMatch));

            var matchedItems = match.MatchedItems;
            Assert.AreEqual(2, matchedItems.Count);
            Assert.AreEqual(itemsName, matchedItems[0].Name);
            Assert.AreNotEqual(matchedItems[0].Id, matchedItems[1].Id);
        }
    }
}