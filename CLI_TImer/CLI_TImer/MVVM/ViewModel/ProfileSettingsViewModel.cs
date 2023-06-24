using CLI_TImer.Classes;
using CLI_TImer.Helpers;
using CLI_TImer.MVVM.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace CLI_TImer.MVVM.ViewModel
{
    internal partial class ProfileSettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<SettingsProfile> profiles;

        [ObservableProperty]
        internal List<TimerType> timerTypes;

        [ObservableProperty]
        public SettingsProfile selectedProfile;

        private int selectedProfileIndex { get { return Profiles.IndexOf(SelectedProfile); } }

        internal ProfileSettingsViewModel()
        {
            PopulateProfilesList();
            PopulateTimerTypeList();
        }

        //Creates a List that contains the values for the UI
        private void PopulateProfilesList()
        {
            Profiles = new ObservableCollection<SettingsProfile>();

            foreach (var profile in AppDataManager.instance.GetProfileList())
            {
                SettingsProfile p = new SettingsProfile();

                p.Name = profile.Name;
                p.Answer = profile.Answer;
                p.TimerType = profile.TimerType;

                p.Hours = $"{TimeConverter.SecondsToHours(profile.Time)} h";
                p.Minutes = $"{TimeConverter.SecondsToMinutes(profile.Time)} m";
                p.Seconds = $"{profile.Time % 60} s";

                p.RingtoneMinutes = $"{TimeConverter.SecondsToMinutes(profile.RingtoneDuration)} m";
                p.RingtoneSeconds = $"{profile.RingtoneDuration % 60} s";

                p.RingtonePath = profile.RingtonePath;
                p.RingtoneEnabled = profile.RingtoneEnabled;

                p.NotificationText = profile.NotificationText;
                p.NotificationEnabled = profile.NotificationEnabled;

                Profiles.Add(p);
            }
        }

        private void PopulateTimerTypeList()
        {
             TimerTypes = Enum.GetValues(typeof(TimerType)).Cast<TimerType>().ToList();
        }

        [RelayCommand]
        public void SaveChanges()
        {
            Trace.WriteLine("Save");
            List<Profile> profileList = new();

            foreach (var p in Profiles)
            {
                Profile profile = new Profile();

                profile.Name = p.Name;
                profile.Answer = p.Answer;
                profile.TimerType = p.TimerType;
                profile.Time = TimeConverter.TimeToSeconds(p.hours, p.minutes, p.seconds);
                profile.RingtonePath = p.RingtonePath;
                profile.RingtoneDuration = TimeConverter.TimeToSeconds(0, p.ringtoneMinutes, p.ringtoneSeconds);
                profile.RingtoneEnabled = p.RingtoneEnabled; 
                profile.NotificationText = p.NotificationText;
                profile.NotificationEnabled = p.NotificationEnabled;
                
                Trace.WriteLine(p.Name + "RingtoneEnabled" + p.RingtoneEnabled);


                profileList.Add(profile);
            }

            AppDataManager.instance.SetProfileList(profileList);
        }

        //public IEnumerable<TimerType> TimerTypeValues
        //{
        //    get
        //    {
        //        return Enum.GetValues(typeof(TimerType))
        //            .Cast<TimerType>();
        //    }
        //}

        [RelayCommand]
        public void SearchFileExplorerForAudio()
        {
            Trace.WriteLine("Open Explorer");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files (*.wav;*.mp3)|*.wav;*.mp3|All files (*.*)|*.*";
            if(openFileDialog.ShowDialog() == true)
            {
                profiles[selectedProfileIndex].RingtonePath = openFileDialog.FileName;
                Trace.WriteLine(openFileDialog.FileName);
            }
        }
    }

    public class SettingsProfile : IProfile
    {
        public string Name { get; set; } = "";
        public string Answer { get; set; } = "";

        TimerType _type;
        public TimerType TimerType
        {
            get { return _type; }
            set
            {
                _type = value;
            }
        }


        public int hours { get; set; }
        public int minutes { get; set; }
        public int seconds { get; set; }
        public int ringtoneSeconds { get; set; }
        public int ringtoneMinutes { get; set; }

        public string Hours
        {
            get => $"{hours} h";
            set
            {
                int h;
                bool success = int.TryParse(value.Split('h')[0], out h);
                if (!success) success = int.TryParse(value.Split(' ')[0], out h);

                if (!success)
                {
                    return;
                }

                hours = h;
            }
        }
        public string Minutes
        {   
            get => $"{minutes} m";
            set
            {
                int m;
                bool success = int.TryParse(value.Split('m')[0], out m);
                if (!success) success = int.TryParse(value.Split(' ')[0], out m);

                if (!success)
                {
                    return;
                }

                if (m > 60)
                {
                    int s = TimeConverter.MinutesToSeconds(m);
                    m = TimeConverter.SecondsToMinutes(s);
                }
                minutes = m;
            }
        }
        public string Seconds
        {
            get => $"{seconds} s";
            set
            {
                int s;
                bool success = int.TryParse(value.Split('s')[0], out s);
                if (!success) success = int.TryParse(value.Split(' ')[0], out s);

                if (!success)
                {
                    return;
                }

                if (s > 60)
                {
                    s %= 60;
                }

                seconds = s;
            }
        }

        public string RingtonePath { get; set; } = "";
        public string DisplayRingtonePath 
        { 
            get 
            {
                if (string.IsNullOrEmpty(RingtonePath)) return string.Empty;
                string[] pathPieces = RingtonePath.Split(@"\");
                return pathPieces[0] + @"\...\" + pathPieces[pathPieces.Length -2] + @"\" + pathPieces[pathPieces.Length-1];
                    
            } 

            set { RingtonePath = value; }
        }

        public string RingtoneMinutes
        {
            get => $"{ringtoneMinutes} m";
            set
            {
                int m;
                bool success = int.TryParse(value.Split('m')[0], out m);
                if (!success) success = int.TryParse(value.Split(' ')[0], out m);

                if (!success)
                {
                    return;
                }

                if (m > 60)
                {
                    int s = TimeConverter.MinutesToSeconds(m);
                    m = TimeConverter.SecondsToMinutes(s);
                }
                ringtoneMinutes = m;
            }
        }
        public string RingtoneSeconds
        {
            get => $"{ringtoneSeconds} s";
            set
            {
                int s;
                bool success = int.TryParse(value.Split('s')[0], out s);
                if (!success) success = int.TryParse(value.Split(' ')[0], out s);

                if (!success)
                {
                    return;
                }

                if (s > 60)
                {
                    s %= 60;
                }

                ringtoneSeconds = s;
            }
        }

        public bool RingtoneEnabled { get; set; }

        public string NotificationText { get; set; }
        public bool NotificationEnabled { get; set; }

        public IEnumerable<TimerType> TimerTypeValues
        {
            get
            {
                return Enum.GetValues(typeof(TimerType))
                    .Cast<TimerType>();
            }
        }
    }
}

