using System.Windows.Input;
using FluentValidation;
using Mediaresearch.Framework.Gui;
using WpfUniverse.Common.Datacontracts;
using WpfUniverse.Gui.Commands;
using WpfUniverse.Gui.Errors;

namespace WpfUniverse.Gui.ViewModels
{
    public class GalaxyDialogViewModel : ErrorBase              // ErrorBase dedi od ViewModelBase, timpadem mame INotify, IDataErrorInfo
    {
        private GalaxyDataContract m_galaxy;

        public GalaxyDialogViewModel()
        {
            SaveGalaxyCommand = new RelayCommand(Save, CanSaveCommand);

            StornoGalaxyCommand = new CommandBase(Storno);

            Validator = new CustomValidator();
        }

        public void Initialize(GalaxyDataContract galaxy)
        {
            m_galaxy = galaxy;
            
        }

        private bool CanSaveCommand( object parameter)
        {
            return IsValid;
        }


        public int Id => m_galaxy.Id;

        public string Jmeno
        {
            get { return m_galaxy.Jmeno; }
            set
            {
                m_galaxy.Jmeno = value;
                NotifyOfPropertyChange(nameof(Jmeno));
               
            }  
        }

        public long PolohaX
        {
            get { return m_galaxy.PolohaX; }
            set
            {
                m_galaxy.PolohaX = value;
                NotifyOfPropertyChange(nameof(PolohaX));
              
            }
        }

        public long PolohaY
        {
            get => m_galaxy.PolohaY;
            set => m_galaxy.PolohaY = value;
        }

        public long PolohaZ
        {
            get => m_galaxy.PolohaZ;
            set => m_galaxy.PolohaZ = value;
        }

        public string ErrorOverride => Error;

        public ICommand SaveGalaxyCommand { get; set; }
        public ICommand StornoGalaxyCommand { get; set; }
        public bool OnSavePressed { get; private set; }


        private void Save(object parameter)
        {
           
            TryClose(true);
        }

        private void Storno()
        {
            TryClose(false);
        }

        private class CustomValidator : AbstractValidator<GalaxyDialogViewModel>
        {
            public CustomValidator()
            {
                RuleFor(d => d.Jmeno).NotEmpty().WithMessage("Jmeno musi byt vyplneno."); 
                RuleFor(d => d.Jmeno).Length(3,20).WithMessage("Delka min 3 znaky. ");
                RuleFor(x => x.PolohaX).LessThan(100).WithMessage("PolohaX musi byt mensi nez 100") ;
                RuleFor(x => x.PolohaY).GreaterThan(300).WithMessage("PolohaY musi byt vetsi nez 300");
            }
        }

       

    }
}