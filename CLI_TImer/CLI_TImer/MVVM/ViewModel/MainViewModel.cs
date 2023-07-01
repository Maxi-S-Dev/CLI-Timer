using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Linq;
using System.Windows.Media;

using Microsoft.Toolkit.Uwp.Notifications;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CLI_Timer.Utils;
using CLI_Timer.Services;
using CLI_Timer.MVVM.View;
using CLI_Timer.MVVM.Model;


namespace CLI_Timer.MVVM.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        #region variables

        //Settings
        SettingsWindow settingsWindow;

        [ObservableProperty]
        public string primaryTimerText = "0h 0m 0s";

        [ObservableProperty]
        public string secondaryTimerText = "";

        public string PauseTimerText = "";

        //Inputs
        [ObservableProperty]
        public string enteredCommand = string.Empty;

        [ObservableProperty]
        public ObservableCollection<Command> commandHistory = new();


        //Code
        private int pausePosition;


        Profile? mainRunningProfile;
        Profile? secondaryRunningProfile;

        Random random = new();

        public virtual Dispatcher Dispatcher { get; protected set; }

        AppDataManager dataManager = AppDataManager.instance;

        #endregion

        public MainViewModel()
        {
            Timer.SetTimer(Properties.Settings.Default.DefaultTime);
        }

        public void UpdateTimers()
        {
            PrimaryTimerText = $"{TimeConverter.SecondsToHours(Timer.TimerSeconds[0])}h {TimeConverter.SecondsToMinutes(Timer.TimerSeconds[0])}m {Timer.TimerSeconds[0] % 60}s";
            SecondaryTimerText = Timer.TimerSeconds[1] == 0 ? "" : $"{TimeConverter.SecondsToHours(Timer.TimerSeconds[1])}h {TimeConverter.SecondsToMinutes(Timer.TimerSeconds[1])}m {Timer.TimerSeconds[1] % 60}s";
        }

        public void MainTimerFinished()
        {
            if(mainRunningProfile.RingtoneEnabled == false) return;
            if (string.IsNullOrEmpty(mainRunningProfile.RingtonePath))
            {
                SoundPlayer.playSound(@"C://Windows/Media/Alarm04.wav", mainRunningProfile.RingtoneDuration);
            }
            else
            {
                SoundPlayer.playSound(mainRunningProfile.RingtonePath, mainRunningProfile.RingtoneDuration);
            }

            if(mainRunningProfile.NotificationEnabled == true) 
            {
                new ToastContentBuilder()
                    .AddText(mainRunningProfile.Name + " finished")
                    .AddText(mainRunningProfile.NotificationText)
                    .AddButton(new ToastButton()
                        .SetContent("Stop"))
                    .AddButton(new ToastButton()
                        .SetContent("Add 5m"))
                    .Show();
            }
        }

        public void SecondaryTimerFinished()
        {
            if (secondaryRunningProfile.RingtoneEnabled == false) return;
            if (string.IsNullOrEmpty(secondaryRunningProfile.RingtonePath))
            {
                SoundPlayer.playSound(@"C://Windows/Media/Alarm08.wav", secondaryRunningProfile.RingtoneDuration);
            }
            else
            {
                SoundPlayer.playSound(secondaryRunningProfile.RingtonePath, secondaryRunningProfile.RingtoneDuration);
            }

            if (secondaryRunningProfile.NotificationEnabled == true)
            {
                new ToastContentBuilder()
                .AddText(secondaryRunningProfile.Name + " finished")
                .AddText(secondaryRunningProfile.NotificationText)
                .AddButton(new ToastButton()
                    .SetContent("Stop"))
                .AddButton(new ToastButton()
                    .SetContent("Add 5m"))
                .Show();
            }
        }


        //Input Commands
        [RelayCommand]
        public void Send()
        {
            string? answer = CommandExecutor.Execute(EnteredCommand);
            if (EnteredCommand.Split(' ')[0] != "clear") AddToHistory(EnteredCommand, answer, "");
            EnteredCommand = "";
        }

        #region commands
        private void CheckCommand(string _command)
        {
            int hours, minutes, seconds;
            hours = seconds = minutes = 0;  
            string[]? command = _command.Split(' ');           

            string? answer = "";


            if (_command.Split("'").Length == 3)
            {
                answer = _command.Split("'")[1];
            }
            else if (_command.Split('"').Length == 3)
            {
                answer = _command.Split('"')[1];
            }

            foreach (string s in command)
            {
                if (string.IsNullOrEmpty(s)) break;
                if (s[^1] == 'h') _=int.TryParse(s.Remove(s.Length-1), out hours);
                if (s[^1] == 'm') _=int.TryParse(s.Remove(s.Length-1), out minutes);
                if (s[^1] == 's') _=int.TryParse(s.Remove(s.Length - 1), out seconds);
            }

            int resultTime = TimeConverter.TimeToSeconds(hours, minutes, seconds);

            switch(command[0])
            {
                case "new":
                    //ProfileManager.AddNewProfile(command[1], answer, resultTime, command[command.Length - 1]);
                    AddToHistory("new Command", $"added '{command[1]}' to command List", "");
                    break;

                case "change":
                    
                    if (command[2] == "time")
                    {
                        ProfileManager.UpdateProfile(command[1], resultTime);
                        AddToHistory("change Profile", $"changed the '{command[2]}' property of '{command[1]}'", "");
                        break;
                    }
                    ProfileManager.UpdateProfile(command[1], command[2], command[3]);
                    AddToHistory("change Profile", $"changed the '{command[2]}' property of '{command[1]}'", "");
                    break;

                case "delete":
                    ProfileManager.DeleteProfile(command[1]);
                    AddToHistory("delete Profile", $"deleted {command[1]}", "");
                    break;

                case "settings":
                    settingsWindow = new SettingsWindow();
                    settingsWindow.Show();
                    break;

                default:
                    AddToHistory("Error", "unknown Command", "");
                    break;
            }
        }

        //Adds a Command to the History
        private void AddToHistory(string title, string? answer, string output)
        {
            int GradientNumber = random.Next(dataManager.GetGradientList().Count());

            var StartRgb = Convert.ToInt32(dataManager.GetGradientList()[GradientNumber].StartHex.Remove(0, 1), 16);
            var EndRgb = Convert.ToInt32(dataManager.GetGradientList()[GradientNumber].StartHex.Remove(0, 1), 16);

            GradientStopCollection gradientStopCollection = new()
            {
                new GradientStop(Color.FromRgb((byte)((StartRgb >> 16) & 0xFF), (byte)((StartRgb >> 8) & 0xFF), (byte)(StartRgb& 0xFF)), 0),
                new GradientStop(Color.FromRgb((byte)((EndRgb >> 16) & 0xFF), (byte)((EndRgb >> 8) & 0xFF), (byte)(EndRgb& 0xFF)), 1)
            };

            CommandHistory.Add(new Command { title = title, answer = answer, output = output, gradientStops = gradientStopCollection});
        }


        //Profile
        public void ClearCommandHistoy() => CommandHistory.Clear();     
        #endregion

        #region AppBehaviour
        //Close Button

        [RelayCommand]
        public static void Close()
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion
    }
}