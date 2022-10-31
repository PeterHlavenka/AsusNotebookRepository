using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TimeTrackerTutorial.PageModels.Base
{
    public class PageModelBase : BindableObject
    {
        private string m_title;
        private bool m_isLoading;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string Title
        {
            get => m_title;
            set => SetProperty(ref m_title, value);
        }

        public bool IsLoading
        {
            get => m_isLoading;
            set => SetProperty(ref m_isLoading, value);
        }

        public virtual Task InitializeAsync(object navigationDate = null)
        {
            return Task.CompletedTask;
        }
    }
}