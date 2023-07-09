using CLI_Timer.Enums;
using CLI_Timer.MVVM.Model;
using CLI_Timer.MVVM.ViewModel;
using CLI_Timer.Services;
using System.Diagnostics;
using System.Windows.Media;

namespace CLI_Timer.Utils
{
    public static class CommandExecutor
    {
        static Profile profile;
        static CommandAction? action;

        private static AppDataManager dataManager = AppDataManager.instance;

        /// <summary>
        /// Executes a given command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string? Execute(string command)
        {
            profile = new();

            command = command.ToLower();
            command = command.Replace('/', '-');
            command = command.Trim();

            string action = command.Split(' ')[0];

            string? parameter = command.Replace(action, string.Empty);

            if (action == "cls") action = "clear";

            switch (action)
            {
                case "start":
                    return Start(parameter);

                case "add":
                    return Add(parameter);

                case "sub":
                    return Sub(parameter);

                case "reset":
                    return Reset(parameter);

                case "clear":
                    App.MainViewModel.ClearCommandHistory();
                    return null;

                case "settings":
                    App.MainViewModel.OpenSettingsWindow();
                    return null;

                case "new":
                    return New(parameter);

                case "delete":
                    return Delete(parameter);

                case "use":
                    return Use(parameter);

                case "close":
                    MainViewModel.Close();
                    return null;

                default: return Error(action);
            }
        }

        private static string Start(string parameter)
        {
            Profile p;

            AnalyseParameters(parameter);

            p = ProfileManager.GetProfile(profile.Name);

            if (profile.Time != 0) p.Time = profile.Time;
            if (profile.TimerType != null) p.TimerType = profile.TimerType; 

            RunProfile(p);

            return p.Answer;
        }

        private static string Add(string parameter)
        {
            AnalyseParameters(parameter);

            int timeToAdd = profile.Time == 0 ? 300 : profile.Time;
            
            if(profile.TimerType == TimerType.primary || profile.TimerType is null)
            {
                Timer.AddTime(0, timeToAdd);

                return $"Added {TimeConverter.SecondsToTimeText(timeToAdd)}";
            }

            if (profile.TimerType == TimerType.secondary)
            {
                Timer.AddTime(1, timeToAdd);

                return $"Added {TimeConverter.SecondsToTimeText(timeToAdd)}";
            }

            if (profile.TimerType == TimerType.third)
            {
                Timer.AddTime(2, timeToAdd);

                return $"Added {TimeConverter.SecondsToTimeText(timeToAdd)}";
            }

            return "error";
        }

        private static string Sub(string parameter)
        {
            AnalyseParameters(parameter);

            int timeToTake = profile.Time == 0 ? -300 : profile.Time;

            if (profile.TimerType == TimerType.primary || profile.TimerType is null)
            {
                if (!Timer.AddTime(0, -timeToTake)) return $"Failed to remove {TimeConverter.SecondsToTimeText(timeToTake)}";

                return $"Removed {TimeConverter.SecondsToTimeText(timeToTake)}";
            }

            if (profile.TimerType == TimerType.secondary)
            {
                if (!Timer.AddTime(1, -timeToTake)) return $"Failed to remove {TimeConverter.SecondsToTimeText(timeToTake)}";

                return $"Removed {TimeConverter.SecondsToTimeText(timeToTake)}";
            }

            if (profile.TimerType == TimerType.third)
            {
                if (!Timer.AddTime(2, -timeToTake)) return $"Failed to remove {TimeConverter.SecondsToTimeText(timeToTake)}";


                return $"Removed {TimeConverter.SecondsToTimeText(timeToTake)}";
            }

            return $"Something went wrong";
        }

        private static string Reset(string parameter)
        {
            if(parameter.Trim() == "config")
            {
                dataManager.DeleteAppData();
                return "Restored default config";
            }

            if (string.IsNullOrEmpty(parameter))
            {
                Timer.Reset();
                return "Reset all Timers";
            }
            
            AnalyseParameters(parameter);

            if(profile.TimerType == TimerType.primary)
            {
                Timer.Reset(0);
                return "Reset primary";
            }

            if (profile.TimerType == TimerType.secondary)
            {
                Timer.Reset(1);
                return "Reset secondary";
            }

            if (profile.TimerType == TimerType.third)
            {
                Timer.Reset(2);
                return "Reset hidden";
            }
            return "Error";
        }

        private static string New(string parameter)
        {
            AnalyseParameters(parameter);

            if (string.IsNullOrEmpty(profile.Name)) return "Please Enter a Name";
            if (profile.Time == 0) return "Please Enter a Time";

            return ProfileManager.AddProfile(profile);
        }

        private static string Delete(string parameter)
        {
            AnalyseParameters(parameter);

            if (string.IsNullOrEmpty(profile.Name)) return "Please enter a name";

            return ProfileManager.RemoveProfile(profile);
        }

        private static string Use(string parameter)
        {
            AnalyseParameters(parameter);

            if (string.IsNullOrEmpty(profile.Name)) return "Please enter a Name";

            return ProfileManager.UpdateProfile(profile);
        }

        private static void AnalyseParameters(string parameter)
        {
            string argument = "";
            for (int i = 0; i < parameter.Length; i++)
            {
                if (char.IsWhiteSpace(parameter[i]))
                {
                    InterpretArgument(argument);
                    argument = string.Empty;
                    continue;
                }

                if (parameter[i] == '-' && i++ < parameter.Length)
                {
                    FindAction(parameter[i++]);
                    continue;
                }

                argument += parameter[i];
            }

            InterpretArgument(argument);
        }

        private static void RunProfile(Profile p)
        {
            if(p.TimerType == TimerType.primary)
            {
                if(p.Time > 0 ) Timer.SetTimer(0, p.Time);
                Timer.StartTimer(0);
                App.MainViewModel.PrimaryRunningProfile = p;
                return;
            }

            if(p.TimerType == TimerType.secondary) 
            {
                if (p.Time > 0) Timer.SetTimer(1, p.Time);
                Timer.StartTimer(1);
                App.MainViewModel.SecondaryRunningProfile = p;
                return;
            }

            if (p.TimerType == TimerType.third)
            {
                if (p.Time > 0) Timer.SetTimer(2, p.Time);
                Timer.StartTimer(2);
                App.MainViewModel.ThirdRunningProfile = p;
                return;
            }
        }


        //Sets the action type which will be performed next
        private static void FindAction(char parameter)
        {
            switch(parameter) 
            {
                case 't':
                    action = CommandAction.Time;
                    Trace.WriteLine("setting time");
                    break;

                case 'n':
                    action = CommandAction.Name;
                    Trace.WriteLine("setting name");
                    break;

                case 'p':
                    profile.TimerType = TimerType.primary;
                    break;

                case 's':
                    profile.TimerType = TimerType.secondary;
                    break;

                case 'h':
                    profile.TimerType = TimerType.third;
                    break;

                case 'a':
                    action = CommandAction.Answer;
                    break;
                   
            }
        }

        //takes in an argument and does something with it depending on the current action and argument
        private static void InterpretArgument(string argument)
        {
            if (action == null) return;

            switch(action)
            {
                case CommandAction.Name:
                    profile.Name = argument;
                    action = null;
                    break;

                case CommandAction.Time:
                    TextToTime(argument);
                    break;

                case CommandAction.Answer:
                    profile.Answer = argument;
                    break;
            }
        }

        //converts a time string (2h) to an integer and adds it to the p
        private static void TextToTime(string timeText)
        {
            if(string.IsNullOrEmpty(timeText)) return;
            int time = 0;
            if (timeText[^1] == 'h')
            {
                _=int.TryParse(timeText.Remove(timeText.Length - 1), out time);
                time = TimeConverter.HoursToSeconds(time);
            }
            else if (timeText[^1] == 'm')
            {
                _=int.TryParse(timeText.Remove(timeText.Length - 1), out time);
                time = TimeConverter.MinutesToSeconds(time);
            }
            else if (timeText[^1] == 's') _=int.TryParse(timeText.Remove(timeText.Length - 1), out time);

            profile.Time += time;
        }

        //Returns an error message
        private static string Error(string action)
        {
            return $"Unknown command {action}";
        }
    }
}
