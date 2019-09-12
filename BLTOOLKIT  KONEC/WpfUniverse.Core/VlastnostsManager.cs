using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUniverse.Core;

namespace WpfUniverse.Entities
{
   public class VlastnostsManager
    {
        private IVlastnostDao m_vlastnostDao;
        private IVlastnostiPlanetDao m_planetDao;
        private ObservableCollection<VlastnostDataContract> m_listOfAllPossibleVlastnosts;

        //CONSTRUCTORS
        public VlastnostsManager(IDaoSource daoSource)
        {
            //m_planetDao = daoSource.GetDaoByEntityType<IVlastnostiPlanetDao, VlastnostiPlanet, int>();
            m_vlastnostDao = daoSource.GetDaoByEntityType<IVlastnostDao, Vlastnost, int>();

            ListOfAllPossibleVlastnosts = new ObservableCollection<VlastnostDataContract>(m_vlastnostDao.SelectAll().Select(x => VlastnostDataContract.Create(x)).ToList());


        }

        //PROPERTIES
        public  ObservableCollection<VlastnostDataContract> ListOfAllPossibleVlastnosts
        {
            get { return m_listOfAllPossibleVlastnosts; }
            set { m_listOfAllPossibleVlastnosts = value;   }
        }
        
    }
}
