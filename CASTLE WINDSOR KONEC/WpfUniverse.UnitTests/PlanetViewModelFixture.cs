using System;
using System.Collections.Generic;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using Moq;
using NUnit.Framework;
using WpfUniverse.Core;
using WpfUniverse.Entities;
using WpfUniverse.ViewModels;
using System.Collections.ObjectModel;

namespace WpfUniverse.UnitTests
{
    [TestFixture]
    internal class PlanetViewModelFixture
    {

        [Test]
        public void AddingPlanetTest()
        {
            
            Mock<IDaoSource> daoSourceMock = new Mock<IDaoSource>();
            Mock<IPlanetDao> planetDaoMock = new Mock<IPlanetDao>();
            Mock<IVlastnostDao> vlastnostDaoMock = new Mock<IVlastnostDao>();
            Mock<IVlastnostiPlanetDao> vlastnostiPlanetDaoMock = new Mock<IVlastnostiPlanetDao>();
            Mock<IGalaxySelector> galaxySelectorMock = new Mock<IGalaxySelector>();
            Mock<IPropertiesManager> propertiesManagerMock = new Mock<IPropertiesManager>();


            var viewModel = new PlanetsViewModel(daoSourceMock.Object, planetDaoMock.Object, vlastnostDaoMock.Object, vlastnostiPlanetDaoMock.Object,
                galaxySelectorMock.Object,propertiesManagerMock.Object);


            var listOfPlanets = new ObservableCollection<PlanetDataContract>();

            var novaPlaneta = new PlanetDataContract()
            {
                Id = 1,
                Jmeno = "aaa",
                GalaxieId = 7,
                Identifikator = new Guid(),
                Properties = new List<Vlastnost>(),
                Velikost = 5
            };


            viewModel.ListOfPlanetsFromSelectedGalaxies = listOfPlanets;

            listOfPlanets.Add(novaPlaneta);

            Assert.IsNotEmpty(listOfPlanets);
            Assert.AreEqual(listOfPlanets[0].GalaxieId  ,   7);
        }
    }
}
