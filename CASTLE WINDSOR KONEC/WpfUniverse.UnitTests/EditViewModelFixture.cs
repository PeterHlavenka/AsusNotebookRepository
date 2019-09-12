using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using BLToolkit.Data;
using Mediaresearch.Framework.DataAccess.BLToolkit.Transactions;
using Moq;
using NUnit.Framework;
using WpfUniverse.Core;
using WpfUniverse.Entities;
using WpfUniverse.ViewModels;

namespace WpfUniverse.UnitTests
{
    [TestFixture]
    public class EditViewModelFixture
    {
        [Test]
        public void EditPropertiesTest()
        {


            Mock<IVlastnostDao> vlastnostDaoMock = new Mock<IVlastnostDao>();
            Mock<IVlastnostiPlanetDao> vlastnostiPlanetMock = new Mock<IVlastnostiPlanetDao>();
            Mock<ITransactionManager> transactionManagerMock = new Mock<ITransactionManager>();
            Mock<IEventRegistrator> eventRegistratorMock = new Mock<IEventRegistrator>();
            Mock<IPlanetSelector> planetSelectorMock = new Mock<IPlanetSelector>();
            Mock<IPropertiesManager> propertiesManagerMock = new Mock<IPropertiesManager>();


            var listOfVlastnosts = new ObservableCollection<VlastnostDataContract>()          // nahradni kolekce bude zamenena za kolekci v propertiesManageru
            {
                new VlastnostDataContract()
                {
                    Id = 1,
                    IsChecked = false,
                    Nazev = "aaa"
                },
                new VlastnostDataContract()
                {
                    Id = 2,
                    IsChecked = true,
                    Nazev = "bbb"
                },
                new VlastnostDataContract()
                {
                    Id = 3,
                    IsChecked = true,
                    Nazev = "ccc"
                },
                new VlastnostDataContract()
                {
                Id = 1,
                IsChecked = false,
                Nazev = "ddd"
            },
            new VlastnostDataContract()
            {
                Id = 2,
                IsChecked = true,
                Nazev = "eee"
            },
            new VlastnostDataContract()
            {
                Id = 3,
                IsChecked = true,
                Nazev = "fff"
            }
            };


            propertiesManagerMock.Setup(d => d.ListOfAllPossibleVlastnosts).Returns(() => listOfVlastnosts);    // vymen kolekce
            transactionManagerMock.Setup(d => d.CallInsideTransaction(It.IsAny<Action>(), It.IsAny<DbManager>(),
                    It.IsAny<IsolationLevel>())).Callback((Action action, DbManager db, IsolationLevel isl) => action.Invoke());


            // Vytvoreni viewModelu s falesnymi zavislostmi
            EditPropertyViewModel viewModel = new EditPropertyViewModel(vlastnostDaoMock.Object, vlastnostiPlanetMock.Object, transactionManagerMock.Object,
                eventRegistratorMock.Object, planetSelectorMock.Object, propertiesManagerMock.Object);

            // Zaregistrovani udalosti zmeny planety
            //eventRegistratorMock.Object.RegisterToEvent();


            // Potrebujeme planetu ktere budeme menit kolekci vlastnosti

            var selectedPlanet = new PlanetDataContract()
            {
                Id = 1,
                GalaxieId = 1,
                Identifikator = new Guid(),
                Jmeno = "ajk",
                Properties = new List<Vlastnost>()
                {
                    listOfVlastnosts[1].ConvertToDbEntity(),
                    listOfVlastnosts[2].ConvertToDbEntity()
                },
                Velikost = 7
            };

            // Kdyz se zmeni v listOfVlastnost v datacontractu isSelected , musi se to projevit na selectedPlanet
            // Odeberu jednu checknutou
            viewModel.SelectedPlanet = selectedPlanet;

            Console.WriteLine($"Pocet polozek v listu pred odebranim vlastnosti : {viewModel.ListOfAllPossibleVlastnosts.Count}");
            Console.WriteLine($"Pocet polozek v selectedPlanet.Properties pred odebranim: {viewModel.SelectedPlanet.Properties.Count}"); ;

            int count = viewModel.ListOfAllPossibleVlastnosts.Count;

            viewModel.DoRemoveSelected(2);
            


            Console.WriteLine();
            Console.WriteLine($"Pocet polozek v listu po odebrani vlastnosti : {viewModel.ListOfAllPossibleVlastnosts.Count}");
            Console.WriteLine($"Pocet polozek v selectedPlanet.Properties po odebrani: {viewModel.SelectedPlanet.Properties.Count}");

            Assert.IsTrue(viewModel.ListOfAllPossibleVlastnosts.Count == count - 1);
            Assert.IsTrue(viewModel.SelectedPlanet.Properties[0].Nazev == "ccc");


            viewModel.DoAllChanges(true);

            Console.WriteLine();
            Console.WriteLine($"viewModel.SelectedPlanet.Properties.Count: {viewModel.SelectedPlanet.Properties.Count} == viewModel.ListOfAllPossibleVlastnosts.Count: {viewModel.ListOfAllPossibleVlastnosts.Count}");
            Assert.IsTrue(viewModel.SelectedPlanet.Properties.Count == viewModel.ListOfAllPossibleVlastnosts.Count);
        }
    }
}
