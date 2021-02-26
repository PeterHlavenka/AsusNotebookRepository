using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentValidation;

namespace IDataErrorInfoAlaValidaceDocx
{
    public class MainWindowViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        private string m_text;
        private IValidator Validator { get; }

        public MainWindowViewModel()
        {
            Text = string.Empty;
            Validator = new MainWindowViewModelValidator();
            Validator.Validate(this);
        }

        public string Text
        {
            get => m_text;
            set
            {
                m_text = value;
                OnPropertyChanged();
            }
        }
        
        public string Error
        {
            get
            {
                if (Validator == null)
                    return string.Empty;

                var validation = Validator.Validate(this).Errors.Select(x => x.ErrorMessage).ToArray();
                if (!validation.Any())
                    return string.Empty;

                return string.Join(Environment.NewLine, validation);
            }
        }

        public string this[string propertyName]
        {
            get
            {
                if (Validator == null)
                    return string.Empty;

                var validation = Validator.Validate(this).Errors.FirstOrDefault(e => e.PropertyName == propertyName);
                OnPropertyChanged(nameof(Error));  // aby se zobrazil tool tip ve view
                return validation == null ? string.Empty : validation.ErrorMessage;
            }
        }

        #region INotifyRegion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
    
    public class MainWindowViewModelValidator: AbstractValidator<MainWindowViewModel>
    {
        public MainWindowViewModelValidator()
        {
            RuleFor(d => d.Text).Matches(d => d.Text.ToUpper()).WithMessage("jen velke pismena");
        }
    }
}