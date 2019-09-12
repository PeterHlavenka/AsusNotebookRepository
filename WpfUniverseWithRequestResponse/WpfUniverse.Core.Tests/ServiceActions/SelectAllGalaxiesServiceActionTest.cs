using System.Collections.Generic;
using System.Linq;
using Mediaresearch.Framework.Mapping;
using Moq;
using NUnit.Framework;
using WpfUniverse.Common.Datacontracts;
using WpfUniverse.Common.Requests;
using WpfUniverse.Core.ServiceActions;
using WpfUniverse.Entities;

namespace WpfUniverse.Core.Tests.ServiceActions
{
    [TestFixture]
    public class SelectAllGalaxiesServiceActionTest
    {
        private Mock<IGalaxyDao> m_galaxyDao;
        private Mock<IMapper<Galaxie, GalaxyDataContract>> m_mapper;

        private SelectAllGalaxiesServiceAction SUT;

        [SetUp]
        public void TestSetUp()
        {
            m_galaxyDao = new Mock<IGalaxyDao>();
            m_mapper = new Mock<IMapper<Galaxie, GalaxyDataContract>>();

            SUT = new SelectAllGalaxiesServiceAction(m_galaxyDao.Object, m_mapper.Object);
        }

        [Test]
        public void Execute()
        {
            const int galaxyId = 5;

            var galaxie = new Galaxie{Id = galaxyId};
            m_galaxyDao.Setup(d => d.SelectAll()).Returns(new List<Galaxie> {galaxie});
            m_mapper.Setup(d => d.Map(galaxie)).Returns(new GalaxyDataContract(galaxyId, string.Empty, 0, 0, 0));

            var response = SUT.Execute(new SelectAllGalaxiesRequest());

            m_galaxyDao.VerifyAll();
            m_mapper.VerifyAll();

            Assert.IsTrue(response.Galaxies.Any(d=>d.Id == galaxyId));
        }
    }
}