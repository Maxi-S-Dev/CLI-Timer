using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using IWshRuntimeLibrary;

namespace CLI_TImer.MVVM.ViewModel
{
    public partial class StartupSettingsViewModel: ObservableObject
    {
        [ObservableProperty,]
        public bool shortcutExits;
        

        public StartupSettingsViewModel()
        {
            shortcutExits = false;

            if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\CLI_TImer.lnk")) ShortcutExits = true;
        }

        [RelayCommand]
        public void ToggleStartupSetting()
        {

            if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\CLI_TImer.lnk"))
            {
                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\CLI_TImer.lnk");
                return;
            }
            CreateShortCut();
        }

        private void CreateShortCut()
        {
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\CLI_TImer.lnk";
            string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CLI_TImer.exe");

            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

            shortcut.Description = "My Application";
            shortcut.TargetPath = targetPath;
            shortcut.Save();
        }
    }
}
