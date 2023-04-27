
using ClasificacionPeliculas.Models;
using Mysqlx.Crud;
using Newtonsoft.Json;
using System.Text;
using System.Threading;

namespace ClasificacionPeliculas.Calls
{
    public class CategoriesCalls
    {
        private static string baseurl = "https://localhost:7084/api/Categories";
        private static int timeout = 5;

        //Get Categories
        public static async Task<IEnumerable<Category>> GetCategories()
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(timeout)
            };

            HttpResponseMessage response = await httpClient.GetAsync(baseurl);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<Category>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        //Get Category
        public static async Task<Category> GetCategory(int id)
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(timeout)
            };

            HttpResponseMessage response = await httpClient.GetAsync($"{baseurl}/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Category>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        //Insert Category
        public static async Task<Category> InsertCategory(Category object_toserialize)
        {
            var json = JsonConvert.SerializeObject(object_toserialize);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(timeout)
            };

            HttpResponseMessage response = await httpClient.PostAsync(baseurl, content);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return JsonConvert.DeserializeObject<Category>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        //Update Category
        public static async Task<Category> UpdateCategory(Category object_toserialize)
        {
            var json = JsonConvert.SerializeObject(object_toserialize);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(timeout)
            };

            HttpResponseMessage response = await httpClient.PutAsync(baseurl, content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Category>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
