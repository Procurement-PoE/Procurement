using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using POEApi.Model;
using System.Windows.Input;
using System.Windows;
using Procurement.ViewModel.ForumExportVisitors;
using System.Reflection;
using Procurement.Controls;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace Procurement.ViewModel
{
    public class ForumExportViewModel : INotifyPropertyChanged
    {
        private List<TabInfo> stashItems;
        private List<int> selected = new List<int>();
        private string text;
        private static List<IVisitor> visitors = null;

        private DelegateCommand copyCommand;

        public DelegateCommand CopyCommand
        {
            get { return copyCommand; }
            set { copyCommand = value; }
        }

        public List<string> AvailableTemplates { get; private set; }

        private string currentTemplate;
        public string CurrentTemplate
        {
            get { return currentTemplate; }
            set
            {
                currentTemplate = value;
                onPropertyChanged("CurrentTemplate");
                Text = getFinal(selected.SelectMany(sid => ApplicationState.Stash[ApplicationState.CurrentLeague].GetItemsByTab(sid))
                                                              .OrderBy(id => id.Y).ThenBy(i => i.X));
            }
        }

        public List<TabInfo> StashItems
        {
            get { return stashItems; }
            set 
            { 
                stashItems = value;
                onPropertyChanged("StashItems");
            }
        }

        public string Text
        {
            get { return text; }
            set 
            { 
                text = value;
                onPropertyChanged("Text");
            }
        }
        
        public ForumExportViewModel()
        {
            copyCommand = new DelegateCommand(copy);
            updateForLeague();
            ApplicationState.LeagueChanged += new PropertyChangedEventHandler(ApplicationState_LeagueChanged);
            visitors = visitors ?? getVisitors();
            AvailableTemplates = new List<string>();
            AvailableTemplates.Add("ForumExportTemplate.txt");
            if (Settings.Lists.ContainsKey("AdditionalTemplates"))
                AvailableTemplates.AddRange(Settings.Lists["AdditionalTemplates"]);

            CurrentTemplate = "ForumExportTemplate.txt";
        }

        private void copy(object parameter)
        {
            if (text != null)
                Clipboard.SetDataObject(text);
        }

        void ApplicationState_LeagueChanged(object sender, PropertyChangedEventArgs e)
        {
            updateForLeague();
        }

        private void updateForLeague()
        {
            var space = ApplicationState.Stash[ApplicationState.CurrentLeague].CalculateFreeSpace();
            space.Remove("All");
            var betterSpace = space.ToDictionary(k => int.Parse(k.Key.Replace("Stash", "")) - 1, k => k.Value); ;

            var tabs = ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs;

            StashItems = tabs.Where(t => betterSpace.ContainsKey(t.i)).Select(t => new TabInfo() { AvailableSpace = betterSpace[t.i], Name = t.Name, Url = t.srcC, ID = t.i }).ToList();
            StashItems.ForEach(s => s.FixName());
        }

        public void update(int key, bool isChecked, bool shouldUpdate)
        {
            if (isChecked)
                selected.Add(key);
            else
                selected.Remove(key);

            if (shouldUpdate)
                updateText();
        }

        public void updateText()
        {
            Text = getFinal(selected.SelectMany(sid => ApplicationState.Stash[ApplicationState.CurrentLeague].GetItemsByTab(sid))
                                                              .OrderBy(id => id.Y).ThenBy(i => i.X));

            var count = Text.Count();
            if (count > 50000)
                MessageBox.Show(string.Format("Shop text is {0} characters, which exceeds the 50,000 character limit on the pathofexile.com forum!", count), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private string getFinal(IEnumerable<Item> items)
        {
            string template = ForumExportTemplateReader.GetTemplate(CurrentTemplate);

            foreach (IVisitor visitor in visitors)
                template = visitor.Visit(items, template);

            template = doPostProcessing(template);

            return template;
            
        }

        private string doPostProcessing(string template)
        {
            string current = template;
            List<int> toRemove = getLinesToRemove(current);
            while (toRemove.Count > 0)
            {
                current = removeLines(current, toRemove);
                toRemove = getLinesToRemove(current);
            }

            return current;
        }

        private string removeLines(string template, List<int> removeLines)
        {
            List<string> lines = readAllLines(template);

            for (int i = removeLines.Count - 1; i > -1; i--)
            {
                lines.RemoveAt(removeLines[i]);
            }

            return string.Join(Environment.NewLine, lines);
        }
        private List<int> getLinesToRemove(string template)
        {
            string start = @"\[spoiler=[\s\S]*?\]";
            string end = @"\[/spoiler\]";

            List<string> lines = readAllLines(template);
            List<int> exludeLines = new List<int>();

            int startLine = -1;
            int endLine = -1;

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                if (Regex.IsMatch(line, start))
                    startLine = i;

                if (Regex.IsMatch(line, end))
                    endLine = i;

                if (endLine == -1)
                    continue;

                bool shouldRemove = true;
                for (int j = startLine + 1; j < endLine; j++)
                {
                    if (lines[j].Trim() != string.Empty)
                    {
                        shouldRemove = false;
                        break;
                    }
                }

                if (shouldRemove && startLine > 0 && endLine > 0)
                    exludeLines.AddRange(Enumerable.Range(startLine, endLine - startLine + 1));

                startLine = -1;
                endLine = -1;
            }

            return exludeLines;
        }
        public List<string> readAllLines(string template)
        {
            List<string> list = new List<string>();
            using (StreamReader reader = new StreamReader(new MemoryStream(Encoding.Default.GetBytes(template)), Encoding.Default))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    list.Add(str);
                }
            }
            return list;
        }

        private static List<IVisitor> getVisitors()
        {
            Type visitorType = typeof(IVisitor);
            return Assembly.GetAssembly(visitorType).GetTypes()
                                                    .Where(t => !(t.IsAbstract || t.IsInterface) && visitorType.IsAssignableFrom(t))
                                                    .Select(t => Activator.CreateInstance(t) as IVisitor)
                                                    .ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        internal void ToggleAll(bool value)
        {
            stashItems.ForEach(si => si.IsChecked = value);
        }
    }
}
