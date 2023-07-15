using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CLI_Timer.MVVM.ViewModel;
namespace CLI_Timer.MVVM.View
{
    /// <summary>
    /// Interaktionslogik für ProfileSettingsView.xaml
    /// </summary>
    public partial class ProfileSettingsView : UserControl
    {
        public ProfileSettingsView()
        {
            InitializeComponent();

            DataContext = new ProfileSettingsViewModel();
        }

        private void Tg_Click(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            var item = (SettingsProfile)toggleButton.DataContext;
            var vm = (ProfileSettingsViewModel)DataContext;
            if(toggleButton.IsChecked == true) 
            {
                vm.ToggleButtonClick(item);
                return;
            }
            vm.ToggleButtonClick(null);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ListView = (ListView)sender;
            var vm = (ProfileSettingsViewModel)DataContext;
            if(vm.SelectedProfile is SettingsProfile selected)
            {
                vm.ToggleButtonClick(selected);
                return;
            }
            vm.ToggleButtonClick(null);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var vm = (ProfileSettingsViewModel)DataContext;
            var item = (SettingsProfile)button.DataContext;

            vm.DeleteItem(item);
        }
    }
}
