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
using CLI_Timer.MVVM.Model;

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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (PalettSettingViewModel)DataContext;
            vm.AddGradient();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var vm = (PalettSettingViewModel)DataContext;
            var item = (Gradient)button.DataContext;

            if(item is not null)
            {
                vm.DeleteGradient(item);
            }
        }
    }
}
