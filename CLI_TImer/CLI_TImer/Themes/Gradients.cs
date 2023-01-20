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
        private static byte[,] gradients = new byte[,] { {196, 113, 242, 247, 108, 198},
                                                         {95,  197, 46,  110, 238, 135},
                                                         {90,  178, 247, 18,  207, 243},
                                                         {247, 76,  6,   249, 188, 44 },
                                                         {173, 253, 162, 17,  211, 243},
                                                         {44,  178, 186, 251, 185, 45}
        
        
        };                                      

        private static byte[,] errorGradients = new byte[,] { {147, 41, 30, 237, 33, 58},
                                                            };

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
