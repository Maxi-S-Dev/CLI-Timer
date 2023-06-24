using CLI_TImer.MVVM.Model;
using CLI_TImer.Helpers;
using CLI_TImer.Classes;

namespace CLI_TImer.Utils
{
    public static class ProfileManager
    {
        private static AppDataManager appDataManager = AppDataManager.instance;

        internal static Profile? getProfileFromCommand(string command)
        {
            foreach (Profile p in appDataManager.GetProfileList())
            {
                if (p.Name == command) return p;
            }
            return null;
        }

        internal static void AddNewProfile(string Name, string answer, int Time, string Type)
        {
            TimerType tp = TimerType.main;
            if (Type == "second") tp = TimerType.second;

            appDataManager.AddNewProfile(new Profile { Name = Name, Answer = answer, Time = Time, TimerType = tp });
        }

        internal static void DeleteProfile(string name)
        {
            Profile? p = getProfileFromCommand(name);
            if (p is null) return;

            appDataManager.RemoveProfile(p);

        }

        internal static void UpdateProfile(string Name, string Property, string Value)
        {
            Profile? p = getProfileFromCommand(Name);

            if (p is null) return;

            appDataManager.RemoveProfile(p);

            switch (Property)
            {
                case "name":
                    p.Name = Value;
                    break;

                case "answer":
                    p.Answer = Value;
                    break;
            }

            appDataManager.AddNewProfile(p);
        }

        internal static void UpdateProfile(string Name, int Time)
        {
            Profile? p = getProfileFromCommand(Name);

            if (p is null) return;

            appDataManager.RemoveProfile(p);

            p.Time = Time;

            appDataManager.AddNewProfile(p);
        }
    }
}