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
using System.Collections.ObjectModel;

namespace CLI_TImer.Helpers
{
    public sealed class AppDataManager
    {
        private static AppDataManager? appDataManager = null;
        private static readonly object padLock = new object();

        private AppData appData;

        private readonly string path = Path.Combine(FileAccessHelper.MainDirectory(), "AppData.json");

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

        #region Color Paletts

        internal List<Gradient> GetGradientList() => appData.gradientList;

        internal void SetGradientList(List<Gradient> gradientList)
        {
            appData.gradientList = gradientList;
            SaveAppData();
        }

        #endregion

        #region AppData
        private void LoadAppData()
        {
            if (File.Exists(path))
                appData = JSONSerializer.JSONToData(File.ReadAllText(path));

            if(appData is null) 
            {
                LoadDefaultAppData();
            }
        }

        private void SaveAppData()
        {
            File.WriteAllText(path, JSONSerializer.DataToJSON(appData));
        }

        private void LoadDefaultAppData()
        {
            appData = new();

            appData.profileList = new List<Profile>
            {
                    new Profile { Name="work", Commands = new string[] { "work" }, Answer = "we are now working", Time = 2700, TimerType = TimerType.main, RingtoneDuration = 10, RingtoneEnabled = true, RingtonePath = "", NotificationText = "Your work time is over", NotificationEnabled = true },
                    new Profile { Name="pause", Commands = new string[] { "break", "pause" }, Answer = "taking a break", Time = 1200, TimerType = TimerType.second, RingtoneDuration = 10, RingtoneEnabled = true, RingtonePath = "", NotificationText = "Your break is over, let's get back!", NotificationEnabled = true}
            };

            appData.gradientList = new List<Gradient>
            {
                new Gradient { StartHex = "#C471F2", EndHex = "#F76CC6"},
                new Gradient { StartHex = "#5FC52E", EndHex = "#6EEE87"},
                new Gradient { StartHex = "#5AB2F7", EndHex = "#12CFF3"},
                new Gradient { StartHex = "#F74C06", EndHex = "#F9BC2C"},
                new Gradient { StartHex = "#ADFDA2", EndHex = "#11D3F3"},
                new Gradient { StartHex = "#2CB2BA", EndHex = "#FBB92D"}
            };

            SaveAppData();
        }
        #endregion

        #region Profile List
        internal void RemoveProfile(Profile profile)
        {
            appData.profileList.Remove(profile);
            SaveAppData();
        }

        internal void AddNewProfile(Profile profile)
        {
            appData.profileList.Add(profile);
            SaveAppData();
        }

        internal void SetProfileList(List<Profile> profileList) 
        { 
            appData.profileList = profileList;
            SaveAppData();
        }

        internal List<Profile> GetProfileList() => appData.profileList;

        #endregion
    }
}

//new Gradient { Startr = 196, Startg = 113, Startb = 242, Endr = 247, Endg = 108, Endb = 198 },
//                new Gradient { Startr = 95, Startg = 197, Startb = 46, Endr = 110, Endg = 238, Endb = 135 },
//                new Gradient { Startr = 90, Startg = 178, Startb = 247, Endr = 18, Endg = 207, Endb = 243 },
//                new Gradient { Startr = 247, Startg = 76, Startb = 6, Endr = 249, Endg = 188, Endb = 44 },
//                new Gradient { Startr = 173, Startg = 253, Startb = 162, Endr = 17, Endg = 211, Endb = 243 },
//                new Gradient { Startr = 44, Startg = 178, Startb = 186, Endr = 251, Endg = 185, Endb = 45 }

//new Gradient { StartRGB = (196 << 16) | (113 << 8) | 242, EndRGB = (247 << 16) | (108 << 8) | 198 },
//                new Gradient { StartRGB = (95  << 16) | (197 << 8) | 46, EndRGB = (110 << 16) | (238 << 8) | 135 },
//                new Gradient { StartRGB = (90  << 16) | (178 << 8) | 247, EndRGB = (18  << 16) | (207 << 8) | 243 },
//                new Gradient { StartRGB = (247 << 16) | (76  << 8) | 6, EndRGB = (249 << 16) | (188 << 8) | 44 },
//                new Gradient { StartRGB = (173 << 16) | (253 << 8) | 162, EndRGB = (17  << 16) | (211 << 8) | 243 },
//                new Gradient { StartRGB = (44  << 16) | (178 << 8) | 186, EndRGB = (251 << 16) | (185 << 8) | 45 }