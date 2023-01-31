using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLI_TImer.MVVM.Model;

namespace CLI_TImer.Classes
{
    internal static class ProfileSelector
    {
        private static readonly List<Profile> profiles = new List<Profile>
        {
            new Profile { Name="Work", Commands = new string[] { "work" }, Answer = "we are now working", Time = 2700, TimerType = TimerType.main },
            new Profile { Name="Pause", Commands = new string[] { "break", "pause" }, Answer = "taking a break", Time = 1200, TimerType = TimerType.second}
        };

        internal static Profile? getProfileFromCommand(string command)
        {
            foreach (Profile p in profiles)
            { 
                foreach (string s in p.Commands)
                {
                    if(command == s) return p;
                }
            }

            return null;
        }

        internal static int? getProfileTimeFromCommand(string command) 
        {
            foreach (Profile p in profiles)
            {
                foreach (string s in p.Commands)
                {
                    if (command == s) return p.Time;
                }
            }

            return null;
        }
    }
    
}