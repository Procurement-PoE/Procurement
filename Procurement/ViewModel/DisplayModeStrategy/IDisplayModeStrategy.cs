using System.Windows.Documents;
using POEApi.Model;

namespace Procurement.ViewModel
{
    internal interface IDisplayModeStrategy
    {
        Block Get();
    }
}
