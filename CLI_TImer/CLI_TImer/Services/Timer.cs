using CLI_TImer.MVVM.ViewModel;
using System;
using System.Windows.Threading;
using CLI_TImer.MVVM.Model;

namespace CLI_TImer.Services
{
    public static class Timer
    {
        private static int MainTimerSeconds = 0;
        private static int SecondTimerSeconds = 0;

        private static TimerType? cT = TimerType.stop;
        private static DispatcherTimer timer = new DispatcherTimer();

        private static MainViewModel Vm = App.MainViewModel;

        public static Timer()
        {
            //timer.Start();
        }

        private static void TimerCountdown(object? sender, EventArgs? e)
        {                
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
            Vm.SetMainTimerText(MainTimerSeconds);
        }

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
            timer.Interval = new TimeSpan(0, 0, 1);
            cT = TimerType.main;
            timer.Start();
        }
        public static void StartSecond()
        {
            StartTimer();
            cT = TimerType.second;
            timer.Start();
        }

        private static void StartTimer()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(TimerCountdown);
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