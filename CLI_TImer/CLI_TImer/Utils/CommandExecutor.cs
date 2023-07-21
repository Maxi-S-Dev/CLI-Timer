using CLI_Timer.Enums;
using CLI_Timer.MVVM.Model;
using CLI_Timer.MVVM.ViewModel;
using CLI_Timer.Services;
using System.Diagnostics;

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

                case "help":
                    return Help(parameter);

                case "pause":
                    return Pause(parameter);

                case "continue":
                    return Continue(parameter);

                default: return Error(action);
            }
        }

        private static string Start(string parameter)
        {
            Profile p;

            if(AnalyseParameters(parameter) is not null)
            {
                return "start will start a timer" +
                    "\n use /n (start /n work) to run a profile" +
                    "\n use /p or /s to start a specific timer" +
                    "\n use /t to specify the time (/t 5h 2m 1s)";
            }

            p = ProfileManager.GetProfile(profile.Name);

            if (profile.Time != 0) p.Time = profile.Time;
            if (profile.TimerType != TimerType.primary) p.TimerType = profile.TimerType; 

            RunProfile(p);

            return p.Answer;
        }

        private static string Add(string parameter)
        {
            AnalyseParameters(parameter);

            if (AnalyseParameters(parameter) is not null)
            {
                return "add will add time to a timer" +
                    "\n add adds 5m to the running timer" +
                    "\n /p or /s specify the timer" +
                    "\n /t to specifies the time (/t 5h 2m 1s)";
            }

            int timeToAdd = profile.Time == 0 ? 300 : profile.Time;
            
            if(profile.TimerType == TimerType.primary)
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

            if (AnalyseParameters(parameter) is not null)
            {
                return "sub will remove time to a timer" +
                "\n sub removes 5m from the running timer" +
                "\n /p or /s specify the timer" +
                "\n /t specifies the time (/t 5h 2m 1s)";
            }

            int timeToTake = profile.Time == 0 ? -300 : profile.Time;

            if (profile.TimerType == TimerType.primary)
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
            
            if (AnalyseParameters(parameter) is not null)
            {
                return " resets will reset the running timer" +
                    "\n /p or /s specifie the the timer" +
                    "\n 'reset config' resets all settings";
            }

            if (profile.TimerType == TimerType.primary)
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
            if (AnalyseParameters(parameter) is not null)
            {
                return "new creates a new profile" +
                    "\n specify the name with /n [[name]]" +
                    "\n specify the time with /t [[1h 1m 1s]]" +
                    "\n specify the type with /p or /s";
            }

            if (string.IsNullOrEmpty(profile.Name)) return "Please Enter a Name";
            if (profile.Time == 0) return "Please Enter a Time";

            return ProfileManager.AddProfile(profile);
        }

        private static string Delete(string parameter)
        {
            if (AnalyseParameters(parameter) is not null)
            {
                return "deletes a profile" +
                    "\n you must provide a name with /n [[NAME]]";
            }

            if (string.IsNullOrEmpty(profile.Name)) return "Please enter a name";

            return ProfileManager.RemoveProfile(profile);
        }

        private static string Use(string parameter)
        {
            if (AnalyseParameters(parameter) is not null)
            {
                return "updates one or more proerties of a timer" +
                    "\n use /n to change the name" +
                    "\n use /t to change the time (/t 1h 2m 3s)" +
                    "\n use /p or /s to change the type";
            }

            if (string.IsNullOrEmpty(profile.Name)) return "Please enter a Name";

            return ProfileManager.UpdateProfile(profile);
        }

        private static string Help(string parameter)
        {
            return $"You can use following commands: " +
                $"\n start - to start a timer" +
                $"\n add or sub - to change the remaining time" +
                $"\n reset - to reset a timer" +
                $"\n new - to create a new timer" +
                $"\n use - to edit a timer" +
                $"\n delete - to delete a timer" +
                $"\n clear - to clear the history" +
                $"\n settings - to open the settings" +
                $"\n close - to close the app" +
                $"\n write /h after a command for more help";
        }

        private static string Pause(string parameter)
        {
            if(AnalyseParameters(parameter) is not null)
            {
                return " pauses the current Timer";
            }

            Timer.Pause();

            return "paused all timers";
        }

        private static string Continue(string parameter)
        {
            if(AnalyseParameters(parameter) is not null)
            {
                return "continues paused timers";
            }

            Timer.Continue();
            return "continued paused timers";
        }

        private static object AnalyseParameters(string parameter)
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
                    if(action == CommandAction.Help) return "help";
                    continue;
                }

                argument += parameter[i];
            }

            InterpretArgument(argument);
            return null;
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

                case 'u':
                    profile.TimerType = TimerType.third;
                    break;

                case 'a':
                    action = CommandAction.Answer;
                    break;

                case 'h':
                    action = CommandAction.Help;
                    return;
                   
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
