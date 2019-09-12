using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfUniverse.Core;
using WpfUniverse.Entities;
using WpfUniverse.Views;

namespace WpfUniverse.ViewModels
{
    public class GalaxyViewModel : INotifyPropertyChanged, IGalaxySelector
    {
        private IGalaxyDao m_galaxyDao;
        private GalaxyDataContract m_selectedGalaxy;

        //CONSTRUCTORS
        public GalaxyViewModel(IDaoSource daoSource)
        {
            m_galaxyDao = daoSource.GetDaoByEntityType<IGalaxyDao, Galaxie, int>();

            ListOfGalaxies = m_galaxyDao.SelectAll().Select(x => GalaxyDataContract.Create(x)).ToList();

            EditGalaxy = new CommandBase(() => m_selectedGalaxy != null, DoEditGalaxy);
        }

        //COMMANDS
        public CommandBase EditGalaxy { get; private set; }


        //PROPERTIES
        /// <summary>
        /// nikdy se nemeni nacte se jen na zacatku , pridavat a ubirat galaxie nejdou.
        /// </summary>       
        public List<GalaxyDataContract> ListOfGalaxies { get; set; }

        /// <summary>
        /// Vlastnost na kterou binduje xaml v datagridu pomoci SelectedItem 
        /// </summary>
        public GalaxyDataContract SelectedGalaxy
        {
            get { return m_selectedGalaxy; }
            set
            {
                Console.WriteLine("Provedla se zmena na SelectedGalaxy");
                m_selectedGalaxy = value;
                OnPropertyChanged(nameof(SelectedGalaxy));
                FireGalaxyChanged(m_selectedGalaxy);                   //informuje posluchace ve tride planetViewModel
                EditGalaxy.FireCanExecute();
            }
        }



        //METHODS
        private void DoEditGalaxy()
        {
            Console.WriteLine("DoEditGalaxy");

            GalaxyDialogWindow gdw = new GalaxyDialogWindow();
            GalaxyDialogViewModel viewModel = new GalaxyDialogViewModel(SelectedGalaxy, gdw);
            gdw.DataContext = viewModel;
            gdw.ShowDialog();

            if (viewModel.OnSavePressed == true)
            {
                m_galaxyDao.Update(SelectedGalaxy.ConvertToDbEntity());
            }
        }

        

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion

        //EVENTS
        // udalost ktera informuje posluchace o tom, ze se zmenila Galaxie
        public event EventHandler<GalaxyDataContract> OnGalaxyChanged;
        public void FireGalaxyChanged(GalaxyDataContract galaxy)
        {
            //otaznik zjisti ze objekt neni null ( if(OnGalaxyChanged != null) )
            OnGalaxyChanged?.Invoke(this, galaxy);                       
        }

    }
}
