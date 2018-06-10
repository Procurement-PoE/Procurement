using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POEApi.Model;
using POEApi.TestHelpers.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.ViewModel.Recipes.Tests
{
    [TestClass]
    public class SameBaseTypeRecipeTests
    {
        static protected Dictionary<bool, List<int>> _qualityLists;

        [TestInitialize]
        public void SetUp()
        {
            _qualityLists = new Dictionary<bool, List<int>>()
            {
                { false, new List<int>() { 19, 2, 0, 5 } },
                { true, new List<int>() { 20, 20, 20, 20 } },
            };
        }

        [TestMethod]
        public void SameBaseTypeRecipeTests_EmptyInput_NoMatches()
        {
            foreach (var strictConditions in new List<bool>() { true, false })
            {
                List<Item> items = new List<Item>();
                items.Should().HaveCount(0);

                SameBaseTypeRecipe recipe = new SameBaseTypeRecipe(60, strictConditions);
                var matches = recipe.Matches(items).ToList();

                matches.Should().NotBeNull();
                matches.Should().HaveCount(0);
            }
        }

        [TestMethod]
        public void SameBaseTypeRecipeTests_OneItem_NoMatches()
        {
            foreach (var strictConditions in new List<bool>() { true, false })
                foreach (var identifiedState in new List<bool>() { true, false })
                    foreach (var qualityList in _qualityLists)
                    {
                        List<Item> items = new List<Item>()
                        {
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Magic).WithId("01")
                                .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[0])
                                .ThatIsIdentified(identifiedState)),
                        };
                        items.Should().HaveCount(1);
                        items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

                        SameBaseTypeRecipe recipe = new SameBaseTypeRecipe(60, strictConditions);
                        var matches = recipe.Matches(items).ToList();

                        matches.Should().NotBeNull();
                        matches.Should().HaveCount(0);
                    }
        }

        [TestMethod]
        public void SameBaseTypeRecipeTests_TwoItems_PartialMatch()
        {
            foreach (var strictConditions in new List<bool>() { true, false })
                foreach (var identifiedState in new List<bool>() { true, false })
                    foreach (var qualityList in _qualityLists)
                    {
                        List<Item> items = new List<Item>()
                        {
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Normal).WithId("01")
                                .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[0])),
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Magic).WithId("02")
                                .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[1])
                                .ThatIsIdentified(identifiedState)),
                        };
                        items.Should().HaveCount(2);
                        items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

                        SameBaseTypeRecipe recipe = new SameBaseTypeRecipe(60, strictConditions);
                        var matches = recipe.Matches(items).ToList();

                        matches.Should().NotBeNull();
                        matches.Should().HaveCount(1);

                        var match = matches.ElementAt(0);
                        match.Instance.Should().BeOfType<SameBaseTypeRecipe>();
                        match.IsMatch.Should().BeTrue();
                        match.Missing.Should().HaveCount(1);
                        match.Missing[0].Should().Be("Item with Rare rarity");
                        match.MatchedItems.Should().HaveCount(2);
                        match.PercentMatch.Should().BeApproximately(200M / 3M, 0.001M);

                        if (identifiedState)
                        {
                            if (qualityList.Key)
                                match.Name.Should().Be(
                                    "1 Orb of Alchemy - Same base type with normal, magic, rare, 20% quality");
                            else
                                match.Name.Should().Be(
                                    "1 Orb of Augmentation - Same base type with normal, magic, and rare");
                        }
                        else
                        {
                            if (qualityList.Key)
                                match.Name.Should().Be(
                                    "2 Orbs of Alchemy - Same base type with normal, magic, rare, 20% quality and " +
                                    "unidentified");
                            else
                                match.Name.Should().Be(
                                    "2 Orbs of Augmentation - Same base type with normal, magic, rare, unidentified");
                        }
                    }
        }

        [TestMethod]
        public void SameBaseTypeRecipeTests_ThreeItems_FullMatch()
        {
            foreach (var strictConditions in new List<bool>() { true, false })
                foreach (var identifiedState in new List<bool>() { true, false })
                    foreach (var qualityList in _qualityLists)
                    {
                        List<Item> items = new List<Item>()
                        {
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Normal).WithId("01")
                                .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[0])),
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Magic).WithId("02")
                                .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[1])
                                .ThatIsIdentified(identifiedState)),
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithId("03")
                                .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[2])
                                .ThatIsIdentified(identifiedState)),
                        };
                        items.Should().HaveCount(3);
                        items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

                        SameBaseTypeRecipe recipe = new SameBaseTypeRecipe(60, strictConditions);
                        var matches = recipe.Matches(items).ToList();

                        matches.Should().NotBeNull();
                        matches.Should().HaveCount(1);

                        var match = matches.ElementAt(0);
                        match.Instance.Should().BeOfType<SameBaseTypeRecipe>();
                        match.IsMatch.Should().BeTrue();
                        match.Missing.Should().HaveCount(0);
                        match.MatchedItems.Should().HaveCount(3);
                        match.PercentMatch.Should().Be(100M);

                        if (identifiedState)
                        {
                            if (qualityList.Key)
                                match.Name.Should().Be(
                                    "1 Orb of Alchemy - Same base type with normal, magic, rare, 20% quality");
                            else
                                match.Name.Should().Be(
                                    "1 Orb of Augmentation - Same base type with normal, magic, and rare");
                        }
                        else
                        {
                            if (qualityList.Key)
                                match.Name.Should().Be(
                                    "2 Orbs of Alchemy - Same base type with normal, magic, rare, 20% quality and " +
                                    "unidentified");
                            else
                                match.Name.Should().Be(
                                    "2 Orbs of Augmentation - Same base type with normal, magic, rare, unidentified");
                        }
                    }
        }

        [TestMethod]
        public void SameBaseTypeRecipeTests_ThreeItemsDifferentBaseType_PartialMatch()
        {
            foreach (var strictConditions in new List<bool>() { true, false })
                foreach (var identifiedState in new List<bool>() { true, false })
                    foreach (var qualityList in _qualityLists)
                    {
                        List<Item> items = new List<Item>()
                        {
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Normal).WithId("01")
                                .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[0])),
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithId("03")
                                .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[1])
                                .ThatIsIdentified(identifiedState)),
                        };
                        var otherBaseType = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Magic)
                            .WithId("02").WithTypeLine("Battered Hat").WithQuality(qualityList.Value[2])
                            .ThatIsIdentified(identifiedState));
                        items.Add(otherBaseType);

                        items.Should().HaveCount(3);
                        items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

                        SameBaseTypeRecipe recipe = new SameBaseTypeRecipe(60, strictConditions);
                        var matches = recipe.Matches(items).ToList();

                        matches.Should().NotBeNull();
                        matches.Should().HaveCount(1);

                        var match = matches.ElementAt(0);
                        match.Instance.Should().BeOfType<SameBaseTypeRecipe>();
                        match.IsMatch.Should().BeTrue();
                        match.Missing.Should().HaveCount(1);
                        match.Missing[0].Should().Be("Item with Magic rarity");
                        match.MatchedItems.Should().HaveCount(2);
                        match.MatchedItems.Should().NotContain(otherBaseType);
                        match.PercentMatch.Should().BeApproximately(200M / 3M, 0.001M);

                        if (identifiedState)
                        {
                            if (qualityList.Key)
                                match.Name.Should().Be(
                                    "1 Orb of Alchemy - Same base type with normal, magic, rare, 20% quality");
                            else
                                match.Name.Should().Be(
                                    "1 Orb of Augmentation - Same base type with normal, magic, and rare");
                        }
                        else
                        {
                            if (qualityList.Key)
                                match.Name.Should().Be(
                                    "2 Orbs of Alchemy - Same base type with normal, magic, rare, 20% quality and " +
                                    "unidentified");
                            else
                                match.Name.Should().Be(
                                    "2 Orbs of Augmentation - Same base type with normal, magic, rare, unidentified");
                        }
                    }
        }

        [TestMethod]
        public void SameBaseTypeRecipeTests_MultipleItemsSameRarity_OnlyOneOfEachRarity()
        {
            foreach (var strictConditions in new List<bool>() { true, false })
                foreach (var identifiedState in new List<bool>() { true, false })
                    foreach (var qualityList in _qualityLists)
                    {
                        List<Item> items = new List<Item>()
                        {
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Normal).WithId("01")
                                .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[0])),
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Normal).WithId("02")
                                .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[1])),
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Magic).WithId("03")
                                .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[2])
                                .ThatIsIdentified(identifiedState)),
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithId("04")
                                .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[3])
                                .ThatIsIdentified(identifiedState)),
                        };
                        items.Should().HaveCount(4);
                        items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

                        SameBaseTypeRecipe recipe = new SameBaseTypeRecipe(60, strictConditions);
                        var matches = recipe.Matches(items).ToList();

                        matches.Should().NotBeNull();
                        matches.Should().HaveCount(1);

                        var match = matches.ElementAt(0);
                        match.Instance.Should().BeOfType<SameBaseTypeRecipe>();
                        match.IsMatch.Should().BeTrue();
                        match.Missing.Should().HaveCount(0);
                        match.MatchedItems.Should().HaveCount(3);
                        match.MatchedItems.Select(i => (i as Gear).Rarity).Should().OnlyHaveUniqueItems();
                        match.PercentMatch.Should().Be(100M);

                        if (identifiedState)
                        {
                            if (qualityList.Key)
                                match.Name.Should().Be(
                                    "1 Orb of Alchemy - Same base type with normal, magic, rare, 20% quality");
                            else
                                match.Name.Should().Be(
                                    "1 Orb of Augmentation - Same base type with normal, magic, and rare");
                        }
                        else
                        {
                            if (qualityList.Key)
                                match.Name.Should().Be(
                                    "2 Orbs of Alchemy - Same base type with normal, magic, rare, 20% quality and " +
                                    "unidentified");
                            else
                                match.Name.Should().Be(
                                    "2 Orbs of Augmentation - Same base type with normal, magic, rare, unidentified");
                        }
                    }
        }

        [TestMethod]
        public void SameBaseTypeRecipeTests_ThreeItemsSomeFullQuality_FindBestMatch()
        {
            foreach (var strictConditions in new List<bool>() { true, false })
                foreach (var identifiedState in new List<bool>() { true, false })
                {
                    List<Item> items = new List<Item>()
                    {
                        new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Normal).WithId("01")
                            .WithTypeLine("Iron Hat")),
                        new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Magic).WithId("02")
                            .WithTypeLine("Iron Hat").WithQuality(20)
                            .ThatIsIdentified(identifiedState)),
                        new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithId("03")
                            .WithTypeLine("Iron Hat").WithQuality(10)
                            .ThatIsIdentified(identifiedState)),
                    };
                    items.Should().HaveCount(3);
                    items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

                    SameBaseTypeRecipe recipe = new SameBaseTypeRecipe(60, strictConditions);
                    var matches = recipe.Matches(items).ToList();

                    matches.Should().NotBeNull();
                    matches.Should().HaveCount(1);

                    var match = matches.ElementAt(0);
                    match.Instance.Should().BeOfType<SameBaseTypeRecipe>();
                    match.IsMatch.Should().BeTrue();
                    if (strictConditions)
                    {
                        // The magic item is not included because it has 20% quality, but the variant being searched
                        // for requires quality != 20%.
                        match.Missing.Should().HaveCount(1);
                        match.Missing[0].Should().Be("Item with Magic rarity");
                        match.MatchedItems.Should().HaveCount(2);
                        match.PercentMatch.Should().BeApproximately(200M / 3M, 0.001M);
                    }
                    else
                    {
                        match.Missing.Should().HaveCount(0);
                        match.MatchedItems.Should().HaveCount(3);
                        match.PercentMatch.Should().Be(100M);
                    }

                    if (identifiedState)
                    {
                        match.Name.Should().Be(
                            "1 Orb of Augmentation - Same base type with normal, magic, and rare");
                    }
                    else
                    {
                        match.Name.Should().Be(
                            "2 Orbs of Augmentation - Same base type with normal, magic, rare, unidentified");
                    }
                }
        }

        [TestMethod]
        public void SameBaseTypeRecipeTests_ThreeItemsSomeIdentified_FindBestMatch()
        {
            foreach (var strictConditions in new List<bool>() { true, false })
                foreach (var qualityList in _qualityLists)
                {
                    List<Item> items = new List<Item>()
                    {
                        new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Normal).WithId("01")
                            .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[0])),
                        new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithId("03")
                            .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[2])
                            .ThatIsIdentified(false)),
                    };
                    var identifiedItem = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Magic).WithId("02")
                        .WithTypeLine("Iron Hat").WithQuality(qualityList.Value[1]).ThatIsIdentified(true));
                    items.Add(identifiedItem);

                    items.Should().HaveCount(3);
                    items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

                    SameBaseTypeRecipe recipe = new SameBaseTypeRecipe();
                    var matches = recipe.Matches(items).ToList();

                    matches.Should().NotBeNull();
                    matches.Should().HaveCount(1);

                    var match = matches.ElementAt(0);
                    match.Instance.Should().BeOfType<SameBaseTypeRecipe>();
                    match.IsMatch.Should().BeTrue();
                    match.Missing.Should().HaveCount(1);
                    match.Missing[0].Should().Be("Item with Magic rarity");
                    match.MatchedItems.Should().HaveCount(2);
                    match.MatchedItems.Should().NotContain(identifiedItem);
                    match.PercentMatch.Should().BeApproximately(200M / 3M, 0.001M);

                    // When not using strict conditions, we still end up with the unidentified variant, since normal-
                    // quality items count towards the unidentified recipe, and thus we have more than the minimum
                    // match percentage with the normal and rare items.  The identified variant is never considered,
                    // since only one item is left, which does not meet the minimum match percentage.

                    if (qualityList.Key)
                        match.Name.Should().Be(
                            "2 Orbs of Alchemy - Same base type with normal, magic, rare, 20% quality and " +
                            "unidentified");
                    else
                        match.Name.Should().Be(
                            "2 Orbs of Augmentation - Same base type with normal, magic, rare, unidentified");
                }
        }

        [TestMethod]
        public void SameBaseTypeRecipeTests_InvalidGearType_DoNotUse()
        {
            foreach (var strictConditions in new List<bool>() { true, false })
                foreach (var identifiedState in new List<bool>() { true, false })
                    foreach (var qualityList in _qualityLists)
                    {
                        List<Item> items = new List<Item>()
                        {
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Normal).WithId("01")
                                .WithTypeLine("Small Life Flask").WithQuality(qualityList.Value[0])),
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Magic).WithId("02")
                                .WithTypeLine("Small Life Flask").WithQuality(qualityList.Value[1])
                                .ThatIsIdentified(identifiedState)),
                            new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithId("03")
                                .WithTypeLine("Small Life Flask").WithQuality(qualityList.Value[2])
                                .ThatIsIdentified(identifiedState)),
                        };
                        items.Should().HaveCount(3);
                        items.Select(i => i.Id).Should().OnlyHaveUniqueItems();
                        items.Select(i => (i as Gear).GearType).ShouldAllBeEquivalentTo(GearType.Flask);

                        SameBaseTypeRecipe recipe = new SameBaseTypeRecipe(60, strictConditions);
                        var matches = recipe.Matches(items).ToList();

                        matches.Should().NotBeNull();
                        matches.Should().HaveCount(0);
                    }
        }
    }
}
