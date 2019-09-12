using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUniverse.Entities
{
    public class PlanetDao : EntityDaoBase<Planeta>
    {


        //konstruktor ziska connectionString z konstruktoru rodicovske tridy
        public PlanetDao(string connectionString) : base(connectionString)
        {

        }

        /// <summary>
        /// Vola metodu rodicovske tridy LoadEntity  ktera nacte vsechny planety vsech galaxii
        /// </summary>
        /// <returns> Vraci List ktery obsahuje vsechny planety vsech galaxii </returns>
        public List<Planeta> LoadAllPlanets()
        {
            //sql dotaz
            string query = "Select Id, Jmeno, Velikost, GalaxieId, Identifikator from dbo.Planeta";

            //vracime vysledek metody LoadEntity v rodicovske tride . Jako parametr metode predame 
            //instanci  SqlReaderu do ktere nacteme data data entity / planety
            return LoadEntity(query, (SqlDataReader reader) => new Planeta()
            {
                Id = reader.GetInt32(0),
                Jmeno = reader.GetString(1),
                Velikost = reader.GetInt32(2),
                GalaxieId = reader.GetInt32(3),
                Identifikator = reader.GetGuid(4)
            });
        }

        /// <summary>
        /// Slouzi k pridavani planet do listu na zaklade id galaxie.
        /// Vola metodu rodicovske tridy LoadEntity  na zaklade id vybere planety jen z teto galaxie. 
        /// Nenacitame vsechny planety vsech galaxii , abychom se nazasekli kdyz jich bude napr. 100 000 000.
        /// 
        /// Dotaz pomoci join vybere udaje z vice tabulek.
        /// Zneni dotazu:
        /// Vyber Id, Jmeno, Velikost, GalaxieId, Identifikator , VlastnostId a NazevVlastnosti
        /// Z databaze dbo.Planeta kde nazev planety je p.
        /// Join neboli Inner Join nezobrazuje vysledky dotazu kde nektera strana neexistuje. Napr planeta ktera nema vlastnosti by ve vysledku nebyla.
        /// Left join uzna vysledek dotazu, pokud existuje leva cast vazby (zde planeta) a prava (ta pripojovana, zde vlastnost) neexistuje.
        /// Do hodnot sloupcu z pripojovane casti se vlozi Null.
        /// Prvnim joinem vybereme z vazaci tabulky VlastnostiPlanet kde nazev vlastnostiPlanet je vp, ty vlastnostiPlanet, kde p.Id je shodne s vp.PlanetaId.
        /// Uz mame promennou vp.
        /// Druhym joinem vybereme z tabulky dbo.Vlastnost kde nazev vlastnosti je v, vlastnosti, kde vp.VlastnostId je v.Id.
        /// Uz mame definovany vyber
        /// Z neho pomoci Where vybereme jak jsme zvykli jen ty vysledky, kde p.GalaxieId  se rovna nasemu SqlParametru ktery jsme dostali jako parametr metody. 
        /// </summary>
        /// <param name="id"> Id galaxie podle ktereho se urcuje ktere planety se budou vybirat </param>
        /// <returns></returns>
        public List<Planeta> LoadPlanetsBasedOnGalaxyId(int id)   //budeme muset pretizit i metodu LoadEntity v EntityDaoBase o int id
        {
            string query = @"Select p.Id, p.Jmeno, p.Velikost, p.GalaxieId, p.Identifikator, v.id as vlastnostId, v.Nazev as vlastnostNazev
                             from dbo.Planeta p
                                left join dbo.VlastnostiPlanet vp on p.Id = vp.PlanetaId
                                left join dbo.Vlastnost v on vp.VlastnostId = v.id
                             where p.GalaxieId = @id";  //vybere planety z kliknute galaxie

            SqlParameter[] paramters = new[] { new SqlParameter("@id", id) };   //tady rikame ze v query bude "@id"  odkazovat na int id


            Func<SqlDataReader, Planeta> func = (SqlDataReader reader) =>            // Delegat chce jako vstup reader a vrati planetu
            {
                Planeta planet = new Planeta()
                {

                    Id = reader.GetInt32(0),
                    Jmeno = reader.GetString(1),
                    Velikost = reader.GetInt32(2),
                    GalaxieId = reader.GetInt32(3),
                    Identifikator = reader.GetGuid(4)
                };
           
                if (reader.IsDBNull(5))                                              // Pokud planeta nema zadne vlastnosti
                {
                    planet.Properties = new List<Vlastnost>();                       // Dame mu novou prazdnou kolekci.
                }
                else
                {
                    Vlastnost vlastnost = new Vlastnost()                            // Jinak z readeru nacteme Vlastnost
                    {
                        Id = reader.GetInt32(5),
                        Nazev = reader.GetString(6)
                    };

                    planet.Properties = new List<Vlastnost>() { vlastnost };          // Vytvorime kolekci a do ni vlozime vlastnost
                }

                return planet;                                                        // Vrati se nam planeta ktera bude mit kolekci s jednou vlastnosti.  Planety ktere maji vic vlastnosti se budou opakovat.
           };



            List<Planeta> result = new List<Planeta>();                                // Misto na ukladani

            var planets = LoadEntity(query, func, paramters);                          // Zavolame rodicovskou metodu ta nam vrati List

            var groupedByPlanets = planets.GroupBy(x => x.Id).ToList();                // Tento List roztridime do skupin , kde klicem skupiny bude Id planety.


            foreach(var groupByPlanet in groupedByPlanets)                             // Pro kazdou skupinu ve skupinach groupedByPlanets
            {
                var planet = groupByPlanet.First();                                    // Do var planet uloz prvni skupinu. 

                List<Vlastnost> properties = new List<Vlastnost>();                    // Vytvorime prazdny List vlastnosti
                
                foreach(var dato in groupByPlanet)                                     // Pro kazdou polozku (planetu) ve skupine
                {
                    properties.AddRange(dato.Properties);                              // Vloz do nove vytvoreneho Listu vlastnosti vsech planet ve skupine => Vsechny planety ve skupine jsou jedna a ta sama protoze skupiny jsou tvoreny podle klice Id
                }

                planet.Properties = properties;                                        // Planete priradime do Properties nasi sesbiranou kolekci vlastnosti.

                result.Add(planet);                                                    // A vlozime ji do seznamu planet.
            }

            return result;                                                             // Ktery vratime.
        }


        /// <summary>
        /// Slouzi ke zmenam vlastnosti vybrane planety. Menit muzeme jen Jmeno a Velikost
        /// Pro vstup do databaze pouzijeme metodu rodicovske tridy  ExcuteUpdate
        /// </summary>
        /// <param name="planeta"> Updatovana planeta </param>
        public void UpdatePlanet(Planeta planeta)
        {
            string query = $"UPDATE dbo.Planeta SET Jmeno = @Jmeno, Velikost = @Velikost WHERE Id = @Id";

            

            SqlParameter[] paramters = new[]
            {
                new SqlParameter("@Id", planeta.Id),
                new SqlParameter("@Jmeno", planeta.Jmeno),
                new SqlParameter("@Velikost", planeta.Velikost),
                new SqlParameter("@GalaxieId", planeta.GalaxieId),
                new SqlParameter("@Identifikator", planeta.Identifikator)
                
            };

            //UpdateVlastnostiPlanets(planeta);  musi byt na vlastnostiPlanetDao

            ExcuteUpdate(query, paramters);
        }


        /// <summary>
        /// Slouzi ke vkladani Planety do databaze . Dialogove okno musi byt prazdne , jen id si nechame vytahnout z databaze, protoze ma nastavenou automatickou
        /// inkrementaci. Dostaneme ho v dotazu pomoci select SCOPE_IDENTITY();. 
        /// 
        /// Pro vstup do databaze pouzijeme metodu rodicovske tridy ExcuteInsert
        /// </summary>
        /// <param name="query"> Dotaz v podstate rika : Vloz do dbo.Planeta na pozice (Jmeno, Velikost, GalaxieId, Identifikator) hodnoty (@Jmeno, @Velikost, @GalaxieId, @Identifikator) a jako Id pouzij udaj z databaze o poslednim Id 
        /// <param name="paramters"> Sql parametry urcuji na kterou skutecnou promennou ukazuje ta ktera znacka  </param>
        /// <param name="planeta"> Upravovana planeta </param>
        public int InsertPlanet(Planeta planeta)
        {
            string query = "INSERT INTO dbo.Planeta (Jmeno, Velikost, GalaxieId, Identifikator) VALUES (@Jmeno, @Velikost, @GalaxieId, @Identifikator); select SCOPE_IDENTITY();";

            SqlParameter[] paramters = new[]
            {
                new SqlParameter("@Jmeno", planeta.Jmeno),
                new SqlParameter("@Velikost", planeta.Velikost),
                new SqlParameter("@GalaxieId", planeta.GalaxieId),
                new SqlParameter("@Identifikator", planeta.Identifikator)
               
            };

            

            planeta.Id = ExcuteInsert(query, paramters);
            return planeta.Id;
        }


        public void RemovePlanet(int id)
        {
            string query = "DELETE dbo.Planeta WHERE id = @Id";

            SqlParameter[] paramters = new[]
            {
                  new SqlParameter("@Id", id)
            };

            ExcuteUpdate(query, paramters);

        }



    }
}
