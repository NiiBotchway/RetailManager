using System.Threading.Tasks;
using RetailManager.DesktopUI.Library.Models;

namespace RetailManager.DesktopUI.Library.Api
{
    public interface IApiHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        //Task<LoggedInUserModel> GetLoggedInUserInfo(string token);
        Task<LoggedInUserModel> GetLoggedInUserInfo();
    }
}