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
    public class ProductEndpoint : IProductEndpoint
    {
        private readonly IUICoreEndpoint _coreEndpoint;
        private readonly ILoggedInUserModel _loggedInUser;

        public ProductEndpoint(IUICoreEndpoint coreEndpoint, ILoggedInUserModel loggedInUser)
        {
            this._coreEndpoint = coreEndpoint;
            this._loggedInUser = loggedInUser;
        }


        public async Task<List<Product>> GetProducts()
        {

            _coreEndpoint.ApiClient.DefaultRequestHeaders.Clear();
            _coreEndpoint.ApiClient.DefaultRequestHeaders.Accept.Clear();
            _coreEndpoint.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _coreEndpoint.ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { _loggedInUser.Token }");

            using (HttpResponseMessage response = await _coreEndpoint.ApiClient.GetAsync("api/Product"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<Product>>();

                    return result;
                }
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
