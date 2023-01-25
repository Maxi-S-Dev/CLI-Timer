using CLI_TImer.MVVM.Model;
using CLI_TImer.Themes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;


namespace CLI_TImer.MVVM.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        //Main Timer 
        private int mainTimerSeconds;

        [ObservableProperty]
        public string mainTimerText = "0h 0m 0s";

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

            if (command.Length == 2)
            { 
                string[] time = command[1].Split(':');

                if(time.Length == 1)
                    _=int.TryParse(time[0], out minutes);

                if (time.Length == 2)
                {
                    _=int.TryParse(time[0], out minutes);
                    _=int.TryParse(time[1], out seconds);
                }

                if (time.Length == 3)
                {
                    _=int.TryParse(time[0], out hours);
                    _=int.TryParse(time[1], out minutes);
                    _=int.TryParse(time[2], out seconds);
                }
            }

            if (command[0] == "work" || command[0] == "start")
            {
                if (hours == 0 && minutes == 0 && seconds == 0) Work(45);
                else Work(hours, minutes, seconds);
            }

            else if (command[0] == "break")
            {
                if (hours == 0 && minutes == 0 && seconds == 0) Pause(20);
                else Pause(hours, minutes, seconds);
            }

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
        private void Work(int minutes) => Work(0, minutes, 0);

        private void Work(int hours, int minutes, int seconds)
        {
            if (mainTimerRunning)
            {
                AddToHistory("work", $"main timer alredy running. \nUse 'end' to stop the main Timer", "");
                return;
            }

            mainTimerRunning = true;

            mainTimerSeconds = 3600 * hours + 60 * minutes + seconds;

            AddToHistory("work", "we are now working", "");
        }
        //Starts Pause profile
        private void Pause(int minutes) => Pause(0, minutes, 0);

        private void Pause(int hours, int minutes, int seconds)
        {
            isPaused = true;

            pauseTimerSeconds = 3600 * hours + 60 * minutes + seconds;

            pausePosition = CommandHistory.Count;

            AddToHistory("break", "we are taking a break", PauseTimerText);
        }
        //subtracts time from current timer
        private void SubtractTimeFromCurrentTimer(int minutes) => SubtractTimeFromCurrentTimer(0, minutes, 0);

        private void SubtractTimeFromCurrentTimer(int hours, int minutes, int seconds)
        {
            if (!isPaused)
            {
                mainTimerSeconds -= 3600 * hours + 60 * minutes + seconds;

                string output = hours == 0 ? "" : $"{hours}h";
                output += minutes == 0 ? "" : $"{minutes}min";
                output += seconds == 0 ? "" : $"{seconds}h";

                AddToHistory("subtract", $"subtracted {output} from the main timer", "");
            }
            else if (isPaused)
            {
                pauseTimerSeconds -= 3600 * hours + 60 * minutes + seconds;

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
                mainTimerSeconds += 3600 * hours + 60 * minutes + seconds;

                string output = hours == 0 ? "" : $"{hours}h";
                output += minutes == 0 ? "" : $"{minutes}min";
                output += seconds == 0 ? "" : $"{seconds}h";

                AddToHistory("add", $"added {output} to the main timer", "");
            }
            if (isPaused)
            {
                pauseTimerSeconds += 3600 * hours + 60 * minutes + seconds;

                string output = hours == 0 ? "" : $"{hours}h";
                output += minutes == 0 ? "" : $"{minutes}min";
                output += seconds == 0 ? "" : $"{seconds}h";

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
                    if (mainTimerSeconds > 0) MainTimer();
                }

                if(isPaused) 
                {
                    PauseTimer();
                }

                Thread.Sleep(10);
            }
        }


        //Zählt den Main Timer runter
        private void MainTimer()
        {
            if (!mainTimerRunning) return;

            mainTimerSeconds--;

            int hours = mainTimerSeconds / 3600;
            int minutes = (mainTimerSeconds % 3600)/ 60;
            int seconds = mainTimerSeconds % 60;

            if (mainTimerSeconds <= 0)
            {
                mainTimerSeconds = 0;
                mainTimerRunning = false;
            }

            MainTimerText = $"{hours}h {minutes}m {seconds}s";
        }

        //Zählt den Pause Timer runter
        private void PauseTimer()
        {
            pauseTimerSeconds--;

            int hours = pauseTimerSeconds / 3600;
            int minutes = (pauseTimerSeconds % 3600)/ 60;
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
            mainTimerSeconds = 0;
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