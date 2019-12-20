using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POEApi.Model;
using POEApi.TestHelpers.Builders;
using System.Collections.Generic;
using System.Linq;

namespace Procurement.ViewModel.Recipes.Tests
{
    [TestClass]
    public class VaalOrbRecipeTests
    {
        [TestMethod]
        public void VaalOrbRecipeTests_UncorruptedNonVaalGems_NoMatch()
        {
            List<Item> items = new List<Item>()
            {
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(false).WithId("01").WithTypeLine("Summon Skeletons")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(false).WithId("02").WithTypeLine("Summon Skitterbots")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(false).WithId("03").WithTypeLine("Fireball")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(false).WithId("04").WithTypeLine("Fireball")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(false).WithId("05").WithTypeLine("Fireball")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(false).WithId("06").WithTypeLine("Fireball")),
            };
            items.Should().HaveCount(6);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            VaalOrbRecipe recipe = new VaalOrbRecipe();
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(0);
        }

        [TestMethod]
        public void VaalOrbRecipeTests_CorruptedNonVaalGems_NoMatch()
        {
            List<Item> items = new List<Item>()
            {
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("01").WithTypeLine("Summon Skeletons")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("02").WithTypeLine("Summon Skitterbots")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("03").WithTypeLine("Fireball")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("04").WithTypeLine("Fireball")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("05").WithTypeLine("Fireball")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(false).WithId("06").WithTypeLine("Fireball")),
            };
            items.Should().HaveCount(6);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            VaalOrbRecipe recipe = new VaalOrbRecipe();
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(0);
        }

        [TestMethod]
        public void VaalOrbRecipeTests_VaalGems_FindMatch()
        {
            List<Item> items = new List<Item>()
            {
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("01").WithTypeLine(
                    "Vaal Summon Skeletons")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("02").WithTypeLine(
                    "Vaal Summon Skitterbots")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("03").WithTypeLine("Vaal Fireball")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("04").WithTypeLine("Vaal Fireball")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("05").WithTypeLine("Vaal Fireball")),
            };
            var nonVaalGem = new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(false).WithId("06").WithTypeLine("Fireball"));
            items.Add(nonVaalGem);

            items.Should().HaveCount(6);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            VaalOrbRecipe recipe = new VaalOrbRecipe();
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(1);
            var match = matches.ElementAt(0);
            match.Instance.Should().BeOfType<VaalOrbRecipe>();
            match.IsMatch.Should().BeTrue();

            match.Missing.Should().HaveCount(2);
            match.Missing.Should().Contain("2 Vaal Skill gems");
            match.Missing.Should().Contain("Sacrifice Fragment");

            match.MatchedItems.Should().HaveCount(5);
            match.MatchedItems.Should().NotContain(nonVaalGem);
            match.Name.Should().Be("1 Vaal Orb");
            match.PercentMatch.Should().Be(62.5M);
        }

        [TestMethod]
        public void VaalOrbRecipeTests_NonGems_NoMatch()
        {
            List<Item> items = new List<Item>()
            {
                new Gear(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("01").WithTypeLine(
                    "Vaal Simple Robe")),
                new Gear(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("02").WithTypeLine(
                    "Vaal Sai")),
                new Gear(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("03").WithTypeLine("Vaal Fireball")),
                new Gear(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("04").WithTypeLine("Fireball")),
                new Gear(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("05").WithTypeLine("Simple Robe")),
                new Gear(Build.A.JsonProxyItem.ThatIsCorrupted(false).WithId("06").WithTypeLine("Iron Hat")),
            };
            items.Should().HaveCount(6);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            VaalOrbRecipe recipe = new VaalOrbRecipe();
            var matches = recipe.Matches(items).ToList();
            matches.Should().HaveCount(0);
        }

        [TestMethod]
        public void VaalOrbRecipeTests_OnlyWithSacrificeFrament_NoMatch()
        {
            List<Item> items = new List<Item>()
            {
                new Gear(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("01").WithFrameType(4).WithTypeLine(
                    "Simple Robe")),
                new Currency(Build.A.JsonProxyItem.ThatIsCorrupted(false).WithFrameType(5).WithId("02").WithTypeLine(
                    "Sacrifice at Midnight")),
            };
            items.Should().HaveCount(2);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            VaalOrbRecipe recipe = new VaalOrbRecipe();
            var matches = recipe.Matches(items).ToList();
            matches.Should().HaveCount(0);
        }

        [TestMethod]
        public void VaalOrbRecipeTests_FullSet_CompleteMatch()
        {
            List<Item> items = new List<Item>()
            {
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("01").WithTypeLine(
                    "Vaal Summon Skeletons")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("02").WithTypeLine(
                    "Vaal Summon Skitterbots")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("03").WithTypeLine("Vaal Fireball")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("04").WithTypeLine("Vaal Fireball")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("05").WithTypeLine("Vaal Fireball")),
                new Currency(Build.A.JsonProxyItem.ThatIsCorrupted(false).WithId("06").WithTypeLine(
                    "Sacrifice at Midnight")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("07").WithTypeLine("Vaal Detonate Dead")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("08").WithTypeLine("Vaal Spark")),
            };


            items.Should().HaveCount(8);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            VaalOrbRecipe recipe = new VaalOrbRecipe();
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(1);
            var match = matches.ElementAt(0);
            match.Instance.Should().BeOfType<VaalOrbRecipe>();
            match.IsMatch.Should().BeTrue();

            match.Missing.Should().HaveCount(0);

            match.MatchedItems.Should().HaveCount(8);
            match.MatchedItems.Should().Contain(items);
            match.Name.Should().Be("1 Vaal Orb");
            match.PercentMatch.Should().Be(100M);
        }

        [TestMethod]
        public void VaalOrbRecipeTests_OtherFragmentType_NotIncluded()
        {
            List<Item> items = new List<Item>()
            {
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("01").WithTypeLine(
                    "Vaal Summon Skeletons")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("02").WithTypeLine(
                    "Vaal Summon Skitterbots")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("03").WithTypeLine("Vaal Fireball")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("04").WithTypeLine("Vaal Fireball")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("05").WithTypeLine("Vaal Fireball")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("07").WithTypeLine("Vaal Detonate Dead")),
                new Gem(Build.A.JsonProxyItem.ThatIsCorrupted(true).WithId("08").WithTypeLine("Vaal Spark")),
            };
            Item wrongFragment = new Currency(Build.A.JsonProxyItem.ThatIsCorrupted(false).WithId("06").WithTypeLine(
                "Fragment of the Hydra"));
            items.Add(wrongFragment);

            items.Should().HaveCount(8);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            VaalOrbRecipe recipe = new VaalOrbRecipe();
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(1);
            var match = matches.ElementAt(0);
            match.Instance.Should().BeOfType<VaalOrbRecipe>();
            match.IsMatch.Should().BeTrue();

            match.Missing.Should().HaveCount(1);
            match.Missing.Should().Contain("Sacrifice Fragment");

            match.MatchedItems.Should().HaveCount(7);
            match.MatchedItems.Should().NotContain(wrongFragment);
            match.Name.Should().Be("1 Vaal Orb");
            match.PercentMatch.Should().Be(87.5M);
        }
    }
}
