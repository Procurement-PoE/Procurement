using System.Collections.Generic;
using POEApi.Model;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class IGNVistior : IVisitor
    {
        private const string TOKEN = "{IGN}";

        public string Visit(IEnumerable<Item> items, string current)
        {
            string name = ApplicationState.CurrentCharacter.Name;

            if (ApplicationState.CurrentLeague == Settings.UserSettings["FavoriteLeague"] && !string.IsNullOrEmpty(Settings.UserSettings["FavoriteCharacter"]))
                name = Settings.UserSettings["FavoriteCharacter"];

            return current.Replace(TOKEN, name);
        }
    }
}
