using RetailManager.DesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.Library.Api
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private readonly IUICoreEndpoint _coreEndpoint;

        public SaleEndpoint(IUICoreEndpoint coreEndpoint, ILoggedInUserModel loggedInUser)
        {
            this._coreEndpoint = coreEndpoint;
        }

        public async Task<Sale> PostSale(Sale sale)
        {
            using (HttpResponseMessage response = await _coreEndpoint.ApiClient.PostAsJsonAsync("api/Sale", sale))
            {
                if (response.IsSuccessStatusCode)
                {
                    //Log success status
                }
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
