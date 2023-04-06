using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLI_TImer.MVVM.Model;
using System.IO;
using CLI_TImer.Classes;
using System.Diagnostics;

namespace CLI_TImer.Helpers
{
    public sealed class AppDataManager
    {
        private static AppDataManager? appDataManager = null;
        private static readonly object padLock = new object();

        private AppData appData;
        private string path = Path.Combine(FileAccessHelper.MainDirectory(), "AppData.json");

        public static AppDataManager instance
        {
            get
            {
                lock (padLock) 
                { 
                    if(appDataManager== null)
                        appDataManager = new AppDataManager();
                }
                return appDataManager;
            }
        }

        public AppDataManager()
        {
            LoadAppData();
            Trace.WriteLine(appData.settings.standardTime);
        }

        private void LoadAppData()
        {
            if (File.Exists(path))
                appData = JSONSerializer.JSONToData(File.ReadAllText(path));

            if(appData is null) 
            {
                LoadDefaultppData();
            }
        }

        private void SaveAppData()
        {
            File.WriteAllText(path, JSONSerializer.DataToJSON(appData));
        }

        internal List<Profile> getProfileList() => appData.profileList;

        private void LoadDefaultppData()
        {
            appData = new();

            appData.profileList = new List<Profile>
                {
                    new Profile { Name="work", Commands = new string[] { "work" }, Answer = "we are now working", Time = 2700, TimerType = TimerType.main },
                    new Profile { Name="pause", Commands = new string[] { "break", "pause" }, Answer = "taking a break", Time = 1200, TimerType = TimerType.second}
                };

            appData.settings = new Settings()
            { standardTime=1550 };

            SaveAppData();
        }

        internal void AddNewProfile(Profile profile)
        {
            appData.profileList.Add(profile);
            SaveAppData();
        }


        internal void DeleteProfile(Profile profile)
        {
            appData.profileList.Remove(profile);
            SaveAppData();
        }

        internal int GetStandardTime() => appData.settings.standardTime;

        internal void SetStandardTime(int time)
        {
            appData.settings.standardTime = time;
            SaveAppData();
        }
        
    }
}
