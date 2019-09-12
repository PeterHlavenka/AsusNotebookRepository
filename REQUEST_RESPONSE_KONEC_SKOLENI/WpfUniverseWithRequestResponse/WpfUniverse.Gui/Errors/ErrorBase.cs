using System;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using FluentValidation;

namespace WpfUniverse.Gui.Errors
{
    public abstract class ErrorBase : Screen, IDataErrorInfo
    {
        public bool IsValid => string.IsNullOrEmpty(Error);
        protected IValidator Validator { get; set; }

        //protected sealed override void OnPropertyChangeInternal(string property)
        //{
        //    OnPropertyChangedEx(nameof(Error));
        //    OnPropertyChangedEx(nameof(IsValid));
        //}

        public override void NotifyOfPropertyChange(string propertyName = "")
        {
            base.NotifyOfPropertyChange(propertyName);

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Error)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsValid)));
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

        public string this[string columnName]
        {
            get
            {
                if (Validator == null)
                    return string.Empty;

                var validation = Validator.Validate(this).Errors.FirstOrDefault(e => e.PropertyName == columnName);

                return validation == null ? string.Empty : validation.ErrorMessage;
            }
        }
    }
}