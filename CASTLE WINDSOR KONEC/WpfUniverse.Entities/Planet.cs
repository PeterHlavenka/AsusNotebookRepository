using System;
using System.Collections.Generic;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Mediaresearch.Framework.DataAccess.BLToolkit;
using Mediaresearch.Framework.DataAccess.BLToolkit.DaoFactory;

namespace WpfUniverse.Entities
{
    [DaoFactory(DaoType = typeof(PlanetDao))]
    [TableName("Planeta", Owner = "dbo")]
    public class Planet: LightDatabaseEntityIdentityIntKey<Planet>
    {
        [MapField("Jmeno")]
        public string Name { get; set; }

        public int Velikost { get; set; }

        public int GalaxieId { get; set; }

        public Guid Identifikator { get; set; }


        [MapIgnore]
        public List<Vlastnost> Properties { get; set; }      
    }
}
