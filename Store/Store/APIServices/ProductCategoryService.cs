using Newtonsoft.Json;
using StoreAPI.Models;
using System.Text;
using System.Threading;

namespace Store.APIServices
{
    public static class ProductCategoryService
    {
        const int TIME_OUT = 150;
        const string URL = $"https://localhost:7122/api/ProductCategories";

        public static async Task<IEnumerable<mProductCategory>> GetProductCategories()
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(TIME_OUT)
            };

            HttpResponseMessage response = await httpClient.GetAsync(URL);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<mProductCategory>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public static async Task<mProductCategory> GetProductCategory(int id)
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(TIME_OUT)
            };

            HttpResponseMessage response = await httpClient.GetAsync($"{URL}/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<mProductCategory>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public static async Task<mProductCategory> AddProductCategory(mProductCategory category)
        {
            var json = JsonConvert.SerializeObject(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(TIME_OUT)
            };

            HttpResponseMessage response = await httpClient.PostAsync(URL, content);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return JsonConvert.DeserializeObject<mProductCategory>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public static async Task<mProductCategory> UpdateProductCategory(int id, mProductCategory category)
        {
            var json = JsonConvert.SerializeObject(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(TIME_OUT)
            };

            HttpResponseMessage response = await httpClient.PutAsync($"{URL}/{id}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return JsonConvert.DeserializeObject<mProductCategory>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public static async Task<mProductCategory> DeleteProductCategory(int id)
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(TIME_OUT)
            };

            HttpResponseMessage response = await httpClient.DeleteAsync($"{URL}/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return JsonConvert.DeserializeObject<mProductCategory>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
