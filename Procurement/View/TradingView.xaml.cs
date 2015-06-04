using System.Windows.Controls;
using Procurement.ViewModel;

namespace Procurement.View
{
    public partial class TradingView : UserControl, IView
    {
        public TradingView()
        {
            InitializeComponent();
            this.DataContext = new TradingViewModel();
            if (ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                tabForumExport.Header = "Экспорт на форум";
                tabForumTemplate.Header = "Шаблон форума";
                tabTradeSettings.Header = "Настройки торговли";
            }
        }

        public new Grid Content
        {
            get { return this.ViewContent; }
        }
    }
}
