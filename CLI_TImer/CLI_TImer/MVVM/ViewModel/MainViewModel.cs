using CLI_TImer.MVVM.Model;
using CLI_TImer.Themes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CLI_TImer.MVVM.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        //Main Timer 
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MainTimerText))]
        public int mainSeconds = 1;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MainTimerText))]
        public int mainMinutes = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MainTimerText))]
        public int mainHours = 0;

        public string MainTimerText => $"{MainHours}h {MainMinutes}m {MainSeconds}s";


        //Break Timer
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PauseTimerText))]
        public int pauseSeconds = 1;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PauseTimerText))]
        public int pauseMinutes = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PauseTimerText))]
        public int pauseHours = 0;


        public string PauseTimerText => $"{PauseHours}h {PauseMinutes}m {PauseSeconds}s";

        [ObservableProperty]
        public string enteredCommand;

        [ObservableProperty]
        public ObservableCollection<Command> commandHistory = new ObservableCollection<Command>();

        

        private bool isPaused = false;

        readonly Thread timerThread;

        public MainViewModel() 
        { 
            timerThread = new Thread(new ThreadStart(Countdown));
            timerThread.Start();
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
            switch (command) 
            {
                case "work":

                    Work();
                    break;
                

                case "break":
                
                    Pause();
                    break;
                

                default:
                    CommandHistory.Add(new Command { title = "Error", answer = "unknown Command", output = "", gradientStops = Gradients.GradientStops()});
                    break;
            }
        }

        private void Work()
        {
            MainMinutes = 45;
            MainSeconds = 1;

            Command work = new Command { title = "work", answer = "we are now working", output = "", gradientStops = Gradients.GradientStops()};
            CommandHistory.Add(work);
        }

        private void Pause()
        {
            isPaused = true;
            PauseMinutes = 20;
            PauseSeconds = 1;

            Command pause = new Command { title = "break", answer = "we are taking a break", output = PauseTimerText, gradientStops= Gradients.GradientStops()};
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

        private void PauseTimer()
        {
            PauseSeconds--;

            CommandHistory.Clear();

            if(PauseHours == 0 && PauseMinutes == 0 && PauseSeconds == 0)
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
    }
}

// Added Break
//