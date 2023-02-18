using System;
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

        internal ProfileManager()
        {
            Trace.WriteLine("df");
            LoadProfileList();
        }

        internal static Profile? getProfileFromCommand(string command)
        {
            if (ProfileList == null) ProfileList = new List<Profile>
            {
                new Profile { Name="work", Commands = new string[] { "work" }, Answer = "we are now working", Time = 2700, TimerType = TimerType.main },
                new Profile { Name="pause", Commands = new string[] { "break", "pause" }, Answer = "taking a break", Time = 1200, TimerType = TimerType.second}
            };
                

            foreach (Profile p in ProfileList)
            {
                if (p.Name == command) return p;                
            }

            return null;
        }

        internal static void AddNewProfile(string Name, string answer, int Time, string Type)
        {
            TimerType tp = TimerType.main;
            if(Type == "second") tp = TimerType.second;
            ProfileList.Add(new Profile { Name = Name, Answer = answer, Time = Time, TimerType = tp });

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

        internal static void UpdateProfile(string Name, string Property, string Value)
        {
            Profile? p = getProfileFromCommand(Name);

            if (p is null) return;

            ProfileList.Remove(p);

            switch (Property)
            {
                case "name":
                    p.Name = Value; 
                    break;

                case "answer":
                    p.Answer = Value;
                    break;
            }

            ProfileList.Add(p);
            SaveProfileList();
        }

        internal static void UpdateProfile(string Name, int Time)
        {
            Profile? p = getProfileFromCommand(Name);

            if (p is null) return;

            ProfileList.Remove(p);

            p.Time = Time;

            ProfileList.Add(p);
            SaveProfileList();
        }

        private static void SaveProfileList()
        { 
            string json = JSONSerializer.ListToJSON(ProfileList);

            File.WriteAllText(Path.Combine(FileAccessHelper.MainDirectory(), "Profiles.json"), json);
        }

        private static void LoadProfileList()
        {
            string path = (Path.Combine(FileAccessHelper.MainDirectory(), "Profiles.json"));
            if(File.Exists(path))
                ProfileList = JSONSerializer.JSONToList(File.ReadAllText(path));
        }
    }
}