using System.Collections.Generic;
using POEApi.Model;
using System;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class LastUpdatedVisitor : IVisitor
    {
        private const string TOKEN = "{LastUpdated}";

        public string Visit(IEnumerable<Item> items, string current)
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                return current.Replace(TOKEN, DateTime.Now.ToString("f", System.Globalization.CultureInfo.CreateSpecificCulture("ru-RU")));
            }
            else
            {
                return current.Replace(TOKEN, DateTime.Now.ToString("f", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
            }
        }
    }
}
