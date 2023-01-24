using CLI_TImer.MVVM.Model;
using CLI_TImer.Themes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CLI_TImer.MVVM.ViewModel
{
    //added work parameter
    //set work standard seconds to 0

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

        private async void CheckCommand(string _command) 
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

                if (time.Length >= 2)
                {
                    _=int.TryParse(time[0], out hours);
                    _=int.TryParse(time[1], out minutes);

                    if(time.Length == 3) _=int.TryParse(time[2], out seconds);
                }
            }

            if (command[0] == "work")
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

            else CommandHistory.Add(new Command { title = "Error", answer = "unknown Command", output = "", gradientStops = Gradients.GradientStops() });
        }

        private void ClearCommandHistoy()
        {
            ResetAllTimers();
            isPaused = false;
            CommandHistory.Clear();   
        }

        private void Work(int minutes) => Work(0, minutes, 0);

        private void Work(int hours, int minutes, int seconds)
        {
            MainHours = hours;
            MainMinutes = minutes;
            MainSeconds = seconds;

            Command work = new() { title = "work", answer = "we are now working", output = "", gradientStops = Gradients.GradientStops()};
            CommandHistory.Add(work);
        }

        private void Pause(int minutes) => Pause(0, minutes, 0);

        private void Pause(int hours, int minutes, int seconds)
        {
            isPaused = true;
            PauseHours = hours;
            PauseMinutes = minutes;
            PauseSeconds = seconds;
            pausePosition = CommandHistory.Count;
            Command pause = new() { title = "break", answer = "we are taking a break", output = PauseTimerText, gradientStops= Gradients.GradientStops()};
            CommandHistory.Add(pause);
        }

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

                Command cmd = new() { title = "subtract", answer = $"subtracted {output} from the main timer", gradientStops= Gradients.GradientStops() };
                CommandHistory.Add(cmd);
            }
            else if (isPaused)
            {
                PauseHours -= hours;
                PauseMinutes -= minutes;
                PauseSeconds -= seconds;

                string output = hours == 0 ? "" : $"{hours}h";
                output += minutes == 0 ? "" : $"{minutes}min";
                output += seconds == 0 ? "" : $"{seconds}h";

                Command cmd = new() { title = "subtract", answer = $"subtracted {output} from the pause timer", gradientStops= Gradients.GradientStops() };
                CommandHistory.Add(cmd);
            }
        }

        private void AddTimeToCurrentTimer(int minutes) => AddTimeToCurrentTimer(0, minutes, 0);

        private void AddTimeToCurrentTimer(int hours, int minutes, int seconds)
        {
            if (!isPaused)
            {
                MainHours += hours;
                MainMinutes += minutes;
                MainSeconds += seconds;

                string output = hours == 0 ? "" : $"{hours}h";
                output += minutes == 0 ? "" : $"{minutes}min";
                output += seconds == 0 ? "" : $"{seconds}h";

                Command pause = new() { title = "add", answer = $"added {output} to the main timer", gradientStops= Gradients.GradientStops() };
                CommandHistory.Add(pause);
            }
            if (isPaused)
            {
                PauseHours += hours;
                PauseMinutes += minutes;
                PauseSeconds += seconds;

                string output = hours == 0 ? "" : $"{hours}h";
                output += minutes == 0 ? "" : $"{minutes}min";
                output += seconds == 0 ? "" : $"{seconds}h";

                Command pause = new() { title = "add", answer = $"added {output} to the pause timer", gradientStops= Gradients.GradientStops() };
                CommandHistory.Add(pause);
            }
        }

        private void EndCurrentTImer()
        {
            if (!isPaused)
            {
                ResetMainTimer();
                Command pause = new() { title = "end", answer = "reseted pause timer", gradientStops= Gradients.GradientStops() };
                CommandHistory.Add(pause);
            }

            if (isPaused)
            {
                ResetPauseTimer();
                isPaused = false;
                Command pause = new() { title = "end", answer = "reseted Pause timer", gradientStops= Gradients.GradientStops() };
                CommandHistory.Add(pause);
            }
        }

        private void EndAllTimers()
        {
            ResetAllTimers();
            Command pause = new() { title = "reset", answer = "reseted all timers", gradientStops= Gradients.GradientStops() };
            CommandHistory.Add(pause);
        }

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
            MainSeconds--;

            if (MainHours == 0 && MainMinutes == 0 && MainSeconds == 0) return;

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
            PauseHours = PauseMinutes = PauseSeconds = 0;
        }

        private void ResetMainTimer()
        {
            MainHours = MainMinutes = MainSeconds = 0;
        }

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
    }
}

