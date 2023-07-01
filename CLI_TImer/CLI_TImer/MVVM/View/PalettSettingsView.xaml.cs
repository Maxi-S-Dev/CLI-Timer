using CLI_Timer.MVVM.ViewModel;
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

namespace CLI_Timer.MVVM.View
{
    /// <summary>
    /// Interaktionslogik für PalettSettingsView.xaml
    /// </summary>
    public partial class PalettSettingsView : UserControl
    {
        public PalettSettingsView()
        {
            InitializeComponent();

            DataContext = new PalettSettingViewModel();
        }
    }
}
