using System.Collections.Generic;

namespace Procurement.ViewModel.Filters
{
    public class ResistanceBase
    {
        protected List<StatFilter> resistances;
        
        public ResistanceBase()
        {
            resistances = new List<StatFilter>();
            resistances.Add(new FireResistance());
            resistances.Add(new ColdResistance());
            resistances.Add(new LightningResistance());
            resistances.Add(new ChaosResistance());
        }
    }
}
