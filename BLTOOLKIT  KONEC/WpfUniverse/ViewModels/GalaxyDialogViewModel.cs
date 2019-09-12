using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUniverse.Core;
using WpfUniverse.Entities;

namespace WpfUniverse.ViewModels
{
    public class GalaxyDialogViewModel
    {
       
        private GalaxyDataContract m_galaxy;
        private IDialogWindow m_dialogWindow;



        public GalaxyDialogViewModel(GalaxyDataContract galaxy, IDialogWindow dialogWindow)
        {
            m_galaxy = galaxy;
            m_dialogWindow = dialogWindow;

            SaveGalaxyCommand = new CommandBase(Save);
            StornoGalaxyCommand = new CommandBase(Storno);
        }



        //VLASTNOSTI BINDOVANE v XAML
        public int Id
        {
            get { return m_galaxy.Id; }
            set { }
        }
        public string Jmeno
        {
            get { return m_galaxy.Jmeno; }
            set { m_galaxy.Jmeno = value; }
        }
        public long PolohaX
        {
            get { return m_galaxy.PolohaX; }
            set { m_galaxy.PolohaX = value; }
        }
        public long PolohaY
        {
            get { return m_galaxy.PolohaY; }
            set { m_galaxy.PolohaY = value; }
        }
        public long PolohaZ
        {
            get { return m_galaxy.PolohaZ; }
            set { m_galaxy.PolohaZ = value; }
        }


        //COMMANDS
        public CommandBase SaveGalaxyCommand { get; private set; }
        public CommandBase StornoGalaxyCommand { get; private set; }
        public bool OnSavePressed { get; private set; }


        // METHODS CALLED BY COMMANDS
        private void Save()
        {
            OnSavePressed = true;
            Console.WriteLine("Save ");
            m_dialogWindow.Close();
        }
        private void Storno()
        {
            OnSavePressed = false;
            Console.WriteLine("Storno galaxy");
            m_dialogWindow.Close();
        }
    }
}
