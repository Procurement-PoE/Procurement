using System.Diagnostics;

namespace Procurement.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            EssenceGenerator.Generate().ForEach(x => Debug.WriteLine(x));
        }
    }
}
