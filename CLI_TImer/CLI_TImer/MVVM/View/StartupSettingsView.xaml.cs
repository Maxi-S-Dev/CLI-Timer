using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CLI_TImer.MVVM.ViewModel;

namespace CLI_TImer.MVVM.View
{
    /// <summary>
    /// Interaktionslogik für MainSettingsView.xaml
    /// </summary>
    public partial class StartupSettingsView : UserControl
    {
        public StartupSettingsView()
        {
            InitializeComponent();

            DataContext = new StartupSettingsViewModel();
        }
    }
}
