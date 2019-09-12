using System.Collections.Generic;
using Mediaresearch.Framework.Communication.Common;
using WpfUniverse.Common.Datacontracts;

namespace WpfUniverse.Common.Responses
{
    public class SelectAllGalaxiesResponse : Response
    {
        public SelectAllGalaxiesResponse(List<GalaxyDataContract> galaxies)
        {
            Galaxies = galaxies;
        }

        public List<GalaxyDataContract> Galaxies { get; private set; }
    }
}