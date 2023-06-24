using CLI_TImer.MVVM.ViewModel;
using System.Windows;


namespace CLI_TImer
{
    public partial class App : Application
    {
        public static MainViewModel MainViewModel = new MainViewModel();

        public App() 
        {
        }
    }
}
