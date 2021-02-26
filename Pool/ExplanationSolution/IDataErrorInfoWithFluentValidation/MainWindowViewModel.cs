using System;
using System.ComponentModel;
using System.Windows.Input;

namespace IDataErrorInfoWithFluentValidation
{
    // VALIDACE ALA C SHARP CORNER
    // PROPERTU ERROR VUBEC NEPOTREBUJEME
    // STRING COLUMN NAME KONTROLUJE VSECHNY PROPERTY NA KTERE JE BINDOVANO
    public class MainWindowViewModel : IDataErrorInfo
    {
        //property to bind to textbox

        public string ValidateInputText { get; set; }

        //ICommand to bind to button

        public ICommand ValidateInputCommand
        {
            get => new RelayCommand();
            set { }
        }

        public int Age { get; set; } = 20;

        #region IDataErrorInfo

        //In this region we are implementing the properties defined in //the IDataErrorInfo interface in System.ComponentModel

        public string this[string columnName] 
        {
            get
            {
                if ("ValidateInputText" == columnName)
                {
                    if (string.IsNullOrEmpty(ValidateInputText))
                    {
                        return "Please enter a Name";
                    }
                }

                else if ("Age" == columnName)
                {
                    if (Age <= 0)
                    {
                        return "age should be greater than 0";
                    }
                }

                return "";
            }
        }

        public string Error => throw new NotImplementedException();
    }

    #endregion

    internal class RelayCommand : ICommand
    {
        public void Execute(object parameter)
        {
            // NIC NEDELA, NA VALIDACI STACI , KDYZ TEXTBOX STRATI FOCUS
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}