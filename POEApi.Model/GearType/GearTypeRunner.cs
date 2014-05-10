using System.Collections.Generic;
using System.Linq;

namespace POEApi.Model
{
    //This class needs to be refactored to read the general/compatible types from a file such as GearTypes.xml
    public abstract class GearTypeRunner
    {
        public abstract bool IsCompatibleType(Gear item);
        public abstract string GetBaseType(Gear item);
        public GearType Type { get; set; }

        public GearTypeRunner(GearType gearType)
        {
            this.Type = gearType;
        }
    }

    public class GearTypeRunnerBase : GearTypeRunner
    {
        protected List<string> generalTypes;
        protected List<string> compatibleTypes;
        protected List<string> incompatibleTypes;

        public GearTypeRunnerBase(GearType gearType, params string[] compatibleTypes)
            : base(gearType)
        {
            this.generalTypes = new List<string>();
            this.compatibleTypes = compatibleTypes.ToList();
            this.incompatibleTypes = new List<string>();
        }

        public override bool IsCompatibleType(Gear item)
        {
            // First, check the general types, to see if there is an easy match.
            foreach (var type in generalTypes)
                if (item.TypeLine.Contains(type))
                    return true;

            // Second, check all known types.
            foreach (var type in compatibleTypes)
                if (item.TypeLine.Contains(type))
                    return true;

            return false;
        }

        public override string GetBaseType(Gear item)
        {
            if (incompatibleTypes != null && incompatibleTypes.Any(t => item.TypeLine.Contains(t)))
                return null;

            foreach (var type in compatibleTypes)
                if (item.TypeLine.Contains(type))
                    return type;

            return null;
        }
    }

    public class RingRunner : GearTypeRunnerBase
    {
        public RingRunner()
            : base(GearType.Ring, "Iron Ring", "Coral Ring", "Paua Ring", "Gold Ring", "Ruby Ring", "Sapphire Ring", "Topaz Ring", "Diamond Ring", "Moonstone Ring", "Prismatic Ring", "Amethyst Ring",
                                  "Two-Stone Ring")
        {
            incompatibleTypes = new List<string>() { "Ringmail" };
        }

        public override bool IsCompatibleType(Gear item)
        {
            if (item.TypeLine.Contains("Ring") && !incompatibleTypes.Any(t => item.TypeLine.Contains(t)))
                return true;

            return false;
        }
    }

    public class AmuletRunner : GearTypeRunnerBase
    {
        public AmuletRunner()
            : base(GearType.Amulet, "Paua Amulet", "Coral Amulet", "Amber Amulet", "Jade Amulet", "Lapis Amulet", "Gold Amulet", "Onyx Amulet", "Agate Amulet", "Turquoise Amulet", "Citrine Amulet")
        {
            generalTypes.Add("Amulet");
        }
    }

    public class HelmetRunner : GearTypeRunnerBase
    {
        public HelmetRunner()
            : base(GearType.Helmet, "Iron Hat", "Cone Helmet", "Barbute Helmet", "Close Helmet", "Gladiator Helmet", "Reaver Helmet", "Siege Helmet", "Samnite Helmet", "Ezomyte Burgonet", "Royal Burgonet", "Eternal Burgonet",
                                    "Leather Cap", "Tricorne", "Leather Hood", "Wolf Pelt", "Hunter Hood", "Noble Tricorne", "Ursine Pelt", "Silken Hood", "Sinner Tricorne", "Lion Pelt",
                                    "Vine Circlet", "Iron Circlet", "Torture Cage", "Tribal Circlet", "Bone Circlet", "Lunaris Circlet", "Steel Circlet", "Necromancer Circlet", "Solaris Circlet", "Mind Cage", "Hubris Circlet",
                                    "Battered Helm", "Visored Sallet", "Gilded Sallet", "Secutor Helm", "Fencer Helm", "Lacquered Helmet", "Fluted Bascinet", "Pig Faced Bascinet", "Nightmare Bascinet", "Sallet", // "Sallet" must be after other base types which contain the word "sallet".
                                    "Rusted Coif", "Soldier Helmet", "Great Helmet", "Crusader Helmet", "Aventail Helmet", "Zealot Helmet", "Great Crown", "Magistrate Crown", "Prophet Crown", "Praetor Crown",
                                    "Scare Mask", "Plague Mask", "Iron Mask", "Festival Mask", "Golden Mask", "Raven Mask", "Callous Mask", "Regicide Mask", "Harlequin Mask", "Vaal Mask", "Deicide Mask")
        {
            generalTypes.AddRange(new List<string>() { "Helmet", "Circlet", "Cap", "Mask", "Chain Coif", "Casque", "Hood", "Ringmail Coif", "Chainmail Coif", "Ring Coif", "Crown", "Burgonet", "Bascinet", "Pelt" });
        }
    }

    public class ChestRunner : GearTypeRunnerBase
    {
        public ChestRunner()
            : base(GearType.Chest, "Plate Vest", "Chestplate", "Copper Plate", "War Plate", "Full Plate", "Arena Plate", "Lordly Plate", "Bronze Plate", "Battle Plate", "Sun Plate", "Colosseum Plate", "Majestic Plate", "Golden Plate", "Crusader Plate", "Astral Plate", "Gladiator Plate", "Glorious Plate",
                                   "Shabby Jerkin", "Strapped Leather", "Buckskin Tunic", "Wild Leather", "Full Leather", "Sun Leather", "Thief's Garb", "Eelskin Tunic", "Frontier Leather", "Glorious Leather", "Coronal Leather", "Cutthroat's Garb", "Sharkskin Tunic", "Destiny Leather", "Exquisite Leather", "Zodiac Leather", "Assassin's Garb",
                                   "Simple Robe", "Silken Vest", "Scholar's Robe", "Silken Garb", "Mage's Vestment", "Silk Robe", "Cabalist Regalia", "Sage's Robe", "Silken Wrap", "Conjurer's Vestment", "Spidersilk Robe", "Destroyer Regalia", "Savant's Robe", "Necromancer Silks", "Occultist's Vestment", "Widowsilk Robe", "Vaal Regalia",
                                   "Scale Vest", "Light Brigandine", "Scale Doublet", "Infantry Brigandine", "Full Scale Armour", "Soldier's Brigandine", "Field Lamellar", "Wyrmscale Doublet", "Hussar Brigandine", "Full Wyrmscale", "Commander's Brigandine", "Battle Lamellar", "Dragonscale Doublet", "Desert Brigandine", "Full Dragonscale", "General's Brigandine", "Triumphant Lamellar",
                                   "Chainmail Vest", "Chainmail Tunic", "Ringmail Coat", "Chainmail Doublet", "Full Ringmail", "Full Chainmail", "Holy Chainmail", "Latticed Ringmail", "Crusader Chainmail", "Ornate Ringmail", "Chain Hauberk", "Devout Chainmail", "Loricated Ringmail", "Conquest Chainmail", "Elegant Ringmail", "Saint's Hauberk", "Saintly Chainmail",
                                   "Padded Vest", "Oiled Vest", "Padded Jacket", "Oiled Coat", "Scarlet Raiment", "Waxed Garb", "Bone Armour", "Quilted Jacket", "Sleek Coat", "Crimson Raiment", "Lacquered Garb", "Crypt Armour", "Sentinel Jacket", "Varnished Coat", "Blood Raiment", "Sadist Garb", "Carnal Armour")
        { }
    }

    public class BeltRunner : GearTypeRunnerBase
    {
        public BeltRunner()
            : base(GearType.Belt, "Rustic Sash", "Chain Belt", "Leather Belt", "Heavy Belt", "Studded Belt", "Cloth Belt")
        {
            generalTypes.Add("Belt");
            generalTypes.Add("Sash");
        }
    }

    public class FlaskRunner : GearTypeRunnerBase
    {
        public FlaskRunner()
            : base(GearType.Flask, "Small Life Flask", "Medium Life Flask", "Large Life Flask", "Greater Life Flask", "Grand Life Flask", "Giant Life Flask", "Colossal Life Flask", "Sacred Life Flask", "Hallowed Life Flask", "Sanctified Life Flask",
                                   "Small Mana Flask", "Medium Mana Flask", "Large Mana Flask", "Greater Mana Flask", "Grand Mana Flask", "Giant Mana Flask", "Colossal Mana Flask", "Sacred Mana Flask", "Hallowed Mana Flask", "Sanctified Mana Flask",
                                   "Small Hybrid Flask", "Medium Hybrid Flask", "Large Hybrid Flask", "Colossal Hybrid Flask", "Sacred Hybrid Flask", "Hallowed Hybrid Flask",
                                   "Quicksilver Flask", "Ruby Flask", "Sapphire Flask", "Topaz Flask", "Amethyst Flask", "Granite Flask", "Diamond Flask", "Jade Flask", "Quartz Flask")
        {
            generalTypes.Add("Flask");
        }
    }

    public class MapRunner : GearTypeRunnerBase
    {
        public MapRunner()
            : base(GearType.Map, "Crypt Map", "Dried Lake Map", "Dunes Map", "Dungeon Map", "Grotto Map", "Orchard Map", "Overgrown Ruin Map", "Tropical Island Map",
                                 "Arcade Map", "Arsenal Map", "Cemetery Map", "Mountain Ledge Map", "Sewer Map", "Thicket Map", "Wharf Map",
                                 "Ghetto Map", "Mud Geyser Map", "Museum Map", "Reef Map", "Spider Lair Map", "Springs Map", "Vaal Pyramid Map",
                                 "Catacomb Map", "Overgrown Shrine Map", "Promenade Map", "Shore Map", "Spider Forest Map", "Tunnel Map",
                                 "Bog Map", "Coves Map", "Graveyard Map", "Pier Map", "Underground Sea Map", "Villa Map",
                                 "Arachnid Nest Map", "Colonnade Map", "Dry Woods Map", "Strand Map", "Temple Map",
                                 "Jungle Valley Map", "Labyrinth Map", "Mine Map", "Torture Chamber Map", "Waste Pool Map",
                                 "Canyon Map", "Cells Map", "Dark Forest Map", "Dry Peninsula Map",
                                 "Gorge Map", "Maze Map", "Residence Map", "Underground River Map",
                                 "Bazaar Map", "Necropolis Map", "Plateau Map",
                                 "Academy Map", "Crematorium Map", "Precinct Map",
                                 "Shipyard Map", "Shrine Map",
                                 "Courtyard Map", "Palace Map")
        {
            generalTypes.Add("Map");
        }
    }

    public class GloveRunner : GearTypeRunnerBase
    {
        public GloveRunner()
            : base(GearType.Gloves, "Iron Gauntlets", "Plated Gauntlets", "Bronze Gauntlets", "Steel Gauntlets", "Antique Gauntlets", "Ancient Gauntlets", "Goliath Gauntlets", "Vaal Gauntlets", "Titan Gauntlets",
                                    "Rawhide Gloves", "Goathide Gloves", "Deerskin Gloves", "Nubuck Gloves", "Eelskin Gloves", "Sharkskin Gloves", "Shagreen Gloves", "Stealth Gloves", "Slink Gloves",
                                    "Wool Gloves", "Velvet Gloves", "Silk Gloves", "Embroidered Gloves", "Satin Gloves", "Samite Gloves", "Conjurer Gloves", "Arcanist Gloves", "Sorcerer Gloves",
                                    "Fishscale Gauntlets", "Ironscale Gauntlets", "Bronzescale Gauntlets", "Steelscale Gauntlets", "Serpentscale Gauntlets", "Wyrmscale Gauntlets", "hydrascale Gauntlets", "Dragonscale Gauntlets",
                                    "Chain Gloves", "Ringmail Gloves", "Mesh Gloves", "Riveted Gloves", "Zealot Gloves", "Soldier Gloves", "Legion Gloves", "Crusader Gloves",
                                    "Wrapped Mitts", "Strapped Mitts", "Clasped Mitts", "Trapper Mitts", "Ambush Mitts", "Carnal Mitts", "Assassin's Mitts", "Murder Mitts")
        {
            generalTypes.Add("Glove");
            generalTypes.Add("Mitts");
            generalTypes.Add("Gauntlets");
        }
    }

    public class BootRunner : GearTypeRunnerBase
    {
        public BootRunner()
            : base(GearType.Boots, "Iron Greaves", "Steel Greaves", "Plated Greaves", "Reinforced Greaves", "Antique Greaves", "Ancient Greaves", "Goliath Greaves", "Vaal Greaves", "Titan Greaves",
                                   "Rawhide Boots", "Goathide Boots", "Deerskin Boots", "Nubuck Boots", "Eelskin Boots", "Sharkskin Boots", "Shagreen Boots", "Stealth Boots", "Slink Boots",
                                   "Wool Shoes", "Velvet Slippers", "Silk Slippers", "Scholar Boots", "Satin Slippers", "Samite Slippers", "Conjurer Boots", "Arcanist Slippers", "Sorcerer Boots",
                                   "Leatherscale Boots", "Ironscale Boots", "Bronzescale Boots", "Steelscale Boots", "Serpentscale Boots", "Wyrmscale Boots", "Hydrascale Boots", "Dragonscale Boots",
                                   "Chain Boots", "Ringmail Boots", "Mesh Boots", "Riveted Boots", "Zealot Boots", "Soldier's Boots", "Legion Boots", "Crusader Boots",
                                   "Wrapped Boots", "Strapped Boots", "Clasped Boots", "Shackled Boots", "Trapper Boots", "Ambush Boots", "Carnal Boots", "Assassin's Boots", "Murder Boots")
        {
            generalTypes.Add("Greaves");
            generalTypes.Add("Slippers");
            generalTypes.Add("Boots");
            generalTypes.Add("Shoes");
        }
    }

    public class AxeRunner : GearTypeRunnerBase
    {
        public AxeRunner()
            : base(GearType.Axe, "Rusted Hatchet", "Jade Hatchet", "Boarding Axe", "Cleaver", "Broad Axe", "Arming Axe", "Decorative Axe", "Spectral Axe", "Jasper Axe", "Tomahawk", "Wrist Chopper", "War Axe", "Chest Splitter", "Ceremonial Axe", "Wraith Axe", "Karui Axe", "Siege Axe", "Reaver Axe", "Butcher Axe", "Vaal Hatchet", "Royal Axe", "Infernal Axe", // one-handed axes
                                 "Stone Axe", "Jade Chopper", "Woodsplitter", "Poleaxe", "Double Axe", "Gilded Axe", "Shadow Axe", "Jasper Chopper", "Timber Axe", "Headsman Axe", "Labrys", "Noble Axe", "Abyssal Axe", "Karui Chopper", "Sundering Axe", "Ezomyte Axe", "Vaal Axe", "Despot Axe", "Void Axe") // two-handed axes
        {
            generalTypes.AddRange(new List<string>() { "Axe", "Chopper", "Splitter", "Labrys", "Tomahawk", "Hatchet", "Poleaxe", "Woodsplitter", "Cleaver" });
        }
    }

    public class ClawRunner : GearTypeRunnerBase
    {
        public ClawRunner()
            : base(GearType.Claw, "Nailed Fist", "Sharktooth Claw", "Awl", "Cat's Paw", "Blinder", "Timeworn Claw", "Sparkling Claw", "Fright Claw", "Thresher Claw", "Gouger", "Tiger's Paw", "Gut Ripper", "Prehistoric Claw", "Noble Claw", "Eagle Claw", "Great White Claw", "Throat Stabber", "Hellion's Paw", "Eye Gouger", "Vaal Claw", "Imperial Claw", "Terror Claw")
        {
            generalTypes.AddRange(new List<string>() { "Fist", "Awl", "Paw", "Blinder", "Ripper", "Stabber", "Claw", "Gouger" });
        }
    }

    public class BowRunner : GearTypeRunnerBase
    {
        public BowRunner()
            : base(GearType.Bow, "Crude Bow", "Short Bow", "Long Bow", "Composite Bow", "Recurve Bow", "Bone Bow", "Royal Bow", "Death Bow", "Grove Bow", "Decurve Bow", "Compound Bow", "Sniper Bow", "Ivory Bow", "Highborn Bow", "Decimation Bow", "Thicket Bow", "Citadel Bow", "Ranger Bow", "Maraketh Bow", "Spine Bow", "Imperial Bow", "Harbinger Bow")
        {
            generalTypes.Add("Bow");
        }
    }

    public class DaggerRunner : GearTypeRunnerBase
    {
        public DaggerRunner()
            : base(GearType.Dagger, "Glass Shank", "Skinning Knife", "Carving Knife", "Stiletto", "Boot Knife", "Copper Kris", "Skean", "Imp Dagger", "Flaying Knife", "Butcher Knife", "Poignard", "Boot Blade", "Golden Kris", "Royal Skean", "Fiend Dagger", "Gutting Knife", "Slaughter Knife", "Ambusher", "Ezomyte Dagger", "Platinum Kris", "Imperial Skean", "Demon Dagger")
        {
            generalTypes.AddRange(new List<string>() { "Dagger", "Shank", "Knife", "Stiletto", "Skean", "Poignard", "Ambusher", "Boot Blade", "Kris" });
        }
    }

    public class MaceRunner : GearTypeRunnerBase
    {
        public MaceRunner()
            : base(GearType.Mace, "Driftwood Club", "Tribal Club", "Spiked Club", "Stone Hammer", "War Hammer", "Bladed Mace", "Ceremonial Mace", "Dream Mace", "Petrified Club", "Barbed Club", "Rock Breaker", "Battle Hammer", "Flanged Mace", "Ornate Mace", "Phantom Mace", "Ancestral Club", "Tenderizer", "Gavel", "Legion Hammer", "Pernarch", "Auric Mace", "Nightmare Mace", // one-handed
                                  "Driftwood Maul", "Tribal Maul", "Mallet", "Sledgehammer", "Spiked Maul", "Brass Maul", "Fright Maul", "Totemic Maul", "Great Mallet", "Steelhead", "Spiny Maul", "Plated Maul", "Dread Maul", "Karui Maul", "Colossus Mallet", "Piledriver", "Meatgrinder", "Imperial Maul", "Terror Maul") // two-handed
        {
            generalTypes.AddRange(new List<string>() { "Club", "Tenderizer", "Mace", "Hammer", "Maul", "Mallet", "Breaker", "Gavel", "Pernarch", "Steelhead", "Piledriver", "Bladed Mace" });
        }
    }

    public class QuiverRunner : GearTypeRunnerBase
    {
        public QuiverRunner()
            : base(GearType.Quiver, "Rugged Quiver", "Cured Quiver", "Conductive Quiver", "Heavy Quiver", "Light Quiver")
        {
            generalTypes.Add("Quiver");
        }
    }

    public class SceptreRunner : GearTypeRunnerBase
    {
        public SceptreRunner()
            : base(GearType.Sceptre, "Driftwood Sceptre", "Darkwood Sceptre", "Bronze Sceptre", "Quartz Sceptre", "Iron Sceptre", "Ochre Sceptre", "Ritual Sceptre", "Shadow Sceptre", "Grinning Fetish", "Sekhem", "Crystal Sceptre", "Lead Sceptre", "Blood Sceptre", "Royal Sceptre", "Abyssal Sceptre", "Karui Sceptre", "Tyrant's Sekhem", "Opal Sceptre", "Platinum Sceptre", "Carnal Sceptre", "Vaal Sceptre", "Void Sceptre")
        {
            generalTypes.Add("Sceptre");
            generalTypes.Add("Fetish");
            generalTypes.Add("Sekhem");
        }
    }

    public class StaffRunner : GearTypeRunnerBase
    {
        public StaffRunner()
            : base(GearType.Staff, "Gnarled Branch", "Primitive Staff", "Long Staff", "Iron Staff", "Coiled Staff", "Royal Staff", "Vile Staff", "Woodful Staff", "Quarterstaff", "Military Staff", "Serpentine Staff", "Highborn Staff", "Foul Staff", "Primordial Staff", "Lathi", "Ezomyte Staff", "Maelström Staff", "Imperial Staff", "Judgement Staff")
        {
            generalTypes.Add("Staff");
            generalTypes.Add("Gnarled Branch");
            generalTypes.Add("Quarterstaff");
            generalTypes.Add("Lathi");
        }
    }

    public class SwordRunner : GearTypeRunnerBase
    {
        public SwordRunner()
            : base(GearType.Sword, "Rusted Sword", "Copper Sword", "Sabre", "Broad Sword", "War Sword", "Ancient Sword", "Elegant Sword", "Dusk Blade", "Variscite Blade", "Cutlass", "Baselard", "Battle Sword", "Elder Sword", "Graceful Sword", "Twilight Blade", "Gemstone Sword", "Corsair Sword", "Gladius", "Legion Sword", "Vaal Blade", "Eternal Sword", "Midnight Blade",
                                   "Rusted Spike", "Whalebone Rapier", "Battered Foil", "Basket Rapier", "Jagged Foil", "Antique Rapier", "Elegant Foil", "Thorn Rapier", "Wyrmbone Rapier", "Burnished Foil", "Estoc", "Serrated Foil", "Primeval Rapier", "Fancy Foil", "Apex Rapier", "Dragonbone Rapier", "Tempered Foil", "Pecoraro", "Spiraled Foil", "Vaal Rapier", "Jeweled Foil", "Harpy Rapier",
                                   "Corroded Blade", "Longsword", "Bastard Sword", "Two-Handed Sword", "Etched Greatsword", "Ornate Sword", "Spectral Sword", "Butcher Sword", "Footman Sword", "Highland Blade", "Engraved Greatsword", "Tiger Sword", "Wraith Sword", "Headman's Sword", "Reaver Sword", "Ezomyte Blade", "Vaal Greatsword", "Lion Sword", "Infernal Sword")
        {
            generalTypes.AddRange(new List<string>() { "Sword", "sword", "Sabre", "Dusk Blade", "Cutlass", "Baselard", "Gladius", "Variscite Blade", "Vaal Blade", "Midnight Blade", "Corroded Blade",
                   "Highland Blade", "Ezomyte Blade", "Rusted Spike", "Rapier", "Foil", "Pecoraro", "Estoc", "Twilight Blade" });
        }
    }

    public class ShieldRunner : GearTypeRunnerBase
    {
        public ShieldRunner()
            : base(GearType.Shield, "Splintered Tower Shield", "Corroded Tower Shield", "Rawhide Tower Shield", "Cedar Tower Shield", "Copper Tower Shield", "Reinforced Tower Shield", "Painted Tower Shield", "Buckskin Tower Shield", "Mahogany Tower Shield", "Bronze Tower Shield", "Girded Tower Shield", "Crested Tower Shield", "Shagreen Tower Shield", "Ebony Tower Shield", "Ezomyte Tower Shield", "Colossal Tower Shield", "Pinnacle Tower Shield",
                                    "Goathide Buckler", "Pine Buckler", "Painted Buckler", "Hammered Buckler", "War Buckler", "Gilded Buckler", "Oak Buckler", "Enameled Buckler", "Corrugated Buckler", "Battle Buckler", "Golden Buckler", "Ironwood Buckler", "Lacquered Buckler", "Vaal Buckler", "Crusader Buckler", "Imperial Buckler",
                                    "Twig Spirit Shield", "Yew Spirit Shield", "Bone Spirit Shield", "Tarnished Spirit Shield", "Jingling Spirit Shield", "Brass Spirit Shield", "Walnut Spirit Shield", "Ivory Spirit Shield", "Ancient Spirit Shield", "Chiming Spirit Shield", "Thorium Spirit Shield", "Lacewood Spirit Shield", "Fossilized Spirit Shield", "Vaal Spirit Shield", "Harmonic Spirit Shield", "Titanium Spirit Shield",
                                    "Rotted Round Shield", "Fir Round Shield", "Studded Round Shield", "Scarlet Round Shield", "Splendid Round Shield", "Maple Round Shield", "Spiked Round Shield", "Crimson Round Shield", "Baroque Round Shield", "Teak Round Shield", "Spiny Round Shield", "Cardinal Round Shield", "Elegant Round Shield",
                                    "Plank Kite Shield", "Linden Kite Shield", "Reinforced Kite Shield", "Layered Kite Shield", "Ceremonial Kite Shield", "Etched Shield", "Steel Kite Shield", "Laminated Kite Shield", "Angelic Kite Shield", "Branded Kite Shield", "Champion Kite Shield", "Mosaic Kite Shield", "Archon Kite Shield",
                                    "Spiked Bundle", "Driftwood Spiked Shield", "Alloyed Spike Shield", "Burnished Spike Shield", "Ornate Spike Shield", "Redwood Spiked Shield", "Compound Spiked Shield", "Polished Spiked Shield", "Sovereign Spiked Shield", "Alder Spike Shield", "Ezomyte Spiked Shield", "Mirrored Spike Shield", "Supreme Spiked Shield")
        {
            generalTypes.Add("Shield");
            generalTypes.Add("Spiked Bundle");
            generalTypes.Add("Buckler");
        }
    }

    public class WandRunner : GearTypeRunnerBase
    {
        public WandRunner()
            : base(GearType.Wand, "Driftwood Wand", "Goat's Horn", "Carved Wand", "Quartz Wand", "Spiraled Wand", "Sage Wand", "Faun's Horn", "Engraved Wand", "Crystal Wand", "Serpent Wand", "Omen Wand", "Demon's Horn", "Imbued Wand", "Opal Wand", "Tornado Wand", "Prophecy Wand")
        {
            generalTypes.Add("Wand");
            generalTypes.Add("Horn");
        }
    }

}