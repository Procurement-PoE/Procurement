using System.Collections.Generic;

namespace Procurement.ViewModel.Filters
{
    public class ResistanceBase
    {
        protected List<IFilter> resistances;
        
        public ResistanceBase()
        {
            resistances = new List<IFilter>();
            resistances.Add(new FireResistance());
            resistances.Add(new ColdResistance());
            resistances.Add(new LightningResistance());
            resistances.Add(new ChaosResistance());
        }
    }
}
