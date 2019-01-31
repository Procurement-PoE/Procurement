using System.Linq;
using System.Diagnostics;

namespace POEApi.Model
{
    public class Gem : SocketableItem
    {
        public int Level { get; set; }
        public double LevelProgress { get; private set; }
        public int Numerator { get; set; }
        public int Denominator { get; set; }

        public Gem(JSONProxy.Item item) : base(item)
        {
            this.Properties = ProxyMapper.GetProperties(item.Properties);
            this.ItemType = ItemType.Gem;

            this.Level = getLevel();

            ExtractGemExperience(item);
        }

        private void ExtractGemExperience(JSONProxy.Item item)
        {
            var experienceProperties = item.AdditionalProperties.FirstOrDefault(x => x.Name == "Experience");

            if (experienceProperties == null)
            {
                return;
            }

            LevelProgress = experienceProperties.Progress;
            var experienceValues = experienceProperties.Values[0].FirstOrDefault()?.ToString();

            if (experienceValues == null || !experienceValues.Contains("/"))
            {
                return;
            }

            var numeratorAndDenominator = experienceValues.Split('/');

            Numerator = int.Parse(numeratorAndDenominator[0]);
            Denominator = int.Parse(numeratorAndDenominator[1]);
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
