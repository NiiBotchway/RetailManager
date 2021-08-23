using RetailManager.DesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.Library.Api
{
    public class UICoreEndpoint : IUICoreEndpoint
    {
        private HttpClient _apiClient;
        private readonly ILoggedInUserModel _loggedInUser;

        public UICoreEndpoint(ILoggedInUserModel loggedInUser)
        {
            InitializeClient();
            _loggedInUser = loggedInUser;
        }

        public HttpClient ApiClient
        {
            get { return _apiClient; }
        }


        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            using (HttpResponseMessage response = await _apiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    //var result = await response.Content.ReadAsStringAsync();
                    //string value = JsonConvert.ToString(result);
                    //string value = JsonConvert.DeserializeObject<string>(result);

                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();

                    _loggedInUser.Token = result.Access_Token;
                    _apiClient.DefaultRequestHeaders.Clear();
                    _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { _loggedInUser.Token }");

                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<LoggedInUserModel> GetLoggedInUserInfo()
        {
            using (HttpResponseMessage response = await _apiClient.GetAsync("api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
                    _loggedInUser.Id = result.Id;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.EmailAddress = result.EmailAddress;
                    _loggedInUser.CreatedDate = result.CreatedDate;

                    return result;
                }
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
