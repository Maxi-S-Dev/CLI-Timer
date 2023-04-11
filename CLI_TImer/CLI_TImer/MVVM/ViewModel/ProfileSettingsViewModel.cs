using CLI_TImer.Helpers;
using CLI_TImer.MVVM.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI_TImer.MVVM.ViewModel
{
    internal partial class ProfileSettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        internal List<Profile> profiles;
        internal ProfileSettingsViewModel() 
        {
            Profiles = AppDataManager.instance.getProfileList();
        }        
    }
}
