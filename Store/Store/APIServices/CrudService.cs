using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Newtonsoft.Json;
using StoreModels.Models;
//using StoreDataAccess.Models;
using System;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Store.APIServices
{
    public class CrudService<K, V> : IDisposable
    {
        private const int TIME_OUT = 10;
        private string _url = $"https://localhost:7122/api/";
        private string _urlToken = $"https://localhost:7122/api/Login/Login";
        private string _apiKey = "asdfghjkl123456789";

        public CrudService(string route)
        {
            _url += route;
        }
        public async Task<IEnumerable<V>> GetAll()
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
                return JsonConvert.DeserializeObject<IEnumerable<V>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<V> GetOne(K id)
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
                return JsonConvert.DeserializeObject<V>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<V> Add(V category)
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

            var token = await GetToken(new Token() { TokenContent = _apiKey });
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.TokenContent);

            HttpResponseMessage response = await httpClient.PostAsync(_url, content);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return JsonConvert.DeserializeObject<V>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<V> Update(K id, V val)
        {
            var json = JsonConvert.SerializeObject(val);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

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

            HttpResponseMessage response = await httpClient.PutAsync($"{_url}/{id}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return JsonConvert.DeserializeObject<V>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public async Task<V> Delete(K id)
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

            HttpResponseMessage response = await httpClient.DeleteAsync($"{_url}/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return JsonConvert.DeserializeObject<V>(await response.Content.ReadAsStringAsync());
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
