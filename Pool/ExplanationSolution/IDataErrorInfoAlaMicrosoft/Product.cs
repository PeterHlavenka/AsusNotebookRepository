using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace IDataErrorInfoAlaMicrosoft
{
 public class Product : IDataErrorInfo
    {
        private int idValue;

        public int Id
        {
            get => idValue;
            set { if (IsIdValid(value) && idValue != value) idValue = value; }
        }

        private string nameValue;
        public string Name
        {
            get => nameValue;
            set { if (IsNameValid(value) && nameValue != value) nameValue = value; }
        }

        // Validates the Id property, updating the errors collection as needed.
        public bool IsIdValid(int value)
        {
            bool isValid = true;

            if (value < 5)
            {
                AddError("Id", ID_ERROR, false);
                isValid = false;
            }
            else RemoveError("Id", ID_ERROR);

            if (value > 10) AddError("Id", ID_WARNING, true);
            else RemoveError("Id", ID_WARNING);

            return isValid;
        }

        // Validates the Name property, updating the errors collection as needed.
        public bool IsNameValid(string value)
        {
            bool isValid = true;

            if (value.Contains(" "))
            {
                AddError("Name", NAME_ERROR, false);
                isValid = false;
            }
            else RemoveError("Name", NAME_ERROR);

            if (value.Length > 5) AddError("Name", NAME_WARNING, true);
            else RemoveError("Name", NAME_WARNING);

            return isValid;
        }

        private Dictionary<String, List<String>> errors =
            new Dictionary<string, List<string>>();
        private const string ID_ERROR = "Value cannot be less than 5.";
        private const string ID_WARNING = "Value should not be greater than 10.";
        private const string NAME_ERROR = "Value must not contain any spaces.";
        private const string NAME_WARNING = "Value should be 5 characters or less.";

        // Adds the specified error to the errors collection if it is not already 
        // present, inserting it in the first position if isWarning is false. 
        public void AddError(string propertyName, string error, bool isWarning)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            if (!errors[propertyName].Contains(error))
            {
                if (isWarning) errors[propertyName].Add(error);
                else errors[propertyName].Insert(0, error);
            }
        }

        // Removes the specified error from the errors collection if it is present. 
        public void RemoveError(string propertyName, string error)
        {
            if (errors.ContainsKey(propertyName) &&
                errors[propertyName].Contains(error))
            {
                errors[propertyName].Remove(error);
                if (errors[propertyName].Count == 0) errors.Remove(propertyName);
            }
        }

        #region IDataErrorInfo Members

        public string Error => throw new NotImplementedException();

        public string this[string propertyName] =>
            (!errors.ContainsKey(propertyName) ? null :
                String.Join(Environment.NewLine, errors[propertyName]));

        #endregion
    }
}