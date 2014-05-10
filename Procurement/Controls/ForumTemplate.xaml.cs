using System.Windows.Controls;
using Procurement.ViewModel;
using System.Windows;

namespace Procurement.Controls
{
    public partial class ForumTemplate : UserControl
    {
        public ForumTemplate()
        {
            InitializeComponent();
            this.DataContext = new ForumTemplateViewModel();
            TemplateText.AcceptsReturn = true;
            //DataObject.AddPastingHandler(TemplateText, new DataObjectPastingEventHandler(OnPaste));
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.DataContext as ForumTemplateViewModel).Save();
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(System.Windows.DataFormats.Text, true);
            if (!isText) 
                return;

            var text = e.SourceDataObject.GetData(DataFormats.Text) as string;

        }
    }
}