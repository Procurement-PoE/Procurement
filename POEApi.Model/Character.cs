using Newtonsoft.Json;

namespace POEApi.Model
{
    public class Character
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("league")]
        public string League { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("classId")]
        public int ClassId { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }
        
        [JsonProperty("expired")]
        public bool Expired { get; set; }
    }
}