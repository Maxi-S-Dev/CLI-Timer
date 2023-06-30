using CLI_TImer.MVVM.ViewModel;
using System;
using System.Windows.Threading;
using CLI_TImer.MVVM.Model;

namespace CLI_TImer.Services
{
    public static class Timer
    {
        private static int[] TimerSeconds = new int[] { 0, 0 };
        private static int currentTimerIndex = 0;

        private static int MainTimerSeconds = 0;
        private static int SecondTimerSeconds = 0;

        private static TimerType? cT = TimerType.stop;
        private static readonly DispatcherTimer dispatcher = new DispatcherTimer();

        private static MainViewModel Vm = App.MainViewModel;

        private static void TimerTick(object? sender, EventArgs? e)
        {
            TimerSeconds[currentTimerIndex]--;

            if (currentTimerIndex == 0)
                App.MainViewModel.SetMainTimerText(TimerSeconds[currentTimerIndex]);

            if (currentTimerIndex == 1)
                App.MainViewModel.UpdatePauseTimerText(TimerSeconds[currentTimerIndex]);


            /*
            if (cT == TimerType.main)
            {
                MainTimerSeconds = MainTimerSeconds <= 0 ? 0: MainTimerSeconds -= 1;
                if (MainTimerSeconds == 0)
                {   
                    cT = TimerType.stop;
                    Vm.MainTimerFinished();
                }
                Vm.SetMainTimerText(MainTimerSeconds);

                return;
            }
            if (cT == TimerType.second)
            {
                SecondTimerSeconds = SecondTimerSeconds <= 0 ? 0: SecondTimerSeconds -= 1;
                if (SecondTimerSeconds <= 0)
                {
                    Vm.SecondaryTimerFinished();
                    cT = TimerType.main;
                }
                Vm.UpdatePauseTimerText(SecondTimerSeconds);
                return;
            }
            Vm.SetMainTimerText(MainTimerSeconds); */
        }

        public static void StartTimer(int index)
        {
            dispatcher.Stop();
            dispatcher.Tick -= TimerTick;

            currentTimerIndex = index;
            dispatcher.Tick += TimerTick;
            dispatcher.Interval = TimeSpan.FromSeconds(1);
            dispatcher.Start();
        }

        public static void SetTimer(int value) => SetTimer(currentTimerIndex, value);

        public static void SetTimer(int index, int value) => TimerSeconds[index] = value;
        
   

        //Set the Time of the Timer
        public static void setMainTimer(int seconds) => MainTimerSeconds = seconds;
        public static void setSecondTimer(int seconds) => SecondTimerSeconds= seconds;
        public static void setCurrentTimer(int seconds)
        {
            if (cT == TimerType.main) MainTimerSeconds = seconds;
            if (cT == TimerType.second) SecondTimerSeconds= seconds;

            Vm.SetMainTimerText(MainTimerSeconds);
            Vm.UpdatePauseTimerText(SecondTimerSeconds);
        }


        //Add Time to Timers
        public static void AddSecondsToCurrentTimer(int seconds)
        {
            if (cT == TimerType.main || cT == TimerType.stop)
            {
                MainTimerSeconds += seconds;
                Vm.SetMainTimerText(MainTimerSeconds);
            }
            if (cT == TimerType.second)
            {
                SecondTimerSeconds+= seconds;
                Vm.UpdatePauseTimerText(SecondTimerSeconds);
            }

            if (MainTimerSeconds < 0) MainTimerSeconds = 0;
            if (SecondTimerSeconds < 0 ) SecondTimerSeconds = 0;
        }

        //Reset The Timers
        public static void ResetAllTimers()
        {
            MainTimerSeconds = SecondTimerSeconds = 0;
            Vm.SetMainTimerText(MainTimerSeconds);
            Vm.UpdatePauseTimerText(SecondTimerSeconds);
        }

        public static void ResetCurrentTimer() 
        {
            if (cT == TimerType.main) ResetMainTimer();
            if (cT == TimerType.second) ResetSecondTimer();
        }
        public static void ResetMainTimer() => MainTimerSeconds = 0;
        public static void ResetSecondTimer() => SecondTimerSeconds = 0;

        //Returns the remaining Timer Seconds
        public static int getMainTimerSeconds => MainTimerSeconds;
        public static int getSecondTimerSeconds => SecondTimerSeconds;

        //Starts a timer
        public static void StartMain()
        {
            StartTimer();
            cT = TimerType.main;
            dispatcher.Start();
        }
        public static void StartSecond()
        {
            StartTimer();
            cT = TimerType.second;
            dispatcher.Start();
        }

        private static void StartTimer()
        {
            dispatcher.Interval = new TimeSpan(0, 0, 1);
            dispatcher.Tick += new EventHandler(TimerTick);
        }

        //Returns which timer is running
        public static TimerType? getCurrentTimer() => cT;

        public static void SetAndStartTimerFromProfile(Profile? p)
        {
            cT = p.TimerType;
            setCurrentTimer(p.Time);
        }
    }
}
public enum TimerType
{
    stop,
    main,
    second,
}