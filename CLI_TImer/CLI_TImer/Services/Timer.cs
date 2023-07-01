using System;
using System.Windows.Threading;

namespace CLI_Timer.Services
{
    public static class Timer
    {

        private static int[] timerSeconds = new int[] { 0, 0, 0};
        public static int[] TimerSeconds
        {
            get => timerSeconds;
            private set 
            { 
                timerSeconds = value;
            }
        }
        private static int currentTimerIndex = 0;

        public static readonly DispatcherTimer dispatcher = new DispatcherTimer();

        private static void TimerTick(object? sender, EventArgs? e)
        {
            timerSeconds[currentTimerIndex]--;

            App.MainViewModel.UpdateTimers();
        }

        public static void StartTimer(int index)
        {
            dispatcher.Stop();
            dispatcher.Tick -= TimerTick;

            App.MainViewModel.UpdateTimers();
            currentTimerIndex = index;
            dispatcher.Tick += TimerTick;
            dispatcher.Interval = TimeSpan.FromSeconds(1);
            dispatcher.Start();
        }

        #region Set Timer
        public static void SetTimer(int value) => SetTimer(currentTimerIndex, value);

        public static void SetTimer(int index, int value)
        {
            timerSeconds[index] = value;
        }
        #endregion

        #region AddoOr Subtract Time

        public static bool AddTime(int index, int value)
        {
            if(-value > timerSeconds[index]) return false;
            timerSeconds[index] += value;
            App.MainViewModel.UpdateTimers();
            return true;
        }

        #endregion

        #region Reset Timers
        public static void Reset()
        {
            timerSeconds[0] = 0;
            timerSeconds[1] = 0;
            App.MainViewModel.UpdateTimers();
        }

        public static void Reset(int index)
        {
            timerSeconds[index] = 0;
            App.MainViewModel.UpdateTimers();
        }
        #endregion
    }
}
public enum TimerType
{
    primary, 
    secondary,
    third
}