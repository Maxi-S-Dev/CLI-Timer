using System.Windows.Controls;
using System.Windows.Data;
using CLI_Timer.MVVM.ViewModel;
using CLI_Timer.Controls;

namespace CLI_Timer.MVVM.View
{
    /// <summary>
    /// Interaktionslogik für MainSettingsView.xaml
    /// </summary>
    public partial class GernalSettingsView : UserControl
    {
        public GernalSettingsView()
        {
            var vm = new GeneralSettingsViewModel();
            DataContext = vm;
            InitializeComponent();
        }
    }
}