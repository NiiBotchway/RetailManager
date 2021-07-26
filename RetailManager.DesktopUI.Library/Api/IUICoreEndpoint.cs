using System.Net.Http;
using System.Threading.Tasks;
using RetailManager.DesktopUI.Library.Models;

namespace RetailManager.DesktopUI.Library.Api
{
    public interface IUICoreEndpoint
    {
        HttpClient ApiClient { get; }
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task<LoggedInUserModel> GetLoggedInUserInfo();
    }
}