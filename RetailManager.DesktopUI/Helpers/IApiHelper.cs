using RetailManager.DesktopUI.Models;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.Helpers
{
    public interface IApiHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}