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
    public partial class PalettSettingViewModel : ObservableObject
    {
        [ObservableProperty]
        public List<Gradient> gradients;

        [ObservableProperty]
        public bool active = true;
        public PalettSettingViewModel() 
        { 
            PopulateGradientList();
        }

        private void PopulateGradientList()
        {
            Gradients = new List<Gradient>();

            Gradients = AppDataManager.instance.GetGradientList().Select(x => x.Copy()).ToList();
        }
    }
}
