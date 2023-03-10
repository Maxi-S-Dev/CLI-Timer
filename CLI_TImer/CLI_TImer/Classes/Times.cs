namespace CLI_TImer.Classes
{
    public static class Times
    {
        public static int TimeToSeconds(int hours, int minutes) => HoursToSeconds(hours) + MinutesToSeconds(minutes);
        public static int TimeToSeconds(int hours, int minutes, int seconds) => HoursToSeconds(hours) + MinutesToSeconds(minutes) + seconds;

        public static int SecondsToHours(int seconds) => seconds / 3600;
        public static int SecondsToMinutes(int seconds) => (seconds % 3600) / 60;

        public static int HoursToSeconds(int hours) => hours * 3600;
        public static int MinutesToSeconds(int minutes) => minutes * 60;
    }
}
