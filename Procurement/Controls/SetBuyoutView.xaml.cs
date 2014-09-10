using POEApi.Model;
using Procurement.ViewModel;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PWXApi;
using System.Drawing;

namespace Procurement.Controls
{
    public partial class SetBuyoutView : UserControl
    {
        private Item theItem;
        public SetBuyoutView(Item item)
        {
            InitializeComponent();
            this.DataContext = new SetBuyoutViewModel();
            this.theItem = item;
        }

        public event PricingInfoHandler Update;
        public event SaveImageHandler SaveImageClicked;
        public delegate void PricingInfoHandler(ItemTradeInfo info);
        public delegate void SaveImageHandler();

        public void Save_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = (this.DataContext as SetBuyoutViewModel);
            Update(new ItemTradeInfo(vm.BuyoutInfo.GetSaveText(), vm.PriceInfo.GetSaveText(), vm.OfferInfo.GetSaveText(), vm.Notes));
        }
        private void RemoveBuyout_Click(object sender, RoutedEventArgs e)
        {
            Update(new ItemTradeInfo());
        }

        public void SaveImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveImageClicked();
        }

        public void SetBuyoutInfo(ItemTradeInfo buyoutInfo)
        {
            (this.DataContext as SetBuyoutViewModel).SetBuyoutInfo(buyoutInfo);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            this.Notes.Text += Environment.NewLine;
            this.Notes.CaretIndex = this.Notes.Text.Length;
            e.Handled = true;
        }

        private void GetPrice_Click(object sender, RoutedEventArgs e)
        {
            ToolTip tt = (ToolTip)((Button)sender).ToolTip;
            tt.Content = (new PriceQuery(this.theItem, true, true)).getResultString();
            tt.IsOpen = true;
        }

        private void GetPrice_MouseLeave(object sender, MouseEventArgs e)
        {
            if (((Button)sender).ToolTip != null)
                ((ToolTip)((Button)sender).ToolTip).IsOpen = false;
        }

        private void CopyItemdata_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(this.theItem.ToString());
        }

    }
}
