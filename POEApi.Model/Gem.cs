using System.Linq;
using System.Diagnostics;

namespace POEApi.Model
{
    public class Gem : SocketableItem
    {
        public int Level { get; set; }

        /// <summary>
        /// Value between 0 and 1 to show how far a gem has progressed to the next level.
        /// </summary>
        public double LevelExperienceProgress { get; private set; }

        /// <summary>
        /// Numerator to show how much experience has been gained for this level
        /// </summary>
        public int ExperienceNumerator { get; set; }

        /// <summary>
        /// Denominator to show how much experience needs to be gained for the next level.
        /// </summary>
        public int ExperienceDenominator { get; set; }

        public Gem(JSONProxy.Item item) : base(item)
        {
            Properties = ProxyMapper.GetProperties(item.Properties);
            ItemType = ItemType.Gem;

            Level = getLevel();

            ExtractGemExperience(item);
        }

        private void ExtractGemExperience(JSONProxy.Item item)
        {
            var experienceProperties = item.AdditionalProperties?.FirstOrDefault(x => x.Name == "Experience");

            if (experienceProperties == null)
            {
                return;
            }

            LevelExperienceProgress = experienceProperties.Progress;
            var experienceValues = experienceProperties.Values[0].FirstOrDefault()?.ToString();

            if (experienceValues == null || !experienceValues.Contains("/"))
            {
                return;
            }

            var numeratorAndDenominator = experienceValues.Split('/');

            int temporaryInt; 
            if(int.TryParse(numeratorAndDenominator[0], out temporaryInt))
            {
                ExperienceNumerator = temporaryInt;
            }

            if(int.TryParse(numeratorAndDenominator[1], out temporaryInt))
            {
                ExperienceDenominator = temporaryInt;
            }

            HasExperience = true;
        }

        private int getLevel()
        {
            Property levelProperty = Properties.Find(p => p.Name == "Level");
            if (levelProperty == null || levelProperty.Values.Count == 0)
                return 1;

            var levelPropertyValue = levelProperty.Values[0].Item1;
            levelPropertyValue = levelPropertyValue.Split(' ')[0];  // fixes "20 (MAX)"

            int level;
            if (!int.TryParse(levelPropertyValue, out level))
                return 1;
            
            return level;
        }

        public bool HasExperience { get; set; }
    }
}
