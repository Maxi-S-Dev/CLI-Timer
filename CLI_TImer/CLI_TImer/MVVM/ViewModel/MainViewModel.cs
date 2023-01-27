using CLI_TImer.MVVM.Model;
using CLI_TImer.Themes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;
using CLI_TImer.Classes;


namespace CLI_TImer.MVVM.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        #region variables

        //Main Timer 
        [ObservableProperty, NotifyPropertyChangedFor(nameof(MainTimerText))]
        private int mainTimerSeconds;

        public string MainTimerText => $"{MainTimerSeconds / 3600}h {(MainTimerSeconds % 3600)/ 60}m {MainTimerSeconds % 60}s";

        //Pause Timer
        private int pauseTimerSeconds;

        public string PauseTimerText = "";


        //Inputs
        [ObservableProperty]
        public string enteredCommand = string.Empty;

        [ObservableProperty]
        public ObservableCollection<Command> commandHistory = new();


        //Code
        private bool isPaused = false;
        private bool mainTimerRunning = false;
        private static bool stopApp = false;
        private int pausePosition;
        readonly Thread timerThread;

        public virtual Dispatcher Dispatcher { get; protected set; }

        #endregion

        public MainViewModel() 
        { 
            timerThread = new Thread(new ThreadStart(Countdown));
            timerThread.Start();     

            Dispatcher= Dispatcher.CurrentDispatcher;
        }

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
            int minutes= 0;
            int seconds = 0;

            foreach(string s in command)
            {
                if (s[s.Length-1] == 'h') hours = Convert.ToInt32(s.Remove(s.Length-1));
                if (s[s.Length-1] == 'm') minutes= Convert.ToInt32(s.Remove(s.Length-1));
                if (s[s.Length-1] == 's') seconds = Convert.ToInt32(s.Remove(s.Length-1));
            }

            if (command[0] == "work") Work(hours, minutes, seconds);

            else if (command[0] == "break") Pause(hours, minutes, seconds);           

            else if (command[0] == "add")
            {
                if (hours == 0 && minutes == 0 && seconds == 0) AddTimeToCurrentTimer(10);
                else AddTimeToCurrentTimer(hours, minutes, seconds);
            }

            else if (command[0] == "subtract")
            {
                if (hours == 0 && minutes == 0 && seconds == 0) SubtractTimeFromCurrentTimer(10);
                else SubtractTimeFromCurrentTimer(hours, minutes, seconds);
            }

            else if (command[0] == "end") EndCurrentTImer();

            else if (command[0] == "reset") EndAllTimers();

            else if (command[0] == "clear") ClearCommandHistoy();

            else if (command[0] == "close") Close();

            else AddToHistory("Error", "unknown Command", "");
        }

        //Adds a Command to the History
        private void AddToHistory(string title, string answer, string output)
        {
            CommandHistory.Add(new Command { title = title, answer = answer, output = output, gradientStops = Gradients.GradientStops() });
        }
        //Clears the CommandLine
        private void ClearCommandHistoy()
        {
            ResetAllTimers();

            isPaused = false;
            CommandHistory.Clear();   
        }
        
        
        //Starts Work profile
        private void Work(int hours, int minutes, int seconds)
        {
            if (mainTimerRunning)
            {
                AddToHistory("work", $"main timer alredy running. \nUse 'end' to stop the main Timer", "");
                return;
            }

            if (hours == 0 && minutes == 0 && seconds == 0) minutes = 45;

            mainTimerRunning = true;

            MainTimerSeconds = Times.TimeToSeconds(hours, minutes, seconds);
           
            AddToHistory("work", "we are now working", "");
        }

        //Starts Pause profile
        private void Pause(int hours, int minutes, int seconds)
        {
            isPaused = true;

            if (hours == 0 && minutes == 0 && seconds == 0) minutes = 20;

            pauseTimerSeconds = Times.TimeToSeconds(hours, minutes, seconds);

            pausePosition = CommandHistory.Count;

            AddToHistory("break", "we are taking a break", PauseTimerText);
        }

        //Subtracts time from current timer
        private void SubtractTimeFromCurrentTimer(int minutes) => SubtractTimeFromCurrentTimer(0, minutes, 0);

        private void SubtractTimeFromCurrentTimer(int hours, int minutes, int seconds)
        {
            if (!isPaused)
            {
                MainTimerSeconds -= Times.TimeToSeconds(hours, minutes, seconds);

                string output = hours == 0 ? "" : $"{hours}h";
                output += minutes == 0 ? "" : $"{minutes}min";
                output += seconds == 0 ? "" : $"{seconds}h";

                AddToHistory("subtract", $"subtracted {output} from the main timer", "");
            }
            else if (isPaused)
            {
                pauseTimerSeconds -= Times.TimeToSeconds(hours, minutes, seconds);

                string output = hours == 0 ? "" : $"{hours}h";
                output += minutes == 0 ? "" : $"{minutes}min";
                output += seconds == 0 ? "" : $"{seconds}h";

                AddToHistory("subtract", $"subtracted {output} from the pause timer", "");
            }
        }
        //adds time to current timer
        private void AddTimeToCurrentTimer(int minutes) => AddTimeToCurrentTimer(0, minutes, 0);

        private void AddTimeToCurrentTimer(int hours, int minutes, int seconds)
        {
            if (!mainTimerRunning) 
            {
                AddToHistory("add", "no active timer", "");
                return; 
            }
            if (!isPaused)
            {
                MainTimerSeconds += Times.TimeToSeconds(hours, minutes, seconds);

                string output = hours == 0 ? "" : $"{hours}h ";
                output += minutes == 0 ? "" : $"{minutes}m ";
                output += seconds == 0 ? "" : $"{seconds}s";

                AddToHistory("add", $"added {output} to the main timer", "");
            }
            if (isPaused)
            {
                pauseTimerSeconds += Times.TimeToSeconds(hours, minutes, seconds);

                string output = hours == 0 ? "" : $"{hours}h ";
                output += minutes == 0 ? "" : $"{minutes}m ";
                output += seconds == 0 ? "" : $"{seconds}s";

                AddToHistory("add", $"added {output} to the pause timer", "");
            }
        }
        //resets current timer
        private void EndCurrentTImer()
        {
            if (!isPaused)
            {
                ResetMainTimer();
                mainTimerRunning = false;
                AddToHistory("end", "reseted main timer", "");
            }

            if (isPaused)
            {
                ResetPauseTimer();
                isPaused = false;
                AddToHistory("end", "reseted pause timer", "");
            }
        }
        //resets all timers
        private void EndAllTimers()
        {
            ResetAllTimers();
            AddToHistory("reset", "reseted all timers", "");
        }

        #endregion

        #region Timer
        //Timer Management
        private void Countdown()
        {
            while (!stopApp)
            {
                if (!isPaused)
                {
                    if (MainTimerSeconds > 0) MainTimer();
                }

                if(isPaused) 
                {
                    PauseTimer();
                }

                Thread.Sleep(1000);
            }
        }


        //Zählt den Main Timer runter
        private void MainTimer()
        {
            if (!mainTimerRunning) return;

            MainTimerSeconds--;

            if (MainTimerSeconds <= 0)
            {
                MainTimerSeconds = 0;
                mainTimerRunning = false;
            }
        }

        //Zählt den Pause Timer runter
        private void PauseTimer()
        {
            pauseTimerSeconds--;

            int hours = Times.SecondsToHours(pauseTimerSeconds);
            int minutes = Times.SecondsToMinutes(pauseTimerSeconds);
            int seconds = pauseTimerSeconds % 60;

            string PauseTimerText = $"{hours}h {minutes}m {seconds}s";

            //Updates the Listview timer
            if (CommandHistory.Count > 0)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Command latestPause = CommandHistory[pausePosition];

                    CommandHistory.Remove(latestPause);

                    latestPause.output = PauseTimerText;

                    CommandHistory.Add(latestPause);

                    CommandHistory.Move(CommandHistory.Count -1, pausePosition);
                }));
            }


            if (pauseTimerSeconds <= 0)
            {
                isPaused = false;
                return;
            }
        }

        private void ResetAllTimers()
        {
            ResetPauseTimer();
            ResetMainTimer();
        }

        private void ResetPauseTimer()
        {
            isPaused= false;
            pauseTimerSeconds = 0;
        }

        private void ResetMainTimer()
        {
            mainTimerRunning = false;
            MainTimerSeconds = 0;
        }
        #endregion

        #region AppBehaviour
        //Close Button

        [RelayCommand]
        public static void Close()
        {
            System.Windows.Application.Current.Shutdown();
        }

        public static void CloseEvent()
        {
            stopApp = true;
        }
        #endregion
    }
}