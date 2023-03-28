using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLI_TImer.MVVM.Model;
using System.IO;
using CLI_TImer.Classes;

namespace CLI_TImer.Helpers
{
    public sealed class AppDataManager
    {
        private static AppDataManager? appDataManager = null;
        private static readonly object padLock = new object();

        private AppData? appData;
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

        }

        private void LoadAppData()
        {
            if (File.Exists(path))
                appData = JSONSerializer.JSONToData(File.ReadAllText(path));

            if(appData is null) 
            {
                appData = new();

                appData.profileList = new List<Profile>
                {
                    new Profile { Name="work", Commands = new string[] { "work" }, Answer = "we are now working", Time = 2700, TimerType = TimerType.main },
                    new Profile { Name="pause", Commands = new string[] { "break", "pause" }, Answer = "taking a break", Time = 1200, TimerType = TimerType.second}
                };
            }
        }


        internal List<Profile> getProfileList() => appData.profileList;
    }
}
