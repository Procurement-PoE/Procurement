using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using POEApi.Model;
using POEApi.Infrastructure;

namespace Procurement.Utility
{
    internal class StashHelper
    {
        private static Dictionary<string, CroppedBitmap> imageCache = new Dictionary<string, CroppedBitmap>();
        private const int _tabImageDefaultVisibleHeight = 26;

        internal static Image GenerateTabImage(Tab tab, bool mouseOver)
        {
            // The images for the pieces of the tabs fetched from the API have the normal and selected (here, referred
            // to as being moused-over) versions stacked vertically.  If we are building the image of the selected tab,
            // we want to use the lower half of the image, so use an offset based on the height that is visible.
            int offset = mouseOver ? _tabImageDefaultVisibleHeight : 0;

            string key = tab.srcL + tab.srcC + tab.srcR + tab.Name + mouseOver.ToString();

            if (!imageCache.ContainsKey(key))
                buildImage(tab, offset, key);

            Image img = new Image();
            img.Source = imageCache[key];
            img.Tag = tab;

            return img;
        }

        private static void buildImage(Tab tab, int offset, string key)
        {
            System.Drawing.Bitmap finalImage = null;
            var images = new List<System.Drawing.Bitmap>();

            try
            {
                System.Drawing.Font font = new System.Drawing.Font(ApplicationState.FontCollection.Families[0], 11);
                int width = 0;
                int height = 0;
                int count = 0;
                float middleWidth = 0;
                foreach (Stream stream in ApplicationState.Model.GetImage(tab))
                {
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(stream);

                    if (count == 1)
                    {
                        using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(200, 200)))
                        {
                            System.Drawing.SizeF measured = g.MeasureString(tab.Name, font);
                            width += (int)measured.Width;
                            middleWidth = measured.Width;
                        }
                    }
                    else
                    {
                        width += bitmap.Width;
                    }
                    height = bitmap.Height > height ? bitmap.Height : height;
                    images.Add(bitmap);
                    count++;
                }

                finalImage = new System.Drawing.Bitmap(width, height);
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(System.Drawing.Color.Transparent);

                    //go through each image and draw it on the final image
                    int woffset = 0;
                    count = 0;
                    foreach (System.Drawing.Bitmap image in images)
                    {
                        int iwidth = image.Width;
                        if (count == 1)
                            iwidth = (int)middleWidth;
                        g.DrawImage(image, new System.Drawing.Rectangle(woffset, 0, iwidth, image.Height));
                        woffset += iwidth;
                        if (count == 1)
                            woffset -= 3; //The right image didn't align, similar to forums
                        count++;
                    }

                    g.DrawString(tab.Name, font, System.Drawing.Brushes.Yellow, images[0].Width - 2, 6); //Top
                    g.DrawString(tab.Name, font, System.Drawing.Brushes.Yellow, images[0].Width - 2, 32); //Mouse over version
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    finalImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    stream.Position = 0;
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    BitmapImage bitmapclone = (BitmapImage)bitmap.Clone();
                    bitmap = null;

                    Int32Rect croppingRectangle = new Int32Rect();
                    if (offset + _tabImageDefaultVisibleHeight > bitmapclone.Height)
                    {
                        // Something unexpected happened when fetching the tab images or piecing together the bitmap,
                        // as the final image is not as tall as expected.  This can happen when we fail to retrieve all
                        // of the parts of the tab image, since the replacement image is not tall enough.  In this
                        // case, do not use a positive offset, and make sure we do not go beyond the final image's
                        // height.
                        int truncatedHeight = Math.Min(_tabImageDefaultVisibleHeight, (int)bitmapclone.Height);
                        croppingRectangle = new Int32Rect(0, 0, (int)bitmapclone.Width, truncatedHeight);
                    }
                    else
                    {
                        croppingRectangle = new Int32Rect(0, offset, (int)bitmapclone.Width,
                            _tabImageDefaultVisibleHeight);
                    }
                    imageCache.Add(key, new CroppedBitmap(bitmapclone, croppingRectangle));
                }
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("Error while building tab image for tab {0} with key {1}: {2}", tab.Name,
                    key, ex.ToString()));

                if (finalImage != null)
                    finalImage.Dispose();

                throw;
            }
            finally
            {
                foreach (System.Drawing.Bitmap image in images)
                    image.Dispose();
            }
        }
    }
}