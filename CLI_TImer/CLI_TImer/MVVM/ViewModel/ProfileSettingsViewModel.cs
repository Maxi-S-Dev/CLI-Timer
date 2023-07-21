using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using CLI_Timer.MVVM.Model;
using CLI_Timer.Services;
using CLI_Timer.Utils;
using System.ComponentModel;

namespace CLI_Timer.MVVM.ViewModel
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

            PropertyChanged += (s, e) =>
            {
                Trace.WriteLine($"property changed {e.PropertyName}");
                if (e.PropertyName != nameof(SettingsProfile.IsExpanded)) return;

                Save();
            };
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

                p.PropertyChanged += ProfileChanged;

                Profiles.Add(p);
            }
        }

        private void ProfileChanged(object sender, PropertyChangedEventArgs e)
        {
            Save();
        }

        private void PopulateTimerTypeList()
        {
             TimerTypes = Enum.GetValues(typeof(TimerType)).Cast<TimerType>().ToList();
        }

        public void Save()
        {
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

                profileList.Add(profile);
            }

            Trace.WriteLine("Saving");
            AppDataManager.instance.SetProfileList(profileList);
        }

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
        
        [RelayCommand]
        public void AddProfile()
        {
            SettingsProfile s = new();
            Profiles.Add(s);
        }

        public void ToggleButtonClick(SettingsProfile clickedItem)
        {
            foreach(var profile in Profiles) 
            {
                profile.IsExpanded = (profile == clickedItem);
            }
        }

        public void DeleteItem(SettingsProfile item)
        {
            if(Profiles.Contains(item)) Profiles.Remove(item);
            Save();
        }
    }

    public partial class SettingsProfile : ObservableObject, IProfile
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string answer;

        [ObservableProperty]
        private TimerType timerType;

        [ObservableProperty]
        private bool isExpanded;


        public int hours { get; set; }
        public int minutes { get; set; }
        public int seconds { get; set; }
        public int ringtoneSeconds { get; set; }
        public int ringtoneMinutes { get; set; }

        public string Hours
        {
            get => $"{hours}h";
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
                OnPropertyChanged(nameof(Hours));
            }
        }
        public string Minutes
        {   
            get => $"{minutes}m";
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
                OnPropertyChanged(nameof(Minutes));
            }
        }
        public string Seconds
        {
            get => $"{seconds}s";
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
                OnPropertyChanged(nameof(Seconds));
            }
        }

        [ObservableProperty]
        private string ringtonePath;
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

        [ObservableProperty]
        private bool ringtoneEnabled;

        [ObservableProperty]
        private string notificationText;

        [ObservableProperty]
        private bool notificationEnabled;

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

