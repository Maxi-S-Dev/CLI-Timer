using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CLI_TImer.Themes
{
    public static class Gradients
    {
        private static byte[,] gradients = new byte[,] { {147, 41, 30, 237, 33, 58},
                                                          {0, 0, 0, 255, 255, 255 } };



        static Random Random = new Random();
        public static GradientStopCollection GradientStops()
        {
            int gradientID = Random.Next(gradients.GetLength(0));

            return new() 
            {
                new GradientStop
                {
                    Color = Color.FromRgb(gradients[gradientID, 0], gradients[gradientID, 1], gradients[gradientID, 2]),
                    Offset = 0
                },

                new GradientStop
                {
                    Color = Color.FromRgb(gradients[gradientID, 3], gradients[gradientID, 4], gradients[gradientID, 5]),
                    Offset = 1
                },
            };
        }
    }
}
