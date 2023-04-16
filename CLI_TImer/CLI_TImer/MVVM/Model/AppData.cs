using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI_TImer.MVVM.Model
{
    internal class AppData
    {
        internal List<Profile> profileList { get; set; } = new();

        internal List<Gradient> gradientList { get; set; } = new();
        internal Settings settings { get; set; } = new();
         
    }
}
