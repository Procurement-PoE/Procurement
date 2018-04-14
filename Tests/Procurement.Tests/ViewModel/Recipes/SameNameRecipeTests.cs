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

namespace Procurement.ViewModel.Recipes.Tests
{
    [TestClass]
    public class SameNameRecipeTests
    {
        public bool IsCloseToValue(decimal expectedValue, decimal epsilon, decimal actualValue)
        {
            decimal difference = Math.Abs(expectedValue - actualValue);
            return difference < epsilon;
        }

        public void CheckTwoItemRecipe(List<POEApi.Model.Item> potentialItems, string name)
        {
            Assert.AreEqual(7, potentialItems.Count);
            List<POEApi.Model.Item> currentItems = new List<POEApi.Model.Item>();

            {
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe twoCopiesRecipe = new SameNameRecipe("Two", 2);
                var matches = twoCopiesRecipe.Matches(currentItems);
                Assert.AreEqual(0, matches.Count());
            }

            {
                // Add a non-matching item.
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe twoCopiesRecipe = new SameNameRecipe("Two", 2);
                var matches = twoCopiesRecipe.Matches(currentItems);
                Assert.AreEqual(0, matches.Count());
            }

            {
                // Add non-matching item with name containing matching name.
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe twoCopiesRecipe = new SameNameRecipe("Two", 2);
                var matches = twoCopiesRecipe.Matches(currentItems);
                Assert.AreEqual(0, matches.Count());
            }

            {
                // Add non-matching item with name that is a substring of the matching name.
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe twoCopiesRecipe = new SameNameRecipe("Two", 2);
                var matches = twoCopiesRecipe.Matches(currentItems);
                Assert.AreEqual(0, matches.Count());
            }

            {
                // Add a second matching item.
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe twoCopiesRecipe = new SameNameRecipe("Two", 2);
                var matches = twoCopiesRecipe.Matches(currentItems);

                Assert.AreEqual(1, matches.Count());
                var match = matches.First();
                Assert.AreEqual(100m, match.PercentMatch);
                Assert.AreEqual(0, match.Missing.Count);
                Assert.IsTrue(match.IsMatch);

                var matchedItems = match.MatchedItems;
                Assert.AreEqual(2, matchedItems.Count);
                Assert.AreEqual(name, matchedItems[0].Name);
                Assert.AreNotEqual(matchedItems[0].Id, matchedItems[1].Id);
            }

            {
                // Add a third matching item.
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe twoCopiesRecipe = new SameNameRecipe("Two", 2);
                var matches = twoCopiesRecipe.Matches(currentItems);

                Assert.AreEqual(1, matches.Count());
                var match = matches.First();
                Assert.AreEqual(100m, match.PercentMatch);
                Assert.AreEqual(0, match.Missing.Count);
                Assert.IsTrue(match.IsMatch);

                var matchedItems = match.MatchedItems;
                Assert.AreEqual(2, matchedItems.Count);
                Assert.AreEqual(name, matchedItems[0].Name);
                Assert.AreNotEqual(matchedItems[0].Id, matchedItems[1].Id);
            }

            {
                // Add a fourth matching item.
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe twoCopiesRecipe = new SameNameRecipe("Two", 2);
                var matches = twoCopiesRecipe.Matches(currentItems);
                Assert.AreEqual(2, matches.Count());

                var firstMmatch = matches.First();
                Assert.AreEqual(100m, firstMmatch.PercentMatch);
                Assert.AreEqual(0, firstMmatch.Missing.Count);
                Assert.IsTrue(firstMmatch.IsMatch);

                var firstMatchedItems = firstMmatch.MatchedItems;
                Assert.AreEqual(2, firstMatchedItems.Count);
                Assert.AreEqual(name, firstMatchedItems[0].Name);
                Assert.AreNotEqual(firstMatchedItems[0].Id, firstMatchedItems[1].Id);

                var secondMatch = matches.ElementAt(1);
                Assert.AreEqual(100m, secondMatch.PercentMatch);
                Assert.AreEqual(0, secondMatch.Missing.Count);
                Assert.IsTrue(secondMatch.IsMatch);

                var secondMatchedItems = secondMatch.MatchedItems;
                Assert.AreEqual(2, secondMatchedItems.Count);
                Assert.AreEqual(name, secondMatchedItems[0].Name);
                Assert.AreNotEqual(secondMatchedItems[0].Id, secondMatchedItems[1].Id);

                Assert.AreNotEqual(firstMatchedItems[0].Id, secondMatchedItems[0].Id);
                Assert.AreNotEqual(firstMatchedItems[0].Id, secondMatchedItems[1].Id);
                Assert.AreNotEqual(firstMatchedItems[1].Id, secondMatchedItems[0].Id);
                Assert.AreNotEqual(firstMatchedItems[1].Id, secondMatchedItems[1].Id);
            }
        }

        public void CheckThreeItemRecipe(List<POEApi.Model.Item> potentialItems, string name)
        {
            Assert.AreEqual(8, potentialItems.Count);
            List<POEApi.Model.Item> currentItems = new List<POEApi.Model.Item>();

            {
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe threeCopiesRecipe = new SameNameRecipe("Three", 3);
                var matches = threeCopiesRecipe.Matches(currentItems);
                Assert.AreEqual(0, matches.Count());
            }

            {
                // Add a non-matching item.
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe threeCopiesRecipe = new SameNameRecipe("Three", 3);
                var matches = threeCopiesRecipe.Matches(currentItems);
                Assert.AreEqual(0, matches.Count());
            }

            {
                // Add non-matching item with name containing matching name.
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe threeCopiesRecipe = new SameNameRecipe("Three", 3);
                var matches = threeCopiesRecipe.Matches(currentItems);
                Assert.AreEqual(0, matches.Count());
            }

            {
                // Add non-matching item with name that is a substring of the matching name.
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe threeCopiesRecipe = new SameNameRecipe("Three", 3);
                var matches = threeCopiesRecipe.Matches(currentItems);
                Assert.AreEqual(0, matches.Count());
            }

            {
                // Add a second matching item.
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe threeCopiesRecipe = new SameNameRecipe("Three", 3);
                var matches = threeCopiesRecipe.Matches(currentItems);

                Assert.AreEqual(1, matches.Count());
                var match = matches.First();
                Assert.IsTrue(IsCloseToValue(66.66m, 0.1m, match.PercentMatch));
                Assert.AreEqual(0, match.Missing.Count);
                Assert.IsTrue(match.IsMatch);

                var matchedItems = match.MatchedItems;
                Assert.AreEqual(2, matchedItems.Count);
                Assert.AreEqual(name, matchedItems[0].Name);
                Assert.AreNotEqual(matchedItems[0].Id, matchedItems[1].Id);
            }

            {
                // Add a third matching item.
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe threeCopiesRecipe = new SameNameRecipe("Three", 3);
                var matches = threeCopiesRecipe.Matches(currentItems);

                Assert.AreEqual(1, matches.Count());
                var match = matches.First();
                Assert.AreEqual(100m, match.PercentMatch);
                Assert.AreEqual(0, match.Missing.Count);
                Assert.IsTrue(match.IsMatch);

                var matchedItems = match.MatchedItems;
                Assert.AreEqual(3, matchedItems.Count);
                Assert.AreEqual(name, matchedItems[0].Name);
                Assert.AreNotEqual(matchedItems[0].Id, matchedItems[1].Id);
                Assert.AreNotEqual(matchedItems[0].Id, matchedItems[2].Id);
                Assert.AreNotEqual(matchedItems[1].Id, matchedItems[2].Id);
            }

            {
                // Add a fourth matching item.
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe threeCopiesRecipe = new SameNameRecipe("Three", 3);
                var matches = threeCopiesRecipe.Matches(currentItems);

                Assert.AreEqual(1, matches.Count());
                var match = matches.First();
                Assert.AreEqual(100m, match.PercentMatch);
                Assert.AreEqual(0, match.Missing.Count);
                Assert.IsTrue(match.IsMatch);

                var matchedItems = match.MatchedItems;
                Assert.AreEqual(3, matchedItems.Count);
                Assert.AreEqual(name, matchedItems[0].Name);
                Assert.AreNotEqual(matchedItems[0].Id, matchedItems[1].Id);
                Assert.AreNotEqual(matchedItems[0].Id, matchedItems[2].Id);
                Assert.AreNotEqual(matchedItems[1].Id, matchedItems[2].Id);
            }

            {
                // Add a fifth matching item.
                currentItems.Add(potentialItems[0]);
                potentialItems.RemoveAt(0);

                SameNameRecipe threeCopiesRecipe = new SameNameRecipe("Three", 3);
                var matches = threeCopiesRecipe.Matches(currentItems);

                Assert.AreEqual(2, matches.Count());
                var firstMatch = matches.First();
                Assert.AreEqual(100m, firstMatch.PercentMatch);
                Assert.AreEqual(0, firstMatch.Missing.Count);
                Assert.IsTrue(firstMatch.IsMatch);

                var firstMatchedItems = firstMatch.MatchedItems;
                Assert.AreEqual(3, firstMatchedItems.Count);
                Assert.AreEqual(name, firstMatchedItems[0].Name);
                Assert.AreNotEqual(firstMatchedItems[0].Id, firstMatchedItems[1].Id);
                Assert.AreNotEqual(firstMatchedItems[0].Id, firstMatchedItems[2].Id);
                Assert.AreNotEqual(firstMatchedItems[1].Id, firstMatchedItems[2].Id);

                var secondMatch = matches.ElementAt(1);
                Assert.IsTrue(IsCloseToValue(66.66m, 0.1m, secondMatch.PercentMatch));
                Assert.AreEqual(0, secondMatch.Missing.Count);
                Assert.IsTrue(secondMatch.IsMatch);

                var secondMatchedItems = secondMatch.MatchedItems;
                Assert.AreEqual(2, secondMatchedItems.Count);
                Assert.AreEqual(name, secondMatchedItems[0].Name);
                Assert.AreNotEqual(secondMatchedItems[0].Id, secondMatchedItems[1].Id);

                Assert.AreNotEqual(firstMatchedItems[0].Id, secondMatchedItems[0].Id);
                Assert.AreNotEqual(firstMatchedItems[0].Id, secondMatchedItems[1].Id);
                Assert.AreNotEqual(firstMatchedItems[1].Id, secondMatchedItems[0].Id);
                Assert.AreNotEqual(firstMatchedItems[1].Id, secondMatchedItems[1].Id);
                Assert.AreNotEqual(firstMatchedItems[2].Id, secondMatchedItems[0].Id);
                Assert.AreNotEqual(firstMatchedItems[2].Id, secondMatchedItems[1].Id);
            }
        }

        [TestMethod]
        public void SameNameRecipeTests_Gear()
        {
            const string itemsName = "Alpha Beta";
            List<POEApi.Model.Item> items = new List<POEApi.Model.Item>
            {
                // First matching item.
                new Gear(Build.A.JsonProxyItem.WithName(itemsName).WithId("1")),
                // Random unrelated item.
                new Gear(Build.A.JsonProxyItem.WithName("Gamma").WithId("10")),
                // Unrelated item with a name containing the matching name.
                new Gear(Build.A.JsonProxyItem.WithName(itemsName + " Bane").WithId("11")),
                // Unrelated item with a name that is a substring of the matching name.
                new Gear(Build.A.JsonProxyItem.WithName("Alpha").WithId("12")),
                // Second matching item.
                new Gear(Build.A.JsonProxyItem.WithName(itemsName).WithId("2")),
                // Third matching item.
                new Gear(Build.A.JsonProxyItem.WithName(itemsName).WithId("3")),
                // Fourth matching item.
                new Gear(Build.A.JsonProxyItem.WithName(itemsName).WithId("4")),
            };

            CheckTwoItemRecipe(new List<POEApi.Model.Item>(items), itemsName);

            items.Add(new Gear(Build.A.JsonProxyItem.WithName(itemsName).WithId("5")));
            CheckThreeItemRecipe(new List<POEApi.Model.Item>(items), itemsName);
        }

        [TestMethod]
        public void SameNameRecipeTests_AbyssJewel()
        {
            const string itemsName = "Alpha Beta";
            List<POEApi.Model.Item> items = new List<POEApi.Model.Item>
            {
                // First matching item.
                new AbyssJewel(Build.A.JsonProxyItem.WithName(itemsName).WithId("1")),
                // Random unrelated item.
                new AbyssJewel(Build.A.JsonProxyItem.WithName("Gamma").WithId("10")),
                // Unrelated item with a name containing the matching name.
                new AbyssJewel(Build.A.JsonProxyItem.WithName(itemsName + " Bane").WithId("11")),
                // Unrelated item with a name that is a substring of the matching name.
                new AbyssJewel(Build.A.JsonProxyItem.WithName("Alpha").WithId("12")),
                // Second matching item.
                new AbyssJewel(Build.A.JsonProxyItem.WithName(itemsName).WithId("2")),
                // Third matching item.
                new AbyssJewel(Build.A.JsonProxyItem.WithName(itemsName).WithId("3")),
                // Fourth matching item.
                new AbyssJewel(Build.A.JsonProxyItem.WithName(itemsName).WithId("4")),
            };

            CheckTwoItemRecipe(new List<POEApi.Model.Item>(items), itemsName);

            items.Add(new AbyssJewel(Build.A.JsonProxyItem.WithName(itemsName).WithId("5")));
            CheckThreeItemRecipe(new List<POEApi.Model.Item>(items), itemsName);
        }

        [TestMethod]
        public void SameNameRecipeTests_MixedTypes()
        {
            const string itemsName = "Alpha Beta";
            List<POEApi.Model.Item> items = new List<POEApi.Model.Item>
            {
                // First matching item.
                new Gear(Build.A.JsonProxyItem.WithName(itemsName).WithId("1")),
                // Random unrelated item.
                new Gear(Build.A.JsonProxyItem.WithName("Gamma").WithId("10")),
                // Unrelated item with a name containing the matching name.
                new AbyssJewel(Build.A.JsonProxyItem.WithName(itemsName + " Bane").WithId("11")),
                // Unrelated item with a name that is a substring of the matching name.
                new AbyssJewel(Build.A.JsonProxyItem.WithName("Alpha").WithId("12")),
                // Second matching item.
                new AbyssJewel(Build.A.JsonProxyItem.WithName(itemsName).WithId("2")),
                // Third matching item.
                new Gear(Build.A.JsonProxyItem.WithName(itemsName).WithId("3")),
                // Fourth matching item.
                new AbyssJewel(Build.A.JsonProxyItem.WithName(itemsName).WithId("4")),
            };

            CheckTwoItemRecipe(new List<POEApi.Model.Item>(items), itemsName);

            items.Add(new Gear(Build.A.JsonProxyItem.WithName(itemsName).WithId("5")));
            CheckThreeItemRecipe(new List<POEApi.Model.Item>(items), itemsName);
        }

        [TestMethod]
        public void SameNameRecipeTests_MultipleResults()
        {
            const string firstResultName = "Alpha Beta";
            const string secondResultName = "Gamma Delta";
            const string unrelatedName = "Epsilon Phi";
            List<POEApi.Model.Item> items = new List<POEApi.Model.Item>
            {
                new Gear(Build.A.JsonProxyItem.WithName(firstResultName).WithId("1")),
                new Gear(Build.A.JsonProxyItem.WithName(unrelatedName).WithId("2")),
                new Gear(Build.A.JsonProxyItem.WithName(secondResultName).WithId("3")),
                new Gear(Build.A.JsonProxyItem.WithName(firstResultName).WithId("4")),
                new Gear(Build.A.JsonProxyItem.WithName(firstResultName).WithId("5")),
                new Gear(Build.A.JsonProxyItem.WithName(secondResultName).WithId("6")),
            };

            SameNameRecipe threeCopiesRecipe = new SameNameRecipe("Three", 3);
            var matches = threeCopiesRecipe.Matches(items);

            Assert.AreEqual(2, matches.Count());
            Assert.IsTrue(matches.All(r => r.MatchedItems.Count > 0));

            var firstMatch = matches.FirstOrDefault(r => r.MatchedItems[0].Name == firstResultName);
            Assert.IsNotNull(firstMatch);
            Assert.AreEqual(3, firstMatch.MatchedItems.Count);
            Assert.IsTrue(firstMatch.MatchedItems.All(i => i.Name == firstResultName));

            var secondMatch = matches.FirstOrDefault(r => r.MatchedItems[1].Name == secondResultName);
            Assert.IsNotNull(secondMatch);
            Assert.AreEqual(2, secondMatch.MatchedItems.Count);
            Assert.IsTrue(secondMatch.MatchedItems.All(i => i.Name == secondResultName));
        }
    }
}