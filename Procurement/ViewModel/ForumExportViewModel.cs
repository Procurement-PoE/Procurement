using POEApi.Infrastructure;
using POEApi.Model;
using Procurement.Controls;
using Procurement.Utility;
using Procurement.ViewModel.ForumExportVisitors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Procurement.ViewModel
{
    public class ForumExportViewModel : INotifyPropertyChanged
    {
        private ExportPreferenceManager preferenceManager;
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

        private DelegateCommand postToThreadCommand;
        public DelegateCommand PostToThreadCommand
        {
            get { return postToThreadCommand; }
            set { postToThreadCommand = value; }
        }

        private DelegateCommand bumpThreadCommand;
        public DelegateCommand BumpThreadCommand
        {
            get { return bumpThreadCommand; }
            set { bumpThreadCommand = value; }
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

        public bool LoggedIn { get { return !ApplicationState.Model.Offline; } }

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
            postToThreadCommand = new DelegateCommand(postToThread);
            bumpThreadCommand = new DelegateCommand(bumpThread);

            preferenceManager = new ExportPreferenceManager();

            updateForLeague();

            ApplicationState.LeagueChanged += new PropertyChangedEventHandler(ApplicationState_LeagueChanged);
            visitors = visitors ?? getVisitors();

            AvailableTemplates = new List<string>();
            AvailableTemplates.Add("ForumExportTemplate.txt");

            if (Settings.Lists.ContainsKey("AdditionalTemplates"))
                AvailableTemplates.AddRange(Settings.Lists["AdditionalTemplates"]);

            currentTemplate = "ForumExportTemplate.txt";

            setSelectedTabs();
            registerTabEvents();
        }

        private void registerTabEvents()
        {
            foreach (var tab in StashItems)
                tab.PropertyChanged += tabSelectionChanged;
        }

        private void deregisterTabEvents()
        {
            foreach (var tab in StashItems)
                tab.PropertyChanged -= tabSelectionChanged;
        }

        private void tabSelectionChanged(object sender, PropertyChangedEventArgs e)
        {
            preferenceManager.UpdateTabSelection(sender as TabInfo);
        }

        private void setSelectedTabs()
        {
            preferenceManager.SetSelectedTabs(StashItems);
        }

        private void bumpThread(object obj)
        {
            if (!settingsValid(false))
                return;

            var confirmation = MessageBox.Show("Are you sure you want to bump your thread? By clicking yes you grant permission for Procurement to bump your forum thread using the account you logged in with, and confirm that you are only bumping within the allowed time interval as per forum rules.", "Confirm shop update", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmation != MessageBoxResult.Yes)
                return;

            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var threadBumped = ApplicationState.Model.BumpThread(Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadId, Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadTitle);

                        if (threadBumped)
                            MessageBox.Show("Shop thread successfully bumped!", "Thread bumped", MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                            MessageBox.Show("Error bumping shop thread, details logged to debuginfo.log", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (ForumThreadException)
                    {
                        MessageBox.Show("The thread title supplied in your settings does not match the title of the thread Procurement tried to bump with the threadId in your settings. Check that your settings are correct", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
        }

        private void postToThread(object obj)
        {
            if (!settingsValid(true))
                return;

            var confirmation = MessageBox.Show("Are you sure you want to update your shop? By clicking yes you grant permission for Procurement to update your forum thread using the account you logged in with.", "Confirm shop update", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmation != MessageBoxResult.Yes)
                return;

            if (!text.Contains("[url=https://code.google.com/p/procurement/][img]http://i.imgur.com/ZHBMImo.png[/img][/url]"))
                text += Environment.NewLine + Environment.NewLine + "[url=https://code.google.com/p/procurement/][img]http://i.imgur.com/ZHBMImo.png[/img][/url]";


            Task.Factory.StartNew(() =>
              {
                  var shopUpdated = ApplicationState.Model.UpdateThread(Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadId, Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadTitle, text);

                  if (shopUpdated)
                      MessageBox.Show("Shop successfully updated!", "Shop updated", MessageBoxButton.OK, MessageBoxImage.Information);
                  else
                      MessageBox.Show("Error updating shop, details logged to debuginfo.log", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
              });
        }

        private bool settingsValid(bool isUpdate)
        {
            if (!Settings.ShopSettings.ContainsKey(ApplicationState.CurrentLeague) || string.IsNullOrEmpty(Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadId) || string.IsNullOrEmpty(Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadTitle))
            {
                MessageBox.Show("No shop settings found for current league, please configure your ThreadId and ThreadTitle under the TradeSettings tab", "Settings not found!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (isUpdate && selected.Count() == 0)
            {
                MessageBox.Show("You need to select at least one tab to update your shop!", "No tabs selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            int threadId;
            if (!int.TryParse(Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadId, out threadId))
            {
                MessageBox.Show("Invalid ThreadId, the ThreadId is the number at the end of the url of your shop, eg: the 12345 in http://www.pathofexile.com/forum/view-thread/12345", "Invalid ThreadId!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }


        private void copy(object parameter)
        {
            if (text != null)
                Clipboard.SetDataObject(text);
        }

        void ApplicationState_LeagueChanged(object sender, PropertyChangedEventArgs e)
        {
            deregisterTabEvents();

            updateForLeague();

            setSelectedTabs();
            registerTabEvents();
        }

        private void updateForLeague()
        {
            var tabs = ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs;
            StashItems = tabs.Select(t => new TabInfo() { Name = t.Name, Url = t.srcC, ID = t.i }).ToList();
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
