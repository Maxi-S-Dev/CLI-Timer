using CLI_TImer.MVVM.Model;
using CLI_TImer.Themes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using CLI_TImer.Classes;
using System.Windows.Input;
using System.IO;
using CLI_TImer.Helpers;

namespace CLI_TImer.MVVM.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        #region variables

        [ObservableProperty]
        public string? mainTimerText;

        public string PauseTimerText = "";


        //Inputs
        [ObservableProperty]
        public string enteredCommand = string.Empty;

        [ObservableProperty]
        public ObservableCollection<Command> commandHistory = new();


        //Code
        private int pausePosition;

        Timer timer;

        public virtual Dispatcher Dispatcher { get; protected set; }

        #endregion

        public MainViewModel()
        {
            timer = new(this);
            SetMainTimerText(0);

            Dispatcher= Dispatcher.CurrentDispatcher;
        }

        #region Set Timer Text

        /// <summary>
        /// Subtract Command
        /// 
        /// </summary>
        

        internal void SetMainTimerText(int time)
        {
            MainTimerText = $"{Times.SecondsToHours(time)}h {Times.SecondsToMinutes(time)}m {time % 60}s";
        }

        internal void UpdatePauseTimerText(int seconds)
        {
            if (CommandHistory.Count == 0) return;
            string PauseTimerText = $"{Times.SecondsToHours(seconds)}h {Times.SecondsToMinutes(seconds)}m {seconds % 60}s";

            Dispatcher.BeginInvoke(new Action(() =>
            {
                Command latestPause = CommandHistory[pausePosition];

                CommandHistory.Remove(latestPause);

                latestPause.output = PauseTimerText;

                CommandHistory.Add(latestPause);

                CommandHistory.Move(CommandHistory.Count -1, pausePosition);
            }));
        }

        #endregion

        //Input Commands
        [RelayCommand]
        public void Send()
        {
            CheckCommand(EnteredCommand);
            EnteredCommand = "";
        }

        #region commands
        private void CheckCommand(string _command)
        {
            string[]? command = _command.Split(' ');

            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            foreach (string s in command)
            {
                if (string.IsNullOrEmpty(s)) break;
                if (s[^1] == 'h') _=int.TryParse(s.Remove(s.Length-1), out hours);
                if (s[^1] == 'm') _=int.TryParse(s.Remove(s.Length-1), out minutes);
                if (s[^1] == 's') _=int.TryParse(s.Remove(s.Length - 1), out seconds);
            }

            int resultTime = Times.TimeToSeconds(hours, minutes, seconds);

            Profile? selectedProfile = ProfileManager.getProfileFromCommand(command[0]);

            if (selectedProfile != null && resultTime != 0) selectedProfile.Time = resultTime;

            if (selectedProfile != null)
            {
                ExecuteProfile(selectedProfile);
                return;
            }

            switch(command[0])
            {
                case "new":
                    ProfileManager.AddNewProfile(command[1], command[2].Split(","), resultTime, command[command.Length - 1]);
                    break;

                case "delete":
                    ProfileManager.DeleteProfile(command[1]);
                    break;

                case "add":
                    AddTimeToCurrentTimer(hours, minutes, seconds);
                    break;

                case "start":
                    timer.startMain();
                    break;

                case "subtract":
                    SubtractTimeFromCurrentTimer(hours, minutes, seconds);
                    break;

                case "end":
                    ResetCurrentTimer();
                    break;

                case "reset":
                    ResetAllTimers();
                    break;

                case "clear":
                    ClearCommandHistoy();
                    break;

                case "close":
                    Close();
                    break;

                default:
                    AddToHistory("Error", "unknown Command", "");
                    break;
            }
        }

        //Adds a Command to the History
        private void AddToHistory(string title, string answer, string output)
        {
            CommandHistory.Add(new Command { title = title, answer = answer, output = output, gradientStops = Gradients.GradientStops() });
        }


        //Profile
        private void ExecuteProfile(Profile profile)
        {
            timer.SetAndStartTimerFromProfile(profile);

            if (profile.TimerType == TimerType.second) pausePosition = CommandHistory.Count;
            AddToHistory(profile.Name, profile.Answer, "");
        }
        private void ClearCommandHistoy() => CommandHistory.Clear();    

        private void SubtractTimeFromCurrentTimer(int hours, int minutes, int seconds)
        {
            timer.AddSecondsToCurrentTimer(-Times.TimeToSeconds(hours, minutes, seconds));
        }
        private void AddTimeToCurrentTimer(int hours, int minutes, int seconds)
        {
            timer.AddSecondsToCurrentTimer(Times.TimeToSeconds(hours, minutes, seconds));
        }
        private void ResetCurrentTimer() => timer.ResetCurrentTimer();
        private void ResetAllTimers() => timer.ResetAllTimers();

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