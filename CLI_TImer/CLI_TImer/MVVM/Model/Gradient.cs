using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CLI_TImer.MVVM.Model
{
    internal class Gradient
    {
        public byte Startr { get; set; }
        public byte Startg { get; set; }
        public byte Startb { get; set; }
        
        public byte Endr { get; set; }
        public byte Endg { get; set; }
        public byte Endb { get; set; }

        internal GradientStopCollection getGradient()
        {
            return new()
            {
                new GradientStop
                {
                    Color = Color.FromRgb(Startr, Startg, Startb),
                    Offset = 0
                },

                new GradientStop
                {
                    Color = Color.FromRgb(Endr, Endg, Endb),
                    Offset = 1
                },
            };
        }
    }
}
