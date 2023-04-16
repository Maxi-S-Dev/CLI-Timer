using CLI_TImer.Classes;
using CLI_TImer.Helpers;
using CLI_TImer.MVVM.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
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
        internal List<SettingsProfile> profiles;

        [ObservableProperty]
        internal List<TimerType> timerTypes;
        internal ProfileSettingsViewModel()
        {
            PopulateProfilesList();
            PopulateTimerTypeList();
            Trace.Write("");
        }

        //Creates a List that contains the values for the UI
        private void PopulateProfilesList()
        {
            Profiles = new List<SettingsProfile>();

            foreach (var profile in AppDataManager.instance.GetProfileList())
            {
                SettingsProfile p = new SettingsProfile();

                p.Name = profile.Name;
                p.Answer = profile.Answer;
                p.TimerType = profile.TimerType;

                p.Hours = $"{Times.SecondsToHours(profile.Time)} h";
                p.Minutes = $"{Times.SecondsToMinutes(profile.Time)} m";
                p.Seconds = $"{profile.Time % 60} s";


                Profiles.Add(p);
            }
        }

        private void PopulateTimerTypeList()
        {
            timerTypes = Enum.GetValues(typeof(TimerType)).Cast<TimerType>().ToList();
        }

        [RelayCommand]
        public void SaveChanges()
        {
            List<Profile> profileList = new();

            foreach (var p in Profiles)
            {
                Profile profile = new Profile();

                profile.Name = p.Name;
                profile.Answer = p.Answer;
                profile.TimerType = p.TimerType;
                profile.Time = Times.TimeToSeconds(p.hours, p.minutes, p.seconds);

                profileList.Add(profile);
            }

            AppDataManager.instance.SetProfileList(profileList);

        }

        public IEnumerable<TimerType> TimerTypeValues
        {
            get
            {
                return Enum.GetValues(typeof(TimerType))
                    .Cast<TimerType>();
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
                    int s = Times.MinutesToSeconds(m);
                    m = Times.SecondsToMinutes(s);
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

