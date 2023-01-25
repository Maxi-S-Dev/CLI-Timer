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
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MainTimerText))]
        public int mainSeconds;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MainTimerText))]
        public int mainMinutes;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MainTimerText))]
        public int mainHours;

        public string MainTimerText => $"{MainHours}h {MainMinutes}m {MainSeconds}s";


        //Pause Timer
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PauseTimerText))]
        public int pauseSeconds;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PauseTimerText))]
        public int pauseMinutes;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PauseTimerText))]
        public int pauseHours;

        public string PauseTimerText => $"{PauseHours}h {PauseMinutes}m {PauseSeconds}s";

        //Inputs
        [ObservableProperty]
        public string enteredCommand = string.Empty;

        [ObservableProperty]
        public ObservableCollection<Command> commandHistory = new();


        //Code
        private bool isPaused = false;
        private bool mainTimerRunning = false;
        private bool stopApp = false;
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

            else if (command[0] == "close") CloseApplication();

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
            Command work;
            if (mainTimerRunning)
            {
                AddToHistory("work", $"main timer alredy running. \nUse 'end' to stop the main Timer", "");
                return;
            }

            mainTimerRunning = true;
            MainHours = hours;
            MainMinutes = minutes;
            MainSeconds = seconds;

            AddToHistory("work", "we are now working", "");
        }
        //Starts Pause profile
        private void Pause(int minutes) => Pause(0, minutes, 0);

        private void Pause(int hours, int minutes, int seconds)
        {
            isPaused = true;

            PauseHours = hours;
            PauseMinutes = minutes;
            PauseSeconds = seconds;

            pausePosition = CommandHistory.Count;

            AddToHistory("break", "we are taking a break", PauseTimerText);
        }
        //subtracts time from current timer
        private void SubtractTimeFromCurrentTimer(int minutes) => SubtractTimeFromCurrentTimer(0, minutes, 0);

        private void SubtractTimeFromCurrentTimer(int hours, int minutes, int seconds)
        {
            if (!isPaused)
            {
                MainHours -= hours;
                MainMinutes -= minutes;
                MainSeconds -= seconds;

                string output = hours == 0 ? "" : $"{hours}h";
                output += minutes == 0 ? "" : $"{minutes}min";
                output += seconds == 0 ? "" : $"{seconds}h";

                AddToHistory("subtract", $"subtracted {output} from the main timer", "");
            }
            else if (isPaused)
            {
                PauseHours -= hours;
                PauseMinutes -= minutes;
                PauseSeconds -= seconds;

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
                MainHours += hours;
                MainMinutes += minutes;
                MainSeconds += seconds;

                string output = hours == 0 ? "" : $"{hours}h";
                output += minutes == 0 ? "" : $"{minutes}min";
                output += seconds == 0 ? "" : $"{seconds}h";

                AddToHistory("add", $"added {output} to the main timer", "");
            }
            if (isPaused)
            {
                PauseHours += hours;
                PauseMinutes += minutes;
                PauseSeconds += seconds;

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
            mainTimerRunning= false;
            AddToHistory("reset", "reseted all timers", "");
        }

        #endregion

        #region Timer
        //Timer Management
        private void Countdown()
        {
            while (true)
            {
                if (stopApp) break;

                if (!isPaused)
                {
                    if (MainSeconds > 0 || MainMinutes > 0 || MainHours > 0) MainTimer();
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
            MainSeconds--;

            if ((MainHours == 0 && MainMinutes == 0 && MainSeconds == 0) || MainHours < 0 || MainMinutes < 0)
            {
                MainHours = MainMinutes = MainSeconds = 0;
                mainTimerRunning = false;
                return;
            }

            if (MainSeconds <= 0)
            {
                MainMinutes--;
                MainSeconds = 59;
            }

            if (MainMinutes == 0 && MainHours > 0)
            {
                MainMinutes = 59;
                MainHours--;
            }
        }

        //Zählt den Pause Timer runter
        private void PauseTimer()
        {
            PauseSeconds--;

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


            if (PauseHours == 0 && PauseMinutes == 0 && PauseSeconds == 0)
            {
                isPaused = false;
                return;
            }

            if (PauseSeconds <= 0)
            {
                PauseMinutes--;
                PauseSeconds = 59;
            }

            if (PauseMinutes == 0 && PauseHours > 0)
            {
                PauseMinutes= 59;
                PauseHours--;
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
            PauseHours = PauseMinutes = PauseSeconds = 0;
        }

        private void ResetMainTimer()
        {
            mainTimerRunning = false;
            MainHours = MainMinutes = MainSeconds = 0;
        }
        #endregion

        #region AppBehaviour
        //Close Button
        [RelayCommand]
        public Task Close()
        {
            CloseApplication();
            return Task.CompletedTask;
        }

        private  void CloseApplication()
        {
            stopApp = true;
            System.Windows.Application.Current.Shutdown();
        }
        #endregion
    }
}

