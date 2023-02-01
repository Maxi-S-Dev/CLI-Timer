using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLI_TImer.MVVM.Model;

namespace CLI_TImer.Classes
{
    internal static class ProfileManager
    {
        private static readonly List<Profile> profiles = new List<Profile>
        {
            new Profile { Name="work", Commands = new string[] { "work" }, Answer = "we are now working", Time = 2700, TimerType = TimerType.main },
            new Profile { Name="pause", Commands = new string[] { "break", "pause" }, Answer = "taking a break", Time = 1200, TimerType = TimerType.second}
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

        internal static void AddNewProfile(string Name, string[] Commands, int Time, string Type)
        {
            TimerType tp = TimerType.main;
            if(Type == "second") tp = TimerType.second;
            profiles.Add(new Profile { Name = Name, Commands = Commands, Time = Time, TimerType = tp });
        }
    }
}