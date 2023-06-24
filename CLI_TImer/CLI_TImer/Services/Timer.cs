using CLI_TImer.MVVM.ViewModel;
using System;
using System.Windows.Threading;
using CLI_TImer.MVVM.Model;

namespace CLI_TImer.Services
{
    public sealed class Timer
    {
        private int MainTimerSeconds = 0;
        private int SecondTimerSeconds = 0;

        private TimerType? cT = TimerType.stop; 
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
        public void setMainTimer(int seconds) => MainTimerSeconds = seconds;
        public void setSecondTimer(int seconds) => SecondTimerSeconds= seconds;
        public void setCurrentTimer(int seconds)
        {
            if (cT == TimerType.main) MainTimerSeconds = seconds;
            if (cT == TimerType.second) SecondTimerSeconds= seconds;

            Vm.SetMainTimerText(MainTimerSeconds);
            Vm.UpdatePauseTimerText(SecondTimerSeconds);
        }

        //Add Time to Timers
        public void AddSecondsToCurrentTimer(int seconds)
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
        public void ResetAllTimers()
        {
            MainTimerSeconds = SecondTimerSeconds = 0;
            Vm.SetMainTimerText(MainTimerSeconds);
            Vm.UpdatePauseTimerText(SecondTimerSeconds);
        }

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
        public TimerType? getCurrentTimer() => cT;

        internal void SetAndStartTimerFromProfile(Profile? p)
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