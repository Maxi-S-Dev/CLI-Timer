using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CLI_Timer.MVVM.Model
{
    public class Gradient : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _startHex;
        public string StartHex { get => _startHex; 
            set 
            { 
                if (_startHex != value) 
                { 
                    _startHex = value;
                    OnPropertyChanged();
                }
            } 
        }

        private string _endHex;
        public string EndHex
        {
            get => _endHex;
            set
            {
                if (_endHex != value)
                {
                    _endHex = value;
                    OnPropertyChanged();
                }
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

        //[JsonIgnore]
        //public Color StartColor
        //{
        //    get
        //    {
        //        byte r = (byte)((StartRGB >> 16) & 0xFF);
        //        byte g = (byte)((StartRGB >> 8) & 0xFF);
        //        byte b = (byte)(StartRGB & 0xFF);
        //        return Color.FromRgb(r, g, b);
        //    }
        //    set { }
        //}

        //[JsonIgnore]
        //public Color EndColor
        //{
        //    get
        //    {
        //        byte r = (byte)((EndRGB >> 16) & 0xFF);
        //        byte g = (byte)((EndRGB >> 8) & 0xFF);
        //        byte b = (byte)(EndRGB & 0xFF);
        //        return Color.FromRgb(r, g, b);
        //    }
        //    set { }
        //}        

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

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
