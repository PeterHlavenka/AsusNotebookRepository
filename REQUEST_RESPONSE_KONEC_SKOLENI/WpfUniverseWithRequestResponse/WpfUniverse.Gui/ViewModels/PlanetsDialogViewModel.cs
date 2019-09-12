using System;
using System.Windows.Input;
using FluentValidation;
using Mediaresearch.Framework.Gui;
using Mediaresearch.Framework.Gui.Controls.Comparers;
using WpfUniverse.Common.Datacontracts;
using WpfUniverse.Gui.Commands;
using WpfUniverse.Gui.Errors;

namespace WpfUniverse.Gui.ViewModels
{
    public class PlanetsDialogViewModel : ErrorBase
    {
        public PlanetsDialogViewModel()
        {
           

            SavePlanetCommand = new RelayCommand(Save, () => IsValid);
            StornoPlanetCommand = new CommandBase(Storno);

            Validator = new CustomValidator(); 
        }


       

        public void Initialize(PlanetDataContract planet)
        {
            Planeta = planet ?? throw new ArgumentNullException();
        }

        

        public ICommand SavePlanetCommand { get; }
        public CommandBase StornoPlanetCommand { get; }


        public PlanetDataContract Planeta { get; set; }

        public int Id
        {
            get => Planeta.Id;
           
        }



        public string Jmeno
        {
            get => Planeta.Jmeno;
            set
            {
                Planeta.Jmeno = value;
                NotifyOfPropertyChange(nameof(Jmeno));
                NotifyOfPropertyChange(nameof(Error));
                NotifyOfPropertyChange(nameof(IsValid));
               
            } 
        }

        public int Velikost
        {
            get => Planeta.Velikost;
            set => Planeta.Velikost = value;
        }

        public int GalaxieId
        {
            get => Planeta.GalaxieId;
            set => Planeta.GalaxieId = value;
        }

        public Guid Identifikator
        {
            get => Planeta.Identifikator;
            set => Planeta.Identifikator = value;
        }

       


        private void Save()
        {
           
            TryClose(true);
        }

        private void Storno()
        {
           
            TryClose(false);
        }

        private class CustomValidator : AbstractValidator<PlanetsDialogViewModel>
        {
            public CustomValidator()
            {
                RuleFor(d => d.Jmeno).NotEmpty().WithLocalizedMessage(() => " Jmeno musi byt vyplneno  ");
            }
        }
    }
}