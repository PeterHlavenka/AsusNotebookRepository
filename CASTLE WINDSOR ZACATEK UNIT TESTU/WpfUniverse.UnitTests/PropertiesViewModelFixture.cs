using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moq;
using NUnit.Framework;
using WpfUniverse.Core;
using WpfUniverse.Entities;
using WpfUniverse.ViewModels;

namespace WpfUniverse.UnitTests
{
    [TestFixture]
    public class PropertiesViewModelFixture
    {
        [Test]
        public void PropertiesTest()
        {
            Mock<IPlanetSelector> planetSelectorMock = new Mock<IPlanetSelector>();
            Mock<IPropertiesManager> propertiesManagerMock = new Mock<IPropertiesManager>();
            Mock<IVlastnostiPlanetDao> planetPropertiesDaoMock = new Mock<IVlastnostiPlanetDao>();

            ObservableCollection<VlastnostDataContract> observableCollection = new ObservableCollection<VlastnostDataContract>()
            {
                new VlastnostDataContract()
                {
                    Id = 1,
                    IsChecked = false,
                    Nazev = "Test1"
                },
                new VlastnostDataContract()
                {
                    Id = 2,
                    IsChecked = false,
                    Nazev = "Test2"
                },
                new VlastnostDataContract()
                {
                    Id = 3,
                    IsChecked = false,
                    Nazev = "Test"
                },
            };

            //var observableCollection = new ObservableCollection<VlastnostDataContract>(properties);

            propertiesManagerMock.Setup(d => d.ListOfAllPossibleVlastnosts).Returns(() => observableCollection);

            PropertiesViewModel viewModel = new PropertiesViewModel(planetSelectorMock.Object, propertiesManagerMock.Object, planetPropertiesDaoMock.Object);
            viewModel.RegisterToEvent();

            PlanetDataContract selectedPlanet = new PlanetDataContract()
            {
                Id = 1,
                GalaxieId = 1,
                Identifikator = Guid.NewGuid(),
                Jmeno = "brebrere",
                Properties = new List<Vlastnost>(),
                Velikost = 10
            };

            viewModel.SelectedPlanet = selectedPlanet;


            observableCollection[0].IsChecked = true;

            Assert.IsNotEmpty(selectedPlanet.Properties);
            Assert.IsTrue(selectedPlanet.Properties.Count == 1);
            Assert.AreEqual(selectedPlanet.Properties[0].Id, observableCollection[0].Id);
            Assert.AreEqual(selectedPlanet.Properties[0].Nazev, observableCollection[0].Nazev);
        }
    }
}
