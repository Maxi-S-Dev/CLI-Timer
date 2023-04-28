using CLI_TImer.Helpers;
using CLI_TImer.MVVM.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI_TImer.MVVM.ViewModel
{
    public partial class PalettSettingViewModel : ObservableObject
    {
        [ObservableProperty]
        public List<Gradient> gradients;

        [ObservableProperty]
        public bool active = true;

        [ObservableProperty]
        public Gradient selectedGradient;

        public PalettSettingViewModel() 
        { 
            PopulateGradientList();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == nameof(SelectedGradient.StartHex))
            {
                Active = true;
                Trace.WriteLine($"{SelectedGradient.StartHex}");
            }
        }
        private void PopulateGradientList()
        {
            Gradients = new List<Gradient>();

            Gradients = AppDataManager.instance.GetGradientList().Select(x => x.Copy()).ToList();
        }

        //Saves all Changes and hides the save Menu
        [RelayCommand]
        public void SaveButtonPressed()
        {
            Active = false;
            Trace.WriteLine($"Hallo Welt | {Active}");
        }
    }
}
