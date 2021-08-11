using RetailManager.DesktopUI.Library.Models;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.Library.Api
{
    public interface ISaleEndpoint
    {
        Task<Sale> PostSale(Sale sale);
    }
}