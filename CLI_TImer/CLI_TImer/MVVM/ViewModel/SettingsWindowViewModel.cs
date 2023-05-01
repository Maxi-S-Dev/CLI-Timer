using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CLI_TImer.MVVM.ViewModel
{
    public partial class SettingsWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        internal object? currentView;

        GernalSettingsViewModel? startupSettingsViewModel= new();
        PalettSettingViewModel? palettSettingViewModel= new();
        SoundSettingsViewModel? soundSettingsViewModel= new();
        ProfileSettingsViewModel? profileSettingsViewModel= new();

        public SettingsWindowViewModel() 
        {
            CurrentView = startupSettingsViewModel;
        }

        [RelayCommand]
        public void ShowStartupSettings()
        {
            CurrentView = startupSettingsViewModel;
        }

        [RelayCommand]
        public void ShowPaletteSettings() 
        { 
            CurrentView = palettSettingViewModel;
        }

        [RelayCommand]
        public void ShowProfileSettings() 
        { 
            CurrentView = profileSettingsViewModel;
        }

        [RelayCommand]
        public void ShowSoundSettings() 
        { 
            CurrentView = soundSettingsViewModel;
        }
    }
}
