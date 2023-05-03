using CLI_TImer.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI_TImer.MVVM.Model
{
    internal class Profile : IProfile
    {
        public string Name { get; set; }
        public string[] Commands { get; set; }
        public string Answer { get; set; }
        public int Time { get; set; }
        public TimerType TimerType { get; set; }

        public string RingtonePath { get; set; }

        public Profile Copy()
        {
            return (Profile)this.MemberwiseClone();
        }
    }

    public interface IProfile 
    { 
        string Name { get; set; }
        string Answer { get; set; }
        TimerType TimerType { get; set; }
    }
}
