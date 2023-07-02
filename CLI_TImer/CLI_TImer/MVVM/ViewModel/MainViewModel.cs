using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Linq;
using System.Windows.Media;

using Microsoft.Toolkit.Uwp.Notifications;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CLI_Timer.Utils;
using CLI_Timer.Services;
using CLI_Timer.MVVM.View;
using CLI_Timer.MVVM.Model;
using System.Net.Http.Headers;

namespace CLI_Timer.MVVM.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        #region variables

        //Settings
        SettingsWindow settingsWindow;

        [ObservableProperty]
        public string primaryTimerText = "0h 0m 0s";

        [ObservableProperty]
        public string secondaryTimerText = "";

        [ObservableProperty]
        public string thirdTimerText = "";


        //Inputs
        [ObservableProperty]
        public string enteredCommand = string.Empty;

        [ObservableProperty]
        public ObservableCollection<Command> commandHistory = new();


        //Code
        public Profile? PrimaryRunningProfile;
        public Profile? SecondaryRunningProfile;
        public Profile? ThirdRunningProfile;

        Random random = new();

        AppDataManager dataManager = AppDataManager.instance;

        #endregion

        public MainViewModel()
        {
            Timer.SetTimer(Properties.Settings.Default.DefaultTime);
        }

        public void UpdateTimers()
        {
            PrimaryTimerText = $"{TimeConverter.SecondsToHours(Timer.TimerSeconds[0])}h {TimeConverter.SecondsToMinutes(Timer.TimerSeconds[0])}m {Timer.TimerSeconds[0] % 60}s";
            SecondaryTimerText = Timer.TimerSeconds[1] == 0 ? "" : $"{TimeConverter.SecondsToHours(Timer.TimerSeconds[1])}h {TimeConverter.SecondsToMinutes(Timer.TimerSeconds[1])}m {Timer.TimerSeconds[1] % 60}s";
            ThirdTimerText = Timer.TimerSeconds[2] == 0 ? "" : $"{TimeConverter.SecondsToHours(Timer.TimerSeconds[2])}h {TimeConverter.SecondsToMinutes(Timer.TimerSeconds[2])}m {Timer.TimerSeconds[2] % 60}s";
        }

        public void TimerFinished(int index)
        {
            Profile? finProfile = null;
            switch(index)
            {
                case 0:
                    finProfile = PrimaryRunningProfile; 
                    break;

                case 1:
                    finProfile = SecondaryRunningProfile;
                    break;

                case 2:
                    finProfile = ThirdRunningProfile;
                    break;
            }

            if (finProfile is null) return;

            if (finProfile.TimerType == TimerType.primary) finProfile.RingtonePath = @"C://Windows/Media/Alarm08.wav";
            if (finProfile.TimerType == TimerType.secondary) finProfile.RingtonePath = @"C://Windows/Media/Alarm04.wav";
            if (finProfile.TimerType == TimerType.third) finProfile.RingtonePath = @"C://Windows/Media/Alarm01.wav";

            if (finProfile.RingtoneEnabled == true) SoundPlayer.playSound(finProfile.RingtonePath, finProfile.RingtoneDuration);
            
            if (finProfile.NotificationEnabled == true)
            {
                new ToastContentBuilder()
                .AddText(finProfile.Name + " finished")
                .AddText(finProfile.NotificationText)
                .AddButton(new ToastButton()
                    .SetContent("Stop"))
                .AddButton(new ToastButton()
                    .SetContent("Add 5m"))
                .Show();
            }
        }

        public void MainTimerFinished()
        {
            if(PrimaryRunningProfile.RingtoneEnabled == false) return;
            if (string.IsNullOrEmpty(PrimaryRunningProfile.RingtonePath))
            {
                SoundPlayer.playSound(@"C://Windows/Media/Alarm04.wav", PrimaryRunningProfile.RingtoneDuration);
            }
            else
            {
                SoundPlayer.playSound(PrimaryRunningProfile.RingtonePath, PrimaryRunningProfile.RingtoneDuration);
            }

            if(PrimaryRunningProfile.NotificationEnabled == true) 
            {
                new ToastContentBuilder()
                    .AddText(PrimaryRunningProfile.Name + " finished")
                    .AddText(PrimaryRunningProfile.NotificationText)
                    .AddButton(new ToastButton()
                        .SetContent("Stop"))
                    .AddButton(new ToastButton()
                        .SetContent("Add 5m"))
                    .Show();
            }
        }

        public void SecondaryTimerFinished()
        {
            if (SecondaryRunningProfile.RingtoneEnabled == false) return;
            if (string.IsNullOrEmpty(SecondaryRunningProfile.RingtonePath))
            {
                SoundPlayer.playSound(@"C://Windows/Media/Alarm08.wav", SecondaryRunningProfile.RingtoneDuration);
            }
            else
            {
                SoundPlayer.playSound(SecondaryRunningProfile.RingtonePath, SecondaryRunningProfile.RingtoneDuration);
            }

            if (SecondaryRunningProfile.NotificationEnabled == true)
            {
                new ToastContentBuilder()
                .AddText(SecondaryRunningProfile.Name + " finished")
                .AddText(SecondaryRunningProfile.NotificationText)
                .AddButton(new ToastButton()
                    .SetContent("Stop"))
                .AddButton(new ToastButton()
                    .SetContent("Add 5m"))
                .Show();
            }
        }


        //Input Commands
        [RelayCommand]
        public void Send()
        {
            string? answer = CommandExecutor.Execute(EnteredCommand);
            if (EnteredCommand.Split(' ')[0] != "clear") AddToHistory(EnteredCommand, answer);
            EnteredCommand = "";
        }

        #region commands

        //Adds a Command to the History
        private void AddToHistory(string title, string? answer)
        {
            int GradientNumber = random.Next(dataManager.GetGradientList().Count());

            var StartRgb = Convert.ToInt32(dataManager.GetGradientList()[GradientNumber].StartHex.Remove(0, 1), 16);
            var EndRgb = Convert.ToInt32(dataManager.GetGradientList()[GradientNumber].StartHex.Remove(0, 1), 16);

            GradientStopCollection gradientStopCollection = new()
            {
                new GradientStop(Color.FromRgb((byte)((StartRgb >> 16) & 0xFF), (byte)((StartRgb >> 8) & 0xFF), (byte)(StartRgb& 0xFF)), 0),
                new GradientStop(Color.FromRgb((byte)((EndRgb >> 16) & 0xFF), (byte)((EndRgb >> 8) & 0xFF), (byte)(EndRgb& 0xFF)), 1)
            };

            CommandHistory.Add(new Command { title = title, answer = answer, gradientStops = gradientStopCollection});
        }

        //Profile
        public void ClearCommandHistory() => CommandHistory.Clear();    
        
        public void OpenSettingsWindow()
        {
            if (settingsWindow == null) settingsWindow = new();

            settingsWindow.Show();
        }
        #endregion

        #region AppBehaviour
        //Close Button

        [RelayCommand]
        public static void Close()
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion
    }
}