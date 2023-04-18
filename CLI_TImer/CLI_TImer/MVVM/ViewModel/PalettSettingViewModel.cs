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
        private List<Gradient> gradients;
        public PalettSettingViewModel() 
        { 
            PopulateGradientList();
        }

        private void PopulateGradientList()
        {
            var gradientList = new List<Gradient>();

            gradientList = AppDataManager.instance.GetGradientList().Select(x => x.Copy()).ToList();
        }
    }
}
