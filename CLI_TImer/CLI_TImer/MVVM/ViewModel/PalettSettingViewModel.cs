using CLI_TImer.Services;
using CLI_TImer.MVVM.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;

using System.Linq;


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

        private Gradient[] selectedHistory = new Gradient[2];

        internal PalettSettingViewModel() 
        { 
            CreateGradientCollection();
        }


        #region GradientCollectionEvents
        private void CreateGradientCollection()
        {
            Gradients = new List<Gradient>(AppDataManager.instance.GetGradientList().Select(x => x.Copy()).ToList());
        }

        #endregion


        #region Save,Delete,New Buttons
        //Saves all Changes and hides the save Menu
        [RelayCommand]
        public void SaveButtonPressed()
        {
            AppDataManager.instance.SetGradientList(Gradients.ToList());
        }

        [RelayCommand]
        public void NewGradient()
        {
            Gradients.Add(new Gradient { StartHex="#FFFFFF", EndHex="#FFFFFF" });
        }

        [RelayCommand] 
        public void DeleteSelected()
        {
            Gradients.Remove(SelectedGradient);
        }
        #endregion
    }
}
