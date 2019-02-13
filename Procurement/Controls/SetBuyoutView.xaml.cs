using POEApi.Model;
using Procurement.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Procurement.Controls
{
    public partial class SetBuyoutView : UserControl
    {
        private SetBuyoutViewModel viewModel;

        public SetBuyoutView(Item item)
        {
            InitializeComponent();

            viewModel = new SetBuyoutViewModel(item);

            this.DataContext = viewModel;
        }

        public event PricingInfoHandler Update;
        public event SaveImageHandler SaveImageClicked;
        public delegate void PricingInfoHandler(ItemTradeInfo info);
        public delegate void SaveImageHandler();

        public void Save_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            Update(new ItemTradeInfo(viewModel.BuyoutInfo.GetSaveText(), viewModel.PriceInfo.GetSaveText(), viewModel.OfferInfo.GetSaveText(), viewModel.Notes));
        }

        public void SaveImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveImageClicked();
        }

        public void Timestamp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Notes.Text += DateTime.Now + Environment.NewLine;
        }

        public void SetBuyoutInfo(ItemTradeInfo buyoutInfo)
        {
            viewModel.SetBuyoutInfo(buyoutInfo);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            this.Notes.Text += Environment.NewLine;
            this.Notes.CaretIndex = this.Notes.Text.Length;
            e.Handled = true;
        }
    }
}
