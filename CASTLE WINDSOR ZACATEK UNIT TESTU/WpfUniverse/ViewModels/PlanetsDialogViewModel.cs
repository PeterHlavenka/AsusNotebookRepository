using System;
using WpfUniverse.Core;
using WpfUniverse.Entities;

namespace WpfUniverse.ViewModels
{
    public class PlanetsDialogViewModel
    {
        private PlanetDataContract m_planeta;     
        private readonly IDialogWindow m_dialogWindow;


       
        public PlanetsDialogViewModel(PlanetDataContract planet, IDialogWindow dialogWindow)
        {
            m_planeta = planet ?? throw new ArgumentNullException();          
            m_dialogWindow = dialogWindow;

            SavePlanetCommand = new CommandBase(Save);
            StornoPlanetCommand = new CommandBase(Storno);
        }
        
        
        public PlanetsDialogViewModel(int galaxyId, IDialogWindow dialogWindow)
            :this( PlanetDataContract.Create(  new Planet() { GalaxieId = galaxyId, Identifikator = Guid.NewGuid() }  )   , dialogWindow)
        {
        }


        
        public CommandBase SavePlanetCommand { get; }
        public CommandBase StornoPlanetCommand { get; }
        


        
        public PlanetDataContract Planeta 
        {
            get => m_planeta;
            set => m_planeta = value;
        }
        public int Id => m_planeta.Id;

        public string Jmeno
        {
            get => m_planeta.Jmeno;
            set => m_planeta.Jmeno = value;
        }
        public int Velikost
        {
            get => m_planeta.Velikost;
            set => m_planeta.Velikost = value;
        }
        public int GalaxieId
        {
            get => m_planeta.GalaxieId;
            set => m_planeta.GalaxieId = value;
        }
        public Guid Identifikator
        {
            get => m_planeta.Identifikator;
            set => m_planeta.Identifikator = value;
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
