using System;
using System.Linq;
using POEApi.Infrastructure;

namespace POEApi.Model
{
    internal class ItemFactory
    {
        public static Item Get(JSONProxy.Item item)
        {
            try
            {
                item.Name = filterString(item.Name);
                item.TypeLine = filterString(item.TypeLine);

                if (!string.IsNullOrWhiteSpace(item.ProphecyText))
                    return new Prophecy(item);

                if(item.AbyssJewel)
                    return new AbyssJewel(item);

                if (item.FrameType == 4)
                    return new Gem(item);

                if (item.DescrText != null && item.DescrText.ToLower() == "right click this item then left click a location on the ground to create the object.")
                    return new Decoration(item);

                if (item.DescrText != null && string.Equals(item.DescrText,
                    "Right-click to add this to your bestiary.", StringComparison.CurrentCultureIgnoreCase))
                    return new FullBestiaryOrb(item);

                if (item.TypeLine.Contains("Leaguestone"))
                    return new Leaguestone(item);

                if (item.FrameType == 5)
                    return GetCurrency(item);

                if (item.TypeLine.Contains("Map") && item.DescrText != null && item.DescrText.Contains("Travel to this Map"))
                    return new Map(item);

                if (item.FrameType == 7)
                    return new QuestItem(item);

                if (item.FrameType == 0)
                {
                    if (item.TypeLine == "Divine Vessel")
                        return new DivineVessel(item);

                    if(item.TypeLine == "Offering to the Goddess")
                        return new Offering(item);

                    if (item.TypeLine.Contains("Scarab")) //TODO: Refactor to use category property: "category": { "maps": ["fragment", "scarab"] }
                        return new Scarab(item);
                }

                return new Gear(item);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                var errorMessage = "ItemFactory unable to instantiate type : " + item.TypeLine;
                Logger.Log(errorMessage);

                try
                {
                    // Try to fall back and create an unknownItem based off of the provided item object.  This will
                    // hopefully preserve enough properties so Procurement does not crash elsewhere and the issue is
                    // more easily debuggable.
                    var baseItemShell = new UnknownItem(item, ex.ToString());
                    Logger.Log("Successfully instantiated base item shell for item.");
                    return baseItemShell;
                }
                catch (Exception innerException)
                {
                    Logger.Log(innerException);
                    errorMessage = "Additionally, failed to instantiate base item shell for type : " + item.TypeLine;
                    Logger.Log(errorMessage);

                    return new UnknownItem();
                }
            }
        }

        private static Item GetCurrency(JSONProxy.Item item)
        {
            var typeline = item.TypeLine.ToLower();

            if (typeline.Contains("essence") || typeline.Contains("remnant of"))
                return new Essence(item);

            if (typeline.Contains("splinter of"))
                return new BreachSplinter(item);

            if (typeline.Contains("blessing"))
                return new BreachStone(item);

            if (item.TypeLine.Contains("Sextant"))
                return new Sextant(item);

            if (item.TypeLine.Contains("Net"))
                return new Net(item);

            return new Currency(item);
        }


        private static string filterString(string json)
        {
            var items = json.Split(new[] {">>"}, StringSplitOptions.None);

            if (items.Count() == 1)
                return json;

            return items[3];
        }
    }
}