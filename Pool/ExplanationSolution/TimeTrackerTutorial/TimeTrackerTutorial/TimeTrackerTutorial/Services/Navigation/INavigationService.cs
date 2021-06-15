using System.Threading.Tasks;

namespace TimeTrackerTutorial.Services.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        /// Navigation method to push onto the Navigation Stack
        /// </summary>
        /// <param name="navigationData"></param>
        /// <param name="setRoot"></param>
        /// <typeparam name="TPageModelBase"></typeparam>
        /// <returns></returns>
        Task NavigateToAsync<TPageModelBase>(object navigationData = null, bool setRoot = false);
        
        /// <summary>
        /// Navigation method to pop off of the Navigation Stack
        /// </summary>
        /// <returns></returns>
        Task GoBackAsync();
    }
}