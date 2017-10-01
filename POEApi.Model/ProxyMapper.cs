using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using POEApi.Infrastructure;

namespace POEApi.Model
{
    internal class ProxyMapper
    {
        internal const string STACKSIZE = "Stack Size";
        internal const string CHARGES = "Currently has %0 of %1 Charges";
        internal const string STASH = "Stash";
        public const string QUALITY = "Quality";
        private static readonly Regex qualityRx = new Regex("[+]{1}(?<quality>[0-9]{1,2}).*");

        #region   Orb Types  

        private static readonly Dictionary<string, OrbType> orbMap = new Dictionary<string, OrbType>
        {
            {"Chaos Orb", OrbType.Chaos},
            {"Divine Orb", OrbType.Divine},
            {"Regal Orb", OrbType.Regal},
            {"Orb of Augmentation", OrbType.Augmentation},
            {"Orb of Alchemy", OrbType.Alchemy},
            {"Alchemy Shard", OrbType.AlchemyShard},
            {"Chromatic Orb", OrbType.Chromatic},
            {"Orb of Transmutation", OrbType.Transmutation},
            {"Transmutation Shard", OrbType.TransmutationShard},
            {"Orb of Scouring", OrbType.Scouring},
            {"Glassblower's Bauble", OrbType.GlassblowersBauble},
            {"Cartographer's Chisel", OrbType.Chisel},
            {"Gemcutter's Prism", OrbType.GemCutterPrism},
            {"Orb of Alteration", OrbType.Alteration},
            {"Alteration Shard", OrbType.AlterationShard},
            {"Orb of Chance", OrbType.Chance},
            {"Orb of Regret", OrbType.Regret},
            {"Exalted Orb", OrbType.Exalted},
            {"Armourer's Scrap", OrbType.ArmourersScrap},
            {"Blessed Orb", OrbType.Blessed},
            {"Blacksmith's Whetstone", OrbType.BlacksmithsWhetstone},
            {"Scroll Fragment", OrbType.ScrollFragment},
            {"Jeweller's Orb", OrbType.JewelersOrb},
            {"Scroll of Wisdom", OrbType.ScrollofWisdom},
            {"Orb of Fusing", OrbType.Fusing},
            {"Portal Scroll", OrbType.PortalScroll},
            {"Albino Rhoa Feather", OrbType.AlbinaRhoaFeather},
            {"Mirror", OrbType.Mirror},
            {"Eternal Orb", OrbType.Eternal},
            {"Imprint", OrbType.Imprint},
            {"Vaal Orb", OrbType.VaalOrb},
            {"Perandus Coin", OrbType.PerandusCoin},
            {"Silver Coin", OrbType.SilverCoin},
            {"Ancient Orb", OrbType.AncientOrb},
            {"Ancient Shard", OrbType.AncientShard},
            {"Annulment Shard", OrbType.AnnulmentShard},
            {"Binding Shard", OrbType.BindingShard},
            {"Chaos Shard", OrbType.ChaosShard},
            {"Engineer's Orb", OrbType.EngineersOrb},
            {"Engineer's Shard", OrbType.EngineersShard},
            {"Exalted Shard", OrbType.ExaltedShard},
            {"Harbinger's Orb", OrbType.HarbingersOrb},
            {"Harbinger's Shard", OrbType.HarbingersShard},
            {"Horizon Shard", OrbType.HorizonShard},
            {"Mirror Shard", OrbType.MirrorShard},
            {"Orb of Annulment", OrbType.AnnulmentOrb},
            {"Orb of Binding", OrbType.BindingOrb},
            {"Orb of Horizons", OrbType.HorizonOrb},
            {"Regal Shard", OrbType.RegalShard}
        };

        #endregion

        private static readonly Dictionary<string, EssenceType> essenceMap = new Dictionary<string, EssenceType>
        {
            {"Whispering Essence of Greed", EssenceType.WhisperingGreed},
            {"Whispering Essence of Contempt", EssenceType.WhisperingContempt},
            {"Whispering Essence of Hatred", EssenceType.WhisperingHatred},
            {"Whispering Essence of Woe", EssenceType.WhisperingWoe},
            {"Muttering Essence of Greed", EssenceType.MutteringGreed},
            {"Muttering Essence of Contempt", EssenceType.MutteringContempt},
            {"Muttering Essence of Hatred", EssenceType.MutteringHatred},
            {"Muttering Essence of Woe", EssenceType.MutteringWoe},
            {"Muttering Essence of Fear", EssenceType.MutteringFear},
            {"Muttering Essence of Anger", EssenceType.MutteringAnger},
            {"Muttering Essence of Torment", EssenceType.MutteringTorment},
            {"Muttering Essence of Sorrow", EssenceType.MutteringSorrow},
            {"Weeping Essence of Greed", EssenceType.WeepingGreed},
            {"Weeping Essence of Contempt", EssenceType.WeepingContempt},
            {"Weeping Essence of Hatred", EssenceType.WeepingHatred},
            {"Weeping Essence of Woe", EssenceType.WeepingWoe},
            {"Weeping Essence of Fear", EssenceType.WeepingFear},
            {"Weeping Essence of Anger", EssenceType.WeepingAnger},
            {"Weeping Essence of Torment", EssenceType.WeepingTorment},
            {"Weeping Essence of Sorrow", EssenceType.WeepingSorrow},
            {"Weeping Essence of Rage", EssenceType.WeepingRage},
            {"Weeping Essence of Suffering", EssenceType.WeepingSuffering},
            {"Weeping Essence of Wrath", EssenceType.WeepingWrath},
            {"Weeping Essence of Doubt", EssenceType.WeepingDoubt},
            {"Wailing Essence of Greed", EssenceType.WailingGreed},
            {"Wailing Essence of Contempt", EssenceType.WailingContempt},
            {"Wailing Essence of Hatred", EssenceType.WailingHatred},
            {"Wailing Essence of Woe", EssenceType.WailingWoe},
            {"Wailing Essence of Fear", EssenceType.WailingFear},
            {"Wailing Essence of Anger", EssenceType.WailingAnger},
            {"Wailing Essence of Torment", EssenceType.WailingTorment},
            {"Wailing Essence of Sorrow", EssenceType.WailingSorrow},
            {"Wailing Essence of Rage", EssenceType.WailingRage},
            {"Wailing Essence of Suffering", EssenceType.WailingSuffering},
            {"Wailing Essence of Wrath", EssenceType.WailingWrath},
            {"Wailing Essence of Doubt", EssenceType.WailingDoubt},
            {"Wailing Essence of Loathing", EssenceType.WailingLoathing},
            {"Wailing Essence of Zeal", EssenceType.WailingZeal},
            {"Wailing Essence of Anguish", EssenceType.WailingAnguish},
            {"Wailing Essence of Spite", EssenceType.WailingSpite},
            {"Screaming Essence of Greed", EssenceType.ScreamingGreed},
            {"Screaming Essence of Contempt", EssenceType.ScreamingContempt},
            {"Screaming Essence of Hatred", EssenceType.ScreamingHatred},
            {"Screaming Essence of Woe", EssenceType.ScreamingWoe},
            {"Screaming Essence of Fear", EssenceType.ScreamingFear},
            {"Screaming Essence of Anger", EssenceType.ScreamingAnger},
            {"Screaming Essence of Torment", EssenceType.ScreamingTorment},
            {"Screaming Essence of Sorrow", EssenceType.ScreamingSorrow},
            {"Screaming Essence of Rage", EssenceType.ScreamingRage},
            {"Screaming Essence of Suffering", EssenceType.ScreamingSuffering},
            {"Screaming Essence of Wrath", EssenceType.ScreamingWrath},
            {"Screaming Essence of Doubt", EssenceType.ScreamingDoubt},
            {"Screaming Essence of Loathing", EssenceType.ScreamingLoathing},
            {"Screaming Essence of Zeal", EssenceType.ScreamingZeal},
            {"Screaming Essence of Anguish", EssenceType.ScreamingAnguish},
            {"Screaming Essence of Spite", EssenceType.ScreamingSpite},
            {"Screaming Essence of Scorn", EssenceType.ScreamingScorn},
            {"Screaming Essence of Envy", EssenceType.ScreamingEnvy},
            {"Screaming Essence of Misery", EssenceType.ScreamingMisery},
            {"Screaming Essence of Dread", EssenceType.ScreamingDread},
            {"Shrieking Essence of Greed", EssenceType.ShriekingGreed},
            {"Shrieking Essence of Contempt", EssenceType.ShriekingContempt},
            {"Shrieking Essence of Hatred", EssenceType.ShriekingHatred},
            {"Shrieking Essence of Woe", EssenceType.ShriekingWoe},
            {"Shrieking Essence of Fear", EssenceType.ShriekingFear},
            {"Shrieking Essence of Anger", EssenceType.ShriekingAnger},
            {"Shrieking Essence of Torment", EssenceType.ShriekingTorment},
            {"Shrieking Essence of Sorrow", EssenceType.ShriekingSorrow},
            {"Shrieking Essence of Rage", EssenceType.ShriekingRage},
            {"Shrieking Essence of Suffering", EssenceType.ShriekingSuffering},
            {"Shrieking Essence of Wrath", EssenceType.ShriekingWrath},
            {"Shrieking Essence of Doubt", EssenceType.ShriekingDoubt},
            {"Shrieking Essence of Loathing", EssenceType.ShriekingLoathing},
            {"Shrieking Essence of Zeal", EssenceType.ShriekingZeal},
            {"Shrieking Essence of Anguish", EssenceType.ShriekingAnguish},
            {"Shrieking Essence of Spite", EssenceType.ShriekingSpite},
            {"Shrieking Essence of Scorn", EssenceType.ShriekingScorn},
            {"Shrieking Essence of Envy", EssenceType.ShriekingEnvy},
            {"Shrieking Essence of Misery", EssenceType.ShriekingMisery},
            {"Shrieking Essence of Dread", EssenceType.ShriekingDread},
            {"Deafening Essence of Greed", EssenceType.DeafeningGreed},
            {"Deafening Essence of Contempt", EssenceType.DeafeningContempt},
            {"Deafening Essence of Hatred", EssenceType.DeafeningHatred},
            {"Deafening Essence of Woe", EssenceType.DeafeningWoe},
            {"Deafening Essence of Fear", EssenceType.DeafeningFear},
            {"Deafening Essence of Anger", EssenceType.DeafeningAnger},
            {"Deafening Essence of Torment", EssenceType.DeafeningTorment},
            {"Deafening Essence of Sorrow", EssenceType.DeafeningSorrow},
            {"Deafening Essence of Rage", EssenceType.DeafeningRage},
            {"Deafening Essence of Suffering", EssenceType.DeafeningSuffering},
            {"Deafening Essence of Wrath", EssenceType.DeafeningWrath},
            {"Deafening Essence of Doubt", EssenceType.DeafeningDoubt},
            {"Deafening Essence of Loathing", EssenceType.DeafeningLoathing},
            {"Deafening Essence of Zeal", EssenceType.DeafeningZeal},
            {"Deafening Essence of Anguish", EssenceType.DeafeningAnguish},
            {"Deafening Essence of Spite", EssenceType.DeafeningSpite},
            {"Deafening Essence of Scorn", EssenceType.DeafeningScorn},
            {"Deafening Essence of Envy", EssenceType.DeafeningEnvy},
            {"Deafening Essence of Misery", EssenceType.DeafeningMisery},
            {"Deafening Essence of Dread", EssenceType.DeafeningDread},
            {"Essence of Insanity", EssenceType.Insanity},
            {"Essence of Horror", EssenceType.Horror},
            {"Essence of Delirium", EssenceType.Delirium},
            {"Essence of Hysteria", EssenceType.Hysteria},
            {"Remnant of Corruption", EssenceType.RemnantOfCorruption}
        };

        private static readonly Dictionary<string, BreachType> breachMap = new Dictionary<string, BreachType>
        {
            {"Chayula", BreachType.Chayula},
            {"Xoph", BreachType.Xoph},
            {"Esh", BreachType.Esh},
            {"Tul", BreachType.Tul},
            {"Uul-Netol", BreachType.UulNetol}
        };

        private static readonly Dictionary<string, TabType> tabTypeMap = new Dictionary<string, TabType>
        {
            {"NormalStash", TabType.Normal},
            {"PremiumStash", TabType.Premium},
            {"CurrencyStash", TabType.Currency},
            {"DivinationCardStash", TabType.DivinationCard},
            {"EssenceStash", TabType.Essence},
            {"QuadStash", TabType.Quad}
        };

        private static readonly Dictionary<string, DivinationCardType> divinationCardMap = new Dictionary<string, DivinationCardType>
        {
            {"Abandoned Wealth", DivinationCardType.AbandonedWealth},
            {"A Mother's Parting Gift", DivinationCardType.AMothersPartingGift},
            {"Anarchy's Price", DivinationCardType.AnarchysPrice},
            {"Assassin's Favour", DivinationCardType.AssassinsFavor},
            {"Atziri's Arsenal", DivinationCardType.AtzirisArsenal},
            {"Audacity", DivinationCardType.Audacity},
            {"Birth of the Three", DivinationCardType.BirthOfTheThree},
            {"Blind Venture", DivinationCardType.BlindVenture},
            {"Boundless Realms", DivinationCardType.BoundlessRealms},
            {"Bowyer's Dream", DivinationCardType.BowyersDream},
            {"Call to the First Ones", DivinationCardType.CallToTheFirstOnes},
            {"Cartographer's Delight", DivinationCardType.CartographersDelight},
            {"Chaotic Disposition", DivinationCardType.ChaoticDisposition},
            {"Coveted Possession", DivinationCardType.CovetedPossession},
            {"Death", DivinationCardType.Death},
            {"Destined to Crumble", DivinationCardType.DestinedtoCrumble},
            {"Dialla's Subjugation", DivinationCardType.DiallasSubjugation},
            {"Doedre's Madness", DivinationCardType.DoedresMadness},
            {"Dying Anguish", DivinationCardType.DyingAnguish},
            {"Earth Drinker", DivinationCardType.EarthDrinker},
            {"Emperor of Purity", DivinationCardType.EmperorOfPurity},
            {"Emperor's Luck", DivinationCardType.EmperorsLuck},
            {"Gemcutter's Promise", DivinationCardType.GemcuttersPromise},
            {"Gift of the Gemling Queen", DivinationCardType.GiftOfTheGemlingQueen},
            {"Glimmer of Hope", DivinationCardType.GlimmerOfHope},
            {"Grave Knowledge", DivinationCardType.GraveKnowledge},
            {"Her Mask", DivinationCardType.HerMask},
            {"Heterochromia", DivinationCardType.Heterochromia},
            {"Hope", DivinationCardType.Hope},
            {"House of Mirrors", DivinationCardType.HouseOfMirrors},
            {"Hubris", DivinationCardType.Hubris},
            {"Humility", DivinationCardType.Humility},
            {"Hunter's Resolve", DivinationCardType.HuntersResolve},
            {"Hunter's Reward", DivinationCardType.HuntersReward},
            {"Jack in the Box", DivinationCardType.JackInTheBox},
            {"Lantador's Lost Love", DivinationCardType.LantadorsLostLove},
            {"Last Hope", DivinationCardType.LastHope},
            {"Light and Truth", DivinationCardType.LightAndTruth},
            {"Lingering Remnants", DivinationCardType.LingeringRemnants},
            {"Lost Worlds", DivinationCardType.LostWorlds},
            {"Loyalty", DivinationCardType.Loyalty},
            {"Lucky Connections", DivinationCardType.LuckyConnections},
            {"Lucky Deck", DivinationCardType.LuckyDeck},
            {"Lysah's Respite", DivinationCardType.LysahsRespite},
            {"Mawr Blaidd", DivinationCardType.MawrBlaidd},
            {"Merciless Armament", DivinationCardType.MercilessArmament},
            {"Might is Right", DivinationCardType.MightIsRight},
            {"Mitts", DivinationCardType.Mitts},
            {"Pride Before the Fall", DivinationCardType.PrideBeforeTheFall},
            {"Prosperity", DivinationCardType.Prosperity},
            {"Rain of Chaos", DivinationCardType.RainOfChaos},
            {"Rain Tempter", DivinationCardType.RainTempter},
            {"Rats", DivinationCardType.Rats},
            {"Scholar of the Seas", DivinationCardType.ScholarOfTheSeas},
            {"Shard of Fate", DivinationCardType.ShardofFate},
            {"Struck by Lightning", DivinationCardType.StruckbyLightning},
            {"The Aesthete", DivinationCardType.TheAesthete},
            {"The Arena Champion", DivinationCardType.TheArenaChampion},
            {"The Artist", DivinationCardType.TheArtist},
            {"The Avenger", DivinationCardType.TheAvenger},
            {"The Battle Born", DivinationCardType.TheBattleBorn},
            {"The Betrayal", DivinationCardType.TheBetrayal},
            {"The Body", DivinationCardType.TheBody},
            {"The Brittle Emperor", DivinationCardType.TheBrittleEmperor},
            {"The Calling", DivinationCardType.TheCalling},
            {"The Carrion Crow", DivinationCardType.TheCarrionCrow},
            {"The Cartographer", DivinationCardType.TheCartographer},
            {"The Cataclysm", DivinationCardType.TheCataclysm},
            {"The Catalyst", DivinationCardType.TheCatalyst},
            {"The Celestial Justicar", DivinationCardType.TheCelestialJusticar},
            {"The Chains that Bind", DivinationCardType.TheChainsThatBind},
            {"The Coming Storm", DivinationCardType.TheComingStorm},
            {"The Conduit", DivinationCardType.TheConduit},
            {"The Cursed King", DivinationCardType.TheCursedKing},
            {"The Dapper Prodigy", DivinationCardType.TheDapperProdigy},
            {"The Dark Mage", DivinationCardType.TheDarkMage},
            {"The Demoness", DivinationCardType.TheDemoness},
            {"The Doctor", DivinationCardType.TheDoctor},
            {"The Doppelganger", DivinationCardType.TheDoppelganger},
            {"The Dragon", DivinationCardType.TheDragon},
            {"The Dragon's Heart", DivinationCardType.TheDragonsHeart},
            {"The Drunken Aristocrat", DivinationCardType.TheDrunkenAristocrat},
            {"The Encroaching Darkness", DivinationCardType.TheEncroachingDarkness},
            {"The Endurance", DivinationCardType.TheEndurance},
            {"The Enlightened", DivinationCardType.TheEnlightened},
            {"The Ethereal", DivinationCardType.TheEthereal},
            {"The Explorer", DivinationCardType.TheExplorer},
            {"The Feast", DivinationCardType.TheFeast},
            {"The Fiend", DivinationCardType.TheFiend},
            {"The Fletcher", DivinationCardType.TheFletcher},
            {"The Flora's Gift", DivinationCardType.TheFlorasGift},
            {"The Formless Sea", DivinationCardType.TheFormlessSea},
            {"The Forsaken", DivinationCardType.TheForsaken},
            {"The Fox", DivinationCardType.TheFox},
            {"The Gambler", DivinationCardType.TheGambler},
            {"The Garish Power", DivinationCardType.TheGarishPower},
            {"The Gemcutter", DivinationCardType.TheGemcutter},
            {"The Gentleman", DivinationCardType.TheGentleman},
            {"The Gladiator", DivinationCardType.TheGladiator},
            {"The Harvester", DivinationCardType.TheHarvester},
            {"The Hermit", DivinationCardType.TheHermit},
            {"The Hoarder", DivinationCardType.TheHoarder},
            {"The Hunger", DivinationCardType.TheHunger},
            {"The Immortal", DivinationCardType.TheImmortal},
            {"The Incantation", DivinationCardType.TheIncantation},
            {"The Inoculated", DivinationCardType.TheInoculated},
            {"The Inventor", DivinationCardType.TheInventor},
            {"The Jester", DivinationCardType.TheJester},
            {"The King's Blade", DivinationCardType.TheKingsBlade},
            {"The King's Heart", DivinationCardType.TheKingsHeart},
            {"The Last One Standing", DivinationCardType.TheLastOneStanding},
            {"The Lich", DivinationCardType.TheLich},
            {"The Lion", DivinationCardType.TheLion},
            {"The Lord in Black", DivinationCardType.TheLordInBlack},
            {"The Lover", DivinationCardType.TheLover},
            {"The Lunaris Priestess", DivinationCardType.TheLunarisPriestess},
            {"The Mercenary", DivinationCardType.TheMercenary},
            {"The Metalsmith's Gift", DivinationCardType.TheMetalsmithsGift},
            {"The Oath", DivinationCardType.TheOath},
            {"The Offering", DivinationCardType.TheOffering},
            {"The One With All", DivinationCardType.TheOneWithAll},
            {"The Opulent", DivinationCardType.TheOpulent},
            {"The Pack Leader", DivinationCardType.ThePackLeader},
            {"The Pact", DivinationCardType.ThePact},
            {"The Penitent", DivinationCardType.ThePenitent},
            {"The Poet", DivinationCardType.ThePoet},
            {"The Polymath", DivinationCardType.ThePolymath},
            {"The Porcupine", DivinationCardType.ThePorcupine},
            {"The Queen", DivinationCardType.TheQueen},
            {"The Rabid Rhoa", DivinationCardType.TheRabidRhoa},
            {"The Risk", DivinationCardType.TheRisk},
            {"The Road to Power", DivinationCardType.TheRoadToPower},
            {"The Saint's Treasure", DivinationCardType.TheSaintsTreasure},
            {"The Scarred Meadow", DivinationCardType.TheScarredMeadow},
            {"The Scavenger", DivinationCardType.TheScavenger},
            {"The Scholar", DivinationCardType.TheScholar},
            {"The Sephirot", DivinationCardType.TheSephirot},
            {"The Sigil", DivinationCardType.TheSigil},
            {"The Siren", DivinationCardType.TheSiren},
            {"The Soul", DivinationCardType.TheSoul},
            {"The Spark and the Flame", DivinationCardType.TheSparkAndTheFlame},
            {"The Spoiled Prince", DivinationCardType.TheSpoiledPrince},
            {"The Standoff", DivinationCardType.TheStandOff},
            {"The Stormcaller", DivinationCardType.TheStormcaller},
            {"The Summoner", DivinationCardType.TheSummoner},
            {"The Sun", DivinationCardType.TheSun},
            {"The Surgeon", DivinationCardType.TheSurgeon},
            {"The Surveyor", DivinationCardType.TheSurveyor},
            {"The Survivalist", DivinationCardType.TheSurvivalist},
            {"The Thaumaturgist", DivinationCardType.TheThaumaturgist},
            {"The Throne", DivinationCardType.TheThrone},
            {"The Tower", DivinationCardType.TheTower},
            {"The Traitor", DivinationCardType.TheTraitor},
            {"The Trial", DivinationCardType.TheTrial},
            {"The Twins", DivinationCardType.TheTwins},
            {"The Tyrant", DivinationCardType.TheTyrant},
            {"The Union", DivinationCardType.TheUnion},
            {"The Valkyrie", DivinationCardType.TheValkyrie},
            {"The Valley of Steel Boxes", DivinationCardType.TheValleyOfSteelBoxes},
            {"The Vast", DivinationCardType.TheVast},
            {"The Visionary", DivinationCardType.TheVisionary},
            {"The Void", DivinationCardType.TheVoid},
            {"The Warden", DivinationCardType.TheWarden},
            {"The Warlord", DivinationCardType.TheWarlord},
            {"The Watcher", DivinationCardType.TheWatcher},
            {"The Web", DivinationCardType.TheWeb},
            {"The Wind", DivinationCardType.TheWind},
            {"The Wolf", DivinationCardType.TheWolf},
            {"The Wolf's Shadow", DivinationCardType.TheWolfsShadow},
            {"The Wolven King's Bite", DivinationCardType.TheWolvenKingsBite},
            {"The Wolverine", DivinationCardType.TheWolverine},
            {"The Wrath", DivinationCardType.TheWrath},
            {"The Wretched", DivinationCardType.TheWretched},
            {"Three Faces in the Dark", DivinationCardType.ThreeFacesInTheDark},
            {"Thunderous Skies", DivinationCardType.ThunderousSkies},
            {"Time-Lost Relic", DivinationCardType.TimeLostRelic},
            {"Tranquillity", DivinationCardType.Tranquillity},
            {"Treasure Hunter", DivinationCardType.TreasureHunter},
            {"Turn the Other Cheek", DivinationCardType.TurnTheOtherCheek},
            {"Vinia's Token", DivinationCardType.ViniasToken},
            {"Volatile Power", DivinationCardType.VolatilePower},
            {"Wealth and Power", DivinationCardType.WealthAndPower}
        };

        public static TabType GetTabType(string type)
        {
            try
            {
                return tabTypeMap.First(m => type.Contains(m.Key)).Value;
            }
            catch (Exception)
            {
                return TabType.Unknown;
            }
        }

        private static string getPropertyByName(List<JSONProxy.Property> properties, string name)
        {
            var prop = properties.Find(p => p.Name == name);

            if (prop == null)
                return string.Empty;

            return (prop.Values[0] as JArray)[0].ToString();
        }

        internal static OrbType GetOrbType(JSONProxy.Item item)
        {
            return GetOrbType(item.TypeLine);
        }

        internal static OrbType GetOrbType(string name)
        {
            try
            {
                return orbMap.First(m => name.Equals(m.Key, StringComparison.CurrentCultureIgnoreCase)).Value;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                Logger.Log("ProxyMapper.GetOrbType Failed! ItemType = " + name);

                return OrbType.Unknown;
            }
        }

        internal static EssenceType GetEssenceType(JSONProxy.Item item)
        {
            return GetEssenceType(item.TypeLine);
        }

        internal static EssenceType GetEssenceType(string name)
        {
            try
            {
                return essenceMap.First(m => name.Contains(m.Key)).Value;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                Logger.Log("ProxyMapper.GetEssenceType Failed! ItemType = " + name);

                return EssenceType.Unknown;
            }
        }

        public static BreachType GetBreachType(JSONProxy.Item item)
        {
            try
            {
                return breachMap.First(m => item.TypeLine.Contains(m.Key)).Value;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                Logger.Log("ProxyMapper.GetBreachType Failed! ItemType = " + item.TypeLine);

                return BreachType.Unknown;
            }
        }

        internal static List<Property> GetProperties(List<JSONProxy.Property> properties)
        {
            return properties.Select(p => new Property(p)).ToList();
        }

        internal static List<Requirement> GetRequirements(List<JSONProxy.Requirement> requirements)
        {
            if (requirements == null)
                return new List<Requirement>();

            return requirements.Select(r => new Requirement(r)).ToList();
        }

        internal static StackInfo GetStackInfo(List<JSONProxy.Property> list)
        {
            var stackSize = list.Find(p => p.Name == STACKSIZE);
            if (stackSize == null)
                return new StackInfo(1, 1);

            var stackInfo = getPropertyByName(list, STACKSIZE).Split('/');

            return new StackInfo(Convert.ToInt32(stackInfo[0]), Convert.ToInt32(stackInfo[1]));
        }

        internal static int GetQuality(List<JSONProxy.Property> properties)
        {
            return Convert.ToInt32(qualityRx.Match(getPropertyByName(properties, QUALITY)).Groups["quality"].Value);
        }

        internal static List<Tab> GetTabs(List<JSONProxy.Tab> tabs)
        {
            try
            {
                return tabs.Select(t => new Tab(t)).ToList();
            }
            catch (Exception ex)
            {
                Logger.Log("Error in ProxyMapper.GetTabs: " + ex);
                throw;
            }
        }

        public static ChargeInfo GetCharges(List<JSONProxy.Property> list)
        {
            try
            {
                var chargeSize = list.Find(p => p.Name == CHARGES);
                if (chargeSize == null)
                    return new ChargeInfo(0, 0);

                var qty = chargeSize.Values[0] as JArray;

                var max = chargeSize.Values[1] as JArray;

                return new ChargeInfo(int.Parse(qty.First.ToString()), int.Parse(max.First.ToString()));
            }
            catch (Exception ex)
            {
                Logger.Log("Error in ProxyMapper.GetCharges: " + ex);
            }

            return new ChargeInfo(1,1);
        }

        internal static DivinationCardType GetDivinationCardType(JSONProxy.Item item)
        {
            return GetDivinationCardType(item.TypeLine);
        }

        internal static DivinationCardType GetDivinationCardType(string name)
        {
            try
            {
                return divinationCardMap.First(m => name.Equals(m.Key, StringComparison.CurrentCultureIgnoreCase)).Value;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                Logger.Log("ProxyMapper.GetDivinationCardType Failed! ItemType = " + name);

                return DivinationCardType.Unknown;
            }
        }
    }
}