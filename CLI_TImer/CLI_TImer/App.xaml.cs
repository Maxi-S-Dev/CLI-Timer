using CLI_TImer.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CLI_TImer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainViewModel MainViewModel = new MainViewModel();

        protected override void OnExit(ExitEventArgs e)
        {
            
        }

    }
}
