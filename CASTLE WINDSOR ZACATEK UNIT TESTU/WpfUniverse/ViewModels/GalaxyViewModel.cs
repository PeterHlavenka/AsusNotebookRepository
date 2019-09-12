using System;
using System.Collections.Generic;
using System.Linq;
using WpfUniverse.Core;
using WpfUniverse.Entities;
using WpfUniverse.Views;

namespace WpfUniverse.ViewModels
{
    public class GalaxyViewModel : ViewModelBase, IGalaxySelector
    {
        private readonly IGalaxyDao m_galaxyDao;
        private GalaxyDataContract m_selectedGalaxy;


        public GalaxyViewModel(IGalaxyDao galaxyDao) 
        {
            m_galaxyDao = galaxyDao;

            ListOfGalaxies = m_galaxyDao.SelectAll().Select(GalaxyDataContract.Create).ToList();

            EditGalaxy = new CommandBase(() => m_selectedGalaxy != null, DoEditGalaxy);
        }


        public CommandBase EditGalaxy { get; }
        public List<GalaxyDataContract> ListOfGalaxies { get; set; }
        public GalaxyDataContract SelectedGalaxy
        {
            get => m_selectedGalaxy;
            set
            {
                m_selectedGalaxy = value;
                OnPropertyChanged(nameof(SelectedGalaxy));
                FireGalaxyChanged(m_selectedGalaxy);
                EditGalaxy.FireCanExecute();
            }
        }

        private void DoEditGalaxy()
        {
            GalaxyDialogWindow gdw = new GalaxyDialogWindow();
            GalaxyDialogViewModel viewModel = new GalaxyDialogViewModel(SelectedGalaxy, gdw);
            gdw.DataContext = viewModel;
            gdw.ShowDialog();

            if (viewModel.OnSavePressed)
            {
                m_galaxyDao.Update(SelectedGalaxy.ConvertToDbEntity());
            }
        }



        public event EventHandler<GalaxyDataContract> OnGalaxyChanged;
        public void FireGalaxyChanged(GalaxyDataContract galaxy)
        {
            OnGalaxyChanged?.Invoke(this, galaxy);
        }
    }
}
