using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Procurement.ViewModel.Cache
{
    public class BitmapCache
    {
        private readonly Func<string, Stream> _imageRetriveFunc;
        private readonly Dictionary<string, BitmapImage> _cache = new Dictionary<string, BitmapImage>();

        public int ImageCount
        {
            get { return _cache.Count; }
        }

        public BitmapCache(Func<string, Stream> imageRetriveFunc)
        {
            _imageRetriveFunc = imageRetriveFunc;
        }

        public BitmapImage this[string imageUrl]
        {
            get
            {
                BitmapImage bitmap;

                if (_cache.TryGetValue(imageUrl, out bitmap))
                {
                    return bitmap;
                }

                bitmap = CreateBitmapImage(_imageRetriveFunc(imageUrl));

                _cache[imageUrl] = bitmap;

                return bitmap;
            }
        }

        public BitmapImage GetByLocalUrl(string url)
        {
            BitmapImage bitmap;

            if (_cache.TryGetValue(url, out bitmap))
            {
                return bitmap;
            }

            bitmap = CreateBitmapImage(url);

            _cache[url] = bitmap;

            return bitmap;
        }

        private static BitmapImage CreateBitmapImage(Stream stream)
        {
            var bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.StreamSource = stream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }

        private static BitmapImage CreateBitmapImage(string url)
        {
            var bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }
    }
}
