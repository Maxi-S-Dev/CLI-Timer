using CLI_TImer.MVVM.Model;
using CLI_TImer.Themes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading;
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
        public string enteredCommand;

        [ObservableProperty]
        public ObservableCollection<Command> commandHistory = new();


        //Code
        private bool isPaused = false;
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



        private void CheckCommand(string command) 
        {
            if (command == "work") Work();

            else if (command == "break") Pause();

            else if (command == "close") System.Windows.Application.Current.Shutdown();

            else if (command == "clear") ClearCommandHistoy();

            else if (command == "add") AddTimeToCurrentTimer();

            else if (command == "subtract") SubtractTimeFromCurrentTimer();

            else if (command == "end") EndCurrentTImer();

            else if (command == "reset") EndAllTimers();

            else CommandHistory.Add(new Command { title = "Error", answer = "unknown Command", output = "", gradientStops = Gradients.GradientStops() });
        }

        private void ClearCommandHistoy()
        {
            resetAllTimers();
            CommandHistory.Clear();   
        }

        private void Work()
        {
            MainMinutes = 45;
            MainSeconds = 1;

            Command work = new() { title = "work", answer = "we are now working", output = "", gradientStops = Gradients.GradientStops()};
            CommandHistory.Add(work);
        }

        private void Pause()
        {
            isPaused = true;
            PauseMinutes = 20;
            PauseSeconds = 1;
            pausePosition = CommandHistory.Count;
            Command pause = new() { title = "break", answer = "we are taking a break", output = PauseTimerText, gradientStops= Gradients.GradientStops()};
            CommandHistory.Add(pause);
        }

        private void SubtractTimeFromCurrentTimer()
        {
            if (!isPaused) MainMinutes -= 10;
            if (isPaused) PauseMinutes -= 10;

            Command pause = new() { title = "subtract", answer = "subtracted 10 Minutes to from timer", gradientStops= Gradients.GradientStops() };
            CommandHistory.Add(pause);
        }

        private void AddTimeToCurrentTimer()
        {
            if (!isPaused) MainMinutes += 10;
            if (isPaused) PauseMinutes += 10;

            Command pause = new() { title = "add", answer = "added 10 Minutes to current timer", gradientStops= Gradients.GradientStops() };
            CommandHistory.Add(pause);
        }

        private void EndCurrentTImer()
        {
            if (!isPaused)
            {
                resetMainTimer();
                Command pause = new() { title = "end", answer = "reseted Main timer", gradientStops= Gradients.GradientStops() };
                CommandHistory.Add(pause);
            }

            if (isPaused)
            {
                resetPauseTimer();
                isPaused = false;
                Command pause = new() { title = "end", answer = "reseted Pause timer", gradientStops= Gradients.GradientStops() };
                CommandHistory.Add(pause);
            }
        }

        private void EndAllTimers()
        {
            resetAllTimers();
            Command pause = new() { title = "reset", answer = "reseted all timers", gradientStops= Gradients.GradientStops() };
            CommandHistory.Add(pause);
        }

        //Timer Management
        private void Countdown()
        {
            while (true)
            {
                if (!isPaused)
                {
                    if (MainSeconds > 0 && MainMinutes >= 0 && MainHours >= 0) MainTimer();
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

            if (MainSeconds == 0)
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

            if (PauseSeconds == 0)
            {
                PauseMinutes--;
                PauseSeconds = 59;
            }

            if (PauseMinutes == 0 && PauseHours> 0)
            {
                PauseMinutes= 59;
                PauseHours--;
            }
        }

        private void resetAllTimers()
        {
            resetPauseTimer();
            resetMainTimer();
        }

        private void resetPauseTimer()
        {
            PauseHours = PauseMinutes = PauseSeconds = 0;
        }

        private void resetMainTimer()
        {
            MainHours = MainMinutes = MainSeconds = 0;
        }
    }
}

