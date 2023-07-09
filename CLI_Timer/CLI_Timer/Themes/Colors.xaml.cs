using System.Windows.Media;

namespace CLI_Timer.Themes
{
    public static class Colors
    {
        public static SolidColorBrush Background { get; set; } = new();

        static Colors()
        {
            Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }
    }
}
