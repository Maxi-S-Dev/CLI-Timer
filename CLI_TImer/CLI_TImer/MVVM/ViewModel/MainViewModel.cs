using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading;
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using CLI_TImer.MVVM.Model;

namespace CLI_TImer.MVVM.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MainTimerText))]
        public int mainSeconds = 1;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MainTimerText))]
        public int mainMinutes = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MainTimerText))]
        public int mainHours = 0;

        [ObservableProperty]
        public string enteredCommand;

        [ObservableProperty]
        public ObservableCollection<Command> commandHistory = new ObservableCollection<Command>();


        public string MainTimerText => $"{MainHours}h {MainMinutes}m {MainSeconds}s";

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
            Command work = new Command { name = command, answer = "we are now working", output = "" };
            CommandHistory.Add(work);
            switch (command) 
            {
                case "work":
                {
                    //work();
                    break;
                }

                
            }
        }

        private void work()
        {
            MainMinutes = 45;
            MainSeconds = 1;

            Command work = new Command { name = "work", answer = "we are now working", output = "" };
            CommandHistory.Add(work);
        }


        //Timer Management
        private void Countdown()
        {
            while (true)
            {
                if (MainSeconds > 0 && MainMinutes >= 0 && MainHours >= 0) MainTimer();

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
    }
}
