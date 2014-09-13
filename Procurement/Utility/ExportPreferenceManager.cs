using POEApi.Infrastructure;
using Procurement.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Procurement.Utility
{
    internal class ExportPreferenceManager
    {
        private const string FILENAME = "tabprefs.xml";
        private Dictionary<string, List<int>> leagueSelectedTabs;
        private XElement xmlDoc;

        private List<int> selectedTabs
        {
            get
            {
                if (!leagueSelectedTabs.ContainsKey(ApplicationState.CurrentLeague))
                    leagueSelectedTabs[ApplicationState.CurrentLeague] = new List<int>();

                return leagueSelectedTabs[ApplicationState.CurrentLeague];
            }
        }

        internal ExportPreferenceManager()
        {
            leagueSelectedTabs = new Dictionary<string, List<int>>();
            leagueSelectedTabs.Add(ApplicationState.CurrentLeague, new List<int>());

            loadPreferences();
        }

        internal List<int> SetTabsAndGetsSelected(List<TabInfo> tabs)
        {
            try
            {
                foreach (var tab in tabs)
                    if (selectedTabs.Contains(tab.ID))
                        tab.IsChecked = true;

                return selectedTabs;
            }
            catch (Exception ex)
            {
                Logger.Log("Error setting selected tabs: " + ex.ToString());                
                return new List<int>();
            }
        }

        internal void UpdateTabSelection(TabInfo tabInfo)
        {
            try
            {
                if (tabInfo.IsChecked && !selectedTabs.Contains(tabInfo.ID))
                    selectedTabs.Add(tabInfo.ID);

                if (!tabInfo.IsChecked && selectedTabs.Contains(tabInfo.ID))
                    selectedTabs.Remove(tabInfo.ID);

                savePreferences();
            }
            catch (Exception ex)
            {
                Logger.Log("Error updating tab selection: " + ex.ToString());
            }
        }

        private void savePreferences()
        {
            try
            {
                xmlDoc.RemoveNodes();

                foreach (var set in leagueSelectedTabs)
                    foreach (var id in set.Value)
                        xmlDoc.Add(new XElement("TabPreference", new XAttribute("League", set.Key), new XAttribute("Id", id)));

                xmlDoc.Save(FILENAME);
            }
            catch (Exception ex)
            {
                Logger.Log("Error saving tab preferences: " + ex.ToString());
            }
        }

        private void loadPreferences()
        {
            try
            {
                if (!File.Exists(FILENAME))
                {
                    this.xmlDoc = new XElement("TabPreferences");
                    return;
                }

                this.xmlDoc = XElement.Load(FILENAME);

                var prefs = xmlDoc.Elements("TabPreference");

                foreach (var league in prefs.Select(x => x.Attribute("League").Value).Distinct())
                    if (!leagueSelectedTabs.ContainsKey(league))
                        leagueSelectedTabs[league] = new List<int>();

                foreach (var preference in prefs)
                {
                    if (!leagueSelectedTabs.ContainsKey(preference.Attribute("League").Value))
                        leagueSelectedTabs[preference.Attribute("League").Value] = new List<int>();

                    leagueSelectedTabs[preference.Attribute("League").Value].Add(Convert.ToInt32(preference.Attribute("Id").Value));
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error loading tab preferences: " + ex.ToString());
            }
        }
    }
}
