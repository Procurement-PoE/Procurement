using POEApi.Infrastructure;
using System;
using System.Linq;

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

                if(!string.IsNullOrWhiteSpace(item.ProphecyText))
                    return new Prophecy(item);

                if (item.FrameType == 4)
                    return new Gem(item);

                if (item.DescrText != null && item.DescrText.ToLower() == "right click this item then left click a location on the ground to create the object.")
                    return new Decoration(item);

                if (item.TypeLine.Contains("Leaguestone"))
                    return new Leaguestone(item);

                if (item.FrameType == 5)
                    return GetCurrency(item);

                if (item.TypeLine.Contains("Map") && item.DescrText != null && item.DescrText.Contains("Travel to this Map"))
                    return new Map(item);

                return new Gear(item);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                var errorMessage = "ItemFactory unable to instantiate type : " + item.TypeLine;
                Logger.Log(errorMessage);
                throw;
            }
        }

        private static Item GetCurrency(JSONProxy.Item item)
        {
            var typeline = item.TypeLine.ToLower();

            if (typeline.Contains("essence") || typeline.Contains("remnant of"))
                return new Essence(item);

            if (typeline.Contains("splinter of"))
                return new Splinter(item);

            if (typeline.Contains("blessing"))
                return new BreachBlessing(item);

            return new Currency(item);
        }


        private static string filterString(string json)
        {
            var items = json.Split(new string[] { ">>" }, StringSplitOptions.None);

            if (items.Count() == 1)
                return json;

            return items[3];
        }
    }
}