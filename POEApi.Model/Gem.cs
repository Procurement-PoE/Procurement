using System.Linq;
using System.Diagnostics;

namespace POEApi.Model
{
    public class Gem : SocketableItem
    {
        public int Level { get; set; }

        public Gem(JSONProxy.Item item) : base(item)
        {
            this.Properties = ProxyMapper.GetProperties(item.Properties);
            this.ItemType = ItemType.Gem;

            this.Level = getLevel();
        }

        private int getLevel()
        {
            int level;
            var levelProperty = Properties.Find(p => p.Name == "Level").Values[0].Item1;
            levelProperty = levelProperty.Split(' ')[0]; //fixes "20 (MAX)"

            if (!int.TryParse(levelProperty, out level))
                return 1;
            
            return level;
        }
    }
}
