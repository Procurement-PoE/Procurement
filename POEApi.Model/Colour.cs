using System.Windows.Media;

namespace POEApi.Model
{
    public class Colour
    {
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }

        public Color WpfColor => Color.FromRgb((byte) r,(byte) g,(byte) b); 
    }
}