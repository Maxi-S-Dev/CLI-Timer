using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLI_TImer.Classes;

namespace CLI_TImer.Helpers
{
    public sealed class AppData
    {
        private static AppData? appData = null;
        private static readonly object padLock = new object();

        private Settings setting;

        public static AppData instance
        {
            get
            {
                lock (padLock) 
                { 
                    if(appData == null)
                        appData = new AppData();
                }
                return appData;
            }
        }


        public AppData()
        {
            

        }
    }
}
