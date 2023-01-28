using System.Threading;

namespace CLI_TImer.Classes
{
    public sealed class Timer
    {
        #region Singelton
        private static Timer? instance = null;
        private static readonly object padLock = new();

        public static Timer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padLock)
                    {
                        instance ??= new();
                    }
                }
                return instance;
            }
        }
        #endregion
    }
}
