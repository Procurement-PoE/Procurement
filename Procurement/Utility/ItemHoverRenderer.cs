﻿using Microsoft.Win32;
using POEApi.Infrastructure;
using POEApi.Model;
using Procurement.Controls;
using Procurement.ViewModel;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Procurement.Utility
{
    internal class ItemHoverRenderer
    {
        private const string SAVE_LOCATION = "Saved Gear Images";

        internal static void SaveToDisk(Item item, Dispatcher dispatcher)
        {
            try
            {
                createSaveFolder();

                SaveFileDialog saveDialog = new SaveFileDialog();

                saveDialog.InitialDirectory = string.Format("{0}\\{1}", Environment.CurrentDirectory, SAVE_LOCATION);
                saveDialog.FileName = string.Format("{0} - {1}.png", item.Name, DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"));
                saveDialog.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
                saveDialog.FilterIndex = 2;
                saveDialog.RestoreDirectory = true;

                var result = saveDialog.ShowDialog();

                if (!result.HasValue || !result.Value)
                    return;

                var itemHover = new ItemHoverImage() { DataContext = ItemHoverViewModelFactory.Create(item) };

                itemHover.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                itemHover.Arrange(new Rect(itemHover.DesiredSize));

                dispatcher.Invoke(DispatcherPriority.Loaded, new Action(() => { }));

                RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)(itemHover.ActualWidth * 1.24), (int)(itemHover.ActualHeight * 1.24), 120, 120, PixelFormats.Pbgra32);
                renderTarget.Render(itemHover);

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTarget));

                createSaveFolder();

                using (var fileStream = File.OpenWrite(saveDialog.FileName))
                {
                    encoder.Save(fileStream);
                }

                MessageBox.Show(Procurement.MessagesRes.ImageSaved, item.Name + Procurement.MessagesRes.ImageSaved2, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.Log(Procurement.MessagesRes.UnableToSaveHoverImageToDisk + ex.ToString());
                MessageBox.Show(Procurement.MessagesRes.ErrorSavingImageErrorLoggedToDebugInfoLog, Procurement.MessagesRes.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void createSaveFolder()
        {
            if (!Directory.Exists(SAVE_LOCATION))
                Directory.CreateDirectory(SAVE_LOCATION);
        }

        private static bool displaySaveDialog()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            saveDialog.FilterIndex = 2;
            saveDialog.RestoreDirectory = true;

            var result = saveDialog.ShowDialog();

            return result.HasValue && result.Value;
        }
    }
}
