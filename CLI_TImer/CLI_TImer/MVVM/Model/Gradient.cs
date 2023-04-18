using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CLI_TImer.MVVM.Model
{
    public class Gradient
    {
        public byte Startr { get; set; }
        public byte Startg { get; set; }
        public byte Startb { get; set; }
        
        public byte Endr { get; set; }
        public byte Endg { get; set; }
        public byte Endb { get; set; }

        internal string StartHex
        {
            get
            {
                return getStartColor().ToString();
            }
            private set { }
        }

        internal string EndHex
        {
            get
            {
                return getEndColor().ToString();
            }
            private set { }
        }

        internal Color getStartColor()
        {
            return Color.FromRgb(Startr, Startg, Startb);
        }

        internal Color getEndColor()
        {
            return Color.FromRgb(Endr, Endg, Endb);
        }

        internal GradientStopCollection getGradient()
        {
            return new()
            {
                new GradientStop
                {
                    Color = getStartColor(),
                    Offset = 0
                },

                new GradientStop
                {
                    Color = getEndColor(),
                    Offset = 1
                },
            };
        }

        internal Gradient Copy()
        {
            return (Gradient)this.MemberwiseClone();
        }
    }
}
