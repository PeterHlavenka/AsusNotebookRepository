using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace INotifyDataErrorInfo
{
    public class MainViewModel : INotifyPropertyChanged, System.ComponentModel.INotifyDataErrorInfo
    {
        private string m_userName;
        private readonly Dictionary<string, List<string>> m_errorsByPropertyName = new Dictionary<string, List<string>>();

        public MainViewModel()
        {
            UserName = null;
        }

        public string UserName
        {
            get => m_userName;
            set
            {
                m_userName = value;
                 // ValidateProperty(nameof(UserName));
                OnPropertyChanged();
            }
        }

        public bool HasErrors => m_errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return m_errorsByPropertyName.ContainsKey(propertyName) ?
                m_errorsByPropertyName[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ValidateProperty(string propertyName)
        {
            ClearErrors(propertyName);
            
            if (string.IsNullOrWhiteSpace(UserName))
                AddError(propertyName, "Username cannot be empty.");
            
            if (string.Equals(UserName, "Admin", StringComparison.OrdinalIgnoreCase))
                AddError(propertyName, "Admin is not valid username.");
            
            if (UserName == null || UserName?.Length <= 5)
                AddError(propertyName, "Username must be at least 6 characters long.");
        }

        private void AddError(string propertyName, string error)
        {
            if (!m_errorsByPropertyName.ContainsKey(propertyName))
                m_errorsByPropertyName[propertyName] = new List<string>();

            if (!m_errorsByPropertyName[propertyName].Contains(error))
            {
                m_errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (m_errorsByPropertyName.ContainsKey(propertyName))
            {
                m_errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        #region INotifyRegion

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public void ButtonClick()
        {
            ValidateProperty(nameof(UserName));
        }
    }
}
