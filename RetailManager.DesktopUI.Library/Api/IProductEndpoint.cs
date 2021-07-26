using RetailManager.DesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.Library.Api
{
    public interface IProductEndpoint
    {
        Task<List<Product>> GetProducts();
    }
}