using WpfUniverse.Core;

namespace WpfUniverse.ViewModels
{
    public class GalaxyDialogViewModel
    {
        private readonly IDialogWindow m_dialogWindow;

        private readonly GalaxyDataContract m_galaxy;


        public GalaxyDialogViewModel(GalaxyDataContract galaxy, IDialogWindow dialogWindow)
        {
            m_galaxy = galaxy;
            m_dialogWindow = dialogWindow;

            SaveGalaxyCommand = new CommandBase(Save);
            StornoGalaxyCommand = new CommandBase(Storno);
        }


        public int Id => m_galaxy.Id;

        public string Jmeno
        {
            get => m_galaxy.Jmeno;
            set => m_galaxy.Jmeno = value;
        }

        public long PolohaX
        {
            get => m_galaxy.PolohaX;
            set => m_galaxy.PolohaX = value;
        }

        public long PolohaY
        {
            get => m_galaxy.PolohaY;
            set => m_galaxy.PolohaY = value;
        }

        public long PolohaZ
        {
            get => m_galaxy.PolohaZ;
            set => m_galaxy.PolohaZ = value;
        }


        public CommandBase SaveGalaxyCommand { get; }
        public CommandBase StornoGalaxyCommand { get; }
        public bool OnSavePressed { get; private set; }


        private void Save()
        {
            OnSavePressed = true;
            m_dialogWindow.Close();
        }

        private void Storno()
        {
            OnSavePressed = false;
            m_dialogWindow.Close();
        }
    }
}