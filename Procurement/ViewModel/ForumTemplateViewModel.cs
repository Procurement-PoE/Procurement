using System.ComponentModel;
using Procurement.Controls;

namespace Procurement.ViewModel
{
    internal class ForumTemplateViewModel : ObservableBase
    {
        public ForumTemplateViewModel()
        {
            this.Text = ForumExportTemplateReader.GetTemplate(null);
            ForumExportTemplateReader.OnTemplateReloaded += new PropertyChangedEventHandler(ForumExportTemplateReader_OnTemplateReloaded);
        }

        void ForumExportTemplateReader_OnTemplateReloaded(object sender, PropertyChangedEventArgs e)
        {
            this.Text = sender.ToString();
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }

        internal void Save()
        {
            ForumExportTemplateReader.SaveTemplate(Text);
        }
    }
}
