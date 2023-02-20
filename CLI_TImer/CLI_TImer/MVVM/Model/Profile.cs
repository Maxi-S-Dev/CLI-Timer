using CLI_TImer.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI_TImer.MVVM.Model
{
    internal class Profile
    {
        public string Name { get; set; }
        public string[] Commands { get; set; }
        public string Answer { get; set; }
        public int Time { get; set; }
        public TimerType TimerType { get; set; }
    }
}
