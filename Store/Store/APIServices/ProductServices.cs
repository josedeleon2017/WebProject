using Newtonsoft.Json;
using StoreModels.Models;

namespace StoreMVC.APIServices
{
    public class ProductServices : IDisposable
    {
        private const int TIME_OUT = 10;
        private string _url = $"https://localhost:7122/api/Products/user";

        public async Task<IEnumerable<mProduct>> GetProductsUser()
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(TIME_OUT)
            };

            HttpResponseMessage response = await httpClient.GetAsync(_url);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<mProduct>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<mProduct> GetOneProductUser(int id)
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(TIME_OUT)
            };

            HttpResponseMessage response = await httpClient.GetAsync($"{_url}/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<mProduct>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
