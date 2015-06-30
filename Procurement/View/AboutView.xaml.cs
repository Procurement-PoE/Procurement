using System.Windows.Controls;

namespace Procurement.View
{
    public partial class AboutView : UserControl, IView
    {
        public AboutView()
        {
            InitializeComponent();
            this.Version.Content = ApplicationState.Version+" (Medved Edition)";

            if (ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                txtOpenSource.Text = "Программное обеспечение с открытым исходным кодом предоставляется безвозмездно!";
                txtForTheLatestUpdates.Text = "Для загрузки последних версий ПО, сообщений об ошибках и предложениях: https://github.com/medvedttn/Procurement";
                txtLegal.Text = "Авторские права:";
                txtLicensed.Text = "Тип лицензии Artistic License 2.0";
                txtCopyrights.Text = "Торговые знаки принадлежат соответствующим правообладателям. Все права защищены. © Grinding Gear Games.© Garena Online.\r\nhttps://www.pathofexile.com, https://web.poe.garena.ru";
                txtAsIs.Text =
"ДАННОЕ ПРОГРАММНОЕ ОБЕСПЕЧЕНИЕ ПРЕДОСТАВЛЯЕТСЯ АВТОРОМ НА УСЛОВИЯХ «КАК ЕСТЬ» "+
"БЕЗ ПРЯМЫХ И ПОДРАЗУМЕВАЕМЫХ ГАРАНТИЙ, ВКЛЮЧАЯ, СРЕДИ ПРОЧЕГО, "+
"ПОДРАЗУМЕВАЕМЫЕ ГАРАНТИИ ГОДНОСТИ К ПРОДАЖЕ И ПРИГОДНОСТИ К КОНКРЕТНОЙ ЦЕЛИ. "+
"НИ ПРИ КАКИХ ОБСТОЯТЕЛЬСТВАХ АВТОР НЕ БУДЕТ НЕСТИ ОТВЕТСТВЕННОСТЬ ЗА "+
"КАКИЕ-ЛИБО ПРЯМЫЕ, НЕПРЯМЫЕ, ПОБОЧНЫЕ, ФАКТИЧЕСКИЕ, ШТРАФНЫЕ ИЛИ КОСВЕННЫЕ "+
"УБЫТКИ (ВКЛЮЧАЯ, СРЕДИ ПРОЧЕГО, ПРИОБРЕТЕНИЕ ЗАМЕНЯЮЩИХ ТОВАРОВ ИЛИ УСЛУГ, "+
"НЕВОЗМОЖНОСТЬ ИСПОЛЬЗОВАНИЯ, УТРАТУ ДАННЫХ ИЛИ ПРИБЫЛИ, А ТАКЖЕ ПРЕРЫВАНИЕ "+
"ДЕЯТЕЛЬНОСТИ) НЕЗАВИСИМО ОТ УСЛОВИЙ ИХ ВОЗНИКНОВЕНИЯ.";
            }
        }

        public new Grid Content
        {
            get { return this.ViewContent; }
        }

        private void DonateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //System.Diagnostics.Process.Start("https://github.com/medvedttn/Procurement/");            
        }
    }
}
