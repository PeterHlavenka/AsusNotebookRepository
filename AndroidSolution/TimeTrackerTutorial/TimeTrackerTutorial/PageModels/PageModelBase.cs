using System.Threading.Tasks;

namespace TimeTrackerTutorial.PageModels
{
    public class PageModelBase : ExtendedBindableObject
    {
        private string m_title;
        private bool m_isLoading;

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