﻿using CLI_TImer.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CLI_TImer.Classes;
using System.Diagnostics;

namespace CLI_TImer
{
    public partial class App : Application
    {
        public static MainViewModel MainViewModel = new MainViewModel();

        ProfileManager? PM = new ProfileManager();

        public App() 
        {
            PM = null;
        }
    }
}
