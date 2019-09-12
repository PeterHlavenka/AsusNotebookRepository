using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUniverse.Core;
using WpfUniverse.Entities;

namespace WpfUniverse.ViewModels
{
    public class PlanetsDialogViewModel
    {
        private PlanetDataContract m_planeta;
        private PlanetDao m_dao;     
        private IDialogWindow m_dialogWindow;

        /// <summary>
        /// KONSTRUKTOR PRO EDITACI 
        /// </summary>
        /// <param name="planet"> planeta ktera se bude upravovat</param>
        /// <param name="dao"> na tride dao se volaji metody pro INSERT UPDATE DELETE </param>
        public PlanetsDialogViewModel(PlanetDataContract planet, IDialogWindow dialogWindow)
        {
            if (planet == null)
            {
                throw new ArgumentNullException();
            }

            m_planeta = planet;          
            m_dialogWindow = dialogWindow;

            SavePlanetCommand = new CommandBase(Save);
            StornoPlanetCommand = new CommandBase(Storno);
        }



        /// <summary>
        /// KONSTRUKTOR PRO PRIDANI NOVE PLANETY
        /// Vola jiny konstruktor teto tridy ktery bere jako parametr planetu s tim, ze rovnou vytvori planetu a naplni ji hodnoty GalaxieId a Identifikator. 
        /// To jsou nalezitosti ktere nepridava uzivatel.
        /// </summary>
        /// <param name="galaxyId">Id galaxie ve ktere se bude planeta pridavat </param>
        /// <param name="dao"> Na tride dao se volaji metody pro INSERT UPDATE DELETE </param>
        public PlanetsDialogViewModel(int galaxyId, IDialogWindow dialogWindow)
            :this( PlanetDataContract.Create(  new Planeta() { GalaxieId = galaxyId, Identifikator = Guid.NewGuid() }  )   , dialogWindow)
        {
        }

        /// <summary>
        /// Tento command se provede po stlaceni tlacitka save ve view
        /// </summary>
        public CommandBase SavePlanetCommand { get; private set; }
        public CommandBase StornoPlanetCommand { get; private set; }



       
       
       
        // Na binding XAML
        public PlanetDataContract Planeta 
        {
            get { return m_planeta; }
            set { m_planeta = value; }
        }
        public int Id
        {
            get { return m_planeta.Id; }
            set { }
        }
        public string Jmeno
        {
            get { return m_planeta.Jmeno; }
            set { m_planeta.Jmeno = value; }
        }
        public int Velikost
        {
            get { return m_planeta.Velikost; }
            set { m_planeta.Velikost = value; }
        }
        public int GalaxieId
        {
            get { return m_planeta.GalaxieId; }
            set { m_planeta.GalaxieId = value; }
        }
        public Guid Identifikator
        {
            get { return m_planeta.Identifikator; }
            set { m_planeta.Identifikator = value; }
        }


        public bool OnStornoPressed { get; private set; }

        public bool OnSavePressed { get; private set; }


        /// <summary>
        /// Metoda je vyvolana stisknutim tlacitka ve view , pomoci commandu SavePlanetCommand
        /// METODA PREZ DAO TRIDU A JEJI METODY VLEZE DO TRIDY EntityDaoBase KDE PROVEDE CRUD OPERACE NA DATABAZI
        /// </summary>
        private void Save()
        {

           
            Console.WriteLine($"Save");
            OnSavePressed = true;
            m_dialogWindow.Close();
        }


        private void Storno()
        {
            Console.WriteLine("Storno");

            OnStornoPressed = true;
            m_dialogWindow.Close();
        }
    }
}
