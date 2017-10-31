using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace POEApi.Model
{
    [DebuggerDisplay("Type: {Type} Stack: {StackInfo.Amount}/{StackInfo.MaxSize}")]
    public class DivinationCard : StackableItem
    {
        public DivinationCard(JSONProxy.Item item) : base(item)
        {
            ItemType = ItemType.DivinationCard;
            Type = ProxyMapper.GetDivinationCardType(item);
            FlavourText = item.FlavourText;
            ImageURL = String.Format("http://webcdn.pathofexile.com/image/gen/divination_cards/{0}.png", item.ArtFilename);

            if (Explicitmods != null)
            {
                List<string> explicitmodsNew = new List<string>();
                foreach (string mod in Explicitmods)
                {
                    String text = mod;
                    Match match = Regex.Match(text, "<.*?>\\{(?<text><.*?>\\{.*?\\})\\}", RegexOptions.Singleline);
                    if (match.Success)
                    {
                        text = match.Groups["text"].Value;
                    }
                    explicitmodsNew.AddRange(text.Split(new string[] { "\r\n" }, StringSplitOptions.None));
                }

                Explicitmods = explicitmodsNew;
            }
               
        }

        public DivinationCardType Type { get; }
        public List<string> FlavourText { get; set; }
        public String ImageURL { get; set; }

    }
}