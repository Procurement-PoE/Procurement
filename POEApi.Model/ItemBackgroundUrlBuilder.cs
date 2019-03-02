using POEApi.Infrastructure;
using System;
using System.Collections.Generic;

namespace POEApi.Model
{
    internal static class ItemBackgroundUrlBuilder
    {
        private const string ElderBackgroundUrl = "https://www.pathofexile.com/image/inventory/ElderBackground.png?w={0}&h={1}";
        private const string ShaperBackgroundUrl = "https://www.pathofexile.com/image/inventory/ShaperBackground.png?w={0}&h={1}&x={2}&y={3}";
        private const int GridSize = 47;

        public static string GetUrl(Item item)
        {
            try
            {
                if (item.Elder)
                    return string.Format(ElderBackgroundUrl, item.W, item.H);

                if (item.Shaper)
                    return string.Format(ShaperBackgroundUrl, item.W, item.H, item.X * GridSize, item.Y * GridSize);

                return string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return string.Empty;
            }
        }
    }
}
