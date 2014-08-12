namespace POEApi.Model
{
    public class Tab
    {
        public bool IsFakeTab { get; set; }
        public string Name { get; set; }
        public int i { get; set; }
        public Colour Colour { get; set; }
        public string srcL { get; set; }
        public string srcC { get; set; }
        public string srcR { get; set; }
    }

    public class Colour
    {
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
    }
}
