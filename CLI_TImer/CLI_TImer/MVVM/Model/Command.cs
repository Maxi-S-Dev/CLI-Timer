using System.Windows.Media;

namespace CLI_Timer.MVVM.Model
{
    public  class Command
    {
        public string? title { get; set; }
        public string? answer{ get; set; }

        public GradientStopCollection? GradientStops { get; set; }
    }
}
