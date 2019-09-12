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
    public class GalaxyViewModel : INotifyPropertyChanged
    {
        private GalaxyDao m_galaxyDao;
        private GalaxyDataContract m_selectedGalaxy;

        public GalaxyViewModel()
        {
            m_galaxyDao = new GalaxyDao(MainWindow.Conn);
            ListOfGalaxies = m_galaxyDao.LoadAllGalaxies().Select(x => GalaxyDataContract.Create(x)).ToList();

            EditGalaxy = new CommandBase(() => m_selectedGalaxy != null, DoEditGalaxy);
        }


        public CommandBase EditGalaxy { get; private set; }



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



        private void DoEditGalaxy()
        {
            Console.WriteLine("DoEditGalaxy");

            GalaxyDialogWindow gdw = new GalaxyDialogWindow();
            GalaxyDialogViewModel viewModel = new GalaxyDialogViewModel(SelectedGalaxy, gdw);
            gdw.DataContext = viewModel;
            gdw.ShowDialog();

            if (viewModel.OnSavePressed == true)
            {
                m_galaxyDao.SaveGalaxy(SelectedGalaxy.ConvertToDbEntity());
            }
        }













        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion


        // udalost ktera informuje posluchace o tom, ze se zmenila Galaxie
        public event EventHandler<GalaxyDataContract> OnGalaxyChanged;
        private void FireGalaxyChanged(GalaxyDataContract galaxy)
        {
            //otaznik zjisti ze objekt neni null ( if(OnGalaxyChanged != null) )
            OnGalaxyChanged?.Invoke(this, galaxy);                       
        }

    }
}
