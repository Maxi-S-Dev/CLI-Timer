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
        public int RingtoneDuration { get; set; }
        public bool RingtoneEnabled { get; set; }

        public string NotificationText { get; set; } = string.Empty;
        public bool NotificationEnabled { get; set; } = false;

        public Profile Copy()
        {
            return (Profile)this.MemberwiseClone();
        }
    }

    public interface IProfile 
    { 
        string Name { get; set; }
        string Answer { get; set; }

        string NotificationText { get ; set; }
        bool NotificationEnabled { get; set; }

        string RingtonePath { get; set; }
        bool RingtoneEnabled { get; set; } 
        TimerType TimerType { get; set; }
    }
}
