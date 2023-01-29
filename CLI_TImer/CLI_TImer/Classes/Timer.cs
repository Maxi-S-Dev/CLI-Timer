using CLI_TImer.MVVM.View;
using CLI_TImer.MVVM.ViewModel;
using System;
using System.Windows.Threading;

namespace CLI_TImer.Classes
{
    public sealed class Timer
    {
        private int MainTimerSeconds = 1000;
        private int SecondTimerSeconds = 0;

        private TimerType cT = TimerType.main;
        private DispatcherTimer timer;

        private MainViewModel Vm;

        public Timer(MainViewModel Vm)
        {
            this.Vm = Vm;
            timer = new DispatcherTimer();
            timer.Interval = new System.TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(TimerCountdown);
            timer.Start();
        }

        private void TimerCountdown(object? sender, EventArgs? e)
        {
            if (cT == TimerType.main)
            {
                MainTimerSeconds--;
                Vm.SetMainTimerText(MainTimerSeconds);
                return;
            }
            if (cT == TimerType.second) SecondTimerSeconds--;
            {
                MainTimerSeconds--;
                Vm.UpdatePauseTimerText(SecondTimerSeconds);
            }
        }

        //Set the Time of the Timer
        public void setMainTimer(int seconds) => MainTimerSeconds = seconds;
        public void setSecondTimer(int seconds) => SecondTimerSeconds= seconds;

        //Add Time to Timers
        public void AddSecondsToCurrentTimer(int seconds)
        {
            if (cT == TimerType.main) MainTimerSeconds += seconds;
            if (cT == TimerType.second) SecondTimerSeconds+= seconds;
        }

        //Reset The Timers
        public void ResetAllTimers() => MainTimerSeconds = SecondTimerSeconds = 0;
        public void ResetCurrentTimer() 
        {
            if (cT == TimerType.main) ResetMainTimer();
            if (cT == TimerType.second) ResetSecondTimer();
        }
        public void ResetMainTimer() => MainTimerSeconds = 0;
        public void ResetSecondTimer() => SecondTimerSeconds = 0;

        //Returns the remaining Timer Seconds
        public int getMainTimerSeconds => MainTimerSeconds;
        public int getSecondTimerSeconds => SecondTimerSeconds;

        //Starts a timer
        public void startMain() => cT = TimerType.main;
        public void startSecond() => cT = TimerType.second;

        //Returns which timer is running
        public TimerType getCurrentTimer() => cT;
    }

    public enum TimerType
    {
        main,
        second
    }
}
