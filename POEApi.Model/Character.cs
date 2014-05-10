namespace POEApi.Model
{
    public class Character
    {
        public string Name { get; set; }
        public string League { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }

        public Character(JSONProxy.Character character)
        {
            this.Name = character.Name;
            this.League = character.League;
            this.Class = character.Class;
            this.Level = character.Level;
        }
    }
}
