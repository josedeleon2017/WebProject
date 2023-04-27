using ClasificacionPeliculas.Models;
using Newtonsoft.Json;
using System.Text;
using System.Threading;

namespace ClasificacionPeliculas.Calls
{
    public class MoviesCalls
    {
        private static string baseurl = "https://localhost:7084/api/Movies";
        private static int timeout = 50;

        //Get Categories
        public static async Task<IEnumerable<Movie>> GetMovies()
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
                return JsonConvert.DeserializeObject<IEnumerable<Movie>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        //Get Movie
        public static async Task<Movie> GetMovie(int id)
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
                return JsonConvert.DeserializeObject<Movie>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        //Insert Movie
        public static async Task<Movie> InsertMovie(Movie object_toserialize)
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
                return JsonConvert.DeserializeObject<Movie>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        //Update Movie
        public static async Task<Movie> UpdateMovie(Movie object_toserialize)
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
                return JsonConvert.DeserializeObject<Movie>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
