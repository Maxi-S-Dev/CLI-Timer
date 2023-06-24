using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IWshRuntimeLibrary;

using CLI_TImer.Utils;

namespace CLI_TImer.MVVM.ViewModel
{
    public partial class GernalSettingsViewModel: ObservableObject
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

        //Applys the time text and formats the output text
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
                }
            }

            Properties.Settings.Default.DefaultTime = TimeConverter.TimeToSeconds(hours, minutes, seconds);
            Properties.Settings.Default.Save();
            RestoreDefaultValues();
        }

        private void RestoreDefaultValues()
        {
            hours = TimeConverter.SecondsToHours(Properties.Settings.Default.DefaultTime);
            minutes = TimeConverter.SecondsToMinutes(Properties.Settings.Default.DefaultTime);
            seconds = Properties.Settings.Default.DefaultTime % 60;

            HoursText = hours + " h";
            MinutesText = minutes + " m";
            SecondsText = seconds + " s";
        }

        public GernalSettingsViewModel()
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
