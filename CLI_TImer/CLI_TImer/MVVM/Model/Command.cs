using System.Windows.Media;

namespace CLI_TImer.MVVM.Model
{
    public  class Command
    {
        public string? title { get; set; }
        public string? answer{ get; set; }
        public string? output { get; set; }


        public GradientStopCollection? gradientStops { get; set; }
    }
}
