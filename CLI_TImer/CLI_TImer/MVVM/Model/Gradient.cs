using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CLI_TImer.MVVM.Model
{
    public class Gradient
    {
        public int StartRGB { get; set; }
        public int EndRGB { get; set; }

        [JsonIgnore]
        public string StartHex
        {
            get
            {
                return StartColor.ToString();
            }
            set { }
        }

        [JsonIgnore]
        public string EndHex
        {
            get
            {
                return EndColor.ToString();
            }
            set { }
        }

        [JsonIgnore]
        public Color StartColor
        {
            get
            {
                byte r = (byte)((StartRGB >> 16) & 0xFF);
                byte g = (byte)((StartRGB >> 8) & 0xFF);
                byte b = (byte)(StartRGB & 0xFF);
                return System.Windows.Media.Color.FromRgb(r, g, b);
            }
            set { }
        }

        [JsonIgnore]
        public Color EndColor
        {
            get
            {
                byte r = (byte)((EndRGB >> 16) & 0xFF);
                byte g = (byte)((EndRGB >> 8) & 0xFF);
                byte b = (byte)(EndRGB & 0xFF);
                return System.Windows.Media.Color.FromRgb(r, g, b);
            }
            set { }
        }

        

        internal GradientStopCollection getGradient()
        {
            return new()
            {
                new GradientStop
                {
                    Color = StartColor,
                    Offset = 0
                },

                new GradientStop
                {
                    Color = EndColor,
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
