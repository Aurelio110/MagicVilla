using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;

namespace MagicVilla_Web.Services
{
    public class VillaServices : BaseServices, IVillaServices
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;
        public VillaServices(IHttpClientFactory clientFactory, IConfiguration configuration)
            : base(clientFactory)
        {
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
            _clientFactory = clientFactory;
        }
        public async Task<T> CreateAsync<T>(Villa villa)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = villa,
                Url = villaUrl + "/api/villaAPI"
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + "/api/villaAPI/" + id
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/villaAPI/"

            });
        }
        public async Task<T> GetAsync<T>(int id)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/villaAPI/" + id
            });
        }

        public async Task<T> GetAsync<T>(string name)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/villaAPI/GetVillaByName?name=" + name
            });
        }

        public async Task<T> UpdateAsync<T>(Villa villa)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = villa,
                Url = villaUrl + "/api/villaAPI/" + villa.Id
            });
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var loginRequest = new { User = username, Password = password };
            var apiRequest = new APIRequest
            {
                ApiType = SD.ApiType.POST,
                Url = villaUrl + "/api/user/login",
                Data = loginRequest
            };

            var response = await SendAsync<APIResponse>(apiRequest);
            if (response != null && response.IsSuccess && response.Result != null)
            {
                dynamic result = JsonConvert.DeserializeObject<dynamic>(response.Result.ToString());
                return true;
            }
            return false;
        }
    }
}
