using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using POEApi.Model;

namespace Procurement.Utility
{
    internal class StashHelper
    {
        private static Dictionary<string, CroppedBitmap> imageCache = new Dictionary<string, CroppedBitmap>();

        internal static Image GenerateTabImage(Tab tab, bool mouseOver)
        {
            List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
            System.Drawing.Bitmap finalImage = null;

            Image img = new Image();
            int offset = mouseOver ? 26 : 0;

            string key = tab.srcL + tab.srcC + tab.srcR + tab.Name + mouseOver.ToString();

            if (!imageCache.ContainsKey(key))
                finalImage = buildImage(tab, images, finalImage, offset, key);

            img.Source = imageCache[key];
            img.Tag = tab;

            return img;
        }

        private static System.Drawing.Bitmap buildImage(Tab tab, List<System.Drawing.Bitmap> images, System.Drawing.Bitmap finalImage, int offset, string key)
        {
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
                    imageCache.Add(key, new CroppedBitmap(bitmapclone, new Int32Rect(0, offset, (int)bitmapclone.Width, 26)));
                }
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                foreach (System.Drawing.Bitmap image in images)
                    image.Dispose();
            }
            return finalImage;
        }
    }
}