using CLI_Timer.MVVM.ViewModel;
using System.Windows;


namespace CLI_Timer
{
    public partial class App : Application
    {
        public static MainViewModel MainViewModel = new MainViewModel();

        public App() 
        {
            InitializeComponent();
        }
    }
}
