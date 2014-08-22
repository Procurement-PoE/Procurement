using POEApi.Infrastructure;
using System;

namespace POEApi.Model
{
    internal class ItemFactory
    {
        public static Item Get(JSONProxy.Item item)
        {
            try
            {
                if (item.frameType == 4)
                    return new Gem(item);

                if (item.DescrText != null && item.DescrText.ToLower() == "right click this item then left click a location on the ground to create the object.")
                    return new Decoration(item);

                if (item.frameType == 5)
                    return new Currency(item);

                if (item.TypeLine.Contains("Map") && item.DescrText != null && item.DescrText.Contains("Travel to this Map"))
                    return new Map(item);

                return new Gear(item);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                var errorMessage = "ItemFactory unable to instanciate type : " + item.TypeLine;
                Logger.Log(errorMessage);
                throw new Exception(errorMessage);
            }
        }
    }
}
