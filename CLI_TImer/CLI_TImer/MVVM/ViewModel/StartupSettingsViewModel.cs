using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using IWshRuntimeLibrary;
using System.Text.RegularExpressions;
using CLI_TImer.Classes;
using CLI_TImer.MVVM.Model;
using CLI_TImer.Helpers;

namespace CLI_TImer.MVVM.ViewModel
{
    public partial class StartupSettingsViewModel: ObservableObject
    {
        [ObservableProperty,]
        public bool shortcutExits;

        [ObservableProperty]
        public string hoursText;

        [ObservableProperty]
        public string minutesText;

        [ObservableProperty]
        public string secondsText;


        private int hours;
        private int minutes;
        private int seconds;

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == nameof(HoursText))
            {

                if (e.PropertyName == nameof(HoursText))
                {

                    bool success = int.TryParse(HoursText.Split('h')[0], out hours);
                    if(!success) success = int.TryParse(HoursText.Split(' ')[0], out hours);

                    if (!success)
                    {
                        RestoreDefaultValues();
                        return;
                    }

                    AppDataManager.instance.SetStandardTime(Times.TimeToSeconds(hours, minutes, seconds));
                }
            }

            else if (e.PropertyName == nameof(MinutesText))
            {

                if (e.PropertyName == nameof(MinutesText))
                {

                    bool success = int.TryParse(MinutesText.Split('m')[0], out minutes);
                    if (!success) success = int.TryParse(MinutesText.Split(' ')[0], out minutes);

                    if (!success)
                    {
                        RestoreDefaultValues();
                        return;
                    }

                    AppDataManager.instance.SetStandardTime(Times.TimeToSeconds(hours, minutes, seconds));
                }
            }

            else if (e.PropertyName == nameof(SecondsText))
            {

                if (e.PropertyName == nameof(SecondsText))
                {

                    bool success = int.TryParse(SecondsText.Split('s')[0], out seconds);
                    if (!success) success = int.TryParse(SecondsText.Split(' ')[0], out seconds);

                    if (!success)
                    {
                        RestoreDefaultValues();
                        return;
                    }

                    AppDataManager.instance.SetStandardTime(Times.TimeToSeconds(hours, minutes, seconds));
                }
            }

            RestoreDefaultValues();

            RestoreDefaultValues();
        }

        private void RestoreDefaultValues()
        {
            hours = Times.SecondsToHours(AppDataManager.instance.GetStandardTime());
            minutes = Times.SecondsToMinutes(AppDataManager.instance.GetStandardTime());
            seconds = AppDataManager.instance.GetStandardTime() % 60;

            HoursText = hours + " h";
            MinutesText = minutes + " m";
            SecondsText = seconds + " s";
        }

        public StartupSettingsViewModel()
        {
            shortcutExits = false;
            RestoreDefaultValues();

            if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\CLI_TImer.lnk")) ShortcutExits = true;
        }

        [RelayCommand]
        public void ToggleStartupSetting()
        {

            if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\CLI_TImer.lnk"))
            {
                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\CLI_TImer.lnk");
                return;
            }
            CreateShortCut();
        }

        private void CreateShortCut()
        {
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\CLI_TImer.lnk";
            string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CLI_TImer.exe");

            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

            shortcut.Description = "My Application";
            shortcut.TargetPath = targetPath;
            shortcut.Save();
        }

         
    }
}
