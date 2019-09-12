using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUniverse.Entities
{
    public class Planeta 
    {

       


        public int Id { get; set; }

        public string Jmeno { get; set; }

        public int Velikost { get; set; }

        public int GalaxieId { get; set; }

        public Guid Identifikator { get; set; }

        public List<Vlastnost> Properties { get; set; }       //planeta uz tuhle vlastnost ma rovnou pri nacteni z db pomoci join query.
    }
}
