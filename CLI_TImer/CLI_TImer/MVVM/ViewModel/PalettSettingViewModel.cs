using CLI_Timer.MVVM.Model;
using CLI_Timer.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace CLI_Timer.MVVM.ViewModel
{
    public partial class PalettSettingViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<Gradient> gradients;

        [ObservableProperty]
        public bool active = true;


        public PalettSettingViewModel() 
        { 
            CreateGradientCollection();
        }

        private void CreateGradientCollection()
        {
            Gradients = new ObservableCollection<Gradient>(AppDataManager.instance.GetGradientList().Select(x => x.Copy()).ToList());

            foreach (Gradient g in Gradients)
            {
                Trace.WriteLine("Test");
                g.PropertyChanged += Save;
            }
        }


        #region Save,Delete,New Buttons
        //Saves all Changes and hides the save Menu
        private void Save(object sender, PropertyChangedEventArgs e)
        {
            Trace.Write("Save");
            AppDataManager.instance.SetGradientList(Gradients.ToList());
        }

        public void AddGradient()
        {
            Gradient g = new Gradient { StartHex="#FFFFFF", EndHex="#FFFFFF" };
            g.PropertyChanged += Save;
            Gradients.Add(g);
        }

        public void DeleteGradient(Gradient gradient)
        {
            Gradients.Remove(gradient);
        }
        #endregion
    }
}
