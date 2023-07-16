using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;

namespace CLI_Timer.MVVM.Model
{
    public partial class Gradient : ObservableObject
    {
        private string startHex;

        public string StartHex { get => startHex; 
            set 
            { 
                startHex = value;
                OnPropertyChanged(nameof(StartHex));
            } 
        }

        private string endHex;
        public string EndHex
        {
            get => endHex;
            set
            {
                endHex = value;
                OnPropertyChanged(nameof(EndHex));
            }
        }

        [JsonIgnore]
        public LinearGradientBrush GradientBrush
        {
            get
            {
                var gradientBrush = new LinearGradientBrush();

                gradientBrush.StartPoint = new Point(0, 0);
                gradientBrush.EndPoint = new Point(1, 0);

                gradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString(StartHex), 0));
                gradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString(EndHex), 1));

                return gradientBrush;
            }

            private set{}
        }      

        internal GradientStopCollection getGradient()
        {
            return new()
            {
                new GradientStop
                {
                    //Color = StartColor,
                    Offset = 0
                },

                new GradientStop
                {
                    //Color = EndColor,
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
