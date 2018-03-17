using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POEApi.Model;
using POEApi.TestHelpers.Builders;
using Procurement.ViewModel.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.ViewModel.Recipes.Tests
{
    [TestClass]
    public class RareSetRecipeTests
    {
        [TestMethod]
        public void RareSetRecipeTests_BasicFullSet_CompleteMatch()
        {
            // TODO: Switch to using Moq and mock the Gear objects.  We don't need to go through all this extra
            // stuff to create the JsonProxyItem objects.

            /*
            GearType[] types = { GearType.Helmet, GearType.Chest, GearType.Belt, GearType.Gloves, GearType.Amulet,
                GearType.Ring, GearType.Ring, GearType.Sceptre, GearType.Shield };
            List<POEApi.Model.Item> items = new List<POEApi.Model.Item>();
            for (int i = 0; i < types.Count(); i++)
            {
                Gear gear = new Gear(Build.A.JsonProxyItem.WithId(i.ToString()));
                gear.Rarity = Rarity.Rare;  // Does not work - private setter.
                items.Add(gear);
            }
            */

            List<Item> items = new List<Item>()
            {
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("01")
                    .WithTypeLine("Iron Hat")),  // Helmet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("02")
                    .WithTypeLine("Plate Vest")),  // Body Armor
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("03")
                    .WithTypeLine("Rustic Sash")),  // Belt
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("04")
                    .WithTypeLine("Iron Gauntlets")),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("05")
                    .WithTypeLine("Iron Greaves")),  // Boots
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("06")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("07")
                    .WithTypeLine("Iron Ring")),  // Ring 1
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("08")
                    .WithTypeLine("Coral Ring")),  // Ring 2
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("09")
                    .WithTypeLine("Driftwood Sceptre")),  // Weapon
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("10")
                    .WithTypeLine("Splintered Tower Shield")),  // Offhand
            };
            items.Should().HaveCount(10);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            RareSetRecipe recipe = new RareSetRecipe(1, 100, false, "set");
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(1);
            var match = matches.ElementAt(0);
            match.Instance.Should().BeOfType<RareSetRecipe>();
            match.IsMatch.Should().BeTrue();
            match.Missing.Should().BeEmpty();
            match.MatchedItems.Should().HaveCount(10);
            match.MatchedItems.Should().Contain(items);
            match.Name.Should().Be("set");
            match.PercentMatch.Should().Be(100M);
        }

        [TestMethod]
        public void RareSetRecipeTests_FullSetDuplicateBaseTypes_CompleteMatch()
        {
            List<Item> items = new List<Item>()
            {
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("01")
                    .WithTypeLine("Iron Hat")),  // Helmet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("02")
                    .WithTypeLine("Plate Vest")),  // Body Armor
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("03")
                    .WithTypeLine("Rustic Sash")),  // Belt
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("04")
                    .WithTypeLine("Iron Gauntlets")),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("05")
                    .WithTypeLine("Iron Greaves")),  // Boots
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("06")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("07")
                    .WithTypeLine("Iron Ring")),  // Ring 1
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("08")
                    .WithTypeLine("Iron Ring")),  // Ring 2
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("09")
                    .WithTypeLine("Driftwood Sceptre")),  // Weapon
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("10")
                    .WithTypeLine("Driftwood Sceptre")),  // Offhand
            };
            items.Should().HaveCount(10);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            RareSetRecipe recipe = new RareSetRecipe(1, 100, false, "set");
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(1);
            var match = matches.ElementAt(0);
            match.Instance.Should().BeOfType<RareSetRecipe>();
            match.IsMatch.Should().BeTrue();
            match.Missing.Should().BeEmpty();
            match.MatchedItems.Should().HaveCount(10);
            match.MatchedItems.Should().Contain(items);
            match.Name.Should().Be("set");
            match.PercentMatch.Should().Be(100M);
        }

        [TestMethod]
        public void RareSetRecipeTests_PartialSet_IncompleteMatch()
        {
            List<Item> items = new List<Item>()
            {
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("01")
                    .WithTypeLine("Iron Hat")),  // Helmet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("02")
                    .WithTypeLine("Plate Vest")),  // Body Armor
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("03")
                    .WithTypeLine("Rustic Sash")),  // Belt
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("04")
                    .WithTypeLine("Iron Gauntlets")),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("05")
                    .WithTypeLine("Iron Greaves")),  // Boots
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("06")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("07")
                    .WithTypeLine("Iron Ring")),  // Ring 1
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("08")
                    .WithTypeLine("Coral Ring")),  // Ring 2
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("09")
                    .WithTypeLine("Driftwood Sceptre")),  // Weapon
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("10")
                    .WithTypeLine("Splintered Tower Shield")),  // Offhand
            };
            items.Should().HaveCount(10);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            List<Item> currentItems = new List<Item>();
            RareSetRecipe recipe = new RareSetRecipe(1, 100, false, "set");
            var matches = recipe.Matches(currentItems).ToList();
            matches.Should().BeEmpty();

            while (items.Count > 0)
            {
                currentItems.Add(items[0]);
                items.RemoveAt(0);

                // Using the assumption that there are 10 possible items in the match and nine items are required to
                // have a match.
                if (currentItems.Count <= 8)
                {
                    matches = recipe.Matches(currentItems).ToList();
                    matches.Should().BeEmpty();
                }
                else
                {
                    matches = recipe.Matches(currentItems).ToList();

                    matches.Should().HaveCount(1);
                    var match = matches.ElementAt(0);
                    match.Instance.Should().BeOfType<RareSetRecipe>();
                    match.IsMatch.Should().BeTrue();
                    match.Missing.Should().HaveCount(10 - currentItems.Count);
                    match.MatchedItems.Should().HaveSameCount(currentItems);
                    match.MatchedItems.Should().Contain(currentItems);
                    match.Name.Should().Be("set");
                    match.PercentMatch.Should().Be(10M * currentItems.Count);
                }
            }
        }

        [TestMethod]
        public void RareSetRecipeTests_ItemsIncludeQuiver_QuiverIsNotUsed()
        {
            List<Item> items = new List<Item>()
            {
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("01")
                    .WithTypeLine("Iron Hat")),  // Helmet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("02")
                    .WithTypeLine("Plate Vest")),  // Body Armor
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("03")
                    .WithTypeLine("Rustic Sash")),  // Belt
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("04")
                    .WithTypeLine("Iron Gauntlets")),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("05")
                    .WithTypeLine("Iron Greaves")),  // Boots
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("06")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("07")
                    .WithTypeLine("Iron Ring")),  // Ring 1
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("08")
                    .WithTypeLine("Coral Ring")),  // Ring 2
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("09")
                    .WithTypeLine("Crude Bow")),  // Weapon
            };
            Gear quiverGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1)
                .WithId("10").WithTypeLine("Rugged Quiver"));
            items.Add(quiverGear);

            items.Should().HaveCount(10);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            RareSetRecipe recipe = new RareSetRecipe(1, 100, false, "set");
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(1);
            var match = matches.ElementAt(0);
            match.Instance.Should().BeOfType<RareSetRecipe>();
            match.IsMatch.Should().BeTrue();
            match.Missing.Should().BeEmpty();
            match.MatchedItems.Should().HaveCount(9);
            match.MatchedItems.Should().NotContain(quiverGear);
            match.Name.Should().Be("set");
            match.PercentMatch.Should().Be(100M);
        }

        [TestMethod]
        public void RareSetRecipeTests_ItemsIncludeBowAndShield_ShieldIsNotUsed()
        {
            List<Item> items = new List<Item>()
            {
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("01")
                    .WithTypeLine("Iron Hat")),  // Helmet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("02")
                    .WithTypeLine("Plate Vest")),  // Body Armor
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("03")
                    .WithTypeLine("Rustic Sash")),  // Belt
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("04")
                    .WithTypeLine("Iron Gauntlets")),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("05")
                    .WithTypeLine("Iron Greaves")),  // Boots
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("06")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("07")
                    .WithTypeLine("Iron Ring")),  // Ring 1
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("08")
                    .WithTypeLine("Coral Ring")),  // Ring 2
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("09")
                    .WithTypeLine("Crude Bow")),  // Weapon
            };
            Gear shieldGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1)
                .WithId("10").WithTypeLine("Splintered Tower Shield"));  // Offhand
            items.Add(shieldGear);

            items.Should().HaveCount(10);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            RareSetRecipe recipe = new RareSetRecipe(1, 100, false, "set");
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(1);
            var match = matches.ElementAt(0);
            match.Instance.Should().BeOfType<RareSetRecipe>();
            match.IsMatch.Should().BeTrue();
            match.Missing.Should().BeEmpty();
            match.MatchedItems.Should().HaveCount(9);
            match.MatchedItems.Should().NotContain(shieldGear);
            match.Name.Should().Be("set");
            match.PercentMatch.Should().Be(100M);
        }

        [TestMethod]
        public void RareSetRecipeTests_ItemsIncludeTwoShields_UseBothForMatch()
        {
            List<Item> items = new List<Item>()
            {
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("01")
                    .WithTypeLine("Iron Hat")),  // Helmet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("02")
                    .WithTypeLine("Plate Vest")),  // Body Armor
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("03")
                    .WithTypeLine("Rustic Sash")),  // Belt
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("04")
                    .WithTypeLine("Iron Gauntlets")),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("05")
                    .WithTypeLine("Iron Greaves")),  // Boots
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("06")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("07")
                    .WithTypeLine("Iron Ring")),  // Ring 1
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("08")
                    .WithTypeLine("Coral Ring")),  // Ring 2
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("09")
                    .WithTypeLine("Corroded Tower Shield")),  // Weapon
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("10")
                    .WithTypeLine("Splintered Tower Shield")),  // Offhand
            };
            items.Should().HaveCount(10);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            RareSetRecipe recipe = new RareSetRecipe(1, 100, false, "set");
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(1);
            var match = matches.ElementAt(0);
            match.Instance.Should().BeOfType<RareSetRecipe>();
            match.IsMatch.Should().BeTrue();
            match.Missing.Should().BeEmpty();
            match.MatchedItems.Should().HaveCount(10);
            match.MatchedItems.Should().Contain(items);
            match.Name.Should().Be("set");
            match.PercentMatch.Should().Be(100M);
        }

        [TestMethod]
        public void RareSetRecipeTests_VariousItemLevels_UseItemsInRange()
        {
            List<Item> items = new List<Item>()
            {
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(50).WithId("01")
                    .WithTypeLine("Iron Hat")),  // Helmet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("01a")
                    .WithTypeLine("Iron Hat")),  // Helmet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(76).WithId("02")
                    .WithTypeLine("Plate Vest")),  // Body Armor
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(49).WithId("02a")
                    .WithTypeLine("Plate Vest")),  // Body Armor
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(55).WithId("03")
                    .WithTypeLine("Rustic Sash")),  // Belt
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(60).WithId("04")
                    .WithTypeLine("Iron Gauntlets")),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(60).WithId("05")
                    .WithTypeLine("Iron Greaves")),  // Boots
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(60).WithId("06")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(70).WithId("07")
                    .WithTypeLine("Iron Ring")),  // Ring 1
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(75).WithId("08")
                    .WithTypeLine("Coral Ring")),  // Ring 2
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(75).WithId("09")
                    .WithTypeLine("Driftwood Sceptre")),  // Weapon
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(75).WithId("10")
                    .WithTypeLine("Splintered Tower Shield")),  // Offhand
            };
            items.Should().HaveCount(12);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            RareSetRecipe recipe = new RareSetRecipe(50, 75, false, "set");
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(1);
            var match = matches.ElementAt(0);
            match.Instance.Should().BeOfType<RareSetRecipe>();
            match.IsMatch.Should().BeTrue();
            match.Missing.Should().HaveCount(1);
            match.Missing[0].Should().Be("Armour");
            match.MatchedItems.Should().HaveCount(9);
            match.MatchedItems.ForEach(i => i.ItemLevel.Should().BeInRange(50, 75));
            match.Name.Should().Be("set");
            match.PercentMatch.Should().Be(90M);
        }

        [TestMethod]
        public void RareSetRecipeTests_SingleValueRange_UseItemsInRange()
        {
            List<Item> items = new List<Item>()
            {
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("01")
                    .WithTypeLine("Iron Hat")),  // Helmet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("02")
                    .WithTypeLine("Plate Vest")),  // Body Armor
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(55).WithId("03")
                    .WithTypeLine("Rustic Sash")),  // Belt
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(21).WithId("03a")
                    .WithTypeLine("Rustic Sash")),  // Belt
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(19).WithId("04")
                    .WithTypeLine("Iron Gauntlets")),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("04a")
                    .WithTypeLine("Iron Gauntlets")),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("05")
                    .WithTypeLine("Iron Greaves")),  // Boots
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("06")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(01).WithId("06a")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(00).WithId("06b")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("07")
                    .WithTypeLine("Iron Ring")),  // Ring 1
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("08")
                    .WithTypeLine("Coral Ring")),  // Ring 2
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("09")
                    .WithTypeLine("Driftwood Sceptre")),  // Weapon
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("10")
                    .WithTypeLine("Splintered Tower Shield")),  // Offhand
            };
            items.Should().HaveCount(14);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            RareSetRecipe recipe = new RareSetRecipe(20, 20, false, "set");
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(1);
            var match = matches.ElementAt(0);
            match.Instance.Should().BeOfType<RareSetRecipe>();
            match.IsMatch.Should().BeTrue();
            match.Missing.Should().HaveCount(1);
            match.Missing[0].Should().Be("Belt");
            match.MatchedItems.Should().HaveCount(9);
            match.MatchedItems.ForEach(i => i.ItemLevel.Should().Be(20));
            match.Name.Should().Be("set");
            match.PercentMatch.Should().Be(90M);
        }

        [TestMethod]
        public void RareSetRecipeTests_VariousItemLevels_LowestItemLevelsUsed()
        {
            List<Item> baseItems = new List<Item>()
            {
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30).WithId("01")
                    .WithTypeLine("Iron Hat")),  // Helmet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30).WithId("02")
                    .WithTypeLine("Plate Vest")),  // Body Armor
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30).WithId("03")
                    .WithTypeLine("Rustic Sash")),  // Belt
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30).WithId("04")
                    .WithTypeLine("Iron Gauntlets")),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30).WithId("05")
                    .WithTypeLine("Iron Greaves")),  // Boots
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30).WithId("06")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30).WithId("07")
                    .WithTypeLine("Iron Ring")),  // Ring 1
            };
            baseItems.Should().HaveCount(7);
            baseItems.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            RareSetRecipe recipe = new RareSetRecipe(20, 50, false, "set");

            {
                List<Item> items = new List<Item>(baseItems);
                var matches = recipe.Matches(items).ToList();
                matches.Should().BeEmpty();
            }

            {
                // The items collection has extra rings; pick the lowest two in range.
                List<Item> items = new List<Item>(baseItems);
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(40)
                    .WithId("08").WithTypeLine("Coral Ring")));  // Ring 2 (in range, too high)
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(22)
                    .WithId("08a").WithTypeLine("Coral Ring")));  // Ring 2 (in range)
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(19)
                    .WithId("08b").WithTypeLine("Coral Ring")));  // Ring 2 (too low)
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30)
                    .WithId("09").WithTypeLine("Driftwood Sceptre")));  // Weapon
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30)
                    .WithId("10").WithTypeLine("Splintered Tower Shield")));  // Offhand
                items.Should().HaveCount(12);
                items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

                var matches = recipe.Matches(items).ToList();

                matches.Should().HaveCount(1);
                var match = matches.ElementAt(0);
                match.Instance.Should().BeOfType<RareSetRecipe>();
                match.IsMatch.Should().BeTrue();
                match.Missing.Should().BeEmpty();
                match.MatchedItems.Should().HaveCount(10);
                match.MatchedItems.ForEach(i => i.ItemLevel.Should().BeInRange(20, 39));
                match.MatchedItems.Select(i => i.Id).Should().OnlyHaveUniqueItems();
                match.Name.Should().Be("set");
                match.PercentMatch.Should().Be(100M);
            }

            {
                // The items collection has a bow with lower item level than the shield, but it should not be picked
                // because the one-handed weapon has the lowest item level.
                List<Item> items = new List<Item>(baseItems);
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30)
                    .WithId("08").WithTypeLine("Coral Ring")));  // Ring 2
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30)
                    .WithId("09").WithTypeLine("Driftwood Sceptre")));  // Weapon
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(40)
                    .WithId("10").WithTypeLine("Splintered Tower Shield")));  // Offhand
                var bowGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(35)
                    .WithId("09a").WithTypeLine("Crude Bow"));  // Two-handed weapon, higher ilevel
                items.Add(bowGear);
                items.Should().HaveCount(11);
                items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

                var matches = recipe.Matches(items).ToList();

                matches.Should().HaveCount(1);
                var match = matches.ElementAt(0);
                match.Instance.Should().BeOfType<RareSetRecipe>();
                match.IsMatch.Should().BeTrue();
                match.Missing.Should().BeEmpty();
                match.MatchedItems.Should().HaveCount(10);
                match.MatchedItems.Should().NotContain(bowGear);
                match.MatchedItems.Select(i => i.Id).Should().OnlyHaveUniqueItems();
                match.Name.Should().Be("set");
                match.PercentMatch.Should().Be(100M);
            }

            {
                // As the above case, but the shield is the item with the lower item level than the bow.
                List<Item> items = new List<Item>(baseItems);
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30)
                    .WithId("08").WithTypeLine("Coral Ring")));  // Ring 2
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(40)
                    .WithId("09").WithTypeLine("Driftwood Sceptre")));  // Weapon
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30)
                    .WithId("10").WithTypeLine("Splintered Tower Shield")));  // Offhand
                var bowGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(35)
                    .WithId("09a").WithTypeLine("Crude Bow"));  // Two-handed weapon, higher ilevel
                items.Add(bowGear);
                items.Should().HaveCount(11);
                items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

                var matches = recipe.Matches(items).ToList();

                matches.Should().HaveCount(1);
                var match = matches.ElementAt(0);
                match.Instance.Should().BeOfType<RareSetRecipe>();
                match.IsMatch.Should().BeTrue();
                match.Missing.Should().BeEmpty();
                match.MatchedItems.Should().HaveCount(10);
                match.MatchedItems.Should().NotContain(bowGear);
                match.MatchedItems.Select(i => i.Id).Should().OnlyHaveUniqueItems();
                match.Name.Should().Be("set");
                match.PercentMatch.Should().Be(100M);
            }

            {
                // The items collection has a bow with lower item level than one of the one-handed weapons, but it
                // should not be picked, as the other one-handed weapon has the lowest item level.
                List<Item> items = new List<Item>(baseItems);
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30)
                    .WithId("08").WithTypeLine("Coral Ring")));  // Ring 2
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30)
                    .WithId("09").WithTypeLine("Driftwood Sceptre")));  // Weapon
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(40)
                    .WithId("10").WithTypeLine("Goat's Horn")));  // Offhand
                var bowGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(35)
                    .WithId("09a").WithTypeLine("Crude Bow"));  // Two-handed weapon, higher ilevel
                items.Add(bowGear);
                items.Should().HaveCount(11);
                items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

                var matches = recipe.Matches(items).ToList();

                matches.Should().HaveCount(1);
                var match = matches.ElementAt(0);
                match.Instance.Should().BeOfType<RareSetRecipe>();
                match.IsMatch.Should().BeTrue();
                match.Missing.Should().BeEmpty();
                match.MatchedItems.Should().HaveCount(10);
                match.MatchedItems.Should().NotContain(bowGear);
                match.MatchedItems.Select(i => i.Id).Should().OnlyHaveUniqueItems();
                match.Name.Should().Be("set");
                match.PercentMatch.Should().Be(100M);
            }

            {
                // The bow should be picked, as it has a lower item level than the sword and shield.
                List<Item> items = new List<Item>(baseItems);
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30)
                    .WithId("08").WithTypeLine("Coral Ring")));  // Ring 2
                var weaponGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(33)
                    .WithId("09").WithTypeLine("Driftwood Sceptre"));  // Weapon
                items.Add(weaponGear);
                var shieldGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(35)
                    .WithId("10").WithTypeLine("Splintered Tower Shield"));  // Offhand
                items.Add(shieldGear);
                var bowGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30)
                    .WithId("09a").WithTypeLine("Crude Bow"));  // Two-handed weapon, higher ilevel
                items.Add(bowGear);
                items.Should().HaveCount(11);
                items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

                var matches = recipe.Matches(items).ToList();

                matches.Should().HaveCount(1);
                var match = matches.ElementAt(0);
                match.Instance.Should().BeOfType<RareSetRecipe>();
                match.IsMatch.Should().BeTrue();
                match.Missing.Should().BeEmpty();
                match.MatchedItems.Should().HaveCount(9);
                match.MatchedItems.Should().NotContain(weaponGear);
                match.MatchedItems.Should().NotContain(shieldGear);
                match.MatchedItems.Should().Contain(bowGear);
                match.MatchedItems.Select(i => i.Id).Should().OnlyHaveUniqueItems();
                match.Name.Should().Be("set");
                match.PercentMatch.Should().Be(100M);
            }

            {
                // As above, but the bow has a lower item level than two one-handed weapons.
                List<Item> items = new List<Item>(baseItems);
                items.Add(new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30)
                    .WithId("08").WithTypeLine("Coral Ring")));  // Ring 2
                var weaponGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(33)
                    .WithId("09").WithTypeLine("Driftwood Sceptre"));  // Weapon
                items.Add(weaponGear);
                var shieldGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(35)
                    .WithId("10").WithTypeLine("Goat's Horn"));  // Offhand
                items.Add(shieldGear);
                var bowGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(30)
                    .WithId("09a").WithTypeLine("Crude Bow"));  // Two-handed weapon, higher ilevel
                items.Add(bowGear);
                items.Should().HaveCount(11);
                items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

                var matches = recipe.Matches(items).ToList();

                matches.Should().HaveCount(1);
                var match = matches.ElementAt(0);
                match.Instance.Should().BeOfType<RareSetRecipe>();
                match.IsMatch.Should().BeTrue();
                match.Missing.Should().BeEmpty();
                match.MatchedItems.Should().HaveCount(9);
                match.MatchedItems.Should().NotContain(weaponGear);
                match.MatchedItems.Should().NotContain(shieldGear);
                match.MatchedItems.Should().Contain(bowGear);
                match.MatchedItems.Select(i => i.Id).Should().OnlyHaveUniqueItems();
                match.Name.Should().Be("set");
                match.PercentMatch.Should().Be(100M);
            }
        }

        [TestMethod]
        public void RareSetRecipeTests_ItemsIncludesOtherTypes_UseOnlyGear()
        {
            List<Item> items = new List<Item>()
            {
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("01")
                    .WithTypeLine("Iron Hat")),  // Helmet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("02")
                    .WithTypeLine("Plate Vest")),  // Body Armor
                // No Belt item.
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("04")
                    .WithTypeLine("Iron Gauntlets")),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("05")
                    .WithTypeLine("Iron Greaves")),  // Boots
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("06")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("07")
                    .WithTypeLine("Iron Ring")),  // Ring 1
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("08")
                    .WithTypeLine("Coral Ring")),  // Ring 2
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("09")
                    .WithTypeLine("Driftwood Sceptre")),  // Weapon
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("10")
                    .WithTypeLine("Splintered Tower Shield")),  // Offhand
            };
            var flaskGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20)
                .WithId("11").WithTypeLine("Small Life Flask"));
            items.Add(flaskGear);
            var mapItem = new Map(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20)
                .WithId("12").WithTypeLine("Desert Map").WithProperty("Map Tier", "5", 1));
            items.Add(mapItem);
            var abyssJewelItem = new AbyssJewel(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20)
                .WithId("13").WithTypeLine("Ghastly Eye Jewel"));
            items.Add(abyssJewelItem);

            items.Should().HaveCount(12);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            RareSetRecipe recipe = new RareSetRecipe(20, 50, false, "set");
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(1);
            var match = matches.ElementAt(0);
            match.Instance.Should().BeOfType<RareSetRecipe>();
            match.IsMatch.Should().BeTrue();
            match.Missing.Should().HaveCount(1);
            match.Missing[0].Should().Be("Belt");
            match.MatchedItems.Should().HaveCount(9);
            match.MatchedItems.Should().NotContain(flaskGear);
            match.MatchedItems.Should().NotContain(mapItem);
            match.MatchedItems.Should().NotContain(abyssJewelItem);
            match.Name.Should().Be("set");
            match.PercentMatch.Should().Be(90M);
        }

        [TestMethod]
        public void RareSetRecipeTests_ItemsIncludesOtherRarities_UseOnlyRareItems()
        {
            List<Item> items = new List<Item>()
            {
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("01")
                    .WithTypeLine("Iron Hat")),  // Helmet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("02")
                    .WithTypeLine("Plate Vest")),  // Body Armor
                // No Belt item.
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("04")
                    .WithTypeLine("Iron Gauntlets")),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("05")
                    .WithTypeLine("Iron Greaves")),  // Boots
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("06")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("07")
                    .WithTypeLine("Iron Ring")),  // Ring 1
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("08")
                    .WithTypeLine("Coral Ring")),  // Ring 2
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("09")
                    .WithTypeLine("Driftwood Sceptre")),  // Weapon
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(20).WithId("10")
                    .WithTypeLine("Splintered Tower Shield")),  // Offhand
            };
            var normalGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Normal).WithItemLevel(20)
                .WithId("11").WithTypeLine("Rustic Sash"));
            items.Add(normalGear);
            var magicGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Magic).WithItemLevel(20)
                .WithId("12").WithTypeLine("Rustic Sash of Grace"));
            items.Add(magicGear);
            var uniqueGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Unique).WithItemLevel(20)
                .WithId("13").WithTypeLine("Rustic Sash"));
            items.Add(uniqueGear);
            var relicGear = new Gear(Build.A.JsonProxyItem.ThatIsARelic(true).WithItemLevel(20)
                .WithId("14").WithTypeLine("Rustic Sash"));
            items.Add(relicGear);

            items.Should().HaveCount(13);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            RareSetRecipe recipe = new RareSetRecipe(20, 50, false, "set");
            var matches = recipe.Matches(items).ToList();

            matches.Should().HaveCount(1);
            var match = matches.ElementAt(0);
            match.Instance.Should().BeOfType<RareSetRecipe>();
            match.IsMatch.Should().BeTrue();
            match.Missing.Should().HaveCount(1);
            match.Missing[0].Should().Be("Belt");
            match.MatchedItems.Should().HaveCount(9);
            match.MatchedItems.Should().NotContain(normalGear);
            match.MatchedItems.Should().NotContain(magicGear);
            match.MatchedItems.Should().NotContain(uniqueGear);
            match.MatchedItems.Should().NotContain(relicGear);
            match.Name.Should().Be("set");
            match.PercentMatch.Should().Be(90M);
        }

        [TestMethod]
        public void RareSetRecipeTests_SomeItemsIdentified_UseCorrectIdentifiedState()
        {
            List<Item> items = new List<Item>()
            {
                // No Helm item.
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("02")
                    .WithTypeLine("Plate Vest")),  // Body Armor
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("03")
                    .WithTypeLine("Rustic Sash")),  // Belt
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("04")
                    .WithTypeLine("Iron Gauntlets")),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("05")
                    .WithTypeLine("Iron Greaves")),  // Boots
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("06")
                    .WithTypeLine("Paua Amulet")),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("07")
                    .WithTypeLine("Iron Ring")),  // Ring 1
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("08")
                    .WithTypeLine("Iron Ring")),  // Ring 2
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("09")
                    .WithTypeLine("Driftwood Sceptre")),  // Weapon
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("10")
                    .WithTypeLine("Splintered Tower Shield")),  // Offhand
            };
            var identifiedHelmGear = new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1)
                .WithId("01").WithTypeLine("Iron Hat").ThatIsIdentified(true));
            items.Add(identifiedHelmGear);

            items.Should().HaveCount(10);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            {
                RareSetRecipe unidentifiedItemsRecipe = new RareSetRecipe(1, 100, false, "unidentified set");
                var unidentifiedItemsRecipeMatches = unidentifiedItemsRecipe.Matches(items).ToList();

                unidentifiedItemsRecipeMatches.Should().HaveCount(1);
                var match = unidentifiedItemsRecipeMatches.ElementAt(0);
                match.Instance.Should().BeOfType<RareSetRecipe>();
                match.IsMatch.Should().BeTrue();
                match.Missing.Should().HaveCount(1);
                match.Missing[0].Should().Be("Helm");
                match.MatchedItems.Should().HaveCount(9);
                match.MatchedItems.Should().NotContain(identifiedHelmGear);
                match.Name.Should().Be("unidentified set");
                match.PercentMatch.Should().Be(90M);
            }

            {
                RareSetRecipe identifiedItemsRecipe = new RareSetRecipe(1, 100, true, "identified set");
                var identifiedItemsRecipeMatches = identifiedItemsRecipe.Matches(items).ToList();
                identifiedItemsRecipeMatches.Should().HaveCount(0);
            }

            items.AddRange(new List<Item>()
            {
                // No helm.
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("02i")
                    .WithTypeLine("Plate Vest").ThatIsIdentified(true)),  // Body Armor
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("03i")
                    .WithTypeLine("Rustic Sash").ThatIsIdentified(true)),  // Belt
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("04i")
                    .WithTypeLine("Iron Gauntlets").ThatIsIdentified(true)),  // Gloves
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("05i")
                    .WithTypeLine("Iron Greaves").ThatIsIdentified(true)),  // Boots
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("06i")
                    .WithTypeLine("Paua Amulet").ThatIsIdentified(true)),  // Amulet
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("07i")
                    .WithTypeLine("Iron Ring").ThatIsIdentified(true)),  // Ring 1
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("08i")
                    .WithTypeLine("Iron Ring").ThatIsIdentified(true)),  // Ring 2
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("09i")
                    .WithTypeLine("Driftwood Sceptre").ThatIsIdentified(true)),  // Weapon
                new Gear(Build.A.JsonProxyItem.WithFrameType((int)Rarity.Rare).WithItemLevel(1).WithId("10i")
                    .WithTypeLine("Driftwood Sceptre").ThatIsIdentified(true)),  // Offhand
            });
            items.Should().HaveCount(19);
            items.Select(i => i.Id).Should().OnlyHaveUniqueItems();

            {
                RareSetRecipe unidentifiedItemsRecipe = new RareSetRecipe(1, 100, false, "unidentified set");
                var unidentifiedItemsRecipeMatches = unidentifiedItemsRecipe.Matches(items).ToList();

                unidentifiedItemsRecipeMatches.Should().HaveCount(1);
                var match = unidentifiedItemsRecipeMatches.ElementAt(0);
                match.Instance.Should().BeOfType<RareSetRecipe>();
                match.IsMatch.Should().BeTrue();
                match.Missing.Should().HaveCount(1);
                match.Missing[0].Should().Be("Helm");
                match.MatchedItems.Should().HaveCount(9);
                match.MatchedItems.ForEach(i => i.Identified.Should().BeFalse());
                match.Name.Should().Be("unidentified set");
                match.PercentMatch.Should().Be(90M);
            }

            {
                RareSetRecipe identifiedItemsRecipe = new RareSetRecipe(1, 100, true, "identified set");
                var identifiedItemsRecipeMatches = identifiedItemsRecipe.Matches(items).ToList();

                identifiedItemsRecipeMatches.Should().HaveCount(1);
                var match = identifiedItemsRecipeMatches.ElementAt(0);
                match.Instance.Should().BeOfType<RareSetRecipe>();
                match.IsMatch.Should().BeTrue();
                match.Missing.Should().BeEmpty();
                match.MatchedItems.Should().HaveCount(10);
                match.MatchedItems.ForEach(i => i.Identified.Should().BeTrue());
                match.Name.Should().Be("identified set");
                match.PercentMatch.Should().Be(100M);
            }
        }
    }
}
