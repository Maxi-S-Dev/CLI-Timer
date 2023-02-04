﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLI_TImer.MVVM.Model;
using CLI_TImer.Helpers;
using System.IO;
using System.Diagnostics;

namespace CLI_TImer.Classes
{
    internal class ProfileManager
    {
        private static List<Profile> ProfileList;
        //{
        //    new Profile { Name="work", Commands = new string[] { "work" }, Answer = "we are now working", Time = 2700, TimerType = TimerType.main },
        //    new Profile { Name="pause", Commands = new string[] { "break", "pause" }, Answer = "taking a break", Time = 1200, TimerType = TimerType.second}
        //};

        internal ProfileManager()
        {
            Trace.WriteLine("df");
            LoadProfileList();
        }

        internal static Profile? getProfileFromCommand(string command)
        {
            foreach (Profile p in ProfileList)
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
            ProfileList.Add(new Profile { Name = Name, Commands = Commands, Time = Time, TimerType = tp });

            SaveProfileList();
        }

        internal static void DeleteProfile(string Name) 
        {
            foreach(Profile p in ProfileList) 
            {
                if (p.Name == Name)
                {
                    ProfileList.Remove(p);
                    break;
                }
            }

            SaveProfileList();
        }

        private static void SaveProfileList()
        { 
            string json = JSONSerializer.ListToJSON(ProfileList);

            File.WriteAllText(Path.Combine(FileAccessHelper.MainDirectory(), "Profiles.json"), json);
        }

        private static void LoadProfileList()
        {
            ProfileList = JSONSerializer.JSONToList(File.ReadAllText(Path.Combine(FileAccessHelper.MainDirectory(), "Profiles.json")));
        }
    }
}