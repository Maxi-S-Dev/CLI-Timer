using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI_Timer.MVVM.Model
{
    internal class AppData
    {
        internal List<Profile> profileList { get; set; } = new();

        internal List<Gradient> gradientList { get; set; } = new();         
    }
}
