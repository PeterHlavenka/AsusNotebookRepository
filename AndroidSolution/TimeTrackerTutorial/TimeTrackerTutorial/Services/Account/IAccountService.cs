using System.Threading.Tasks;

namespace TimeTrackerTutorial.Services.Account
{
    public interface IAccountService
    {
        Task<bool> LoginAsync(string username, string password);
    }
}