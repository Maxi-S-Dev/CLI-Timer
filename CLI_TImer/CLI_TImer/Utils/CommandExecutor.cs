﻿using CLI_Timer.Enums;
using CLI_Timer.MVVM.Model;
using CLI_Timer.Services;
using System.Configuration;
using System.Diagnostics;
using System.Reflection.Metadata;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using Windows.ApplicationModel.Background;
using Windows.UI.StartScreen;

namespace CLI_Timer.Utils
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
            profile = new();

            command = command.ToLower();
            command = command.Replace('/', '-');

            string action = command.Split(' ')[0];
            if (string.IsNullOrWhiteSpace(action)) return Error();

            string? parameter = command.Replace(action, string.Empty);

            switch (action)
            {
                case "start":
                    return Start(parameter);

                case "add":
                    return Add(parameter);

                case "sub":
                    return Sub(parameter);

                case "reset":
                    return ResetTimer(parameter);


                case "clear":
                    App.MainViewModel.ClearCommandHistoy();
                    return null;

                default: return Error();
            }
        }

        private static string Start(string parameter)
        {
            Profile p;

            if (string.IsNullOrWhiteSpace(parameter))
            {
                p = NewProfileManager.DefaultProfile;
            }

            AnalyseParameters(parameter);
            

            p = NewProfileManager.GetProfile(profile.Name);

            if (profile.Time != 0) p.Time = profile.Time;
            if (profile.TimerType != null) p.TimerType = profile.TimerType; 

            RunProfile(p);

            return p.Answer;
        }

        private static string Add(string parameter)
        {
            AnalyseParameters(parameter);

            int timeToAdd = profile.Time == 0 ? 300 : profile.Time;
            
            if(profile.TimerType == TimerType.primary)
            {
                Timer.AddTime(0, timeToAdd);

                return $"Added {timeToAdd} seconds";
            }

            if (profile.TimerType == TimerType.secondary)
            {
                Timer.AddTime(1, timeToAdd);

                return $"Added {timeToAdd} seconds";
            }

            if (profile.TimerType == TimerType.third)
            {
                Timer.AddTime(2, timeToAdd);

                return $"Added {timeToAdd} seconds";
            }

            return "error";
        }

        private static string Sub(string parameter)
        {
            AnalyseParameters(parameter);

            int timeToTake = profile.Time == 0 ? -300 : -profile.Time;

            if (profile.TimerType == TimerType.primary)
            {
                if (!Timer.AddTime(0, timeToTake)) return $"Failed to remove {timeToTake} seconds";

                return $"Removed {timeToTake} seconds";
            }

            if (profile.TimerType == TimerType.secondary)
            {
                if (!Timer.AddTime(1, timeToTake)) return $"Failed to remove {timeToTake} seconds";

                return $"Removed {timeToTake} seconds";
            }

            if (profile.TimerType == TimerType.third)
            {
                if (!Timer.AddTime(2, timeToTake)) return $"Failed to remove {timeToTake} seconds";
                

                return $"Removed {timeToTake} seconds";
            }

            return $"Removed {timeToTake} seconds";
        }

        private static string ResetTimer(string parameter)
        {
            Timer.Reset();
            return "Reset all Timers";
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

        private static void RunProfile(Profile profile)
        {
            if(profile.TimerType == TimerType.primary)
            {
                Timer.SetTimer(0, profile.Time);
                Timer.StartTimer(0);
                return;
            }

            if(profile.TimerType == TimerType.secondary) 
            {
                Timer.SetTimer(1, profile.Time);
                Timer.StartTimer(1);
                return;
            }

            if (profile.TimerType == TimerType.third)
            {
                Timer.SetTimer(2, profile.Time);
                Timer.StartTimer(2);
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