using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CLI_TImer.MVVM.ViewModel
{
    public partial class SettingsWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        internal object currentView;

        MainSettingsViewModel mainSettingsViewModel = new();

        public SettingsWindowViewModel() 
        {
            currentView = mainSettingsViewModel;
        }
    }
}
