using System;
using Caliburn.Micro;
using WpfUniverse.Common.Datacontracts;

namespace WpfUniverse.Gui.ViewModels
{
    public class MainViewModel : Screen
    {
        public MainViewModel(GalaxyViewModel galaxyViewModel, PlanetsViewModel planetsViewModel)
        {
            GalaxyViewModel = galaxyViewModel;
            PlanetsViewModel = planetsViewModel;
        }

        public GalaxyViewModel GalaxyViewModel { get; }

        public  PlanetsViewModel PlanetsViewModel { get; }

        public void Initialize()
        {
            GalaxyViewModel.Initialize();
        }
    }
}