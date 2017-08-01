using System.Collections.Generic;

namespace POEApi.Model
{
    public class Prophecy : Item
    {
        public string ProphecyText { get; set; }
        public List<string> FlavourText { get; set; }
        public string ProphecyDifficultyText { get; set; }

        internal Prophecy(JSONProxy.Item item) : base(item)
        {
            this.ProphecyText = item.ProphecyText;
            this.FlavourText = item.FlavourText;
            this.ProphecyDifficultyText = item.ProphecyDiffText;
        }

        protected override int getConcreteHash()
        {
            return new { id = Id }.GetHashCode();
        }
    }
}