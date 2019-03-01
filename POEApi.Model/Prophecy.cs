using System.Collections.Generic;

namespace POEApi.Model
{
    public class Prophecy : Item
    {
        public string ProphecyText { get; set; }
        public string ProphecyDifficultyText { get; set; }

        internal Prophecy(JSONProxy.Item item) : base(item)
        {
            ProphecyText = item.ProphecyText;
            ProphecyDifficultyText = item.ProphecyDiffText;
        }
    }
}