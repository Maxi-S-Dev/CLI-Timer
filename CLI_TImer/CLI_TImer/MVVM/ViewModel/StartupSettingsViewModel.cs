using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI_TImer.MVVM.ViewModel
{
    internal class StartupSettingsViewModel
    {
        public List<int> ZeroToTwentyfour {
            get
            {
                List<int> list = new List<int>();
                for (int i = 0; i <= 24; i++)
                {
                    list.Add(i);
                }
                return list;
            }
            private set { }
        }

        public List<int> ZeroToSixty
        {
            get
            {
                List<int> list = new List<int>();
                for (int i = 0; i <= 60; i++)
                {
                    list.Add(i);
                }
                return list;
            }
            private set { }
        }
    }
}
