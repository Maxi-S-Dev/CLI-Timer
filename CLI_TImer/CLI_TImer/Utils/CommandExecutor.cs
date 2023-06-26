using CLI_TImer.Enums;
using CLI_TImer.MVVM.Model;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.UI.StartScreen;

namespace CLI_TImer.Utils
{
    public static class CommandExecutor
    {
        static Profile profile;
        static CommandAction? action;
        public static string Execute(string command)
        {
            command = command.ToLower();

            string action = command.Split(' ') [0];
            if (string.IsNullOrWhiteSpace(action)) return Error();

            string? parameter = command.Replace(action, string.Empty);

            switch (action) 
            {
                case "start": return Start(parameter);


                default: return "Error";
            }
        }

        private static string Start(string parameter)
        {
            profile = new();
            string argument = ""; 

            if (string.IsNullOrWhiteSpace(parameter))
            {
                Trace.WriteLine("Default Profile");
                return null;
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


            Trace.WriteLine($"Profile Name: {profile.Name}");
            Trace.WriteLine($"NewTime: {profile.Time}");
            return null;
        }

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

        private static string Error()
        {
            return "Error";
        }
    }
}
