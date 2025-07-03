using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;

using Newtonsoft.Json;
using System.Linq;
using System.Text.Json;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseServices : IBaseService
    {

        public IHttpClientFactory httpClient { get; set; }
        public APIResponse responseModel { get; set; }
        public BaseServices(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;

        }
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("magicvilla_villaapi");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(
                        JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8,
                        "application/json");
                }
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;

                var clientResponse = await client.SendAsync(message);

                var apiContent = await clientResponse.Content.ReadAsStringAsync();

                var resultData = JsonConvert.DeserializeObject<object[]>(apiContent);
                var APIResponse = new APIResponse
                {
                    StatusCode = clientResponse.StatusCode,
                    IsSuccess = clientResponse.IsSuccessStatusCode,
                    Result = resultData
                };

                return (T)(object)APIResponse;
            }
            catch (Exception e)
            {
                var dto = new APIResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = new List<string> { Convert.ToString(e.Message) }
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponse = JsonConvert.DeserializeObject<T>(res);
                return apiResponse;
            }
        }
    }
}
