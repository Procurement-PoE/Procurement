using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class AccurayFilter : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Attacks; }
        }

        public AccurayFilter()
            : base("Increased Accuracy", "Increased Accuracy", "Accuracy")
        { }
    }
}
