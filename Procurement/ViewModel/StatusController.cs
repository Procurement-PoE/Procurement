﻿using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Procurement.ViewModel
{
    internal class StatusController
    {
        private Brush brush;
        private RichTextBox statusBox { get; set; }

        public StatusController(RichTextBox statusBox)
        {
            this.statusBox = statusBox;
            this.brush = statusBox.Foreground;
        }

        public void Ok()
        {
            CheckAccessAndInvoke(() => displayResult("OK", Brushes.Green));
        }

        public void NotOK()
        {
            CheckAccessAndInvoke(() => displayResult("ER", Brushes.Red));
        }

        public void DisplayMessage(string message)
        {
            CheckAccessAndInvoke((Action)delegate()
            {
                Run text = new Run(message);

                text.Foreground = brush;
                text.Text = "\r" + getPaddedString(text.Text);
                ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

                statusBox.ScrollToEnd();
            });
        }

        public void HandleError(string error, Action toggleControls)
        {
            CheckAccessAndInvoke((Action)delegate()
            {
                Run text = new Run();

                text.Foreground = Brushes.White;
                text.Text = "\r\r[";
                ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

                text = new Run();
                text.Foreground = Brushes.Red;
                text.Text = Procurement.MessagesRes.Error;
                ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

                text = new Run();
                text.Foreground = Brushes.White;
                text.Text = "] ";
                ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

                text = new Run(error + "\r\r");
                text.Foreground = Brushes.White;
                ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

                statusBox.ScrollToEnd();
                toggleControls();                
            });
        }

        private void displayResult(string message, Brush colour)
        {
            Run text = new Run();

            text.Foreground = Brushes.White;
            text.Text = "[";
            ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

            text = new Run();
            text.Foreground = colour;
            text.Text = message;
            ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

            text = new Run();
            text.Foreground = Brushes.White;
            text.Text = "]";

            ((Paragraph)statusBox.Document.Blocks.LastBlock).Inlines.Add(text);

            statusBox.ScrollToEnd();
        }

        private string getPaddedString(string text)
        {
            return text.PadRight(90, ' ');
        }

        private void CheckAccessAndInvoke(Action a)
        {
            if (statusBox.Dispatcher.CheckAccess())
            {
                a();
                return;
            }

            statusBox.Dispatcher.Invoke(a);
        }
    }
}