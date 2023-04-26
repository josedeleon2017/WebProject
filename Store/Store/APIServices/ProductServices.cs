using Newtonsoft.Json;
using StoreModels.Models;
using System.Text;

namespace StoreMVC.APIServices
{
    public class ProductServices : IDisposable
    {
        private const int TIME_OUT = 10;
        private string _url = $"https://localhost:7122/api/Products/user";
        private string _urlToken = $"https://localhost:7122/api/Login/Login";
        private string _apiKey = "asdfghjkl123456789";

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

            var token = await GetToken(new Token() { TokenContent = _apiKey });
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.TokenContent);

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

            var token = await GetToken(new Token() { TokenContent = _apiKey });
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.TokenContent);

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

        public async Task<Token> GetToken(Token token)
        {
            var json = JsonConvert.SerializeObject(token);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(TIME_OUT)
            };

            HttpResponseMessage response = await httpClient.PostAsync(_urlToken, content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Token>(await response.Content.ReadAsStringAsync());
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
