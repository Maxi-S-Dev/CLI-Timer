using CLI_TImer.Helpers;
using CLI_TImer.MVVM.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        public ObservableCollection<Gradient> gradients;

        [ObservableProperty]
        public bool active = true;

        [ObservableProperty]
        public Gradient selectedGradient;

        private Gradient[] selectedHistory = new Gradient[2];

        public PalettSettingViewModel() 
        { 
            CreatedGradientCollection();
        }



        private void UpdateListView()
        {
            Gradient g = SelectedGradient;
            int index = Gradients.IndexOf(SelectedGradient);
            Gradients.Remove(SelectedGradient);
            Gradients.Insert(index, g);
        }
        private void CreatedGradientCollection()
        {
            Gradients = new ObservableCollection<Gradient>(AppDataManager.instance.GetGradientList().Select(x => x.Copy()).ToList());
            Gradients.CollectionChanged += CollectionChanged;
        }


        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Trace.WriteLine("Collection Changed");
            if(e.OldItems != null) 
            {
                foreach (INotifyPropertyChanged item in e.OldItems) 
                { 
                    item.PropertyChanged -= ItemPropertyChanged;
                }
            }
            
            if(e.NewItems != null)
            {
                foreach(INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += ItemPropertyChanged;
                }
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Trace.WriteLine(e.PropertyName);
            UpdateListView();
        }



        //Saves all Changes and hides the save Menu
        [RelayCommand]
        public void SaveButtonPressed()
        {
            Active = false;
        }
    }
}
