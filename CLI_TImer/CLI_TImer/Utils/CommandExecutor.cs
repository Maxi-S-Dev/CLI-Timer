using CLI_TImer.Enums;
using CLI_TImer.MVVM.Model;
using CLI_TImer.Services;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.UI.StartScreen;

namespace CLI_TImer.Utils
{
    public static class CommandExecutor
    {
        static Profile profile;
        static CommandAction? action;

        /// <summary>
        /// Executes a given command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string? Execute(string command)
        {
            command = command.ToLower();
            command = command.Replace('/', '-');

            string action = command.Split(' ')[0];
            if (string.IsNullOrWhiteSpace(action)) return Error();

            string? parameter = command.Replace(action, string.Empty);

            switch (action)
            {
                case "start":
                    return Start(parameter);


                case "clear":
                    App.MainViewModel.ClearCommandHistoy();
                    return null;

                default: return Error();
            }
        }

        private static string Start(string parameter)
        {
            profile = new();
            Profile p;
            string argument = "";

            if (string.IsNullOrWhiteSpace(parameter))
            {
                p = NewProfileManager.DefaultProfile;
            }

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

            p = NewProfileManager.GetProfile(profile.Name);

            if (profile.Time != 0) p.Time = profile.Time;

            RunProfile(p);

            return p.Answer;
        }

        private static void RunProfile(Profile profile)
        {
            if(profile.TimerType == TimerType.main)
            {
                Timer.SetTimer(0, profile.Time);
                Timer.StartTimer(0);
            }

            if(profile.TimerType == TimerType.second) 
            {
                Timer.SetTimer(1, profile.Time);
                Timer.StartTimer(1);
            }
        }


        //Sets the action type which will be perfomed next
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
            }
        }

        //converts a time string (2h) to an integer and adds it to the profile
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
        private static string Error()
        {
            return "Erro Description";
        }
    }
}
