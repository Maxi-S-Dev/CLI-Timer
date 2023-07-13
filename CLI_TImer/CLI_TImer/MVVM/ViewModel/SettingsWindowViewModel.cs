using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace CLI_Timer.MVVM.ViewModel
{
    public partial class SettingsWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        internal object? currentView;

        GeneralSettingsViewModel? generalSettingViewModel= new();
        PalettSettingViewModel? palettSettingViewModel= new();
        SoundSettingsViewModel? soundSettingsViewModel= new();
        ProfileSettingsViewModel? profileSettingsViewModel= new();

        public SettingsWindowViewModel() 
        {
            CurrentView = generalSettingViewModel;
        }

        [RelayCommand]
        public void ShowStartupSettings()
        {
            CurrentView = generalSettingViewModel;
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
