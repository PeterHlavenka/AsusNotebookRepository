using System;
using System.Windows.Navigation;
using WpfUniverse.Core;
using WpfUniverse.Entities;

namespace WpfUniverse.ViewModels
{
    public class PlanetsDialogViewModel
    {
        private readonly IDialogWindow m_dialogWindow;


        public PlanetsDialogViewModel(PlanetDataContract planet, IDialogWindow dialogWindow)
        {
            Planeta = planet ?? throw new ArgumentNullException();
            m_dialogWindow = dialogWindow;

            SavePlanetCommand = new CommandBase(Save);
            StornoPlanetCommand = new CommandBase(Storno);
        }


        public PlanetsDialogViewModel(int galaxyId, IDialogWindow dialogWindow)
            : this(PlanetDataContract.Create(new Planet { GalaxieId = galaxyId, Identifikator = Guid.NewGuid() }),
                dialogWindow)
        {
        }


        public CommandBase SavePlanetCommand { get; }
        public CommandBase StornoPlanetCommand { get; }


        public PlanetDataContract Planeta { get; set; }

        public int Id
        {
            get => Planeta.Id;

        }



        public string Jmeno
        {
            get => Planeta.Jmeno;
            set => Planeta.Jmeno = value;
        }

        public int Velikost
        {
            get => Planeta.Velikost;
            set => Planeta.Velikost = value;
        }

        public int GalaxieId
        {
            get => Planeta.GalaxieId;
            set => Planeta.GalaxieId = value;
        }

        public Guid Identifikator
        {
            get => Planeta.Identifikator;
            set => Planeta.Identifikator = value;
        }

        public bool OnStornoPressed { get; private set; }
        public bool OnSavePressed { get; private set; }


        private void Save()
        {
            OnSavePressed = true;
            m_dialogWindow.Close();
        }

        private void Storno()
        {
            OnStornoPressed = true;
            m_dialogWindow.Close();
        }
    }
}