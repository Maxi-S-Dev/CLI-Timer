using CLI_TImer.MVVM.Model;
using CLI_TImer.Themes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;
using CLI_TImer.Classes;
using System.Diagnostics;
using System.Configuration;
using System.Timers;

namespace CLI_TImer.MVVM.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        #region variables

        [ObservableProperty]
        public string mainTimerText = "";

        public string PauseTimerText = "";


        //Inputs
        [ObservableProperty]
        public string enteredCommand = string.Empty;

        [ObservableProperty]
        public ObservableCollection<Command> commandHistory = new();


        //Code
        private int pausePosition;

        Classes.Timer timer;

        public virtual Dispatcher Dispatcher { get; protected set; }

        #endregion

        public MainViewModel()
        {
            timer = new(this);

            Dispatcher= Dispatcher.CurrentDispatcher;
        }

        #region Set Timer Text

        internal void SetMainTimerText(int time)
        {
            MainTimerText = $"{Times.SecondsToHours(time)}h {Times.SecondsToMinutes(time)}m {time % 60}s";
        }

        internal void UpdatePauseTimerText(int seconds)
        {
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
            string[] command = _command.Split(' ');
            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            foreach (string s in command)
            {
                if (s[^1] == 'h') hours = Convert.ToInt32(s.Remove(s.Length-1));
                if (s[^1] == 'm') minutes= Convert.ToInt32(s.Remove(s.Length-1));
                if (s[^1] == 's') seconds = Convert.ToInt32(s.Remove(s.Length-1));
            }

            if (command[0] == "work") Work(hours, minutes, seconds);

            else if (command[0] == "break") Pause(hours, minutes, seconds);

            else if (command[0] == "add") AddTimeToCurrentTimer(hours, minutes, seconds);

            else if (command[0] == "subtract") SubtractTimeFromCurrentTimer(hours, minutes, seconds);

            else if (command[0] == "end") ResetCurrentTimer();

            else if (command[0] == "reset") ResetAllTimers();

            else if (command[0] == "clear") ClearCommandHistoy();

            else if (command[0] == "close") Close();

            else AddToHistory("Error", "unknown Command", "");
        }

        //Adds a Command to the History
        private void AddToHistory(string title, string answer, string output)
        {
            CommandHistory.Add(new Command { title = title, answer = answer, output = output, gradientStops = Gradients.GradientStops() });
        }


        private void ClearCommandHistoy() => CommandHistory.Clear();    
        private void Work(int hours, int minutes, int seconds)
        {
            if (hours == 0 && minutes == 0 && seconds == 0) minutes = 45;

            timer.setMainTimer(Times.TimeToSeconds(hours, minutes, seconds));
        }
        private void Pause(int hours, int minutes, int seconds)
        {            
            if (hours == 0 && minutes == 0 && seconds == 0) minutes = 20;

            timer.setSecondTimer(Times.TimeToSeconds(hours, minutes, seconds));
            timer.startSecond();

            pausePosition = CommandHistory.Count;
        }
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