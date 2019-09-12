using System.Collections.Generic;
using System.Linq;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using System.Data;
using System;

namespace WpfUniverse.Entities
{
    public class PlanetDao : EntityIdentityKeyDaoBase<Planeta, UniverseDataModel, int, Planeta>, IPlanetDao
    {

        public PlanetDao(string dbAlias)
            : base(dbAlias)
        {
        }

        public PlanetDao(string dbAlias, IsolationLevel isolationLevel)
            : base(dbAlias, isolationLevel)
        {
        }

        public IEnumerable<Planeta> LoadPlanetsBasedOnGalaxyId(int id)
        {
            using (UniverseDataModel model = CreateDbContext())
            {
                var query = (from pl in model.Planeta                                                               // Vybirame z tabulky Planeta

                                 // VYTRIDIME SI KOLEKCI VLASTNOSTI
                             join vp in model.VlastnostiPlanet on pl.Id equals vp.PlanetaId into vpl                // Z tabulky VlastnostiPlanet vyber ty polozky, kde se Id planety shoduje s PlanetaId, uloz si je prozatim do kolekce vpl.   Kolekce obsahuje jen VlastnostiPlanet kde pl.Id == vp.PlanetaId                
                             from vp in vpl.DefaultIfEmpty()                                                        // Z VlastnostiPlanet ve vpl, (pokud je prazdna  (nejsou zadne vlastnosti) vyber defaultni),
                             join v in model.Vlastnost on vp.VlastnostId equals v.Id into vl                        // vyber z tabulky Vlastnosti ty, ktere maji Id shodne s VlastnostId polozek ve vpl a uloz si je prozatim do kolekce vl.   Kolekce obsahuje jen Vlastnosti kde v.Id == vp.VlastnostId
                             from v in vl.DefaultIfEmpty()                                                          // Pokud neni kolekce vl prazdna, vyber z ni vlastnost,


                             where pl.GalaxieId == id                                                               // Vyber planety ve kterych GalaxieId odpovida parametru metody.
                             select new { Planet = pl, Property = v });                                             // Vytvor kolekci objektu, ktere budou mit vlastnosti Planet a Property.



                // MAME KOLEKCI VE KTERE SE PLANETY OPAKUJOU PODLE TOHO KOLIK MAJI VLASTNOSTI MUSIME JE ROZTRIDIT DO KOLEKCI PODLE KLICE ID PLANETY.

                var groupedByPlanet = query.ToList().GroupBy(x => x.Planet.Id);


                List<Planeta> result = new List<Planeta>();                                                                // Nove uloziste

                foreach (var dato in groupedByPlanet)                                                                       // Pro kazdou skupinu ve skupinach serazenych podle planety,  (v kazde skupine se opakuje planeta tolikrat, kolikrat ma vlastnost.)
                {
                    var planet = dato.First().Planet;                                                                      // Planeta bude prvni polozka
                    planet.Properties = dato.Where(d => d.Property != null).Select(x => x.Property).ToList();              // Do vlastnosti teto nasi planety vlozime cely hotovy List. Vezmeme ho ze skupiny dato => Linq dotaz nam prosel vsechny polozky ve skupine a vratil seznam vsech objektu Property.  (Vybirame z anonymniho typu ktery ma vlastnosti Planet a Property).               

                    result.Add(planet);                                                                                    // Vloz do resultu.
                }


                return result;                                                                                              // Vrat result.
            }
        }

    }

    public interface IPlanetDao : ISimpleDao<Planeta, int>
    {
        IEnumerable<Planeta> LoadPlanetsBasedOnGalaxyId(int id);
    }
}
