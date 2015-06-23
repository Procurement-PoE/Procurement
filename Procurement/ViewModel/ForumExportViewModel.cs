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
            selected = preferenceManager.SetTabsAndGetsSelected(StashItems);
            updateText();
        }

        private void bumpThread(object obj)
        {
            if (!settingsValid(false))
                return;

            var confirmation = MessageBox.Show(Procurement.MessagesRes.AreYouSureYouWantToBumpYourThread, Procurement.MessagesRes.ConfirmShopUpdate, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmation != MessageBoxResult.Yes)
                return;

            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var threadBumped = ApplicationState.Model.BumpThread(Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadId, Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadTitle);

                        if (threadBumped)
                            MessageBox.Show(Procurement.MessagesRes.ShopThreadSuccessfullyBumped, Procurement.MessagesRes.ThreadBumped, MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                            MessageBox.Show(Procurement.MessagesRes.ErrorBumpingShopThreadDetailsLoggedToDebuginfoLog, Procurement.MessagesRes.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (ForumThreadException)
                    {
                        MessageBox.Show(Procurement.MessagesRes.TheThreadTitleSuppliedInYourSettingsDoesNotMatch, Procurement.MessagesRes.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
        }

        private void postToThread(object obj)
        {
            if (!settingsValid(true))
                return;

            var confirmation = MessageBox.Show(Procurement.MessagesRes.AreYouSureYouWantToUpdateYourShop, Procurement.MessagesRes.ConfirmShopUpdate, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmation != MessageBoxResult.Yes)
                return;

            if (!text.Contains("[url=https://github.com/medvedttn/Procurement][img]http://i.imgur.com/f1bzioI.png[/img][/url]"))
                text += Environment.NewLine + Environment.NewLine + "[url=https://github.com/medvedttn/Procurement][img]http://i.imgur.com/f1bzioI.png[/img][/url]";


            Task.Factory.StartNew(() =>
              {
                  var shopUpdated = ApplicationState.Model.UpdateThread(Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadId, Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadTitle, text);

                  if (shopUpdated)
                      MessageBox.Show(Procurement.MessagesRes.ShopSuccessfullyUpdated, Procurement.MessagesRes.ShopUpdated, MessageBoxButton.OK, MessageBoxImage.Information);
                  else
                      MessageBox.Show(Procurement.MessagesRes.ErrorUpdatingShopDetailsLoggedToDebuginfoLog, Procurement.MessagesRes.Error, MessageBoxButton.OK, MessageBoxImage.Error);
              });
        }

        private bool settingsValid(bool isUpdate)
        {
            if (!Settings.ShopSettings.ContainsKey(ApplicationState.CurrentLeague) || string.IsNullOrEmpty(Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadId) || string.IsNullOrEmpty(Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadTitle))
            {
                MessageBox.Show(Procurement.MessagesRes.NoShopSettingsFoundForCurrentLeague, Procurement.MessagesRes.SettingsNotFound, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (isUpdate && selected.Count() == 0)
            {
                MessageBox.Show(Procurement.MessagesRes.YouNeedToSelectAtLeastOneTabToUpdateYourShop, Procurement.MessagesRes.NoTabsSelected, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            int threadId;
            if (!int.TryParse(Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadId, out threadId))
            {
                MessageBox.Show(Procurement.MessagesRes.InvalidThreadIdTheThreadIdIsTheNumber, Procurement.MessagesRes.InvalidThreadId, MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (!ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs.Exists(tab_item => tab_item.Name == "Manually Selected Tab"))
            {
                //add new virtual manual selection tab
                Tab ManuallySelectedTab = new Tab();
                ManuallySelectedTab.Hidden = true;
                ManuallySelectedTab.i = 999;
                ManuallySelectedTab.IsFakeTab = true;
                ManuallySelectedTab.Name = "Manually Selected Tab";
                ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs.Add(ManuallySelectedTab);
            }

            var tabs = ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs;
            StashItems = tabs.Select(t => new TabInfo() { Name = t.Name, Url = t.srcC, ID = t.i }).ToList();
            StashItems.ForEach(s => s.FixName());
        }

        public void update(int key, bool isChecked, bool shouldUpdate)
        {
            if (isChecked && !selected.Contains(key))
                selected.Add(key);
            else if (!isChecked)
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
                MessageBox.Show(string.Format(Procurement.MessagesRes.ShopTextIs0CharactersWhichExceedsThe50000CharacterLimit, count), Procurement.MessagesRes.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
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
